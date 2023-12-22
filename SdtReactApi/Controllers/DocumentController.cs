using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Method To Upload Document 
        /// </summary>
        [HttpPost]
        [Route("FileUpload")]
        public IActionResult FileUpload()
        {
            var uploadedFileIds = _documentService.UploadFiles(Request.Form.Files);
            return Ok(uploadedFileIds);
        }

        /// <summary>
        /// Method To download Document 
        /// </summary>
        [HttpGet]
        [Route("DownloadDocument/{documentID}")]
        public IActionResult DownloadDocument(int documentID)
        {

            DocumentModel attachment = _documentService.DownloadDocument(documentID);

            if (attachment != null)
            {
                return File(new MemoryStream(attachment.DataFiles), Helpers.GetMimeTypes()[attachment.FileType], attachment.Name);
            }
            return Ok("Can't find the Document");
        }
    }
}
