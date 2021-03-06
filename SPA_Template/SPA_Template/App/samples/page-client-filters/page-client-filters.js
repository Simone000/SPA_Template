﻿define(["knockout", "text!./page-client-filters.html", "toastr", "api", "common", "knockoutgrids", "bootstrap"], function (ko, pageTemplate, toastr, api, common, knockoutgrids) {

    function Azienda(Azienda) {
        var self = this;

        self.id = Azienda.ID;
        self.nome = Azienda.Nome;
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
        self.note = Dipendente.Note;

        self.reparto = Dipendente.Reparto ? new Reparto(Dipendente.Reparto) : null;

        self.isSelected = ko.observable(false);
    };

    function pageModel(params) {
        var self = this;

        self.dipendenti = ko.observableArray();

        //Distinct Aziende contained in dipendenti
        self.id_azienda = ko.observable('Tutti');
        self.aziende = ko.computed(function () {
            var risArray = new Array({ desc: 'Tutti', value: 'Tutti' });

            if (self.dipendenti()) {
                var tmp = new Array();
                ko.utils.arrayForEach(self.dipendenti(), function (item) {
                    var value = item.reparto.azienda.id; //select value
                    var desc = item.reparto.azienda.nome; //select desc
                    var alreadyExistPos = tmp.indexOf(value);
                    if (alreadyExistPos < 0) {
                        tmp.push(value);
                        risArray.push({ desc: desc, value: value });
                    }
                });
            }

            //Sorting
            risArray = risArray.sort(function (l, r) {
                if (l.value == 'Tutti')
                    return -1;
                if (r.value == 'Tutti')
                    return 1;
                return l.desc > r.desc ? 1 : -1;
            });

            return risArray;
        }, self);

        //Distinct Reparti contained in dipendenti
        self.id_reparto = ko.observable('Tutti');
        self.reparti = ko.computed(function () {
            var risArray = new Array({ desc: 'Tutti', value: 'Tutti' });

            if (self.dipendenti()) {
                var tmp = new Array();
                ko.utils.arrayForEach(self.dipendenti(), function (item) {
                    var value = item.reparto.id; //select value
                    var desc = item.reparto.nome; //select desc
                    var alreadyExistPos = tmp.indexOf(value);
                    if (alreadyExistPos < 0) {
                        tmp.push(value);
                        risArray.push({ desc: desc, value: value });
                    }
                });
            }

            //Sorting
            risArray = risArray.sort(function (l, r) {
                if (l.value == 'Tutti')
                    return -1;
                if (r.value == 'Tutti')
                    return 1;
                return l.desc > r.desc ? 1 : -1;
            });

            return risArray;
        }, self);

        self.dipendenti_filtered = ko.computed(function () {
            var filtered = self.dipendenti();

            //Filtro su azienda
            if (self.id_azienda() != 'Tutti') {
                filtered = ko.utils.arrayFilter(filtered, function (item) {
                    return item.reparto.azienda.id == self.id_azienda();
                });
            }

            //Filtro su reparto
            if (self.id_reparto() != 'Tutti') {
                filtered = ko.utils.arrayFilter(filtered, function (item) {
                    return item.reparto.id == self.id_reparto();
                });
            }

            return filtered;
        }, self);

        self.dipendentiPaged = new knockoutgrids.ClientGrid(self.dipendenti_filtered, 5, 'reparto.azienda.nome');


        //Load sample data
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
    }

    return { viewModel: pageModel, template: pageTemplate };
});