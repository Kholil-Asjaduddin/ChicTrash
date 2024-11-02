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

        public User GetUserById(int userId)
        {
            User user = null;

            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM user_table WHERE user_id = @UserId", conn);
                cmd.Parameters.AddWithValue("UserId", userId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        UserUserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                        UserName = reader.GetString(reader.GetOrdinal("user_name")),
                        UserEmail = reader.GetString(reader.GetOrdinal("email")),
                        UserPassword = reader.GetString(reader.GetOrdinal("password")),
                        UserPhone = reader.GetString(reader.GetOrdinal("phone")),
                        UserAdress = reader.GetString(reader.GetOrdinal("address")),
                        UserMoney = reader.GetDouble(reader.GetOrdinal("money")),
                        SellerId = reader.IsDBNull(reader.GetOrdinal("seller_id")) ? null : reader.GetGuid(reader.GetOrdinal("seller_id")).ToString(),
                        CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? null : reader.GetGuid(reader.GetOrdinal("customer_id")).ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saat mencoba mendapatkan user berdasarkan ID: " + ex.Message);
            }

            return user;
        }

        public int GetUserIdByEmail(string email)
        {
            int userId = -1;

            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT user_id FROM user_table WHERE email = @Email", conn);
                cmd.Parameters.AddWithValue("Email", email);
                userId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saat mencoba mendapatkan user ID berdasarkan email: " + ex.Message);
            }

            return userId;
        }
    }
}
