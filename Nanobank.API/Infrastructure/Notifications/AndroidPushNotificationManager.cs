using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nanobank.API.Infrastructure.Notifications
{
  public class AndroidPushNotificationManager : IPushNotificationManager
  {
    private static readonly string Url = "https://fcm.googleapis.com/fcm/send";
    private static readonly string ServerKey = "AAAA6ATymIM:APA91bEkfzWhzJeVaKwmh2-9JEZ6obEp2v2Zjt2W4gj1_jLW-Q8_Ep3bsf5Ld24nTYlVGfqJNte0_g02cvkvoxeRDJrTMDw11fImrzq7szknAREOH7eFk68KZDHgVPmIzDYl22uRtnq0";

    public async Task SendAsync(string androidDeviceToken, string title, string message)
    {
      string postData = await GetDataAsync(androidDeviceToken, title, message);
      byte[] byteArray = Encoding.UTF8.GetBytes(postData);

      WebRequest request = WebRequest.Create(Url);

      request.Method = "post";
      request.ContentType = "application/json";
      request.Headers.Add($"Authorization: key={ServerKey}");
      request.ContentLength = byteArray.Length;

      using (Stream dataStream = await request.GetRequestStreamAsync())
      {
        await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
      }
    }

    private Task<string> GetDataAsync(string token, string title, string body)
    {
      var jsonData = new
      {
        to = token,
        data = new 
        {
          title = title,
          body = body
        }
      };

      return Task.Factory.StartNew(() => JsonConvert.SerializeObject(jsonData));
    }
  }
}