using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nanobank.API.Infrastructure.Formatters
{
  public class PdfMediaTypeFormatter<T> : MediaTypeFormatter
    where T : class
  {
    private static readonly Type SupportedType = typeof(T);

    public PdfMediaTypeFormatter()
    {
      SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/pdf"));
      MediaTypeMappings.Add(new UriPathExtensionMapping("pdf", "application/pdf"));
    }

    public async override Task WriteToStreamAsync(
      Type type, 
      object value, 
      Stream writeStream, 
      HttpContent content, 
      TransportContext transportContext)
    {
      var doc = PdfGenerator.CreatePdf(value == null ? null : value as T);

      using (var ms = new MemoryStream())
      {
        doc.Save(ms, false);
        var bytes = ms.ToArray();
        await writeStream.WriteAsync(bytes, 0, bytes.Length);
      }
    }

    public override bool CanReadType(Type type)
    {
      return SupportedType == type;
    }

    public override bool CanWriteType(Type type)
    {
      return SupportedType == type;
    }
  }
}