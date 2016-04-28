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

    self.loadDipendenti = function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) {
        //var dipendenti_tmp = new Array();
        //dipendenti_tmp.push(new Dipendente({ ID: 1, Nome: "Dipendente 1", Reparto: { ID: 1, Nome: "Reparto 1", Azienda: { ID: 1, Nome: "Azienda 1" } } }));
        //dipendenti_tmp.push(new Dipendente({ ID: 2, Nome: "Dipendente 2", Reparto: { ID: 2, Nome: "Reparto 2", Azienda: { ID: 3, Nome: "Azienda 3" } } }));
        //dipendenti_tmp.push(new Dipendente({ ID: 3, Nome: "Dipendente 3", Reparto: { ID: 3, Nome: "Reparto 3", Azienda: { ID: 2, Nome: "Azienda 2" } } }));
        //dipendenti_tmp.push(new Dipendente({ ID: 4, Nome: "Dipendente 4", Reparto: { ID: 1, Nome: "Reparto 1", Azienda: { ID: 1, Nome: "Azienda 1" } } }));
        //dipendenti_tmp.push(new Dipendente({ ID: 5, Nome: "Dipendente 5", Reparto: { ID: 2, Nome: "Reparto 2", Azienda: { ID: 3, Nome: "Azienda 3" } } }));
        //dipendenti_tmp.push(new Dipendente({ ID: 6, Nome: "Dipendente 6", Reparto: { ID: 3, Nome: "Reparto 3", Azienda: { ID: 2, Nome: "Azienda 2" } } }));
        //self.dipendenti(dipendenti_tmp);

        $.ajax({
            url: 'api/Aziende/GetAziendePaged/',
            dataType: 'json',
            asyc: true,
            type: "get",
            success: function (data) {
                self.dipendenti(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("errore!");
            }
        });
    };
    self.loadDipendenti(5, 0, 'Nome', true, 'reparto.azienda.nome', '');

    self.dipendentiPaged = ko.observable(new knockoutgrids.ServerGrid(self.dipendenti(), 5, 'reparto.azienda.nome',
            function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) { //OnChange
                console.log("OnChange" + " PageSize:" + PageSize + " CurrPage:" + CurrPage + " SortBy:" + SortBy + " IsDesc:" + IsDesc + " SearchBy:" + SearchBy + " Search:" + Search);
                if (!Search)
                    Search = '';
                self.loadDipendenti(PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
            })
    );

    /*
    self.loadAziende = function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) {
        function success(data) {
            self.aziende(ko.utils.arrayMap(data, function (item) {
                return new common.AziendaServizi(item);
            }));
        };
        function error(jqXHR, desc) {
            toastr["error"](desc, "Errore!");
        };
        api.GetAziendeServiziPaged($('#div_aziendeServiziBlock'), success, error, PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
    };

    self.aziendePaged = ko.observable(
        new knockoutgrids.ServerGrid(self.aziende, 10, 'nome',
            function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) { //OnChange
                console.log("OnChange" + " PageSize:" + PageSize + " CurrPage:" + CurrPage + " SortBy:" + SortBy + " IsDesc:" + IsDesc + " SearchBy:" + SearchBy + " Search:" + Search);
                if (!Search)
                    Search = '';
                self.loadAziende(PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
            })
    );

    self.loadAziende(10, 1, 'Nome', true, 'reparto.azienda.nome', '');

    //subscribe
    self.statoAziendaSelezionato.subscribe(function () {
        self.aziendePaged().refresh();
    });
    self.servizioSelezionato.subscribe(function () {
        self.aziendePaged().refresh();
    });
    */

    return self;
};

var model = new pageModel();
ko.applyBindings(model);