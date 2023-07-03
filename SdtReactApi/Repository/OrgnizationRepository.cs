using Dapper;
using System.Data;
using SmartdustApi.Repository.Interface;
using SmartdustApi.Infrastructure;
using SmartdustApi.Models;

namespace SmartdustApi.Repository
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
        public List<OrganizationModel> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<OrganizationModel>("select * from [Organization] WHERE IsDeleted = 0").ToList();
        }
       
    }
}
