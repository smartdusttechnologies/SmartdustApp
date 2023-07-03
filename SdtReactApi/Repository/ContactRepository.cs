using Dapper;
using SmartdustApi.Common;
using SmartdustApi.Infrastructure;
using SmartdustApi.Model;
using SmartdustApi.Repository.Interfaces;
using System.Data;

namespace SmartdustApi.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ContactRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public RequestResult<bool> Save(ContactDTO contact)
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
