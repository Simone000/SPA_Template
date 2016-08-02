define(["knockout", "text!./page-login.html", "toastr", "api", "bootstrap"], function (ko, AnyLoginTemplate, toastr, api) {
    function AnyLogin(params) {
        var self = this;

        self.email = ko.observable('');
        self.password = ko.observable('');

        self.login = function () {
            function success(data) {
                window.location.replace("");
            };
            function error(jqXHR, desc) {
                //In case of missing connection do not use form
                if (jqXHR["status"] == 0) {
                    toastr["error"](desc, "Errore!");
                }
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