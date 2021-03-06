﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;
using SharedUtilsNoReference;
using System.Data.Entity;

namespace SPA_TemplateHelpers.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        #region SpaTemplate Methods

        [HttpGet]
        [Route("GetUserInfo")]
        [ResponseType(typeof(UserInfoModel))]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            string userId = User.Identity.GetUserId();
            var utente = await UserManager.FindByIdAsync(userId);
            if (utente == null)
            {
                Trace.TraceError("GetUserInfoByRange, user == null, userId: {0}", userId);
                return BadRequest("Utente non trovato");
            }

            var model = new UserInfoModel()
            {
                Username = utente.UserName,
                Email = utente.Email,
                IsAdmin = User.IsInRole("Admin")
            };

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel Model)
        {
            //todo: I should pass username instead of email in order to optionally support username that aren't emails
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            
            ApplicationUser user = await UserManager.FindAsync(Model.Email, Model.Password);
            if (user == null)
            {
                Trace.TraceWarning("Account/Login, User o Password non valide, UserManager.FindAsync({0}, {1}) == null",
                                   Model.Email, Model.Password);
                return BadRequest("User o Password non valide");
            }

            //Controllo che sia attivo
            if (!user.IsEnabled)
            {
                Trace.TraceWarning("Account/Login, Login ma account IsEnabled=false, Email: {0}", Model.Email);
                return BadRequest("L'account NON è attivo, per favore contatta il fornitore del servizio");
            }

            //controllo che l'email sia stata confermata
            if (!user.EmailConfirmed)
            {
                //todo: aggiungere opzione in spaSettings per invio automatico email conferma al login
                if(true) //se non è confermata, reinvio il link ti attivazione
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Link("DefaultApi", new { controller = "Account/ConfirmEmail", userId = user.Id, code = code });
                    string encodedCode = HttpUtility.UrlEncode(code);
                    var callbackUrl = BaseUrl + string.Format("/api/Account/ConfirmEmail?userId={0}&code={1}", user.Id, encodedCode);
                    await UserManager.SendEmailAsync(user.Id, "Conferma account", "Per confermare l'account, fare clic <a href=\"" + callbackUrl + "\">qui</a>");
                }

                //if I don't allow NotEmailConfirmed to be logged => I return badrequest instead of doing the login
                if(SpaSettings.IsEmailConfirmedRequired)
                    return BadRequest("La email non è stata confermata, ti abbiamo inviato una nuova email con il link di attivazione.");
            }

            var result = await SignInManager.PasswordSignInAsync(Model.Email, Model.Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.LockedOut:
                    Trace.TraceError("Account/Login, LockedOut, Utente: " + Model.Email);
                    return BadRequest("A seguito di ripetuti tentativi di accesso il suo account è stato bloccato");

                case SignInStatus.Success:
                    Trace.TraceInformation("Account/Login, Success: " + Model.Email);
                    return Ok();

                case SignInStatus.Failure:
                default:
                    Trace.TraceWarning("Account/Login, User o pass non valide, email: {0}, pass: {1}", Model.Email, Model.Password);
                    return BadRequest("User o Password non valide");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("LogOff")]
        public IHttpActionResult LogOff()
        {
            //HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();
            HttpContext.Current.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

            //redirect alla homepage
            return Redirect(BaseUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        [ResponseType(typeof(RegisterResultModel))]
        public async Task<IHttpActionResult> Register(RegisterViewModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!SpaSettings.IsRegistrationEnabled)
            {
                Trace.TraceError("Account/Register, l'utente: {0} ha provato a registrarsi ma IsRegistrationEnabled=false",
                                 Model.Email);
                return BadRequest("Spiacente, la registrazione utenti non è attivata per questo sito.");
            }

            var user = new ApplicationUser { UserName = Model.Email, Email = Model.Email, IsEnabled = true };
            var result = await UserManager.CreateAsync(user, Model.Password);
            if (result.Succeeded)
            {
                var ris = new RegisterResultModel()
                {
                    IsLogged = false,
                    IsConfirmEmailSent = false
                };

                if (SpaSettings.IsEmailConfirmedRequired)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    ris.IsLogged = true;
                }

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Link("DefaultApi", new { controller = "Account/ConfirmEmail", userId = user.Id, code = code });
                string encodedCode = HttpUtility.UrlEncode(code);
                var callbackUrl = BaseUrl + string.Format("/api/Account/ConfirmEmail?userId={0}&code={1}", user.Id, encodedCode);
                await UserManager.SendEmailAsync(user.Id, "Conferma account", "Per confermare l'account, fare clic <a href=\"" + callbackUrl + "\">qui</a>");
                ris.IsConfirmEmailSent = true;

                return Ok(ris);
            }

            Trace.TraceError("Account/Register, result != Succeded: " + string.Join("; ", result.Errors));
            return BadRequest(string.Join("; ", result.Errors));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("UserId or Code not valid");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                //Redirect to HomePage
                return Redirect(BaseUrl);
            }

            Trace.TraceError("Account/ConfirmEmail, result != succeded: " + string.Join("; ", result.Errors));
            return BadRequest("Codice non valido o scaduto");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByEmailAsync(Model.Email);
            if (user == null)
                return BadRequest("Utente non trovato");

            //invio mail con link
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string encodedCode = HttpUtility.UrlEncode(code);
            var callbackUrl = BaseUrl
                              + "/#/account/resetpass/"
                              + encodedCode
                              + "/"
                              + user.Id;

            await UserManager.SendEmailAsync(user.Id,
                                             "Reset Password",
                                             "Per reimpostare la password, fare clic <a href=\"" + callbackUrl + "\">qui</a>");

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            string decodedCode = HttpUtility.UrlDecode(Model.Code);
            var result = await UserManager.ResetPasswordAsync(Model.UserID, decodedCode, Model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }

            var errors = string.Join("; ", result.Errors);
            Trace.TraceError("Account/ResetPassword, result!succeded: " + errors);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), Model.OldPassword, Model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return Ok();
            }

            var errors = string.Join("; ", result.Errors);
            Trace.TraceError("Account/ChangePassword, result != succeded: " + errors);
            return BadRequest(errors);
        }

        #endregion

        #region External Auth

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalLogin")]
        public IHttpActionResult ExternalLogin(string provider, string returnUrl) //provider = Facebook
        {
            var callBack = BaseUrl
                           + "/api/Account/ExternalLoginCallback"
                           + @"?returnUrl=" + returnUrl;

            return new ChallengeResult(provider, this, callBack);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalLoginCallback")]
        public async Task<IHttpActionResult> ExternalLoginCallback(string returnUrl)
        {
            var logininfo = await AuthenticationManager_GetExternalLoginInfoAsync_WithExternalBearer();
            if (logininfo == null)
            {
                //Errore con auth facebook => lo porto alla pagina di registrazione utente
                Trace.TraceError("Account/ExternalLoginCallback, logininfo==null");
                return Redirect(BaseUrl
                                + @"/#/Account/regutente/");
            }

            //se l'utente ha già aggiunto questo external login => loggo direttamente
            var result = await SignInManager.ExternalSignInAsync(logininfo, isPersistent: false);
            if (result == SignInStatus.Success)
            {
                Trace.TraceInformation("Account/ExternalLoginCallback, Success: " + logininfo.Email);
                return Redirect(BaseUrl
                                + returnUrl);
            }

            //l'utente era già autenticato => aggiunge facebook come sistema di login (in theory never happens)
            if (User.Identity.IsAuthenticated)
            {
                var risAddLogin = await UserManager.AddLoginAsync(User.Identity.GetUserId(), logininfo.Login);
                if (risAddLogin.Succeeded)
                {
                    return Redirect(BaseUrl
                                    + returnUrl);
                }
                var errori = string.Join(Environment.NewLine, risAddLogin.Errors);
                Trace.TraceError("Account/ExternalLoginCallback, risAddLogin Failed: " + errori);
                return BadRequest(errori);
            }

            //creo un nuovo utente nel sistema e redirect per fargli completare i campi obbligatori
            var externalName = logininfo.ExternalIdentity.Name;
            string extName = string.Empty;
            string extCognome = string.Empty;
            if (externalName.Split(' ').Length > 1)
            {
                extName = externalName.Split(' ').First();
                extCognome = externalName.Split(' ').Skip(1).First();
            }
            else
            {
                extName = externalName;
            }

            //rendo univoco l'username
            logininfo.DefaultUserName = logininfo.DefaultUserName.TrimNull();
            string uniqueUsername = logininfo.DefaultUserName;

            //todo: test that it works!
            var countUtenti = await UserManager.Users
                                               .Where(p => p.UserName == uniqueUsername)
                                               .LongCountAsync();
            if (countUtenti > 0)
                uniqueUsername += countUtenti;

            //check it returned a valid email
            if (string.IsNullOrEmpty(logininfo.Email))
            {
                Trace.TraceWarning("Account/ExternalLoginCallback, logininfo.Email is null or empty");
            }

            var user = new ApplicationUser()
            {
                UserName = uniqueUsername,
                Email = logininfo.Email,
                //Nome = extName,
                //Cognome = extCognome,
                IsEnabled = true
            };

            var creaUser = await UserManager.CreateAsync(user);
            if (creaUser.Succeeded != true)
            {
                var errori = string.Join(Environment.NewLine, creaUser.Errors);
                Trace.TraceError("Account/ExternalLoginCallback, creaUser Failed: " + errori);
                return BadRequest(errori);
            }
            //aggiungo il login esterno a questo nuovo utente e lo redirecto per fargli compilare anche i campi obbligatori
            var addLoginSuNuovo = await UserManager.AddLoginAsync(user.Id, logininfo.Login);
            if (addLoginSuNuovo.Succeeded != true)
            {
                var errori = string.Join(Environment.NewLine, addLoginSuNuovo.Errors);
                Trace.TraceError("Account/ExternalLoginCallback, addLoginSuNuovo Failed: " + errori);
                return BadRequest(errori);
            }
            //login col nuovo user
            await SignInManager.SignInAsync(user, false, false);

            //redirect a compila campi obbligatori
            return Redirect(BaseUrl
                            + @"#/utente/anagrafica/");
        }

        [NonAction]
        private async Task<ExternalLoginInfo> AuthenticationManager_GetExternalLoginInfoAsync_WithExternalBearer()
        {
            ExternalLoginInfo loginInfo = null;

            //ClaimsIdentity ext = await HttpContext.Current.Request.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            var result = await HttpContext.Current.Request.GetOwinContext().Authentication.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (result != null && result.Identity != null)
            {
                var idClaim = result.Identity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (idClaim != null)
                {
                    loginInfo = new ExternalLoginInfo()
                    {
                        DefaultUserName = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", ""),
                        Login = new UserLoginInfo(idClaim.Issuer, idClaim.Value),
                        ExternalIdentity = result.Identity
                    };
                }
            }
            return loginInfo;
        }

        #endregion
    }

    public class ChallengeResult : IHttpActionResult
    {
        public string LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }
        public string ReturnUrl { get; set; }

        public ChallengeResult(string loginProvider, ApiController controller, string ReturnUrl)
        {
            this.LoginProvider = loginProvider;
            this.Request = controller.Request;
            this.ReturnUrl = ReturnUrl;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = ReturnUrl
            };

            HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(properties, LoginProvider);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }


    #region Account Models

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Posta elettronica")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma password")]
        [Compare("Password", ErrorMessage = "La password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterResultModel
    {
        public bool IsLogged { get; set; }
        public bool IsConfirmEmailSent { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string UserID { get; set; }
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "La nuova password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserInfoModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        //Roles
        public bool IsAdmin { get; set; }
    }

    #endregion

}
