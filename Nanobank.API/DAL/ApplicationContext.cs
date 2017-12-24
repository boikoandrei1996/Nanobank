using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nanobank.API.DAL.Models;

namespace Nanobank.API.DAL
{
  public class ApplicationContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationContext() : base("ApplicationContext", throwIfV1Schema: false)
    {
    }

    public virtual DbSet<CreditCard> Cards { get; set; }
    public virtual DbSet<Deal> Deals { get; set; }
    public virtual DbSet<Complain> Complains { get; set; }
  }
}