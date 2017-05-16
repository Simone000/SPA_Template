define(["knockout", "text!./page-register.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.email = ko.observable();
        self.password = ko.observable();
        self.confirmPassword = ko.observable();

        self.register = function () {
            function success(data) {
                if (data.IsLogged === false && data.IsConfirmEmailSent === true) {
                    toastr["success"]("Ti abbiamo inviato una email con un link", "Registrazione Effettuata!");
                    window.location = "#";
                    return;
                }

                window.location.replace("");
            };
            function error(jqXHR, desc) {
                //toastr["error"](desc, "Errore!");
            };
            api.Register($("#div_register"), success, error, self.email(), self.password(), self.confirmPassword());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});