using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Converter.Business.DTO
{
    public class FileDocumentDTO
    {
        public int IdDocument { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string HtmlContent { get; set; }
        public string FileName { get; set; }
        public string GroupFileName { get; set; }
        public string GroupFileAlias { get; set; }
        public int DocumentType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsClientViewable { get; set; }
        public string BlobFileUrl { get; set; }
        public int CountFormsUsing { get; set; }
        public bool Watermark { get; set; }
        public int Mode { get; set; }
        public string AllowedInsurances { get; set; }
        public List<int> AllowedInsurancesList { get; set; }

        public List<int> InsurancesUsing { get; set; }

        // Get from assignation
        public string Condition { get; set; }

        // Building
        public bool StandalonePdf { get; set; }
        public int OrderInsidePdf { get; set; }
        /// <summary>
        /// Single HTML for unique documents
        /// </summary>
        public string ReplacedHtml { get; set; }
        /// <summary>
        /// List of a HTML Content. Each string is a different document.
        /// </summary>
        public List<string> IterableReplacedHtml { get; set; }
        public List<int> InitialIndex { get; set; }
        public MemoryStream StreamData { get; set; }
    }

    public enum TemplatePdfType
    {
        PdfAttachment = 1,
        ClientDocument = 2,
        Iterable = 3,
        File = 4
    }
}
