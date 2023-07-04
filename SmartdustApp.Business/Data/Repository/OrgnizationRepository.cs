using Dapper;
using System.Data;
using SmartdustApp.Business.Infrastructure;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        /// <summary>
        /// using the userRespository
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        public OrganizationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        ///get the data of the Orgnaization
        /// </summary>
        /// <returns></returns>
        public List<Organization> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<Organization>("select * from [Organization] WHERE IsDeleted = 0").ToList();
        }
       
    }
}
