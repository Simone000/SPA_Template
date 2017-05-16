define(["knockout", "text!./page-utentiruoli.html", "toastr", "api", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.utenti = ko.observableArray();

        self.loadUtenti = function () {
            function success(data) {
                self.utenti(data);
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    window.location = "#/account/login";
                    window.location.reload(false);
                    return;
                }
                toastr["error"](desc, "Errore!");
            };
            api.GetUtentiRuoli($("#div_utenti"), success, error);
        };

        self.loadUtenti();

        self.utentiPaged = ko.computed(function () {
            return new knockoutgrids.ClientGrid(self.utenti(), 20, 'Username');
        });


        self.updateRuolo = function (Username, Ruolo, NuovoStato) {
            console.log("update " + Username + " " + Ruolo + " " + NuovoStato);

            function success(data) {
                self.loadUtenti();
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    window.location = "#/account/login";
                    window.location.reload(false);
                    return;
                }
                toastr["error"](desc, "Errore!");
            };
            api.UpdateRuoloUtente($("#div_utenti"), success, error, Username, Ruolo, NuovoStato);
        };


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});