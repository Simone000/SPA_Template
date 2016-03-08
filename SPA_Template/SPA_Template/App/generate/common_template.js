; (function (define) {
    define(["jquery", "knockout", "knockoutgrids"], function ($, ko, knockoutgrids) {

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
                if (self.datajs() == null || self.datajs() <= 0)
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


{METHODS_CALL}

        return {
            Data: Data,
            Today: Today,

{METHODS_NAME}

        };
    });
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
    if (typeof module !== 'undefined' && module.exports) { //Node
        module.exports = factory(require('jquery'), require('ko'), require('knockoutgrids'));
    } else {
        window['common'] = factory(window['jQuery'], window['ko'], window['knockoutgrids']);
    }
}));