﻿angular.module('signupServices', []).factory('$signupServices', function ($http) {
    return {//https://github.com/mgonto/restangular <== look into this?
      getGroup: function (ids) {

          return $http.get(siteRoot + "api/Sessions/Groups", { params: { Ids: ids } }).then(function (result) {
                return result.data;
            });
        },
        getSignup: function (ids) {
            return $http.get(siteRoot + "api/Sessions", { params: { Ids: ids } }).then(function (result) {
                return result.data;
            });
        },
        saveSignup: function(signup) {
            return $http.post(siteRoot + "api/Sessions/Save", { Signup: signup }).then(function (result) {
                return result.data;
            });
        }/*,
        getSections: function (ids) {
            return $http.get("/api/Pages/Sections", { params: { Ids: ids } }).then(function (result) {
                return result.data;
            });
        },
        newSection: function () {
            return $http.get("/api/Pages/New").then(function (result) {
                return result.data;
            });
        },
        createSection: function (data) {
            return $http.post("/api/Pages/Sections", { Section: data }).then(function (result) {
                return result.data;
            });
        },
        updateSection: function (data) {
            return $http.put("/api/Pages/Sections", { Section: data }).then(function (result) {
                return result.data;
            });
        },
        deleteSection: function (id) {

            return $http.delete("/api/Pages/Sections", { params: { Id: id } }).then(function (result) {
                return result.data;
            });
        },
        getAuditTrail: function (id) {
            return $http.get("/api/Pages/Audit", { params: { Id: id } }).then(function (result) {
                return result.data;
            });
        },
        doRollBack: function (record) {
            return $http.post("/api/Pages/Audit/Rollback", { Record: record }).then(function (result) {
                return result.data;
            });
        },
        doSearch: function (search) {
            return $http.post("/api/Pages/Search", { ApplicationSearch: search }).then(function (result) {
                return result.data;
            });
        }*/
    };
});