define(["jquery", "knockout", "sammy", "bootstrap"], function ($, ko, sammy) {

    //components
    ko.components.register("components-header", {
        require: "App/components/header/header"
    });

    //tests
    ko.components.register("tests-page-test1", {
        require: "App/tests/page-test1/page-test1"
    });

    //Main
    function BodyModel() {
        var self = this;

        //tests
        self.test1 = ko.observable();
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
        //this.before(function () {
        //    //Application Insights
        //    var pageName = window.location.hash;
        //    if (!window.location.hash)
        //        pageName = "Index";
        //    appInsights.trackPageView(pageName);
        //});

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

        
        //routing chiamato se falliscono tutti gli altri (da lasciare per ultimo!)
        this.get('', function () {
            model.body(new BodyModel());
            model.body().test1(true);
        });

    }).run();
});