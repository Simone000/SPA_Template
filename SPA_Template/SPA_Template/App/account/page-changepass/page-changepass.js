define(["knockout", "text!./page-changepass.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.oldPass = ko.observable();
        self.newPassword = ko.observable();
        self.confirmNewPassword = ko.observable();

        self.changePass = function () {
            function success(data) {
                toastr["success"]("Password Cambiata!");
                window.location.replace("");
            };
            function error(jqXHR, desc) {
                //redirect on Unauthorized
                if (jqXHR["status"] == 401) {
                    window.location = "/#/account/login";
                    return;
                }

                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.ChangePassword($("#div_cambiapass"), success, error, self.oldPass(), self.newPassword(), self.confirmNewPassword());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});