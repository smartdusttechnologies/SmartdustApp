using Dapper;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Data.Repository
{
    public class DocumentRepository: IDocumentRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public DocumentRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        /// <summary>
        /// File Upload in DB
        /// </summary>
        public int FileUpload(DocumentModel File)
        {
            string query = @"INSERT INTO [DocumentTable](Name, FileType, DataFiles)
                    VALUES (@Name, @FileType, @DataFiles);
                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.QuerySingle<int>(query, File);
        }

        /// <summary>
        /// Documrnt download
        /// </summary>
        public DocumentModel DownloadDocument(int documentID)
        {
            using IDbConnection con = _connectionFactory.GetConnection;
            return con.Query<DocumentModel>(@"select * from [DocumentTable] where ID = @ID ", new { ID = documentID }).FirstOrDefault();
        }
    }
}
