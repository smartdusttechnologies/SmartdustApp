using System.Data;

namespace SmartdustApp.Business.Infrastructure
{
    /// <summary>
    /// Implimenting interface for connection factory
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Implimenting IDb connection with GetConnection method
        /// </summary>
        IDbConnection GetConnection { get; }
    }
}