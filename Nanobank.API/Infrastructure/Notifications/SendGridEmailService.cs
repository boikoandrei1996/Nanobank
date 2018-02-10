using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using Nanobank.API.DAL.Loggers;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nanobank.API.Infrastructure.Notifications
{
  public class SendGridEmailService : IIdentityMessageService
  {
    private static readonly string ApiKeySettingName = "NanobankSendGridApiKey";
    private readonly ILogger _logger;

    public SendGridEmailService(ILogger logger)
    {
      _logger = logger;
    }

    public async Task SendAsync(IdentityMessage message)
    {
      if (!WebConfigurationManager.AppSettings.AllKeys.Contains(ApiKeySettingName))
      {
        string errorMessage = $"Web.config doesn't have '{ApiKeySettingName}'.";
        _logger.Fatal(errorMessage);
        throw new InvalidOperationException(errorMessage);
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