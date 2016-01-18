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

    self.reparto = Dipendente.Reparto ? new Reparto(Dipendente.Reparto) : null;

    self.isSelected = ko.observable(false);
};


/*
    var eventiDisponibili_temp = ko.utils.arrayMap(DettaglioEvento.EventiDisponibili, function (evento) {
        return new Evento(evento);
    });
    self.eventiDisponibili = ko.observableArray(eventiDisponibili_temp);
 */

function pageModel(params) {
    var self = this;

    self.dipendenti = ko.observableArray();

    self.loadDipendenti = function () {
        var dipendenti_tmp = new Array();
        dipendenti_tmp.push(new Dipendente({ ID: 1, Nome: "Dipendente 1", Reparto: { ID: 1, Nome: "Reparto 1", Azienda: { ID: 1, Nome: "Azienda 1" } } }));
        dipendenti_tmp.push(new Dipendente({ ID: 2, Nome: "Dipendente 2", Reparto: { ID: 2, Nome: "Reparto 2", Azienda: { ID: 3, Nome: "Azienda 3" } } }));
        dipendenti_tmp.push(new Dipendente({ ID: 3, Nome: "Dipendente 3", Reparto: { ID: 3, Nome: "Reparto 3", Azienda: { ID: 2, Nome: "Azienda 2" } } }));

        self.dipendenti(dipendenti_tmp);
    };
    self.loadDipendenti();

    self.dipendentiPaged = ko.computed(function () {
        return new knockoutgrids.ClientGrid(self.dipendenti(), 10, 'reparto.azienda.nome');
    }, self);


    return self;
};

var model = new pageModel();
ko.applyBindings(model);