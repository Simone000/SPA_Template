; (function (define) {
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


        function UserInfo(UserInfo) {
            var self = this;

            self.email = UserInfo.Email;
            self.isAdmin = UserInfo.IsAdmin;
        };

        function UtenteRuoli(UtenteRuoli) {
            var self = this;

            self.username = UtenteRuoli.Username;
            self.isAdmin = UtenteRuoli.IsAdmin;
        };

        function GetAzienda(GetAzienda) {
            var self = this;

            self.id = GetAzienda.ID;
            self.nome = GetAzienda.Nome;
            self.reparti = GetAzienda.Reparti;
            self.citta = GetAzienda.Citta;
            self.testDate = new Data(GetAzienda.TestDate);
            self.testDate2 = new Data(GetAzienda.TestDate2);
        };



        return {
            Data: Data,
            Today: Today,

            GetAzienda: GetAzienda,
            UserInfo: UserInfo,
            UtenteRuoli: UtenteRuoli

        };
    });
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
    if (typeof module !== 'undefined' && module.exports) { //Node
        module.exports = factory(require('jquery'), require('ko'));
    } else {
        window['common'] = factory(window['jQuery'], window['ko']);
    }
}));