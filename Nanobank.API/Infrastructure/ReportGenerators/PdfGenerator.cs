using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Nanobank.API.Models.ResponseViewModels;
using PdfSharp.Pdf;

namespace Nanobank.API.Infrastructure.ReportGenerators
{
  public class PdfGenerator
  {
    public static PdfDocument CreatePdf<T>(T model)
    {
      if (model != null && model is ReportResponseViewModel)
      {
        var reportModel = model as ReportResponseViewModel;
        var document = new Document();

        WriteInDocument(document, reportModel);

        return RenderDocument(document);
      }

      return RenderDocument(null);
    }

    private static void WriteInDocument(Document document, ReportResponseViewModel reportModel)
    {
      var sec = document.Sections.AddSection();
      sec.AddParagraph("My test complain report.");
      sec.AddParagraph($"Complain Id: {reportModel.ComplainId}");
      sec.AddParagraph($"User owner name: {reportModel.DealOwnerUsername}");
      sec.AddParagraph($"User creditor name: {reportModel.DealCreditorUsername}");
      sec.AddParagraph($"Date of creating: {reportModel.DateOfCreating.ToShortDateString()}");
      sec.AddParagraph($"Complain: {reportModel.ComplainText}");
    }

    private static PdfDocument RenderDocument(Document document)
    {
      var rend = new PdfDocumentRenderer { Document = document };
      rend.RenderDocument();
      return rend.PdfDocument;
    }
  }
}