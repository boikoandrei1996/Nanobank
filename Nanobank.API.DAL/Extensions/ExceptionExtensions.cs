using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Nanobank.API.DAL.Extensions
{
  public static class ExceptionExtensions
  {
    public static string[] GetValidationErrors(this DbEntityValidationException ex)
    {
      var validationErrors = new List<string>();

      foreach (var error in ex.EntityValidationErrors)
      {
        validationErrors.AddRange(error.ValidationErrors.Select(err => $"[{err.PropertyName}]: '{err.ErrorMessage}'"));
      }

      return validationErrors.ToArray();
    }
  }
}