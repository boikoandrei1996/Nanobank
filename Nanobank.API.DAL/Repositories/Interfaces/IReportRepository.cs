using System;
using System.Threading.Tasks;
using Nanobank.API.DAL.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repositories.Interfaces
{
  public interface IReportRepository : IDisposable
  {
    Task<ReportResponseViewModel> GetReport(string complainId);
  }
}
