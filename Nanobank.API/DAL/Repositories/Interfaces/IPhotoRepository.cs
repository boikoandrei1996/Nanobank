using System;
using System.Threading.Tasks;
using Nanobank.API.DAL.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repositories.Interfaces
{
  public interface IPhotoRepository : IDisposable
  {
    Task<PhotoResponseViewModel> GetPhoto(string username);
  }
}
