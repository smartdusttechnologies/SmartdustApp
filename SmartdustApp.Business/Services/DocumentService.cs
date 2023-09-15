using Microsoft.AspNetCore.Http;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository;
using SmartdustApp.Business.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Method To Upload Document 
        /// </summary>
        public List<int> UploadFiles(IFormFileCollection files)
        {
            List<int> uploadedFileIds = new List<int>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var uploadedFileId = UploadSingleFile(file);
                    uploadedFileIds.Add(uploadedFileId);
                }
            }

            return uploadedFileIds;
        }

        private int UploadSingleFile(IFormFile file)
        {
            var newFileName = GenerateUniqueFileName(file.FileName);
            var fileModel = new DocumentModel
            {
                Name = newFileName,
                FileType = Path.GetExtension(newFileName)
            };
            // Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".xlsx", ".pdf" };
            if (!allowedExtensions.Contains(fileModel.FileType.ToLower()))
            {
                throw new InvalidOperationException("Unsupported file type.");
            }

            // Validate file size
            if (file.Length > 1024 * 1024) // 1 MB
            {
                throw new InvalidOperationException("File size should not exceed 1MB.");
            }
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileModel.DataFiles = memoryStream.ToArray();
            }

            return _documentRepository.FileUpload(fileModel);
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            var fileExtension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{fileExtension}";
        }

        /// <summary>
        /// Method To download Document 
        /// </summary>
        public DocumentModel DownloadDocument(int documentID)
        {
            var document = _documentRepository.DownloadDocument(documentID);
            return document;

        }
    }
}
