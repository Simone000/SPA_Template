define(["knockout", "text!./page-signalr-sample.html", "toastr", "api", "common", "bootstrap"], function (ko, pageTemplate, toastr, api, common) {
    function pageModel(params) {
        var self = this;

        self.items = ko.observableArray();

        //Logging (browser console)
        //$.connection.testHub.logging = true;

        var testHub = $.connection.testHub;

        testHub.client.addItem = function (newitem) {
            var match = ko.utils.arrayFirst(self.items(), function (item) {
                return newitem === item;
            });
            //if it isn't already present => push the new item
            if (!match) {
                self.items.push(newitem);
            }
        };

        $.connection.hub.reconnected(function () {
            console.log("reconnected");

            //recheck
            if (self.isChatOpen() === false) {
                self.loadStatoChat();
            }
        });

        $.connection.hub.disconnected(function () {
            console.log("disconnected");
            setTimeout(function () {
                console.log("restart");
                $.connection.hub.start();
            }, 5000); // Restart connection after 5 seconds.
        });

        // Start the connection.
        $.connection.hub.start().done(function () {
            console.log("connection to hub done");
        });

        return self;
    }
    return { viewModel: pageModel, template: pageTemplate };
});