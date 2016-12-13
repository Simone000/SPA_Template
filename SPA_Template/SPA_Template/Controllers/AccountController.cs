using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using SPA_TemplateHelpers;

namespace SPA_Template.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseAPIController
    {
        #region UserManager e SignInManager
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        [HttpGet]
        [Route("GetUserInfo")]
        [ResponseType(typeof(UserInfoModel))]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var email = await UserManager.GetEmailAsync(User.Identity.GetUserId());
            var model = new UserInfoModel()
            {
                Email = email,
                IsAdmin = User.IsInRole("Admin")
            };

            return Ok(model);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

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
            HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();

            //redirect alla homepage
            return Redirect(BaseUrl);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = Model.Email, Email = Model.Email };
            var result = await UserManager.CreateAsync(user, Model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Link("DefaultApi", new { controller = "Account/ConfirmEmail", userId = user.Id, code = code });
                await UserManager.SendEmailAsync(user.Id, "Conferma account", "Per confermare l'account, fare clic <a href=\"" + callbackUrl + "\">qui</a>");

                return Ok();
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

            var user = await UserManager.FindByNameAsync(Model.Email);
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



            //l'utente era già autenticato => aggiunge facebook come sistema di login (TEORICAMENTE NON SI VERIFICA MAI)
            if (User.Identity.IsAuthenticated)
            {
                var risAddLogin = await UserManager.AddLoginAsync(User.Identity.GetUserId(), logininfo.Login);
                if (risAddLogin.Succeeded)
                {
                    return Redirect(HttpContext.Current.Request.Url.Scheme
                                + @"://"
                                + HttpContext.Current.Request.Url.Authority
                                + returnUrl);
                }
                var errori = string.Join(Environment.NewLine, risAddLogin.Errors);
                Trace.TraceError("Account/ExternalLoginCallback, risAddLogin Failed: " + errori);
                return BadRequest(errori);
            }
            


            ////creo un nuovo utente nel sistema e redirect per fargli completare i campi obbligatori
            //var externalName = logininfo.ExternalIdentity.Name;
            //string extName = string.Empty;
            //string extCognome = string.Empty;
            //if (externalName.Split(' ').Length > 1)
            //{
            //    extName = externalName.Split(' ').First();
            //    extCognome = externalName.Split(' ').Skip(1).First();
            //}
            //else
            //{
            //    extName = externalName;
            //}
            //rendo univoco l'username
            //var countUtenti = await db.Users.LongCountAsync();
            //string uniqueUsername = logininfo.DefaultUserName + countUtenti;
            //
            //var user = new ApplicationUser()
            //{
            //    UserName = uniqueUsername,
            //    Email = logininfo.Email,
            //    Nome = extName,
            //    Cognome = extCognome,
            //    Telefono = string.Empty,
            //    Cellulare = string.Empty
            //};
            //
            //var creaUser = await userManager.CreateAsync(user);
            //if (creaUser.Succeeded != true)
            //{
            //    var errori = string.Join(Environment.NewLine, creaUser.Errors);
            //    Trace.TraceError("Account/ExternalLoginCallback, creaUser Failed: " + errori);
            //    return BadRequest(errori);
            //}
            ////aggiungo il login esterno a questo nuovo utente e lo redirecto per fargli compilare anche i campi obbligatori
            //var addLoginSuNuovo = await userManager.AddLoginAsync(user.Id, logininfo.Login);
            //if (addLoginSuNuovo.Succeeded != true)
            //{
            //    var errori = string.Join(Environment.NewLine, addLoginSuNuovo.Errors);
            //    Trace.TraceError("Account/ExternalLoginCallback, addLoginSuNuovo Failed: " + errori);
            //    return BadRequest(errori);
            //}
            ////login col nuovo user
            //await signinManager.SignInAsync(user, false, false);

            //redirect a compila campi obbligatori
            return Redirect(HttpContext.Current.Request.Url.Scheme
                            + @"://"
                            + HttpContext.Current.Request.Url.Authority
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
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
        public string Email { get; set; }

        //Ruoli
        public bool IsAdmin { get; set; }
    }

    #endregion
}
