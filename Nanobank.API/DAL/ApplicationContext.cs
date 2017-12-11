using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
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

    public ApplicationContext() : base("ApplicationContext", throwIfV1Schema: false)
    {
    }

    public static ApplicationContext Create()
    {
      return new ApplicationContext();
    }

    public virtual DbSet<CreditCard> Cards { get; set; }
  }

  public class DbInitializer :
    DropCreateDatabaseAlways<ApplicationContext>
    //CreateDatabaseIfNotExists<ApplicationContext>
  {
    internal void InternalSeed(ApplicationContext context)
    {
      CreateCreditCards(context);
      CreateRoles(ApplicationRoleManager.Create(context));
      CreateUsers(ApplicationUserManager.Create(context));
    }

    protected override void Seed(ApplicationContext context)
    {
      base.Seed(context);

      CreateCreditCards(context);
      CreateRoles(ApplicationRoleManager.Create(context));
      CreateUsers(ApplicationUserManager.Create(context));
    }

    private void CreateCreditCards(ApplicationContext context)
    {
      var cards = new List<CreditCard>();
      for(int i = 1; i <= 10; i++)
      {
        cards.Add(new CreditCard
        {
          CardNumber = $"{i.ToString("D4")}{i.ToString("D4")}{i.ToString("D4")}{i.ToString("D4")}",
          Balance = i * 100
        });
      }

      context.Cards.AddRange(cards);
      context.SaveChanges();
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
          Password = "admin123",
          Roles = new [] { RoleTypes.Admin, RoleTypes.User },
          Email = "admin@mail.ru",
          PhoneNumber = "375291234567",
          FirstName = "adminFN",
          LastName = "adminLN",
          Patronymic = "adminPatr",
          CardNumber = $"{1.ToString("D4")}{1.ToString("D4")}{1.ToString("D4")}{1.ToString("D4")}",
          CardOwnerFullName = "adminFN adminLN",
          CardDateOfExpire = DateTime.Now.AddMonths(1),
          CardCVV2Key = "123",
          PassportImage = new byte[0],
          ImageMimeType = "png"
        },
        new
        {
          UserName = "user1",
          Password = "user123",
          Roles = new [] { RoleTypes.User },
          Email = "user1@mail.ru",
          PhoneNumber = "375291234567",
          FirstName = "user1FN",
          LastName = "user1LN",
          Patronymic = "user1Patr",
          CardNumber = $"{2.ToString("D4")}{2.ToString("D4")}{2.ToString("D4")}{2.ToString("D4")}",
          CardOwnerFullName = "user1FN user1LN",
          CardDateOfExpire = DateTime.Now.AddMonths(1),
          CardCVV2Key = "123",
          PassportImage = new byte[0],
          ImageMimeType = "png"
        },
      };

      foreach (var user in users)
      {
        var newUser = new ApplicationUser 
        {
          UserName = user.UserName,
          Email = user.Email,
          PhoneNumber = user.PhoneNumber,
          UserInfo = new UserInfo
          {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            CardNumber = user.CardNumber,
            CardOwnerFullName = user.CardOwnerFullName,
            CardDateOfExpire = user.CardDateOfExpire,
            CardCVV2Key = user.CardCVV2Key,
            PassportImage = user.PassportImage,
            ImageMimeType = user.ImageMimeType
          },
          IsApproved = user.UserName == "admin" ? true : false
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