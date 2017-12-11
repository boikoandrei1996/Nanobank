using System.Data.Entity.Migrations;
using Nanobank.API.DAL;

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
      var dbInitializer = new DbInitializer();
      dbInitializer.InternalSeed(context);
      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.People.AddOrUpdate(
      //      p => p.FullName,
      //      new Person { FullName = "Andrew Peters" },
      //      new Person { FullName = "Brice Lambson" },
      //      new Person { FullName = "Rowan Miller" }
      //    );
      //
    }
  }
}
