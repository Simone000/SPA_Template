define(["knockout", "text!./page-utenti.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.utentes = ko.observableArray();
        self.utentesPaginate = new knockoutgrids.ClientGrid(self.utentes, 10, 'username');

        self.loadutentes = function () {
            function success(data) {
                self.utentes(ko.utils.arrayMap(data, function (item) {
                    return new common.Utente(item);
                }));
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    return;
                }
                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.GetUtenti($('#div_utentes'), success, error);
        };
        self.loadutentes();

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});