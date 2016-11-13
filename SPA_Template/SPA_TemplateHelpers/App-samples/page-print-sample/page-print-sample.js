define(["knockout", "text!./page-print-sample.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        self.print = function () {
            var divContent = $("#div_printContent");
            var content = divContent.html();

            var w = window.open();
            w.document.write(content);
            w.print();
            w.close();
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});