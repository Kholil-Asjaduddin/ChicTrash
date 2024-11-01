using System;
using Npgsql;
using DotNetEnv;

namespace ChicTrash
{
    public class DatabaseService
    {
        private readonly string? _connectionString;

        public DatabaseService()
        {
            // Muat variabel dari file .env
            Env.TraversePath().Load();
            _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public bool CheckDatabaseConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return conn.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateUser(string email, string password)
        {
            using var conn = GetConnection();
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT * FROM user_table WHERE email = @Email AND password = @Password", conn);
            cmd.Parameters.AddWithValue("Email", email);
            cmd.Parameters.AddWithValue("Password", password);

            return cmd.ExecuteScalar() != null;
        }
    }
}
