using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.DAL;
using Nanobank.API.DAL.Models;
using Nanobank.API.Infrastructure.Identity;
using Newtonsoft.Json;

namespace Nanobank.API.Migrations
{
  internal sealed class Configuration : DbMigrationsConfiguration<ApplicationContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
      ContextKey = "Nanobank.API.DAL.ApplicationContext";
    }

    protected override void Seed(ApplicationContext context)
    {
      base.Seed(context);

      var userManager = ApplicationUserManager.Create(context);

      CreateCreditCards(context);
      CreateRoles(ApplicationRoleManager.Create(context));
      CreateUsers(userManager);
      CreateDeals(context, userManager);
    }

    private void CreateCreditCards(ApplicationContext context)
    {
      for (int i = 1; i <= 10; i++)
      {
        var card = new CreditCard
        {
          CardNumber = $"{i.ToString("D4")}{i.ToString("D4")}{i.ToString("D4")}{i.ToString("D4")}",
          Balance = i * 100
        };

        context.Cards.AddOrUpdate(c => c.CardNumber, card);
      }

      context.SaveChanges();
    }

    private void CreateDeals(ApplicationContext context, ApplicationUserManager manager)
    {
      var deals = new[]
      {
        new
        {
          Title = "Deal 1",
          StartAmount = 50m,
          DealDurationInMonth = (short)2,
          PercentRate = 18m,
          OwnerUserName = "admin",
          CreditorUserName = "user1",
          DealStartDate = DateTime.Today.Date.AddMonths(-1),
          RatingPositive = (short)4,
          RatingNegative = (short)0,
          DealClosedDate = default(DateTime?),
          IsClosed = false
        },
        new
        {
          Title = "Deal 2",
          StartAmount = 100m,
          DealDurationInMonth = (short)1,
          PercentRate = 25m,
          OwnerUserName = "user1",
          CreditorUserName = "user2",
          DealStartDate = DateTime.Today.Date.AddDays(-2),
          RatingPositive = (short)2,
          RatingNegative = (short)1,
          DealClosedDate = (DateTime?)DateTime.Today.AddDays(-5),
          IsClosed = true
        },
        new
        {
          Title = "Deal 3",
          StartAmount = 200m,
          DealDurationInMonth = (short)5,
          PercentRate = 10.5m,
          OwnerUserName = "user2",
          CreditorUserName = "admin",
          DealStartDate = DateTime.Today.Date.AddMonths(-3),
          RatingPositive = (short)1,
          RatingNegative = (short)3,
          DealClosedDate = default(DateTime?),
          IsClosed = false
        }
      };

      foreach (var deal in deals)
      {        
        if (context.Deals.FirstOrDefault(d => d.Title == deal.Title) != null)
        {
          continue;
        }

        var newDeal = new Deal
        {
          Id = Guid.NewGuid().ToString(),
          Title = deal.Title,
          StartAmount = deal.StartAmount,
          DealDurationInMonth = deal.DealDurationInMonth,
          PercentRate = deal.PercentRate,
          UserOwner = manager.FindByName(deal.OwnerUserName),
          UserCreditor = manager.FindByName(deal.CreditorUserName),
          DealStartDate = deal.DealStartDate,
          RatingPositive = deal.RatingPositive,
          RatingNegative = deal.RatingNegative,
          DealClosedDate = deal.DealClosedDate,
          IsClosed = deal.IsClosed
        };

        context.Deals.Add(newDeal);
      }

      try
      {
        context.SaveChanges();
      }
      catch(DbEntityValidationException ex)
      {
        throw;
      }
    }

    private void CreateRoles(ApplicationRoleManager manager)
    {
      foreach (var roleName in RoleTypes.AllRoles)
      {
        if (manager.FindByName(roleName) == null)
        {
          IdentityResult result = manager.Create(new IdentityRole(roleName));

          if (!result.Succeeded)
          {
            throw new InvalidOperationException($"Can not create role: '{roleName}'");
          }
        }
      }
    }

    private void CreateUsers(ApplicationUserManager manager)
    {
      var users = new[]
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
        new
        {
          UserName = "user2",
          Password = "user123",
          Roles = new [] { RoleTypes.User },
          Email = "user2@mail.ru",
          PhoneNumber = "375291234567",
          FirstName = "user2FN",
          LastName = "user2LN",
          Patronymic = "user2Patr",
          CardNumber = $"{3.ToString("D4")}{3.ToString("D4")}{3.ToString("D4")}{3.ToString("D4")}",
          CardOwnerFullName = "user2FN user2LN",
          CardDateOfExpire = DateTime.Now.AddMonths(1),
          CardCVV2Key = "123",
          PassportImage = new byte[0],
          ImageMimeType = "png"
        }
      };

      foreach (var user in users)
      {
        if (manager.FindByName(user.UserName) != null)
        {
          continue;
        }

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
          IsApproved = true
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
