define(["jquery", "knockout", "sammy", "bootstrap"], function ($, ko, sammy) {

    //components
    ko.components.register("components-header", {
        require: "App/components/header/header"
    });
    ko.components.register("components-azienda-recap", {
        require: "App/components/azienda-recap/azienda-recap"
    });
    ko.components.register("components-dipendenti-grid", {
        require: "App/components/dipendenti-grid/dipendenti-grid"
    });
    ko.components.register("components-reparti-grid", {
        require: "App/components/reparti-grid/reparti-grid"
    });
    ko.components.register("components-cal-appuntamenti", {
        require: "App/components/cal-appuntamenti/cal-appuntamenti"
    });
    ko.components.register("components-agg-previsti", {
        require: "App/components/agg-previsti/agg-previsti"
    });
    ko.components.register("components-medicocompetente", {
        require: "App/components/medicocompetente/medicocompetente"
    });
    ko.components.register("components-specialisti-byspecialita", {
        require: "App/components/specialisti-byspecialita/specialisti-byspecialita"
    });
    ko.components.register("components-listini-grid", {
        require: "App/components/listini-grid/listini-grid"
    });
    ko.components.register("components-prestazioni-fattura", {
        require: "App/components/prestazioni-fattura/prestazioni-fattura"
    });
    ko.components.register("components-prestazioni-fattura-associabili", {
        require: "App/components/prestazioni-fattura-associabili/prestazioni-fattura-associabili"
    });
    ko.components.register("components-records-fattura", {
        require: "App/components/records-fattura/records-fattura"
    });

    //tests
    ko.components.register("tests-page-test1", {
        require: "App/tests/page-test1/page-test1"
    });


    //Account
    ko.components.register("account-page-login", {
        require: "App/account/page-login/page-login"
    });
    ko.components.register("account-page-user", {
        require: "App/account/page-user/page-user"
    });
    ko.components.register("account-page-changepass", {
        require: "App/account/page-changepass/page-changepass"
    });
    ko.components.register("account-page-lostpassword", {
        require: "App/account/page-lostpassword/page-lostpassword"
    });
    ko.components.register("account-page-resetpass", {
        require: "App/account/page-resetpass/page-resetpass"
    });
    ko.components.register("account-page-register", {
        require: "App/account/page-register/page-register"
    });

    //Amm
    ko.components.register("amm-page-utenti", {
        require: "App/amm/page-utenti/page-utenti"
    });
    ko.components.register("amm-page-creautente", {
        require: "App/amm/page-creautente/page-creautente"
    });

    //planner
    ko.components.register("planner-page-home", {
        require: "App/planner/page-home/page-home"
    });
    ko.components.register("aziende-grid", {
        require: "App/planner/aziende-grid/aziende-grid"
    });
    ko.components.register("planner", {
        require: "App/planner/planner/planner"
    });
    ko.components.register("dettaglio-evento", {
        require: "App/planner/dettaglio-evento/dettaglio-evento"
    });
    ko.components.register("eventi-aziendaservizio", {
        require: "App/planner/eventi-aziendaservizio/eventi-aziendaservizio"
    });
    ko.components.register("servizio-eventi", {
        require: "App/planner/servizio-eventi/servizio-eventi"
    });

    //amministrazione
    ko.components.register("amministrazione-page-home", {
        require: "App/amministrazione/page-home/page-home"
    });
    ko.components.register("amministrazione-page-formazionedipendenti", {
        require: "App/amministrazione/page-formazionedipendenti/page-formazionedipendenti"
    });
    //amministrazione/listini
    ko.components.register("amministrazione-page-listini", {
        require: "App/amministrazione/page-listini/page-listini"
    });
    ko.components.register("amministrazione-page-listini-aziendali", {
        require: "App/amministrazione/page-listini-aziendali/page-listini-aziendali"
    });
    ko.components.register("amministrazione-page-listini-professionisti", {
        require: "App/amministrazione/page-listini-professionisti/page-listini-professionisti"
    });

    

    //fatturazione
    ko.components.register("fatturazione-page-home", {
        require: "App/fatturazione/page-home/page-home"
    });

    //Fatturazione/Attiva
    ko.components.register("fatturazione-page-fatturazioneattiva", {
        require: "App/fatturazione/page-fatturazioneattiva/page-fatturazioneattiva"
    });
    ko.components.register("fatturazione-attiva-page-fattura-edit", {
        require: "App/fatturazione/attiva/page-fattura-edit/page-fattura-edit"
    });
    ko.components.register("fatturazione-attiva-page-fattura-emetti", {
        require: "App/fatturazione/attiva/page-fattura-emetti/page-fattura-emetti"
    });
    ko.components.register("fatturazione-attiva-page-fatture-daemettere", {
        require: "App/fatturazione/attiva/page-fatture-daemettere/page-fatture-daemettere"
    });
    ko.components.register("fatturazione-attiva-page-fatture-emesse", {
        require: "App/fatturazione/attiva/page-fatture-emesse/page-fatture-emesse"
    });
    ko.components.register("fatturazione-attiva-page-flussicbi", {
        require: "App/fatturazione/attiva/page-flussicbi/page-flussicbi"
    });
    ko.components.register("fatturazione-attiva-page-prestdafatturare", {
        require: "App/fatturazione/attiva/page-prestdafatturare/page-prestdafatturare"
    });


    //Fatturazione/Passiva
    ko.components.register("fatturazione-passiva-page-fatturericevute", {
        require: "App/fatturazione/passiva/page-fatturericevute/page-fatturericevute"
    });


    ko.components.register("fatturazione-page-fatturazionepassiva", {
        require: "App/fatturazione/page-fatturazionepassiva/page-fatturazionepassiva"
    });
    ko.components.register("fatturazione-page-fatturazionereport", {
        require: "App/fatturazione/page-fatturazionereport/page-fatturazionereport"
    });
    ko.components.register("fatturazione-page-report-erogato", {
        require: "App/fatturazione/page-report-erogato/page-report-erogato"
    });
    ko.components.register("fatturazione-page-report-fatturato", {
        require: "App/fatturazione/page-report-fatturato/page-report-fatturato"
    });

    



    //protocolloGenerale
    ko.components.register("protocollogenerale-page-home", {
        require: "App/protocollogenerale/page-home/page-home"
    });

    //segreteria
    ko.components.register("segreteria-page-home", {
        require: "App/segreteria/page-home/page-home"
    });
    ko.components.register("segreteria-page-specialisti", {
        require: "App/segreteria/page-specialisti/page-specialisti"
    });
    ko.components.register("segreteria-page-specialita", {
        require: "App/segreteria/page-specialita/page-specialita"
    });
    //segreteria/specialita/id_specialita
    ko.components.register("segreteria-page-specialita-edit", {
        require: "App/segreteria/page-specialita-edit/page-specialita-edit"
    });
    //segreteria/aziende
    ko.components.register("segreteria-page-aziende", {
        require: "App/segreteria/aziende/page-aziende/page-aziende"
    });
    ko.components.register("segreteria-page-aziende-crea", {
        require: "App/segreteria/aziende/page-aziende-crea/page-aziende-crea"
    });
    //segreteria/aziende/id_azienda
    ko.components.register("segreteria-page-azienda", {
        require: "App/segreteria/aziende/page-azienda/page-azienda"
    });


    //servizi
    ko.components.register("servizi-page-home", {
        require: "App/servizi/page-home/page-home"
    });
    ko.components.register("servizi-page-commerciale", {
        require: "App/servizi/page-commerciale/page-commerciale"
    });
    ko.components.register("servizi-page-constecnica", {
        require: "App/servizi/page-constecnica/page-constecnica"
    });
    ko.components.register("servizi-page-formazione", {
        require: "App/servizi/page-formazione/page-formazione"
    });
    ko.components.register("servizi-page-sorvsani", {
        require: "App/servizi/page-sorvsani/page-sorvsani"
    });
    ko.components.register("servizi-page-prestazioni", {
        require: "App/servizi/page-prestazioni/page-prestazioni"
    });
    ko.components.register("servizi-page-clienti", {
        require: "App/servizi/page-clienti/page-clienti"
    });


    //Main
    function BodyModel() {
        var self = this;

        //tests
        self.test1 = ko.observable();

        //Account
        self.changepass = ko.observable();
        self.login = ko.observable();
        self.lostpassword = ko.observable();
        self.register = ko.observable();
        self.resetpass = ko.observable();
        self.user = ko.observable();

        //Amm
        self.ammUtenti = ko.observable();
        self.ammCreaUtente = ko.observable();

        //Planner
        self.planner = ko.observable();

        //Amministrazione
        self.amministrazione = ko.observable();
        self.formazioneDipendenti = ko.observable();
        self.listini = ko.observable();
        self.listiniAziendali = ko.observable();
        self.listiniProfessionisti = ko.observable();
        
        //Fatturazione
        self.fatturazione = ko.observable();
        self.fatturazioneReport = ko.observable();
        self.reportErogato = ko.observable();
        self.reportFatturato = ko.observable();

        //Fatturazione/Passiva
        self.fatturazionePassiva = ko.observable();
        self.fatturazionePassivaRicevute = ko.observable();

        //Fatturazione/Attiva
        self.fatturazioneAttiva = ko.observable();
        self.attivaFatturaEdit = ko.observable();
        self.attivaFatturaEmetti = ko.observable();
        self.attivaFatturaDaEmettere = ko.observable();
        self.attivaFatturaEmesse = ko.observable();
        self.attivaFatturaFlussiCBI = ko.observable();
        self.attivaFatturaPrestDaFatturare = ko.observable();

        //Protocollo Generale
        self.protocollogenerale = ko.observable();

        //Segreteria
        self.segreteria = ko.observable();
        self.specialisti = ko.observable();
        self.specialita = ko.observable();
        self.specialitaEdit = ko.observable();
        //Segreteria/aziende
        self.aziende = ko.observable();
        self.aziendeCrea = ko.observable();
        self.azienda = ko.observable();

        //Servizi
        self.servizi = ko.observable();
        self.commerciale = ko.observable();
        self.constecnica = ko.observable();
        self.formazione = ko.observable();
        self.sorvsani = ko.observable();
        self.prestazioni = ko.observable();
        self.clienti = ko.observable();
    };
    function MainModel() {
        var self = this;

        self.header = ko.observable();
        self.body = ko.observable();
    }

    var model = new MainModel();
    ko.applyBindings(model);

    sammy('#div_body', function () {
        var self = this;

        //nascondo icona iniziale loading
        $("#div_starterPage").remove();

        // Override this function so that Sammy doesn't mess with forms
        this._checkFormSubmission = function (form) {
            return (false);
        };

        //evito che sovrascriva link reali (si attiva solo con #)
        this.disable_push_state = true;


        //Tests
        this.get('#/tests/test1', function () {
            model.body(new BodyModel());
            model.body().test1(true);
        });

        //Account
        this.get('#/account/changepass', function () {
            model.body(new BodyModel());
            model.body().changepass(true);
        });
        this.get('#/account/login', function () {
            model.body(new BodyModel());
            model.body().login(true);
        });
        this.get('#/account/lostpassword', function () {
            model.body(new BodyModel());
            model.body().lostpassword(true);
        });
        this.get('#/account/register', function () {
            model.body(new BodyModel());
            model.body().register(true);
        });
        this.get('#/account/resetpass/:code/:userid', function () {
            model.body(new BodyModel());
            model.body().resetpass({ code: this.params.code, userid: this.params.userid });
        });
        this.get('#/account/user', function () {
            model.body(new BodyModel());
            model.body().user(true);
        });


        //Amministrazione
        this.get('#/amministrazione', function () {
            model.body(new BodyModel());
            model.body().amministrazione(true);
        });
        this.get('#/amministrazione/utenti', function () {
            model.body(new BodyModel());
            model.body().ammUtenti(true);
        });
        this.get('#/amministrazione/utenti/crea', function () {
            model.body(new BodyModel());
            model.body().ammCreaUtente(true);
        });
        this.get('#/amministrazione/formazionedipendenti', function () {
            model.body(new BodyModel());
            model.body().formazioneDipendenti(true);
        });
        //Amministrazione/listini
        this.get('#/amministrazione/listini', function () {
            model.body(new BodyModel());
            model.body().listini(true);
        });
        this.get('#/amministrazione/listini/aziendali', function () {
            model.body(new BodyModel());
            model.body().listiniAziendali(true);
        });
        this.get('#/amministrazione/listini/professionisti', function () {
            model.body(new BodyModel());
            model.body().listiniProfessionisti(true);
        });


        //Fatturazione
        this.get('#/fatturazione', function () {
            model.body(new BodyModel());
            model.body().fatturazione(true);
        });

        //Fatturazione/Attiva
        this.get('#/fatturazione/attiva', function () {
            model.body(new BodyModel());
            model.body().fatturazioneAttiva(true);
        });
        this.get('#/fatturazione/attiva/fatture/:id_fattura/edit', function () {
            model.body(new BodyModel());
            model.body().attivaFatturaEdit({ id_fattura: parseInt(this.params.id_fattura) });
        });
        this.get('#/fatturazione/attiva/fatture/:id_fattura/emetti', function () {
            model.body(new BodyModel());
            model.body().attivaFatturaEmetti({ id_fattura: parseInt(this.params.id_fattura) });
        });
        this.get('#/fatturazione/attiva/fatture/daemettere', function () {
            model.body(new BodyModel());
            model.body().attivaFatturaDaEmettere(true);
        });
        this.get('#/fatturazione/attiva/fatture/emesse', function () {
            model.body(new BodyModel());
            model.body().attivaFatturaEmesse(true);
        });
        this.get('#/fatturazione/attiva/flussicbi', function () {
            model.body(new BodyModel());
            model.body().attivaFatturaFlussiCBI(true);
        });
        this.get('#/fatturazione/attiva/prestdafatturare', function () {
            model.body(new BodyModel());
            model.body().attivaFatturaPrestDaFatturare(true);
        });


        this.get('#/fatturazione/passiva', function () {
            model.body(new BodyModel());
            model.body().fatturazionePassiva(true);
        });
        this.get('#/fatturazione/passiva/fatturericevute', function () {
            model.body(new BodyModel());
            model.body().fatturazionePassivaRicevute(true);
        });
        this.get('#/fatturazione/report', function () {
            model.body(new BodyModel());
            model.body().fatturazioneReport(true);
        });
        this.get('#/fatturazione/report/erogato', function () {
            model.body(new BodyModel());
            model.body().reportErogato(true);
        });
        this.get('#/fatturazione/report/fatturato', function () {
            model.body(new BodyModel());
            model.body().reportFatturato(true);
        });

        //Protocollo Generale
        //this.get('#/protocollo', function () {
        //    model.body(new BodyModel());
        //    model.body().protocollogenerale(true);
        //});

        //Segreteria
        this.get('#/segreteria', function () {
            model.body(new BodyModel());
            model.body().segreteria(true);
        });
        this.get('#/segreteria/specialisti', function () {
            model.body(new BodyModel());
            model.body().specialisti(true);
        });
        this.get('#/segreteria/specialita', function () {
            model.body(new BodyModel());
            model.body().specialita(true);
        });
        this.get('#/segreteria/specialita/:id_specialita', function () {
            model.body(new BodyModel());
            model.body().specialitaEdit({ id_specialita: parseInt(this.params.id_specialita) });
        });
        //segreteria/aziende
        this.get('#/segreteria/aziende', function () {
            model.body(new BodyModel());
            model.body().aziende({ id_servizio: 0});
        });
        this.get('#/segreteria/aziende/sorvsani', function () {
            model.body(new BodyModel());
            model.body().aziende({ id_servizio: 1 });
        });
        this.get('#/segreteria/aziende/formazione', function () {
            model.body(new BodyModel());
            model.body().aziende({ id_servizio: 2 });
        });
        this.get('#/segreteria/aziende/constecnica', function () {
            model.body(new BodyModel());
            model.body().aziende({ id_servizio: 3 });
        });
        this.get('#/segreteria/aziende/commerciale', function () {
            model.body(new BodyModel());
            model.body().aziende({ id_servizio: 4 });
        });

        this.get('#/segreteria/aziende/crea', function () {
            model.body(new BodyModel());
            model.body().aziendeCrea(true);
        });
        //segreteria/aziende/id_azienda
        this.get('#/segreteria/aziende/:id_azienda', function () {
            model.body(new BodyModel());
            model.body().azienda({ id_azienda: parseInt(this.params.id_azienda), id_servizio: 0 });
        });
        //segreteria/aziende/id_azienda/servizio
        this.get('#/segreteria/aziende/:id_azienda/sorvsani', function () {
            model.body(new BodyModel());
            model.body().azienda({ id_azienda: parseInt(this.params.id_azienda), id_servizio: 1 });
        });
        this.get('#/segreteria/aziende/:id_azienda/formazione', function () {
            model.body(new BodyModel());
            model.body().azienda({ id_azienda: parseInt(this.params.id_azienda), id_servizio: 2 });
        });
        this.get('#/segreteria/aziende/:id_azienda/constecnica', function () {
            model.body(new BodyModel());
            model.body().azienda({ id_azienda: parseInt(this.params.id_azienda), id_servizio: 3 });
        });
        this.get('#/segreteria/aziende/:id_azienda/commerciale', function () {
            model.body(new BodyModel());
            model.body().azienda({ id_azienda: parseInt(this.params.id_azienda), id_servizio: 4 });
        });

        //Servizi
        this.get('#/servizi', function () {
            model.body(new BodyModel());
            model.body().servizi(true);
        });
        this.get('#/servizi/commerciale', function () {
            model.body(new BodyModel());
            model.body().commerciale(true);
        });
        this.get('#/servizi/constecnica', function () {
            model.body(new BodyModel());
            model.body().constecnica(true);
        });
        this.get('#/servizi/formazione', function () {
            model.body(new BodyModel());
            model.body().formazione(true);
        });
        this.get('#/servizi/sorvsani', function () {
            model.body(new BodyModel());
            model.body().sorvsani(true);
        });
        //servizio => prestazioni
        this.get('#/servizi/sorvsani/prestazioni', function () {
            model.body(new BodyModel());
            model.body().prestazioni({ id_servizio: 1 });
        });
        this.get('#/servizi/formazione/prestazioni', function () {
            model.body(new BodyModel());
            model.body().prestazioni({ id_servizio: 2 });
        });
        this.get('#/servizi/constecnica/prestazioni', function () {
            model.body(new BodyModel());
            model.body().prestazioni({ id_servizio: 3 });
        });
        this.get('#/servizi/commerciale/prestazioni', function () {
            model.body(new BodyModel());
            model.body().prestazioni({ id_servizio: 4 });
        });
        //servizio => clienti
        this.get('#/servizi/sorvsani/clienti', function () {
            model.body(new BodyModel());
            model.body().clienti({ id_servizio: 1 });
        });
        this.get('#/servizi/formazione/clienti', function () {
            model.body(new BodyModel());
            model.body().clienti({ id_servizio: 2 });
        });
        this.get('#/servizi/constecnica/clienti', function () {
            model.body(new BodyModel());
            model.body().clienti({ id_servizio: 3 });
        });
        this.get('#/servizi/commerciale/clienti', function () {
            model.body(new BodyModel());
            model.body().clienti({ id_servizio: 4 });
        });


        //routing chiamato se falliscono tutti gli altri (da lasciare per ultimo!)
        this.get('', function () {
            model.body(new BodyModel());
            model.body().planner(true);
        });

    }).run();
});