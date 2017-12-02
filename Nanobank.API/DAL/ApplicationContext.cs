using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Nanobank.API.DAL
{
  public class ApplicationContext : IdentityDbContext<IdentityUser>
  {
    static ApplicationContext()
    {
      Database.SetInitializer(new DbInitializer());
    }

    public ApplicationContext() : base("ApplicationContext")
    {
    }
  }

  public class DbInitializer :
    //DropCreateDatabaseAlways<ApplicationContext>
    CreateDatabaseIfNotExists<ApplicationContext>
  {
    protected override void Seed(ApplicationContext context)
    {
      base.Seed(context);
    }
  }
}