define(["jquery", "knockout"], function ($, ko) {

    function Today() {
        var self = this;

        self.datetime = ko.observable(new Date());

        self.toString = ko.computed(function () {
            var giorno = self.datetime().getDate();
            var mese = self.datetime().getMonth() + 1;
            var anno = self.datetime().getFullYear();
            return giorno + "/" + mese + "/" + anno;
        }, self);
    };

    function Data(JS_Data) { //millisecondi da epochtime
        var self = this;

        self.datajs = ko.observable(JS_Data);

        self.datetime = ko.computed(function () {
            if (self.datajs() <= 0)
                return null;
            return new Date(self.datajs());
        }, self);

        self.toString = ko.computed(function () {
            if (self.datetime() == null)
                return "";

            var giorno = self.datetime().getDate();
            var mese = self.datetime().getMonth() + 1;
            var anno = self.datetime().getFullYear();

            //fix formattazione per giorni e mesi
            var giornoToString = giorno + "";
            var meseToString = mese + "";
            if (giornoToString.length == 1)
                giornoToString = "0" + giornoToString;
            if (meseToString.length == 1)
                meseToString = "0" + meseToString;

            return giornoToString + "/" + meseToString + "/" + anno;
        }, self);

        //todo: aggiungere localizzazione
    };

    function Appuntamento(Appuntamento) {
        var self = this;

        self.id = Appuntamento.ID;

        self.dataEsecuzione = new Data(Appuntamento.DataEsecuzione_JS);

        self.dataToString = Appuntamento.DataToString;
        self.specialista = new Specialista(Appuntamento.Specialista);

        self.prestazione = Appuntamento.Prestazione;
    };

    function Specialista(Specialista) {
        var self = this;

        self.id = Specialista.ID;
        self.nome = Specialista.Nome;
    };

    function Reparto(Reparto) {
        var self = this;

        self.id = Reparto.ID;
        self.nome = Reparto.Nome;

        self.azienda = Reparto.Azienda ? new Azienda(Reparto.Azienda) : null;
    };

    function File(File) {
        var self = this;

        self.id = File.ID;
        self.nome = File.Nome;
    };

    function Dipendente(Dipendente) {
        var self = this;

        self.id = Dipendente.ID;
        self.nome = Dipendente.Nome;
        
        self.dataUltimaVisita = new Data(Dipendente.DataUltimaVisita_JS);
        self.giudizio = Dipendente.Giudizio;

        self.reparto = new Reparto(Dipendente.Reparto);
    };

    function Documento(Documento) {
        var self = this;

        self.id = Documento.ID;
        self.tipologia = Documento.Tipologia;
        self.dataEsecuzione = new Data(Documento.DataEsecuzione_JS);
        self.dataRilascio = new Data(Documento.DataRilascio_JS);
        self.isClosed = Documento.IsClosed;

        self.file = Documento.File ? new File(Documento.File) : null;
    };

    function Azienda(Azienda) {
        var self = this;

        self.id = Azienda.ID;
        self.nome = Azienda.Nome;
        self.indirizzo = Azienda.Indirizzo;

        self.nomeContatto = Azienda.NomeContatto;
        self.telefono = Azienda.Telefono;
        self.fax = Azienda.Fax;
        
        self.emails = Azienda.Emails; //Array
        self.pecs = Azienda.PECs; //Array
        self.periodicitaSopralluogo = Azienda.PeriodicitaSopralluogo;


        self.isMedicinaAttiva = Azienda.IsMedicinaAttiva;
        self.isSicurezzaAttiva = Azienda.IsSicurezzaAttiva;
    };

    function Prestazione(Prestazione) {
        var self = this;

        self.id = Prestazione.ID;

        self.nome = Prestazione.Nome;
        self.periodo = Prestazione.Periodo;
        self.inizio = Prestazione.Inizio;
        self.mese = Prestazione.Mese;

        self.dataProxEsecuzione = new Data(Prestazione.DataProxEsecuzione_JS);
    };

    function GiudizioPrestazione(GiudizioPrestazione) {
        var self = this;

        self.id = GiudizioPrestazione.ID;
        self.dataVisita = new Data(GiudizioPrestazione.DataVisita_JS);

        self.prestazione = GiudizioPrestazione.Prestazione;
        self.motivo = GiudizioPrestazione.Motivo;
        self.giudizio = GiudizioPrestazione.Giudizio;
    };

    function Giudizio(Giudizio) {
        var self = this;

        self.id = Giudizio.ID;

        self.desc = Giudizio.Desc;
        self.prossimaVisita = Giudizio.ProssimaVisita;
        self.prossimaVisita = new Data(Giudizio.ProssimaVisita_JS);
        self.prossimaVisitaDesc = Giudizio.ProssimaVisitaDesc;
        self.prescrizioni = Giudizio.Prescrizioni;
        self.prescrizioniTemporanee = Giudizio.PrescrizioniTemporanee;
        self.prescrizioniPermanenti = Giudizio.PrescrizioniPermanenti;
        self.limitazioni = Giudizio.Limitazioni;
        self.limitazioniTemporanee = Giudizio.LimitazioniTemporanee;
        self.limitazioniPermanenti = Giudizio.LimitazioniPermanenti;
        self.annotazioni = Giudizio.Annotazioni;
    };

    function TipologiaPrestazione(TipologiaPrestazione) {
        var self = this;

        self.id = TipologiaPrestazione.ID;
        self.desc = TipologiaPrestazione.Desc;
    };

    return {
        Data: Data,
        Appuntamento: Appuntamento,
        Specialista: Specialista,
        Reparto: Reparto,
        File: File,
        Dipendente: Dipendente,
        Documento: Documento,
        Azienda: Azienda,
        Prestazione: Prestazione,
        GiudizioPrestazione: GiudizioPrestazione,
        Giudizio: Giudizio,
        TipologiaPrestazione: TipologiaPrestazione
    };
});