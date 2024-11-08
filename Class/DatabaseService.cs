using System;
using Npgsql;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
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

        public NpgsqlConnection GetConnection()
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
            User? user  = null;

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
                MessageBox.Show("Error saat mencoba mendapatkan user berdasarkan ID: " + ex.Message);
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
                MessageBox.Show("Error saat mencoba mendapatkan user ID berdasarkan email: " + ex.Message);
            }

            return userId;
        }
        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            using var conn = GetConnection();

            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM item", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new Item
                    {
                        ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                        SellerId = reader.GetGuid(reader.GetOrdinal("seller_id")),
                        CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("customer_id")),
                        ItemName = reader.GetString(reader.GetOrdinal("item_name")),
                        Category = reader.GetString(reader.GetOrdinal("category")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        Price = (double)reader.GetDecimal(reader.GetOrdinal("price")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                        Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")) // Check if image is fetched properly
                    });
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error saat mencoba mendapatkan daftar item: " + ex.Message);
            }

            return items;
        }

        public List<Cart> getUserCart(int userId)
        {
            using var conn = GetConnection();
            List<Cart> cartItem = new List<Cart>();
            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT cart.item_id, cart.quantity, item_name, category, description, price, image FROM cart JOIN item ON item.item_id = cart.item_id AND user_id = @user_id;", conn);
                cmd.Parameters.AddWithValue("userId", userId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cartItem.Add(new Cart{
                            itemId =  reader.GetInt32(reader.GetOrdinal("item_id")),
                            itemPrice = reader.GetDouble(reader.GetOrdinal("price")),
                            itemImage = reader.GetString(reader.GetOrdinal("image")),
                            itemQuantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                            itemName = reader.GetString(reader.GetOrdinal("item_name")),
                            itemDescription = reader.GetString(reader.GetOrdinal("description")),
                            itemCategory = reader.GetString(reader.GetOrdinal("category"))
                        });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            MessageBox.Show(cartItem[2].itemName);
            return cartItem;
        }

        public void inputIntoCart(int userId, Item item, int quantity)
        {
            using var conn = GetConnection();
            try
            {
                conn.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO cart (user_id, item_id, quantity) VALUES (@UserId, @ItemId, @Quantity)", conn);
                cmd.Parameters.AddWithValue("@UserId", userId );
                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            conn.Close();
        }

        public bool registerUser(User user, bool isBuyer = false, bool isSeller = false)
        {
            int user_id;
            using var conn = GetConnection();
            conn.Open();
        try{
            if (isBuyer == true)
            {
                NpgsqlCommand command =
                    new NpgsqlCommand(
                        "INSERT INTO user_table (user_name, email, password, phone, address, money) VALUES (@user_name , @email, @password, @phone,@address,0)",
                        conn);
                command.Parameters.AddWithValue("user_name", user.UserName);
                command.Parameters.AddWithValue("email", user.UserEmail);
                command.Parameters.AddWithValue("password", user.UserPassword);
                command.Parameters.AddWithValue("phone", user.UserPhone.ToString());
                command.Parameters.AddWithValue("address", user.UserAdress);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = "SELECT user_id FROM user_table WHERE user_name=@user_name";
                command.Parameters.AddWithValue("user_name", user.UserName);
                user_id = (int)command.ExecuteScalar();
                command.CommandText = "INSERT INTO customer (customer_id, user_id) VALUES (@customer_id, @user_id)";
                command.Parameters.AddWithValue("customer_id", Guid.NewGuid());
                command.Parameters.AddWithValue("user_id", user_id);
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE user_table SET customer_id=@customer_id WHERE user_id=@user_id ";
                command.ExecuteNonQuery();
                
                MessageBox.Show("User created successfully");
                return true;
            }
            else if (isSeller == true)
            {
                NpgsqlCommand command =
                    new NpgsqlCommand(
                        "INSERT INTO user_table ( user_name, email, password, phone, address, money) VALUES (@user_name , @email, @password, @phone,@address,0);",
                        conn);
                command.Parameters.AddWithValue("user_name", user.UserName);
                command.Parameters.AddWithValue("email", user.UserEmail);
                command.Parameters.AddWithValue("password", user.UserPassword);
                command.Parameters.AddWithValue("phone", user.UserPhone.ToString());
                command.Parameters.AddWithValue("address", user.UserAdress);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = "SELECT user_id FROM user_table WHERE user_name=@user_name";
                command.Parameters.AddWithValue("user_name", user.UserName);
                user_id = (int)command.ExecuteScalar();
                command.CommandText = "INSERT INTO seller (seller_id, user_id) VALUES (@seller_id, @user_id)";
                command.Parameters.AddWithValue("seller_id", Guid.NewGuid());
                command.Parameters.AddWithValue("user_id", user_id);
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE user_table SET seller_id=@seller_id WHERE user_id=@user_id ";
                command.ExecuteNonQuery();
                
                MessageBox.Show("User created successfully");
                conn.Close();
                return true;
            }
            else
            {
                MessageBox.Show("Please select an option");
                return false;
            }
            

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            conn.Close();
        }
            return false;
        }
    }
    
    
}
