using Microsoft.AspNetCore.Http;
using SmartdustApp.Business.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface IDocumentService
    {
        /// <summary>
        /// Method To Upload Document 
        /// </summary>
        List<int> UploadFiles(IFormFileCollection files);

        /// <summary>
        /// Method To download Document 
        /// </summary>
        DocumentModel DownloadDocument(int documentID);
    }
}
