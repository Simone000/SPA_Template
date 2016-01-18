define(["knockout", "text!./page-creautente.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.email = ko.observable();
        self.password = ko.observable();
        self.confirmPassword = ko.observable();

        self.creaUtente = function () {
            function success(data) {
                window.location = "/#/amministrazione/utenti";
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    window.location = "/#/account/login";
                    return;
                }
                //toastr["error"](desc, "Errore!");
            };
            api.CreaUtente($("#div_creautente"), success, error, self.email(), self.password(), self.confirmPassword());
        };


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});