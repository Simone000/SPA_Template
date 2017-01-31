define(["knockout", "text!./page-home.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    //define(["knockout", "text!./page-home.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        self.tipi = ko.observableArray();
        self.id_tipo = ko.observable();
        self.movimentis = ko.observableArray();
        self.movimentis_filtered = ko.computed(function () {
            var filtered = self.movimentis();

            if (self.id_tipo() != null) {
                filtered = ko.utils.arrayFilter(filtered, function (item) {
                    return item.id_Tipo == self.id_tipo();
                });
            }

            return filtered;
        }, self);
        self.movimentisPaginate = new knockoutgrids.ClientGrid(self.movimentis_filtered, 10, 'causale');


        self.loadmovimentis = function () {
            function success(data) {
                self.movimentis(ko.utils.arrayMap(data, function (item) {
                    return new common.Movimenti(item);
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
            api.GetMovimenti($('#div_movimentis'), success, error, params);
        };
        self.loadmovimentis();

        self.loadTipi = function () {
            function success(data) {
                var tmp = new Array();
                tmp.push(new common.BasicListItem({ ID: null, Desc: "Tutti" }));

                ko.utils.arrayForEach(data, function (val) {
                    tmp.push(new common.BasicListItem(val));
                });

                //todo: aggiungere sorting (magari in common.basiclistitem)

                self.tipi(tmp);
            };
            function error(jqXHR, desc) {
                toastr["error"](desc, "Errore!");
            };
            api.GetTipi($('.div_tipi'), success, error);
        };
        self.loadTipi();


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});