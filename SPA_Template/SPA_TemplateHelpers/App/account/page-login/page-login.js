define(["knockout", "text!./page-login.html", "toastr", "api", "bootstrap"], function (ko, AnyLoginTemplate, toastr, api) {
    function AnyLogin(params) {
        var self = this;

        self.email = ko.observable('assistenzaclienti@cosi.it');
        self.password = ko.observable('assistenzaclienti..');

        self.login = function () {
            function success(data) {
                window.location.replace("");
            };
            function error(jqXHR, desc) {
                //toastr["error"](desc, "Errore!");
            };
            api.Login($('#div_login'), success, error, self.email(), self.password(), null);
        };

        self.enter = function (data, event) {
            if (event.keyCode == 13)
                self.login();
            return true;
        };

        return self;
    }
    return { viewModel: AnyLogin, template: AnyLoginTemplate };
});