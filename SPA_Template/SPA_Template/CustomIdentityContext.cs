using Microsoft.AspNet.Identity.EntityFramework;
using SPA_Template.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Template
{
    public partial class CustomIdentityContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        public CustomIdentityContext()
            : base("name=CustomIdentityContext")
        {
        }

        public static CustomIdentityContext Create()
        {
            return new CustomIdentityContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
