define(["knockout", "text!./ko-clientgrid-t1.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
    function pageModel(params) {
        var self = this;

        self.nomeObsArray = ko.observableArray();

        self.nomeObsArrayPaginate = ko.computed(function () {
            return new knockoutgrids.ClientGrid(self.nomeObsArray(), 10, 'nome');
        }, self);

        self.loadnomeObsArray = function () {
            function success(data) {
                self.nomeObsArray(ko.utils.arrayMap(data, function (item) {
                    return new common.nomeModello(item);
                }));
            };
            function error(jqXHR, desc) {
                toastr["error"](desc, "Errore!");
            };
            api.ActionName($('#div_nomeObsArray'), success, error, params);
        };
        self.loadnomeObsArray();


        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});