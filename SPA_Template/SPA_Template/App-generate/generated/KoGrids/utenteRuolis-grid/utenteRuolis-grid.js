define(["knockout", "text!./utenteRuolis-grid.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.utenteRuolis = ko.observableArray();

        self.utenteRuolisPaginate = ko.computed(function () {
            return new knockoutgrids.ClientGrid(self.utenteRuolis(), 10, 'nome');
        }, self);

        self.loadutenteRuolis = function () {
            function success(data) {
                self.utenteRuolis(ko.utils.arrayMap(data, function (item) {
                    return new common.UtenteRuoli(item);
                }));
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    //window.location = "/#/account/login";
                    return;
                }
                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.GetUtenti($('#div_utenteRuolis'), success, error, params);
        };
        self.loadutenteRuolis();


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});