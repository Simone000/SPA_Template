define(["knockout", "text!./page-test1.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.tipologia = 1; //todo: params

        self.routeAziendeOrProfessionisti = function () {
            if (self.tipologia == 1)
                return "ListiniAziende";
            return "ListiniProfessionisti";
        };

        self.listinos = ko.observableArray();

        self.loadlistinos = function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) {
            function success(data) {
                self.listinos(ko.utils.arrayMap(data, function (item) {
                    return new common.Listino(item);
                }));
            };
            function error(jqXHR, desc) {
                toastr["error"](desc, "Errore!");
            };
            api.GetListiniPaged($('#div_listinos'), success, error, PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search, self.tipologia);
        };

        self.listinosPaginate = ko.observable(
            new knockoutgrids.ServerGrid(self.listinos, 10, 'descrizione',
                function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) { //OnChange
                    console.log("OnChange" + " PageSize:" + PageSize + " CurrPage:" + CurrPage + " SortBy:" + SortBy + " IsDesc:" + IsDesc + " SearchBy:" + SearchBy + " Search:" + Search);
                    if (!Search)
                        Search = '';
                    self.loadlistinos(PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
                })
        );

        self.loadlistinos(10, 0, 'descrizione', true, 'descrizione', '');


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});