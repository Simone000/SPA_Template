define(["knockout", "text!./page-editazienda.html", "toastr", "common", "api", "bootstrap"], function (ko, pageTemplate, toastr, common, api) {
    function pageModel(params) {
        var self = this;

        self.id = params.id; //todo: load azienda by id
        console.log("edit id: " + params.id);


        self.nome = ko.observable();
        self.descrizione = ko.observable();

        self.SalvaAzienda = function () {
            function success(data) {
                console.log("Success");
            };
            function error(jqXHR, desc) {
                console.log("Error");
            };
            api.SalvaAzienda($('#div_editAzienda'), success, error, self.nome(), self.descrizione());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});