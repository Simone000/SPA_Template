using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using SPA_TemplateHelpers;

namespace SPA_Template.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        #region SpaTemplate

        [NonAction]
        private static async Task<List<IdentityRole>> GetRolesAsync()
        {
            List<IdentityRole> ruoli;
            using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            {
                ruoli = await roleManager.Roles.ToListAsync().ConfigureAwait(false);
            }
            return ruoli;
        }

        [HttpGet]
        [Route("GetUtenti")]
        [ResponseType(typeof(List<UtenteModel>))]
        public async Task<IHttpActionResult> GetUtenti()
        {
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var utenti = await userManager.Users
                                              .Select(p => new UtenteModel()
                                              {
                                                  ID = p.Id,
                                                  Username = p.UserName,
                                                  Email = p.Email,
                                                  IsEnabled = p.IsEnabled,
                                                  EmailConfirmed = p.EmailConfirmed
                                              })
                                              .ToListAsync();
                return Ok(utenti);
            }
        }

        [HttpGet]
        [Route("GetUtentiRuoli")]
        [ResponseType(typeof(List<UtenteRuoliModel>))]
        public async Task<IHttpActionResult> GetUtentiRuoli()
        {
            //tutti i ruoli presenti a sistema
            var ruoli = await GetRolesAsync();
            string idRuoloAdmin = ruoli.Where(p => p.Name == "Admin").Select(p => p.Id).First();


            //lego utenti con ruoli
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var utenti = await userManager.Users
                                              .Include(p => p.Roles)
                                              .Select(p => new UtenteRuoliModel()
                                              {
                                                  Username = p.UserName,
                                                  IsAdmin = p.Roles.Any(q => q.RoleId == idRuoloAdmin)
                                              })
                                              .ToListAsync();
                return Ok(utenti);
            }
        }

        [HttpPost]
        [Route("UpdateRuoloUtente")]
        public async Task<IHttpActionResult> UpdateRuoloUtente(UpdateRuoloUtenteModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var utente = await userManager.FindByNameAsync(Model.Username);
                if (utente == null)
                    return NotFound();

                if (Model.NuovoStato)
                {
                    await userManager.AddToRoleAsync(utente.Id, Model.Ruolo);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(utente.Id, Model.Ruolo);
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("CreaUtente")]
        public async Task<IHttpActionResult> CreaUtente(CreaUtenteModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = Model.Email, Email = Model.Email, IsEnabled = true };
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var result = await userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    //va in eccezione GenerateEmailConfirmationTokenAsync
                    //string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Link("DefaultApi", new { controller = "Account/ConfirmEmail", userId = user.Id, code = code });
                    //await userManager.SendEmailAsync(user.Id, "Conferma account", "Per confermare l'account, fare clic <a href=\"" + callbackUrl + "\">qui</a>");

                    return Ok();
                }

                Trace.TraceError("Admin/CreaUtente, result != Succeded: " + string.Join("; ", result.Errors));
                return BadRequest(string.Join("; ", result.Errors));
            }
        }

        #endregion
    }


    #region Admin Models

    public class UtenteModel
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
    }

    public class UtenteRuoliModel
    {
        public string Username { get; set; }

        //lista di tutti i ruoli presenti a sistema
        public bool IsAdmin { get; set; }
    }

    public class UpdateRuoloUtenteModel
    {
        public string Username { get; set; }
        public string Ruolo { get; set; }
        public bool NuovoStato { get; set; }
    }

    public class CreaUtenteModel
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

    #endregion

}
