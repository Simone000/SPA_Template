//define(["knockout", "text!./page-faq.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {
define(["knockout", "text!./page-faq.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        self.id_faq = ko.observable(params.id_faq);

        //focus e expand selected
        if (self.id_faq()) {
            var div_faq = $("#faq_" + self.id_faq());
            if (div_faq) {
                div_faq.addClass("in");
                var offset = div_faq.offset();
                if (offset) {
                    $(window).scrollTop(offset.top - 100);
                }
            }
        }

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});