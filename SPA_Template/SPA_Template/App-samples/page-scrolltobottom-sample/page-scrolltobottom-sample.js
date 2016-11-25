define(["knockout", "text!./page-scrolltobottom-sample.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        self.shouldScrollToBottom = ko.observable(true);

        //scroll to bottom (to be called after adding an element
        if (self.shouldScrollToBottom() === true) {
            var element = document.getElementById("div_container");
            if (element) {
                element.scrollTop = element.scrollHeight - element.clientHeight;
            }
        }

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});