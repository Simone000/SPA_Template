﻿define(["knockout", "text!./header.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        //fix dropdown
        $(document).on('click', '.navbar-collapse.in', function (e) {
            if ($(e.target).is('a') && $(e.target).attr('class') != 'dropdown-toggle') {
                $(this).collapse('hide');
            }
        });

        self.isAuth = ko.observable(false);
        self.username = ko.observable();
        self.isAdmin = ko.observable(false);

        self.loadInfo = function () {
            function success(data) {
                self.isAuth(true);

                self.username(data.Email);
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