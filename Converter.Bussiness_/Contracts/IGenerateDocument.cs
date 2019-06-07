﻿using Converter.Business.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebSupergoo.ABCpdf9;

namespace Converter.Business.Contracts
{
    public interface IGenerateDocument
    {
        Doc GetMergePdf(string html);
        Doc CreatePDFFromString(string htmlData);
        MemoryStream GetDocumentsStream(List<FileDocumentDTO> doc, bool mergeDocs);
    }
}
