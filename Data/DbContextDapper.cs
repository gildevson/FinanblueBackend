using System.Data; //→ contém interfaces e tipos básicos de acesso a dados.
using System.Data.SqlClient; //permite conectar ao **SQL Server**.
using Microsoft.Extensions.Configuration; //serve para **ler configurações** do `appsettings.json`, como a string de conexão.

namespace FinanblueBackend.Data
{
    public class DbContextDapper
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DbContextDapper(IConfiguration config)
        {
            _config = config;
            //_connectionString = _config.GetConnectionString("DefaultConnection");
            _connectionString = _config.GetConnectionString("ConnectionWork");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
