//define(["knockout", "text!./page-empty.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
define(["knockout", "text!./page-empty.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});