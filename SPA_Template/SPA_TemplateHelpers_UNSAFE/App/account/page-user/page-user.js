define(["knockout", "text!./page-user.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.userInfo = ko.observable();

        self.loadInfo = function () {
            function success(data) {
                self.userInfo(new common.UserInfo(data));
            };
            function error(jqXHR, desc) {
                //redirect on Unauthorized
                if (jqXHR["status"] == 401) {
                    window.location = "#/account/login";
                    window.location.reload(false);
                    return;
                }
                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.GetUserInfo($('#div_userInfo'), success, error);
        };
        self.loadInfo();

        self.logout = function () {
            function success(data) {
                window.location.replace("");
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