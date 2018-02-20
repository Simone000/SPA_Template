//define(["knockout", "text!./page-sessionstorage.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
define(["knockout", "text!./page-sessionstorage.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        //saving a boolean preference
        self.thingIsHidden = ko.observable(false);
        if (typeof (Storage) !== "undefined") {
            //with a saved thing => I do apply it (boolean are still saved as strings)
            if (sessionStorage.thingIsHidden) {
                self.aziendaGridHidden(sessionStorage.thingIsHidden === "true");
            }
        }
        self.changeGridVisibility = function () {
            self.thingIsHidden(!self.thingIsHidden());

            if (typeof (Storage) !== "undefined") {
                sessionStorage.thingIsHidden = self.thingIsHidden();
            }
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});