
using System;
using MySqlConnector;

namespace WinFormsApp1.Data
{
    public static class DbConfig
    {
        public static MySqlConnection GetConnection()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "127.0.0.1",
                Port = 3306,
                Database = "cyberbot_db",
                UserID = "root",
                Password = Environment.GetEnvironmentVariable(
                    "BLACKVAULT_DB_PASSWORD") ?? ""
            };

            return new MySqlConnection(builder.ConnectionString);
        }
    }
}
