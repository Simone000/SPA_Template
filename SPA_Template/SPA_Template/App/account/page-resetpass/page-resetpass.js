define(["knockout", "text!./page-resetpass.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.userid = params.userid;
        self.code = params.code;
        self.encodedCode = encodeURIComponent(params.code);

        self.password = ko.observable();
        self.confirmPassword = ko.observable();

        self.resetPass = function () {
            function success(data) {
                toastr["success"]("Password Cambiata!");
                window.location = "#/account/login";
                window.location.reload(false);
            };
            function error(jqXHR, desc) {
                toastr["error"](desc, "Errore!");
            };
            api.ResetPassword($('#div_resetPass'), success, error, self.userid, self.encodedCode, self.password(), self.confirmPassword());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});