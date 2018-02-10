namespace Nanobank.API.DAL.EFModels
{
  public static class RoleTypes
  {
    public const string Admin = "Admin";
    public const string User = "User";

    public static string[] AllRoles
    {
      get
      {
        return new[]
        {
          Admin,
          User
        };
      }
    }
  }
}