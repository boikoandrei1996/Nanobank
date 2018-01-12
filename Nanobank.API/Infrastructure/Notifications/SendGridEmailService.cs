using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nanobank.API.Infrastructure.Notifications
{
  public class SendGridEmailService : IIdentityMessageService
  {
    private static readonly string ApiKeySettingName = "NanobankSendGridApiKey";

    public async Task SendAsync(IdentityMessage message)
    {
      if (!WebConfigurationManager.AppSettings.AllKeys.Contains(ApiKeySettingName))
      {
        throw new InvalidOperationException("Web.config doesn't have 'send grid api key'.");
      }

      var client = new SendGridClient(WebConfigurationManager.AppSettings[ApiKeySettingName]);

      var myMessage = new SendGridMessage
      {
        From = new EmailAddress("nanobank.admin@gmail.com", "Nanobank admin"),
        Subject = message.Subject,
        HtmlContent = message.Body
      };

      myMessage.AddTo(message.Destination);

      await client.SendEmailAsync(myMessage);
    }
  }
}