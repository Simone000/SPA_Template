﻿; (function (define) {
    define(["jquery", "blockUI"], function ($) {

        //funzioni richiamate da tutti gli altri
        function Get(divToBlock, success, error, doesReturnJson, url) {
            divToBlock.block({
                message: '<span><img src="/Content/Images/busy.gif" />Loading...</span>',
                css: { border: '1px solid #e2e2e2' }
            });

            //salvo l'ultimo accordion aperto per riaprirlo dopo il load
            var accordionsAperti = $('.in');

            var returnDataType = "json";
            if (doesReturnJson != true)
            {
                returnDataType = "text";
            }

            $.ajax({
                url: url,
                dataType: returnDataType,
                asyc: true,
                type: "get",
                success: function (data) {
                    divToBlock.unblock();

                    //riapro ultimi accordion aperti
                    $.each(accordionsAperti, function (index) {
                        var id_elem = accordionsAperti[index].id;
                        $("#" + id_elem).addClass("in");
                    });

                    success(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    divToBlock.unblock();

                    if (jqXHR.status == 401) {
                        return error(jqXHR, "Not Authorized");
                    }

                    try {
                        var errore = $.parseJSON(jqXHR.responseText);
                        var msg_err = errore["Message"];
                        if (msg_err == null) {
                            msg_err = errore["ExceptionMessage"];
                        }
                        return error(jqXHR, msg_err);
                    }
                    catch (e) {
                        return error(jqXHR, "Errore Generico");
                    }
                }
            });
        };

        function Post(divToBlock, success, error, doesReturnJson, url, data) {
            divToBlock.block({
                message: '<span><img src="/Content/Images/busy.gif" />Loading...</span>',
                css: { border: '1px solid #e2e2e2' }
            });

            //salvo l'ultimo accordion aperto per riaprirlo dopo il load
            var accordionsAperti = $('.in');

            //nascondo errori di validazione
            $('.has-error').removeClass('has-error');
            $('.error').addClass('hide');
            $('.validation-summary-errors').addClass('hide');

            var returnDataType = "json";
            if (doesReturnJson != true) {
                returnDataType = "text";
            }

            $.ajax({
                url: url,
                dataType: returnDataType,
                asyc: true,
                type: "post",
                data: data,
                success: function (data) {
                    divToBlock.unblock();

                    //riapro ultimi accordion aperti
                    $.each(accordionsAperti, function (index) {
                        var id_elem = accordionsAperti[index].id;
                        $("#" + id_elem).addClass("in");
                    });

                    success(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    divToBlock.unblock();

                    if (jqXHR.status == 401) {
                        return error(jqXHR, "Not Authorized");
                    }

                    try {
                        var errore = $.parseJSON(jqXHR.responseText);

                        //validazione ModelState
                        var modelErrors = errore["ModelState"];
                        if (modelErrors != null) {
                            var isFirst = true;
                            $.each(modelErrors, function (key, value) {

                                //cerco l'elemento by name (Model.Descrizione => cerco input con name Descrizione)
                                var keyFailedValidation = key.split(".", 2);
                                var inputFailedValidation = $('input[name=' + keyFailedValidation[1] + ']');
                                if (inputFailedValidation != null) {
                                    //focus solo per il primo elemento
                                    if (isFirst) {
                                        inputFailedValidation.focus();
                                        isFirst = false;
                                    }

                                    //form-group in error mode
                                    inputFailedValidation.closest('.form-group').removeClass('has-success').addClass('has-error');

                                    //label to show error
                                    var errorLabel = inputFailedValidation.closest('.form-group').find('.error');
                                    if (errorLabel != null) {
                                        errorLabel.text(value);
                                        errorLabel.removeClass('hide');
                                    }
                                }
                            });
                        }

                        var msg_err = errore["Message"];
                        if (msg_err == null)
                        {
                            msg_err = errore["ExceptionMessage"];
                        }

                        //mostro summary (è solo un msg di errore inviato con badrequest(string))
                        if (modelErrors == null)
                        {
                            var div_summary = divToBlock.find('.validation-summary-errors');
                            if (div_summary != null) {
                                div_summary.find('li').text(msg_err);
                                div_summary.removeClass('hide')
                            }
                        }

                        return error(jqXHR, msg_err);
                    }
                    catch (e) {
                        return error(jqXHR, "Errore Generico");
                    }
                }
            });
        };

        function Post_File(divToBlock, success, error, doesReturnJson, url, formData) {
            divToBlock.block({
                message: '<span><img src="/Content/Images/busy.gif" />Loading...</span>',
                css: { border: '1px solid #e2e2e2' }
            });

            //salvo l'ultimo accordion aperto per riaprirlo dopo il load
            var accordionsAperti = $('.in');

            var returnDataType = "json";
            if (doesReturnJson != true) {
                returnDataType = "text";
            }

            $.ajax({
                url: url,
                dataType: returnDataType,
                headers: { 'Cache-Control': 'no-cache' },
                asyc: true,
                processData: false,
                contentType: false,
                type: "post",
                data: formData,
                success: function (data) {
                    divToBlock.unblock();

                    //riapro ultimi accordion aperti
                    $.each(accordionsAperti, function (index) {
                        var id_elem = accordionsAperti[index].id;
                        $("#" + id_elem).addClass("in");
                    });

                    success(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    divToBlock.unblock();

                    if (jqXHR.status == 401) {
                        return error(jqXHR, "Not Authorized");
                    }

                    try {
                        var errore = $.parseJSON(jqXHR.responseText);
                        var msg_err = errore["Message"];
                        if (msg_err == null) {
                            msg_err = errore["ExceptionMessage"];
                        }
                        return error(jqXHR, msg_err);
                    }
                    catch (e) {
                        return error(jqXHR, "Errore Generico");
                    }
                }
            });
        };



{METHODS_CALL}


        /*
            self.method = function () {
                function success(data) {
                    self.aziende(ko.utils.arrayMap(data, function (item) {
                        return new common.AziendaServizi(item);
                    }));
                };
                function error(jqXHR, desc) {
                    toastr["error"](desc, "Errore!");
                };
                api.method($('#div'), success, error, params);
            };
         */

        return {
{METHODS_NAME}
        };
    });
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
    if (typeof module !== 'undefined' && module.exports) { //Node
        module.exports = factory(require('jquery'), require('blockUI'));
    } else {
        window['api'] = factory(window['jQuery'], window['blockUI']);
    }
}));