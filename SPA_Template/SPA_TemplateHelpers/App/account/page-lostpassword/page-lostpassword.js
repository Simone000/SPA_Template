define(["knockout", "text!./page-lostpassword.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.email = ko.observable();

        self.lostPass = function () {
            function success(data) {
                toastr["success"]("Email di recupero inviata");
                window.location = "/#/account/login";
            };
            function error(jqXHR, desc) {
                toastr["error"](desc, "Errore!");
            };
            api.ForgotPassword($('#div_lostPass'), success, error, self.email());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});