﻿define(["knockout", "text!./page-internal.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});