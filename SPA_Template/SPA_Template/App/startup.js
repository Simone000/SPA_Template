define(["jquery", "knockout", "sammy", "bootstrap"], function ($, ko, sammy) {

    //components
    ko.components.register("components-header", {
        require: "App/components/header/header"
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
    ko.components.register("amm-page-utentiruoli", {
        require: "App/amm/page-utentiruoli/page-utentiruoli"
    });
    ko.components.register("amm-page-creautente", {
        require: "App/amm/page-creautente/page-creautente"
    });

    //Errors
    ko.components.register("errors-page-generic", {
        require: "App/errors/page-generic/page-generic"
    });
    ko.components.register("errors-page-internal", {
        require: "App/errors/page-internal/page-internal"
    });
    ko.components.register("errors-page-notfound", {
        require: "App/errors/page-notfound/page-notfound"
    });

    //Home
    ko.components.register("page-home", {
        require: "App/page-home/page-home"
    });

    //Faq
    ko.components.register("faq-page-faq", {
        require: "App/faq/page-faq/page-faq"
    });

    //aziende
    ko.components.register("aziende-page-aziende", {
        require: "App/aziende/page-aziende/page-aziende"
    });
    ko.components.register("aziende-page-aziende2", {
        require: "App/aziende/page-aziende2/page-aziende2"
    });
    ko.components.register("aziende-page-editazienda", {
        require: "App/aziende/page-editazienda/page-editazienda"
    });

    //samples
    ko.components.register("samples-page-client-filters", {
        require: "App/samples/page-client-filters/page-client-filters"
    });
    ko.components.register("samples-page-client-filters-ws", {
        require: "App/samples/page-client-filters-ws/page-client-filters-ws"
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
        self.ammUtentiRuoli = ko.observable();
        self.ammCreaUtente = ko.observable();

        //Errors
        self.errorsGeneric = ko.observable();
        self.errorsInternal = ko.observable();
        self.errorsNotFound = ko.observable();

        //home
        self.home = ko.observable();


        //Aziende
        self.aziende = ko.observable();
        self.aziende2 = ko.observable();
        self.editAzienda = ko.observable();

        //Samples
        self.clientFilters = ko.observable();
        self.clientFiltersWs = ko.observable();
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
            //Application Insights with SammyJs
            var pageName = this.path;
            pageName = pageName.replace("#", "");
            if (!this.path || pageName == "/") {
                pageName = "Index";
            }
            else {
                //Replace params value with it's name
                //Object.keys not compatible with IE < 9
                var params = this.params;
                Object.keys(params).forEach(function (key) {
                    var val = params[key];
                    pageName = pageName.replace(val, key);
                });
            }

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
        this.get('#/amministrazione/utenti/ruoli', function () {
            model.body(new BodyModel());
            model.body().ammUtentiRuoli(true);
        });
        this.get('#/amministrazione/utenti/crea', function () {
            model.body(new BodyModel());
            model.body().ammCreaUtente(true);
        });

        //Errors
        this.get('#/errors/generic', function () {
            model.body(new BodyModel());
            model.body().errorsGeneric(true);
        });
        this.get('#/errors/internal', function () {
            model.body(new BodyModel());
            model.body().errorsInternal(true);
        });
        this.get('#/errors/notfound', function () {
            model.body(new BodyModel());
            model.body().errorsNotFound(true);
        });


        this.get('#/aziende', function () {
            model.body(new BodyModel());
            model.body().aziende(true);
        });

        this.get('#/aziende2', function () {
            model.body(new BodyModel());
            model.body().aziende2(true);
        });


        //edit azienda
        this.get('#/aziende/:id_azienda/edit', function () {
            model.body(new BodyModel());
            model.body().editAzienda({ id: this.params.id_azienda });
        });

        //Samples
        this.get('#/samples/client/filters', function () {
            model.body(new BodyModel());
            model.body().clientFilters(true);
        });
        this.get('#/samples/client/filtersws', function () {
            model.body(new BodyModel());
            model.body().clientFiltersWs(true);
        });


        //routing chiamato se falliscono tutti gli altri (da lasciare per ultimo!)
        this.get('', function () {
            model.body(new BodyModel());
            model.body().home(true); //setting default page
        });

    }).run();
});