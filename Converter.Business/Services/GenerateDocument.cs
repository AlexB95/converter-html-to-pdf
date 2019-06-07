using Converter.Business.Contracts;
using Converter.Business.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebSupergoo.ABCpdf9;

namespace Converter.Business.Services
{
    class GenerateDocument : IGenerateDocument
    {
        public Doc CreatePDFFromString(string htmlData)
        {
            //Create the pdf
            var pdfDoc = new Doc();
            pdfDoc.Rect.Inset(30, 50);
            pdfDoc.Color.String = "255 255 255";

            var pdfID = pdfDoc.AddImageHtml(htmlData, true, 0, true);
            //We now chain subsequent pages together. We stop when we reach a page which wasn't truncated
            while (true)
            {
                pdfDoc.FrameRect();
                if (pdfDoc.GetInfo(pdfID, "Truncated") != "1")
                    break;
                pdfDoc.Page = pdfDoc.AddPage();
                pdfID = pdfDoc.AddImageToChain(pdfID);
            }

            //After adding the pages we can flatten them. We can't do this until after the pages have been
            //added because flattening will invalidate our previous ID and break the chain.
            for (var i = 1; i <= pdfDoc.PageCount; i++)
            {
                pdfDoc.PageNumber = i;
                pdfDoc.Flatten();
            }
            return pdfDoc;
        }

        public MemoryStream GetDocumentsStream(FileDocumentDTO doc, bool mergeDocs)
        {
            // List of final docs for non-preview
            var mergedPdf = new List<Doc>();

            // Single doc for preview
            var previewPdf = new Doc();

            #region " SINGLE HTML FILE "

            if (doc.DocumentType == (int)TemplatePdfType.PdfAttachment)
            {
                var htmlDoc = GetMergePdf(doc.ReplacedHtml);
                if (mergeDocs)
                {
                    previewPdf.Append(htmlDoc);
                }
                else
                {
                    mergedPdf.Add(htmlDoc);
                }

            }

            #endregion

            var responseStream = new MemoryStream();
            if (mergeDocs)
            {
                mergedPdf.Add(previewPdf);
            }
            foreach (var d in mergedPdf)
            {
                using (var ms = new MemoryStream())
                {
                    d.Save(ms);
                    responseStream = new MemoryStream(ms.ToArray());
                }
            }

            return responseStream;
        }

        public Doc GetMergePdf(string html)
        {
            var arlFormsHTML = html.Split(new List<string> { "@jump@" }.ToArray(),
                StringSplitOptions.RemoveEmptyEntries);
            var pdfAllDocs = new Doc();
            foreach (var item in arlFormsHTML)
            {
                var pdfAppend = new Doc();
                pdfAppend = CreatePDFFromString(item);
                pdfAllDocs.Append(pdfAppend);
                pdfAppend.Dispose();
            }
            return pdfAllDocs;
        }
    }
    public enum TemplatePdfType
    {
        PdfAttachment = 1,
        ClientDocument = 2,
        Iterable = 3,
        File = 4
    }
}
