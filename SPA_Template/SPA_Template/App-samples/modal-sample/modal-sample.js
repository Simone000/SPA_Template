define(["knockout", "text!./modal-sample.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;


        self.id_modal1 = ko.observable();
        self.showModal1 = function (id) {
            self.id_modal1(id);
            $('#modal_1').modal('show');
            self.loadModal1();
        };
        self.newQuestion = ko.observable('');
        self.newAnswer = ko.observable('');
        self.loadModal1 = function () {
            //todo: after load set newQuestion and newAnswer
        };
        self.updateModal1 = function () {
            //todo: onsuccess
            $('#modal_1').modal('hide');
            self.newQuestion('');
            self.newAnswer('');
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});