using System;
using System.Threading.Tasks;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repositories.Interfaces
{
  public interface IPhotoRepository : IDisposable
  {
    Task<PhotoResponseViewModel> GetPhoto(string username);
  }
}
