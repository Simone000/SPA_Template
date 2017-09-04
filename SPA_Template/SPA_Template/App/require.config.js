var require = {
    urlArgs: "v=6",
    baseUrl: "/",
    waitSeconds: 0,
    paths: {
        "bootstrap": "Scripts/bootstrap",
        "jquery": "Scripts/jquery-3.1.1",
        "blockUI": "Scripts/jquery.blockUI",
        "knockout": "Scripts/knockout-3.4.2",
        "knockoutgrids": "Scripts/knockout-grids",
        "text": "Scripts/text",
        "sammy": "Scripts/sammy-0.7.5",
        "datepicker": "Scripts/bootstrap-datepicker",
        "datepickerITA": "Scripts/locales/bootstrap-datepicker.it.min",
        "common": "App/common",
        "toastr": "Scripts/toastr",
        "api": "App/api"

        //signalr:
        //"signalr": "Scripts/jquery.signalR-2.2.1",
        //"hubs": "signalr/hubs?"
    },
    shim: {
        "bootstrap": {
            deps: ["jquery"]
        },
        "toastr": {
            deps: ["jquery"]
        },
        "blockUI": {
            deps: ["jquery"]
        },
        "datepicker": {
            deps: ["jquery", "bootstrap"]
        },
        "datepickerITA": {
            deps: ["jquery", "bootstrap", "datepicker"]
        },
        "sammy": {
            deps: ["jquery"]
        },
        "common": {
            deps: ["knockout", "jquery"]
        },
        "knockoutgrids": {
            deps: ["knockout", "jquery"]
        },
        "api": {
            deps: ["jquery"]
        }
        //signalr:
        //"signalr": {
        //    deps: ["jquery"]
        //},
        //"hubs": {
        //    deps: ["signalr"]
        //}
    }
}
