using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nanobank.API.Models;

namespace Nanobank.API.DAL.Interface
{
  public interface IPhotoRepository : IDisposable
  {
    Task<PhotoResponseViewModel> GetPhoto(string username);
  }
}
