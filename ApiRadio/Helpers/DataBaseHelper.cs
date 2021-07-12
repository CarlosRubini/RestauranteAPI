using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace ApiRadio
{
    public static class DatabaseHelper
    {
        private const string DEFAULT_CONNECTION_STRING = "DBDEFAULT";

        public static string GetConnectionString(string name)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings is null)
                throw new ConfigurationErrorsException("A cadeia de caracteres para conexão com o banco de dados não foi configurada.");
            return settings.ConnectionString;
        }

        public static IDbConnection CreateConnection()
        {
            string connectionString = GetConnectionString(DatabaseHelper.DEFAULT_CONNECTION_STRING);
            return new OleDbConnection(connectionString);
        }
    }
}