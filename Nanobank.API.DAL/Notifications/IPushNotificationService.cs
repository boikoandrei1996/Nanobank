using System.Threading.Tasks;

namespace Nanobank.API.DAL.Notifications
{
  public interface IPushNotificationService
  {
    Task SendAsync(string androidDeviceToken, string title, string message);
  }
}
