; (function (define) {
    define(["jquery", "blockUI"], function ($) {

        var baseUrl = ''; //if frontend hosted in a different place (ex. cordova): http://website.net
        var busyGifPath = '/Content/Images/busy.gif';

        //funzioni richiamate da tutti gli altri
        function Get(divToBlock, success, error, doesReturnJson, url) {
            divToBlock.block({
                message: '<span><img src="' + busyGifPath + '" />Loading...</span>',
                css: { border: '1px solid #e2e2e2' }
            });

            //salvo l'ultimo accordion aperto per riaprirlo dopo il load
            var accordionsAperti = $('.in');

            var returnDataType = "json";
            if (doesReturnJson != true) {
                returnDataType = "text";
            }

            $.ajax({
                url: baseUrl + url,
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
                        return error(jqXHR, "Errore, prova a ricaricare la pagina o premi CTRL+R");
                    }
                }
            });
        };

        function Post(divToBlock, success, error, doesReturnJson, url, data) {
            divToBlock.block({
                message: '<span><img src="' + busyGifPath + '" />Loading...</span>',
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
                url: baseUrl + url,
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
                        var firstError = null;
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
                                        firstError = value;
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
                        if (msg_err == null) {
                            msg_err = errore["ExceptionMessage"];
                        }

                        //se è l'errore default di ModelState => riscrivo il msg scrivendo il primo model error che trovo
                        if (msg_err == "The request is invalid." && firstError != null) {
                            msg_err = firstError;
                        }

                        //mostro summary (è solo un msg di errore inviato con badrequest(string))
                        if (modelErrors == null) {
                            var div_summary = divToBlock.find('.validation-summary-errors');
                            if (div_summary != null) {
                                div_summary.find('li').text(msg_err);
                                div_summary.removeClass('hide')
                            }
                        }

                        return error(jqXHR, msg_err);
                    }
                    catch (e) {
                        return error(jqXHR, "Errore, prova a ricaricare la pagina o premi CTRL+R");
                    }
                }
            });
        };

        function Post_File(divToBlock, success, error, doesReturnJson, url, formData) {
            divToBlock.block({
                message: '<span><img src="' + busyGifPath + '" />Loading...</span>',
                css: { border: '1px solid #e2e2e2' }
            });

            //salvo l'ultimo accordion aperto per riaprirlo dopo il load
            var accordionsAperti = $('.in');

            var returnDataType = "json";
            if (doesReturnJson != true) {
                returnDataType = "text";
            }

            $.ajax({
                url: baseUrl + url,
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
                        return error(jqXHR, "Errore, prova a ricaricare la pagina o premi CTRL+R");
                    }
                }
            });
        };



        
//Account
function GetUserInfo(divToBlock, success, error) {
	Get(divToBlock, success, error, true, "/api/Account/GetUserInfo");
};
function Login(divToBlock, success, error, Email, Password, ReturnUrl) {
	Post(divToBlock, success, error, false, "/api/Account/Login", { Email: Email, Password: Password, ReturnUrl: ReturnUrl });
};
function LogOff(divToBlock, success, error) {
	Get(divToBlock, success, error, false, "/api/Account/LogOff");
};
function Register(divToBlock, success, error, Email, Password, ConfirmPassword) {
	Post(divToBlock, success, error, false, "/api/Account/Register", { Email: Email, Password: Password, ConfirmPassword: ConfirmPassword });
};
function ConfirmEmail(divToBlock, success, error, userId, code) {
	Get(divToBlock, success, error, false, "/api/Account/ConfirmEmail?userId=" + userId + "&code=" + code);
};
function ForgotPassword(divToBlock, success, error, Email) {
	Post(divToBlock, success, error, false, "/api/Account/ForgotPassword", { Email: Email });
};
function ResetPassword(divToBlock, success, error, UserID, Code, Password, ConfirmPassword) {
	Post(divToBlock, success, error, false, "/api/Account/ResetPassword", { UserID: UserID, Code: Code, Password: Password, ConfirmPassword: ConfirmPassword });
};
function ChangePassword(divToBlock, success, error, OldPassword, NewPassword, ConfirmPassword) {
	Post(divToBlock, success, error, false, "/api/Account/ChangePassword", { OldPassword: OldPassword, NewPassword: NewPassword, ConfirmPassword: ConfirmPassword });
};
function ExternalLogin(divToBlock, success, error, provider, returnUrl) {
	Get(divToBlock, success, error, false, "/api/Account/ExternalLogin?provider=" + provider + "&returnUrl=" + returnUrl);
};
function ExternalLoginCallback(divToBlock, success, error, returnUrl) {
	Get(divToBlock, success, error, false, "/api/Account/ExternalLoginCallback?returnUrl=" + returnUrl);
};

