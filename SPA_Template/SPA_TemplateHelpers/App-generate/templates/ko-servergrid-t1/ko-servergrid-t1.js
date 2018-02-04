define(["knockout", "text!./ko-servergrid-t1.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.prestazioniAziendaDaFatturares = ko.observableArray();

        self.loadprestazioniAziendaDaFatturares = function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) {
            function success(data) {
                self.prestazioniAziendaDaFatturares(ko.utils.arrayMap(data, function (item) {
                    return new common.PrestazioniAziendaDaFatturare(item);
                }));
            };
            function error(jqXHR, desc) {
                if (jqXHR["status"] == 401) {
                    //window.location = "#/account/login";
                    //window.location.reload(false);
                    return;
                }
                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.GetPrestazioniAttiveDaFatturarePaged($('#div_prestazioniAziendaDaFatturares'), success, error, PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
        };

        self.prestazioniAziendaDaFatturaresPaginate = ko.observable(
            new knockoutgrids.ServerGrid(self.prestazioniAziendaDaFatturares, 10, 'NomeOrganizzazione',
                function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) { //OnChange
                    //console.log("OnChange" + " PageSize:" + PageSize + " CurrPage:" + CurrPage + " SortBy:" + SortBy + " IsDesc:" + IsDesc + " SearchBy:" + SearchBy + " Search:" + Search);
                    if (!Search)
                        Search = '';
                    self.loadprestazioniAziendaDaFatturares(PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
                })
        );
        self.loadprestazioniAziendaDaFatturares(10, 0, 'NomeOrganizzazione', true, 'NomeOrganizzazione', '');

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});