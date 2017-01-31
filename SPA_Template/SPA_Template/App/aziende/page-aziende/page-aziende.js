define(["knockout", "text!./page-aziende.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.getAziendas = ko.observableArray();

        self.getAziendasPaginate = ko.computed(function () {
            return new knockoutgrids.ClientGrid(self.getAziendas(), 10, 'nome');
        }, self);

        self.loadgetAziendas = function () {
            function success(data) {
                self.getAziendas(ko.utils.arrayMap(data, function (item) {
                    return new common.GetAzienda(item);
                }));
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    return;
                }
                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.GetAziende($('#div_getAziendas'), success, error, params);
        };
        self.loadgetAziendas();


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});