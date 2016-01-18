define(["knockout", "text!./page-user.html", "toastr", "api","bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.email = ko.observable();

        self.loadInfo = function () {
            function success(data) {
                self.email(data.Email);
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    window.location = "/";
                    return;
                }
                toastr["error"](desc, "Errore!");
            };
            api.GetUserInfo($('#div_userInfo'), success, error);
        };

        self.loadInfo();


        self.logout = function () {
            function success(data) {
                window.location = "/";
            };
            function error(jqXHR, desc) {
                toastr["error"](desc, "Errore!");
            };
            api.LogOff($("#div_pageUtente"), success, error);
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});