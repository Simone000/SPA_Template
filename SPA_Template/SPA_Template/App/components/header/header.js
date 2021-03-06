﻿define(["knockout", "text!./header.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        self.isRegistrationEnabled = common.settings.isRegistrationEnabled;

        //fix dropdown
        $(document).on('click', '.navbar-collapse.in', function (e) {
            if ($(e.target).is('a') && $(e.target).attr('class') != 'dropdown-toggle') {
                $(this).collapse('hide');
            }
        });

        self.isAuth = ko.observable(false);
        self.isAdmin = ko.observable(false);

        self.username = ko.observable();
        self.userTrimmed = ko.computed(function () {
            if (!self.username())
                return '';

            if (self.username().length <= 30)
                return self.username();

            return self.username().substring(0, 27) + '...';
        }, self);

        self.loadInfo = function () {
            function success(data) {
                self.isAuth(true);

                self.username(data.Username);
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    //non autenticato => non faccio nulla (default anon)
                    return;
                }
                toastr["error"](desc, "Errore!");
            };
            api.GetUserInfo($('#div_header'), success, error);
        };

        if (self.isAuth() != true) {
            self.loadInfo();
        }

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});