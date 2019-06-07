using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Converter.Business.Contracts;
using Converter.Business.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocsConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDocumentsController : ControllerBase
    {
        private readonly IGenerateDocument generateDocument;

        public FileDocumentsController(IGenerateDocument _generateDocument)
        {
            generateDocument = _generateDocument;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Preview(List<IFormFile> files)
        {

            if (files.Count < 1)
            {
                return NotFound();
            }

            var fileDocumenstDto = new List<FileDocumentDTO>();
            var docsStream = new MemoryStream();
            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var html = new StringBuilder();
                        using (var reader = new StreamReader(formFile.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                                html.AppendLine(reader.ReadLine());
                        }
                        fileDocumenstDto.Add(new FileDocumentDTO
                        {
                            ReplacedHtml = html.ToString(),
                            DocumentType = (int)TemplatePdfType.PdfAttachment,
                            Name = formFile.FileName
                        });
                    }
                }
                docsStream = generateDocument.GetDocumentsStream(fileDocumenstDto, true);
                var document = new StreamContent(docsStream);

                if (document == null)
                {
                    return this.NotFound(new { doc = "Document Not found" });
                }
                
            } catch (Exception e)
            {
                return this.StatusCode(500);
            }
            var fileName = (files.Count == 1 ? files[0].FileName.Replace(".html", "") : "Test document" ) + ".pdf";
            return File(docsStream.ToArray(), "application/octet-stream", fileName);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
