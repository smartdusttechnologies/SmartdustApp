using Dapper;
using System.Data;
using SmartdustApp.Business.Infrastructure;
using SmartdustApp.Business.Models;
using SmartdustApp.Business.Data.Repository.Interfaces;

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
        public List<OrganizationModel> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<OrganizationModel>("select * from [Organization] WHERE IsDeleted = 0").ToList();
        }
       
    }
}
