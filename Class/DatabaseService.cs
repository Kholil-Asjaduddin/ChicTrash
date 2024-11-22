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
            _connectionString = DotNetEnv.Env.GetString("DB_CONNECTION");
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
            User? user = null;

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
                        SellerId = reader.IsDBNull(reader.GetOrdinal("seller_id"))
                            ? null
                            : reader.GetGuid(reader.GetOrdinal("seller_id")),
                        CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id"))
                            ? null
                            : reader.GetGuid(reader.GetOrdinal("customer_id"))
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
                        CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id"))
                            ? (Guid?)null
                            : reader.GetGuid(reader.GetOrdinal("customer_id")),
                        ItemName = reader.GetString(reader.GetOrdinal("item_name")),
                        Category = reader.GetString(reader.GetOrdinal("category")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        Price = (double)reader.GetDecimal(reader.GetOrdinal("price")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                        Image = reader.IsDBNull(reader.GetOrdinal("image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("image")) // Check if image is fetched properly
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
                using var cmd =
                    new NpgsqlCommand(
                        "SELECT cart.item_id, cart.quantity, item_name, category, description, price, image FROM cart JOIN item ON item.item_id = cart.item_id AND user_id = @user_id;",
                        conn);
                cmd.Parameters.AddWithValue("userId", userId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cartItem.Add(new Cart
                    {
                        itemId = reader.GetInt32(reader.GetOrdinal("item_id")),
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

            return cartItem;
        }

        public void inputIntoCart(int userId, Item item, int quantity = 1)
        {
            using var conn = GetConnection();
            try
            {
                conn.Open();
                using var cmd =
                    new NpgsqlCommand(
                        "INSERT INTO cart (user_id, item_id, quantity) VALUES (@UserId, @ItemId, @Quantity)  ON CONFLICT (user_id, item_id)  DO UPDATE SET quantity = cart.quantity + EXCLUDED.quantity;",
                        conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
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
            try
            {
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

        public void checkoutItems(List<int> cartItemIds, double totalPrice)
{
    using var conn = GetConnection();
    conn.Open();
    try
    {
        // Ambil user_id dari salah satu item di cart
        int userId = -1;
        using (var cmd = new NpgsqlCommand("SELECT user_id FROM cart WHERE item_id = @ItemId LIMIT 1", conn))
        {
            cmd.Parameters.AddWithValue("ItemId", cartItemIds.First());
            userId = (int)cmd.ExecuteScalar();
        }

        if (userId != -1)
        {
            // Mengambil list item_id, quantity, dan seller_id dari cartItemIds
            List<(int itemId, int quantity, int sellerUserId)> cartItems = new List<(int itemId, int quantity, int sellerUserId)>();
            foreach (var itemId in cartItemIds)
            {
                // Akses semua record pada tabel item dengan item_id yang sesuai
                using (var cmd = new NpgsqlCommand(
                           "SELECT c.item_id, c.quantity, i.seller_id FROM cart c JOIN item i ON c.item_id = i.item_id WHERE c.user_id = @UserId AND c.item_id = @ItemId",
                           conn))
                {
                    cmd.Parameters.AddWithValue("UserId", userId);
                    cmd.Parameters.AddWithValue("ItemId", itemId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int itemSellerId = reader.GetInt32(reader.GetOrdinal("seller_id"));

                            // Gunakan seller_id tersebut untuk mengakses user_table
                            int sellerUserId;
                            using (var cmdUser = new NpgsqlCommand("SELECT user_id FROM user_table WHERE seller_id = @SellerId", conn))
                            {
                                cmdUser.Parameters.AddWithValue("SellerId", itemSellerId);
                                sellerUserId = (int)cmdUser.ExecuteScalar();
                            }

                            // Tambahkan data yang diperlukan ke list cartItems
                            cartItems.Add((reader.GetInt32(reader.GetOrdinal("item_id")),
                                reader.GetInt32(reader.GetOrdinal("quantity")), sellerUserId));
                        }
                    }
                }
            }

            // Insert record baru ke order_table
            foreach (var (itemId, quantity, sellerUserId) in cartItems)
            {
                using (var cmd = new NpgsqlCommand(
                           "INSERT INTO order_table (customer_id, seller_id, item_id, quantity) VALUES (@CustomerId, @SellerId, @ItemId, @Quantity)",
                           conn))
                {
                    cmd.Parameters.AddWithValue("CustomerId", userId);
                    cmd.Parameters.AddWithValue("SellerId", sellerUserId);
                    cmd.Parameters.AddWithValue("ItemId", itemId);
                    cmd.Parameters.AddWithValue("Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }
            }

            // Mengurangi uang pengguna sebesar harga total item di keranjang
            using (var cmd = new NpgsqlCommand(
                       "UPDATE user_table SET money = money - @price WHERE user_id = @user_id", conn))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("price", totalPrice);
                cmd.ExecuteNonQuery();
            }

            // Mengosongkan keranjang belanja pengguna setelah checkout
            using (var cmd = new NpgsqlCommand("DELETE FROM cart WHERE user_id = @user_id", conn))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Item Successfully Checked");
        }
        else
        {
            throw new Exception("User ID tidak ditemukan di cart.");
        }
    }
    catch (Exception e)
    {
        // Menangani kesalahan dan menampilkan pesan kesalahan
        MessageBox.Show(e.Message);
    }
}

        public List<Article> GetArticles()
        {
            List<Article> articles = new List<Article>();
            using var conn = GetConnection();
            conn.Open();
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM article", conn);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    articles.Add(new Article
                        {
                            ArticleId = Convert.ToInt32(reader["article_id"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                        }
                    );
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            conn.Close();
            return articles;
        }

        public List<Order> GetOrders(int user_id)
        {
            List<Order> orders = new List<Order>();
            using var conn = GetConnection();
            conn.Open();
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(
                    "SELECT order_table.order_id, item.item_id, item.item_name, user_table.address, order_table.quantity FROM order_table JOIN item ON item.item_id = order_table.item_id JOIN user_table ON order_table.customer_id = user_table.user_id WHERE order_table.seller_id = @user_id;",
                    conn);
                command.Parameters.AddWithValue("user_id", user_id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = Convert.ToInt32(reader["order_id"]),
                        Address = Convert.ToString(reader["address"]),
                        ProductName = Convert.ToString(reader["item_name"]),
                        ProductId = Convert.ToInt32(reader["item_id"]),
                        Quantity = Convert.ToInt32(reader["quantity"]),
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            conn.Close();
            return orders;
        }

        public void UploadItems(Guid seller_id, string name, int quantity, double price, string description,
            string imagePath, string category)
        {
            using var conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                NpgsqlCommand command =
                    new NpgsqlCommand(
                        "INSERT INTO item (seller_id, item_name, category, description, price, quantity, image) VALUES (@seller_id, @name, @category, @description, @price, @quantity, @image)",
                        conn);
                command.Parameters.AddWithValue("seller_id", seller_id);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("category", category);
                command.Parameters.AddWithValue("description", description);
                command.Parameters.AddWithValue("price", price);
                command.Parameters.AddWithValue("quantity", quantity);
                command.Parameters.AddWithValue("image", imagePath);
                command.ExecuteNonQuery();
                MessageBox.Show("Item Successfully Uploaded");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            conn.Close();

        }

        public void UpdateUserDetails(int userId, string newAddress = null, string newPhone = null,
            string newPassword = null)
        {
            // Build the SQL command dynamically based on non-null values
            string query = "UPDATE user_table SET ";
            bool firstField = true;

            if (newAddress != "")
            {
                query += "address = @newAddress";
                firstField = false;
            }

            if (newPhone != "")
            {
                if (!firstField) query += ", ";
                query += "phone = @newPhone";
                firstField = false;
            }

            if (newPassword != "")
            {
                if (!firstField) query += ", ";
                query += "password = @newPassword";
            }

            query += " WHERE user_id = @userId;";
            using var conn = GetConnection();
            conn.Open();
            try
            {
                using var command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("userId", userId);
                if (newAddress != null) command.Parameters.AddWithValue("newAddress", newAddress);
                if (newPhone != null) command.Parameters.AddWithValue("newPhone", newPhone);
                if (newPassword != null) command.Parameters.AddWithValue("newPassword", newPassword);

                command.ExecuteNonQuery();
                MessageBox.Show("User Successfully Updated");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            conn.Close();

        }

        public void TopUpUserMoney(int userId, double moneyToTopUp)
        {
            using var conn = GetConnection(); // Assumes you have a method to get your NpgsqlConnection object
            try
            {
                conn.Open();
                string query =
                    "UPDATE user_table SET money = COALESCE(money, 0) + @moneyToTopUp WHERE user_id = @userId";

                using var command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("moneyToTopUp", moneyToTopUp);
                command.ExecuteNonQuery();
                MessageBox.Show("Money Successfully Updated");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            conn.Close();
        }

        public void EditItem(int item_id, string description, string price, string quantity, string imagePath)
        {
            using var conn = GetConnection();
            conn.Open();
            
            // Start query construction
            string query = "UPDATE item SET ";
            bool firstField = true;

            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;

                // Dynamically build the query
                if (!string.IsNullOrWhiteSpace(description))
                {
                    query += "description = @description";
                    cmd.Parameters.AddWithValue("description", description);
                    firstField = false;
                }

                if (!string.IsNullOrWhiteSpace(price))
                {
                    double numPrice;
                    if (!double.TryParse(price, out numPrice))
                    {
                        throw new ArgumentException("Invalid price value.");
                    }

                    query += (firstField ? "" : ", ") + "price = @price";
                    cmd.Parameters.AddWithValue("price", numPrice);
                    firstField = false;
                }

                if (!string.IsNullOrWhiteSpace(quantity))
                {
                    double newQuantity;
                    if (!double.TryParse(quantity, out newQuantity))
                    {
                        throw new ArgumentException("Invalid quantity value.");
                    }

                    query += (firstField ? "" : ", ") + "quantity = @quantity";
                    cmd.Parameters.AddWithValue("quantity", newQuantity);
                    firstField = false;
                }

                if (!string.IsNullOrWhiteSpace(imagePath))
                {
                    query += (firstField ? "" : ", ") + "image = @image";
                    cmd.Parameters.AddWithValue("image", imagePath);
                }

                // Add the WHERE clause
                query += " WHERE item_id = @item_id";
                cmd.Parameters.AddWithValue("item_id", item_id);

                // Assign the built query to the command text
                cmd.CommandText = query;

                // Execute the query
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item Successfully Updated");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteItem(int item_id)
        {
            using var conn = GetConnection();
            conn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM item WHERE item_id = @item_id", conn);
                cmd.Parameters.AddWithValue("item_id", item_id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Item Successfully Deleted");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            conn.Close();
        }

        public List<Item> getSellerItem(Guid user_id)
        {
            using var conn = GetConnection();
            conn.Open();
            List<Item> sellerItems = new List<Item>();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM item WHERE seller_id = @user_id;", conn);
                cmd.Parameters.AddWithValue("user_id", user_id);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sellerItems.Add(new Item
                    {
                        ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                        SellerId = reader.GetGuid(reader.GetOrdinal("seller_id")),
                        CustomerId = reader.IsDBNull(reader.GetOrdinal("customer_id"))
                            ? (Guid?)null
                            : reader.GetGuid(reader.GetOrdinal("customer_id")),
                        ItemName = reader.GetString(reader.GetOrdinal("item_name")),
                        Category = reader.GetString(reader.GetOrdinal("category")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        Price = (double)reader.GetDecimal(reader.GetOrdinal("price")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                        Image = reader.IsDBNull(reader.GetOrdinal("image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("image")) // Check if image is fetched properly
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
            conn.Close();
            return sellerItems;
        }
    }
   
}
