using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.DAL.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using Newtonsoft.Json;

namespace Nanobank.API.DAL
{
  public class ApplicationContext : IdentityDbContext<ApplicationUser>
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

      CreateRoles(ApplicationRoleManager.Create(context));
      CreateUsers(ApplicationUserManager.Create(context));
    }

    private void CreateRoles(ApplicationRoleManager manager)
    {
      foreach (var roleName in RoleTypes.AllRoles)
      {
        IdentityResult result = manager.Create(new IdentityRole(roleName));
        if (!result.Succeeded)
        {
          throw new InvalidOperationException($"Can not create role: '{roleName}'");
        }
      }
    }

    private void CreateUsers(ApplicationUserManager manager)
    {
      var users = new []
      {
        new
        {
          UserName = "admin",
          Email = "admin@mail.ru",
          Password = "admin123",
          Roles = new [] { RoleTypes.Admin, RoleTypes.User }
        },
        new
        {
          UserName = "user1",
          Email = "user1@mail.ru",
          Password = "user123",
          Roles = new [] { RoleTypes.User }
        },
      };

      foreach (var user in users)
      {
        var newUser = new ApplicationUser 
        {
          UserName = user.UserName,
          Email = user.Email
        };

        IdentityResult result = manager.Create(newUser, user.Password);
        if (!result.Succeeded)
        {
          throw new InvalidOperationException($"Can not create user: {user.UserName}");
        }

        result = manager.AddToRoles(newUser.Id, user.Roles);
        if (!result.Succeeded)
        {
          throw new InvalidOperationException($"Can not add roles '{string.Join("; ", user.Roles)}' for user '{user.UserName}'");
        }
      }
    }

    private IList<T> LoadJson<T>(string fileName)
    {
      string basePath = HttpContext.Current.Server.MapPath("~/App_Data/InitDataFiles");

      try
      {
        string path = Path.Combine(basePath, fileName);
        using (var stream = new StreamReader(path))
        {
          string json = stream.ReadToEnd();

          return JsonConvert.DeserializeObject<List<T>>(json);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"LoadJson exception: type: '{ex.GetType()}' message: '{ex.Message}'");
        return new List<T>();
      }
    }
  }
}