//Admin
function GetUtenti(divToBlock, success, error) {
	Get(divToBlock, success, error, true, "/api/Admin/GetUtenti");
};
function UpdateRuoloUtente(divToBlock, success, error, Username, Ruolo, NuovoStato) {
	Post(divToBlock, success, error, false, "/api/Admin/UpdateRuoloUtente", { Username: Username, Ruolo: Ruolo, NuovoStato: NuovoStato });
};
function CreaUtente(divToBlock, success, error, Email, Password, ConfirmPassword) {
	Post(divToBlock, success, error, false, "/api/Admin/CreaUtente", { Email: Email, Password: Password, ConfirmPassword: ConfirmPassword });
};

//Aziende
function GetAzienda(divToBlock, success, error, ID_Azienda, Filtro) {
	Get(divToBlock, success, error, false, "/api/Samples/Aziende/GetAzienda?ID_Azienda=" + ID_Azienda + "&Filtro=" + Filtro);
};
function GetAziende(divToBlock, success, error) {
	Get(divToBlock, success, error, true, "/api/Samples/Aziende/GetAziende");
};
function UpdateAzienda(divToBlock, success, error, Nome, Descrizione, TestDate, TestDate2) {
	Post(divToBlock, success, error, false, "/api/Samples/Aziende/UpdateAzienda", { Nome: Nome, Descrizione: Descrizione, TestDate: TestDate, TestDate2: TestDate2 });
};

//Exceptions
function TestExc1(divToBlock, success, error) {
	Get(divToBlock, success, error, false, "/api/Samples/Exceptions/TestExc1");
};



        /*
        self.method = function () {
            function success(data) {
                //self.aziende(ko.utils.arrayMap(data, function (item) {
                //    return new common.AziendaServizi(item);
                //}));

                //toastr["success"]("", "Ok!");
            };
            function error(jqXHR, desc) {
                //redirect on Unauthorized
                //if (jqXHR["status"] == 401) {
                //    window.location = "/#/account/login";
                //    return;
                //}

                //comment if not using validation-summary-errors
                toastr["error"](desc, "Errore!");
            };
            api.method($('#div'), success, error, params);
        };
        */

        return {
//Account
GetUserInfo: GetUserInfo, 
Login: Login, 
LogOff: LogOff, 
Register: Register, 
ConfirmEmail: ConfirmEmail, 
ForgotPassword: ForgotPassword, 
ResetPassword: ResetPassword, 
ChangePassword: ChangePassword, 
ExternalLogin: ExternalLogin, 
ExternalLoginCallback: ExternalLoginCallback,

//Admin
GetUtenti: GetUtenti, 
UpdateRuoloUtente: UpdateRuoloUtente, 
CreaUtente: CreaUtente,

//Aziende
GetAzienda: GetAzienda, 
GetAziende: GetAziende, 
UpdateAzienda: UpdateAzienda,

//Exceptions
TestExc1: TestExc1


};
    });
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
    if (typeof module !== 'undefined' && module.exports) { //Node
        module.exports = factory(require('jquery'), require('blockUI'));
    } else {
        window['api'] = factory(window['jQuery'], window['blockUI']);
    }
}));