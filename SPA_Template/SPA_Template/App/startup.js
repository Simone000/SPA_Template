define(["jquery", "knockout", "sammy", "bootstrap"], function ($, ko, sammy) {

    //components
    ko.components.register("components-header", {
        require: "App/components/header/header"
    });

    //tests
    ko.components.register("tests-page-test1", {
        require: "App/spa-tests/page-test1/page-test1"
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


    //aziende
    ko.components.register("aziende-page-aziende", {
        require: "App/aziende/page-aziende/page-aziende"
    });
    ko.components.register("aziende-page-editazienda", {
        require: "App/aziende/page-editazienda/page-editazienda"
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

        //Amministrazione
        self.ammUtenti = ko.observable();
        self.ammCreaUtente = ko.observable();

        //Aziende
        self.aziende = ko.observable();
        self.editAzienda = ko.observable();
    };
    function MainModel() {
        var self = this;

        self.header = ko.observable();
        self.body = ko.observable();
    };
    var model = new MainModel();
    ko.applyBindings(model);

    sammy('#div_body', function () {
        var self = this;

        //nascondo icona iniziale loading
        $("#div_starterPage").remove();

        //{ except: { path: '#!special' } }, 
        this.before(function () {
            //Application Insights
            var pageName = window.location.hash;
            if (!window.location.hash)
                pageName = "Index";
            appInsights.trackPageView(pageName);
        });

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


        //Amm
        this.get('#/amministrazione/utenti', function () {
            model.body(new BodyModel());
            model.body().ammUtenti(true);
        });
        this.get('#/amministrazione/utenti/crea', function () {
            model.body(new BodyModel());
            model.body().ammCreaUtente(true);
        });


        //edit azienda
        this.get('#/aziende/:id_azienda/edit', function () {
            model.body(new BodyModel());
            model.body().editAzienda({ id: this.params.id_azienda });
        });


        //routing chiamato se falliscono tutti gli altri (da lasciare per ultimo!)
        this.get('', function () {
            model.body(new BodyModel());
            model.body().aziende(true); //in questo caso la lista delle aziende è la pagina di default
        });

    }).run();
});