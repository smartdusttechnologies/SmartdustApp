using Dapper;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Infrastructure;
using SmartdustApp.Business.Data.Repository.Interfaces;
using System.Data;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ContactRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public RequestResult<bool> Save(ContactModel contact)
        {
            string query = @"Insert into [Contact] (Name,Mail,Address,Subject,Phone,Message)
                                                  values (@Name,@Mail,@Address,@Subject,@Phone,@Message)";
            using IDbConnection db = _connectionFactory.GetConnection;

            var result = db.Execute(query, contact);
            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }
    }
}
