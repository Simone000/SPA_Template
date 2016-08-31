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

    self.loadDipendenti = function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) {
        $.ajax({
            url: "/api/Dipendenti/GetDipendentiPaged" + "?PageSize=" + PageSize + "&CurrPage=" + CurrPage + "&SortBy=" + SortBy + "&IsDesc=" + IsDesc + "&SearchBy=" + SearchBy + "&Search=" + Search,
            dataType: 'json',
            asyc: true,
            type: "get",
            success: function (data) {
                self.dipendenti(ko.utils.arrayMap(data, function (item) {
                    return new Dipendente(item);
                }));
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("errore!");
            }
        });
    };

    self.dipendentiPaged = ko.observable(
        new knockoutgrids.ServerGrid(self.dipendenti, 5, 'reparto.azienda.nome',
            function (PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search) { //OnChange
                console.log("OnChange" + " PageSize:" + PageSize + " CurrPage:" + CurrPage + " SortBy:" + SortBy + " IsDesc:" + IsDesc + " SearchBy:" + SearchBy + " Search:" + Search);
                if (!Search)
                    Search = '';
                self.loadDipendenti(PageSize, CurrPage, SortBy, IsDesc, SearchBy, Search);
            })
    );

    self.loadDipendenti(5, 0, 'Nome', true, 'reparto.azienda.nome', '');

    /*
    //subscribe (in case of filters)
    self.statoAziendaSelezionato.subscribe(function () {
        self.dipendentiPaged().refresh();
    });
    self.servizioSelezionato.subscribe(function () {
        self.dipendentiPaged().refresh();
    });
    */

    return self;
};

var model = new pageModel();
ko.applyBindings(model);