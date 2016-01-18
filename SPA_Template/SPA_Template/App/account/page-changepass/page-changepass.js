define(["knockout", "text!./page-changepass.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.oldPass = ko.observable();
        self.newPassword = ko.observable();
        self.confirmNewPassword = ko.observable();

        self.changePass = function () {
            function success(data) {
                toastr["success"]("Password Cambiata!");
                window.location = "#/";
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    window.location = "/";
                    return;
                }
                //toastr["error"](desc, "Errore!");
            };
            api.ChangePassword($("#div_cambiapass"), success, error, self.oldPass(), self.newPassword(), self.confirmNewPassword());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});