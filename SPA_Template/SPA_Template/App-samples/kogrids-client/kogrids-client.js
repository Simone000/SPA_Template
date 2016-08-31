function Azienda(Azienda) {
    var self = this;

    self.id = Azienda.ID;
    self.nome = ko.observable(Azienda.Nome);
};
function Reparto(Reparto) {
    var self = this;

    self.id = Reparto.ID;
    self.nome = Reparto.Nome;

    self.azienda = Reparto.Azienda ? new Azienda(Reparto.Azienda) : null;
};
function Dipendente(Dipendente) {
    var self = this;

    self.id = Dipendente.ID;
    self.nome = Dipendente.Nome;
    self.check1 = Dipendente.Check1;

    self.reparto = Dipendente.Reparto ? new Reparto(Dipendente.Reparto) : null;

    self.isSelected = ko.observable(false);
};

function pageModel(params) {
    var self = this;

    self.dipendenti = ko.observableArray();
    self.dipendentiPaged = new knockoutgrids.ClientGrid(self.dipendenti, 5, 'reparto.azienda.nome');

    //self.dipendentiPaged_Old = ko.computed(function () {
    //    return new knockoutgrids.ClientGrid(self.dipendenti(), 5, 'reparto.azienda.nome');
    //}, self);

    self.loadDipendenti = function () {
        var dipendenti_tmp = new Array();

        dipendenti_tmp.push(new Dipendente({
            ID: 1,
            Nome: "Dipendente Nome Lungo",
            Check1: false,
            Reparto: {
                ID: 1, Nome: "Reparto 1",
                Azienda: { ID: 1, Nome: "Azienda 1" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 2,
            Nome: "Dipendente 2",
            Check1: true,
            Reparto: {
                ID: 2,
                Nome: "Reparto 2",
                Azienda: { ID: 3, Nome: "Azienda 3" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 3,
            Nome: "Dipendente Nome Molto Lungo",
            Check1: false,
            Reparto: {
                ID: 3,
                Nome: "Reparto 3",
                Azienda: { ID: 2, Nome: "Azienda 2" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 4,
            Nome: "Dipendente 4",
            Check1: false,
            Reparto: {
                ID: 3,
                Nome: "Reparto 3",
                Azienda: { ID: 2, Nome: "Azienda 2" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 5,
            Nome: "Dipendente Nome Lungo",
            Check1: false,
            Reparto: {
                ID: 1, Nome: "Reparto 1",
                Azienda: { ID: 1, Nome: "Azienda 1" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 6,
            Nome: "Dipendente 2",
            Check1: true,
            Reparto: {
                ID: 2,
                Nome: "Reparto 2",
                Azienda: { ID: 3, Nome: "Azienda 3" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 7,
            Nome: "Dipendente Nome Molto Lungo",
            Check1: false,
            Reparto: {
                ID: 3,
                Nome: "Reparto 3",
                Azienda: { ID: 2, Nome: "Azienda 2" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 8,
            Nome: "Dipendente 4",
            Check1: false,
            Reparto: {
                ID: 3,
                Nome: "Reparto 3",
                Azienda: { ID: 2, Nome: "Azienda 2" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 9,
            Nome: "Dipendente Nome Lungo",
            Check1: false,
            Reparto: {
                ID: 1, Nome: "Reparto 1",
                Azienda: { ID: 1, Nome: "Azienda 1" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 10,
            Nome: "Dipendente 2",
            Check1: true,
            Reparto: {
                ID: 2,
                Nome: "Reparto 2",
                Azienda: { ID: 3, Nome: "Azienda 3" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 11,
            Nome: "Dipendente Nome Molto Lungo",
            Check1: false,
            Reparto: {
                ID: 3,
                Nome: "Reparto 3",
                Azienda: { ID: 2, Nome: "Azienda 2" }
            }
        }));
        dipendenti_tmp.push(new Dipendente({
            ID: 12,
            Nome: "Dipendente 4",
            Check1: false,
            Reparto: {
                ID: 3,
                Nome: "Reparto 3",
                Azienda: { ID: 2, Nome: "Azienda 2" }
            }
        }));

        self.dipendenti(dipendenti_tmp);
    };
    self.loadDipendenti();


    /*
    self.loadAzienda = function () {
        if (self.id_azienda() == null)
            return;
        
        function success(data) {
            self.azienda(new common.AziendaServizi(data));
        };
        function error(jqXHR, desc) {
            if (jqXHR["status"] == 401) {
                window.location = "#/login";
                return;
            }
        
            toastr["error"](desc, "Errore!");
        };
        api.GetAziendaServizio($('#div_dettaglioAzienda'), success, error, self.id_azienda());
    };
    */


    return self;
};

var model = new pageModel();
ko.applyBindings(model);