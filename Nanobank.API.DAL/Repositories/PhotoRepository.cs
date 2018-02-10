using System;
using System.Threading.Tasks;
using Nanobank.API.DAL.Managers;
using Nanobank.API.DAL.Models.ResponseViewModels;
using Nanobank.API.DAL.Repositories.Interfaces;

namespace Nanobank.API.DAL.Repositories
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