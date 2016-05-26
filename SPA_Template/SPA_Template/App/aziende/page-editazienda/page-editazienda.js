define(["knockout", "text!./page-editazienda.html", "toastr", "common", "api", "bootstrap"], function (ko, pageTemplate, toastr, common, api) {
    function pageModel(params) {
        var self = this;

        self.id = params.id; //todo: load azienda by id


        self.nome = ko.observable();
        self.descrizione = ko.observable();

        self.date1 = new common.Data(1464198629828.369);
        self.date2 = new common.Data(1464198629828.369); //nullable


        self.SalvaAzienda = function () {
            function success(data) {
                console.log("Success");
            };
            function error(jqXHR, desc) {
                console.log("Error");
            };

            // Nome: Nome, Descrizione: Descrizione, TestDate: TestDate, TestDate2: TestDate2
            api.UpdateAzienda($('#div_editAzienda'), success, error, self.nome(), self.descrizione(), self.date1.newValue(), self.date2.newValue());
        };

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});