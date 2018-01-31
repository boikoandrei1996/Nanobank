using System.Data.Entity;
using System.Threading.Tasks;
using Nanobank.API.DAL.Interface;
using Nanobank.API.DAL.Models;
using Nanobank.API.Models.ResponseViewModels;

namespace Nanobank.API.DAL.Repository
{
  public class ReportRepository : IReportRepository
  {
    private readonly ApplicationContext _context;

    public ReportRepository(ApplicationContext context)
    {
      _context = context;
    }

    public async Task<ReportResponseViewModel> GetReport(string complainId)
    {
      var complain = await _context.Complains.FirstOrDefaultAsync(c => c.Id == complainId);
      if (complain == null)
      {
        return null;
      }

      return MapReport(complain);
    }

    public void Dispose()
    {
      _context.Dispose();
    }

    private ReportResponseViewModel MapReport(Complain complain)
    {
      return new ReportResponseViewModel
      {
        ComplainId = complain.Id,
        ComplainText = complain.Text,
        DealId = complain.DealId,
        DateOfCreating = complain.DateOfCreating,
        DealOwnerUsername = complain.Deal.UserOwner.UserName,
        DealCreditorUsername = complain.Deal.UserCreditor.UserName
      };
    }
  }
}