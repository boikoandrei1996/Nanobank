using System;
using System.Threading.Tasks;
using Nanobank.API.DAL.Interface;
using Nanobank.API.Infrastructure.Identity;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Repository
{
  public class PhotoRepository : IPhotoRepository
  {
    private readonly ApplicationUserManager _userManager;

    public PhotoRepository(
      ApplicationUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<PhotoResponseViewModel> GetPhoto(string username)
    {
      var user = await _userManager.FindByNameAsync(username);
      if (user == null)
      {
        return null;
      }

      return new PhotoResponseViewModel
      {
        PassportImage = Convert.ToBase64String(user.UserInfo.PassportImage),
        ImageMimeType = user.UserInfo.ImageMimeType
      };
    }

    public void Dispose()
    {
      _userManager.Dispose();
    }
  }
}