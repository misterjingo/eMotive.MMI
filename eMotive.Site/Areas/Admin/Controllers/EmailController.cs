﻿using System.Linq;
using System.Web.Mvc;
using eMotive.Managers.Interfaces;
using eMotive.Models.Objects.Email;
using eMotive.Models.Objects.StatusPages;
using eMotive.Services.Interfaces;
using Extensions;

namespace eMotive.MMI.Areas.Admin.Controllers
{
    
    public class EmailController : Controller
    {
        private readonly IEmailService emailService;
        private readonly INotificationService notificationService;
        private readonly IUserManager userManager;
        private readonly string[] searchType;
        public EmailController(IEmailService _emailService, IUserManager _userManager, INotificationService _notificationService)
        {
            emailService = _emailService;
            notificationService = _notificationService;
            userManager = _userManager;
            searchType = new[] {"Email"};
        }

        [Authorize(Roles = "Super Admin, Admin, Moderator")]
        public ActionResult Index(EmailSearch emailSearch)
        {
            emailSearch.Type = searchType;

            var searchItem = emailService.DoSearch(emailSearch);
            var user = userManager.Fetch(User.Identity.Name);

            if (user == null)
            {
                emailSearch.CanCreate = false;
            }
            else
            {
                if (user.Roles.Any(n => n.Name == "Super Admin"))
                    emailSearch.CanCreate = true;
            }

            if (searchItem.Items.HasContent())
            {
                emailSearch.NumberOfResults = searchItem.NumberOfResults;
                emailSearch.Emails = emailService.FetchRecordsFromSearch(searchItem);

                return View(emailSearch);
            }
            return View(new EmailSearch());
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public ActionResult Create()
        {
            return View(emailService.New());
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public ActionResult Create(Email email)
        {
            if (ModelState.IsValid)
            {
                int id;
                if (emailService.CreateMessage(email, out id))
                {
                    var successView = new SuccessView
                    {
                        Message = string.Format("The {0} email was successfully updated.", email.Key),
                        Links = new[]
                            {
                                new SuccessView.Link {Text = "Edit the new Email", URL = Url.Action("Edit", "Email", new {key = email.Key, area="Admin"})},//string.Format("/Admin/Email/Edit/{0}", email.Key)},
                                new SuccessView.Link {Text = "Return to Email Home", URL = Url.Action("Index", "Email", new {area="Admin"})}

                            }
                    };

                    TempData["SuccessView"] = successView;

                    return RedirectToAction("Success", "Home", new { area = "Admin" });
                }

                var errors = notificationService.FetchIssues();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("error", error);
                }
            }

            return View(email);
        }


        [HttpGet]
        [Authorize(Roles = "Super Admin, Admin, Moderator")]
        public ActionResult Edit(string key)
        {
            return View(emailService.FetchMessage(key));
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Moderator")]
        public ActionResult Edit(Email email)
        {
            if (ModelState.IsValid)
            {
                if (emailService.SaveMessage(email))
                {
                    var successView = new SuccessView
                        {
                            Message = string.Format("The {0} email was successfully updated.", email.Key),
                            Links = new[]
                            {
                                new SuccessView.Link {Text = string.Format("Return to Edit '{0}' Email", email.Key), URL = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri},
                                new SuccessView.Link {Text = "Return to Email Home", URL = Url.Action("Index", "Email", new {area="Admin"})}

                            }
                        };

                    TempData["SuccessView"] = successView;

                    return RedirectToAction("Success", "Home", new { area = "Admin"});
                }

                var errors = notificationService.FetchIssues();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("error", error);
                }
            }

            return View(email);
        }

    }
}
