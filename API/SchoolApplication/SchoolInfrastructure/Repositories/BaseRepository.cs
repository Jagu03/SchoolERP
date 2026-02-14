using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SchoolInfrastructure.Repositories
{
    /// <summary>
    /// Base repository class providing common functionality for all repositories.
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly string ConnectionString;
        protected readonly ILogger Logger;

        protected BaseRepository(IConfiguration configuration, ILogger logger, string connectionStringName = "LiveDBContext")
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            var connectionString = configuration?.GetConnectionString(connectionStringName);
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{connectionStringName}' is not configured. " +
                    $"Configure it in appsettings.json or set the environment variable.");
            }
            
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Logs and throws a database exception with a descriptive message.
        /// </summary>
        protected InvalidOperationException HandleDatabaseError(SqlException ex, string operationName, string? details = null)
        {
            var message = $"Database error during {operationName}.";
            if (!string.IsNullOrEmpty(details))
                message += $" Details: {details}";
            
            Logger.LogError(ex, message);
            return new InvalidOperationException($"An error occurred while performing {operationName}.", ex);
        }

        /// <summary>
        /// Logs an unexpected exception.
        /// </summary>
        protected void LogUnexpectedError(Exception ex, string operationName, string? details = null)
        {
            var message = $"Unexpected error during {operationName}.";
            if (!string.IsNullOrEmpty(details))
                message += $" Details: {details}";
            
            Logger.LogError(ex, message);
        }
    }
}

