using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nanobank.API.Infrastructure.Notifications
{
  public interface IPushNotificationManager
  {
    Task SendAsync(string androidDeviceToken, string title, string message);
  }
}
