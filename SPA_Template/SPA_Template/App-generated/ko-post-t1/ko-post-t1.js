define(["knockout", "text!./ko-post-t1.html", "toastr", "api", "bootstrap"], function (ko, pageTemplate, toastr, api) {
    function pageModel(params) {
        var self = this;

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});