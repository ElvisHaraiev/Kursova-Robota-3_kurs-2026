using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Курсова
{
    public static class DbHelper
    {
        public static string connectionString = "Server=localhost;Database=restaurant_db;Uid=root;Pwd=;";
        private static string serverConnectionString = "Server=localhost;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка підключення:\n" + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return conn;
        }

        public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = GetConnection())
            {
                if (conn.State != ConnectionState.Open) return new DataTable();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = GetConnection())
            {
                if (conn.State != ConnectionState.Open) return 0;

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(serverConnectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS `restaurant_db` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;", conn);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "USE `restaurant_db`;";
                    cmd.ExecuteNonQuery();

                    string createTablesScript = @"
                CREATE TABLE IF NOT EXISTS `categories` (
                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                    `name` VARCHAR(100) NOT NULL
                ) ENGINE=InnoDB;

                CREATE TABLE IF NOT EXISTS `clients` (
                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                    `phone` VARCHAR(20) UNIQUE,
                    `name` VARCHAR(100),
                    `address` TEXT,
                    `total_orders` INT DEFAULT 0,
                    `client_type` VARCHAR(50)
                ) ENGINE=InnoDB;

                CREATE TABLE IF NOT EXISTS `menuitems` (
                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                    `name` VARCHAR(100) NOT NULL,
                    `price` DECIMAL(18, 2), -- Змінено на DECIMAL
                    `category` VARCHAR(100),
                    `is_available` TINYINT(1) DEFAULT 1,
                    `image_path` TEXT
                ) ENGINE=InnoDB;

                CREATE TABLE IF NOT EXISTS `orders` (
                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                    `TableName` VARCHAR(100),
                    `WaiterName` VARCHAR(100),
                    `OrderDetails` TEXT,
                    `TotalAmount` DECIMAL(18, 2), -- Змінено на DECIMAL
                    `PaymentMethod` VARCHAR(50),
                    `Status` VARCHAR(50),
                    `OrderDate` DATETIME DEFAULT CURRENT_TIMESTAMP
                ) ENGINE=InnoDB;

                CREATE TABLE IF NOT EXISTS `order_items` (
                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                    `order_id` INT,
                    `item_name` VARCHAR(100),
                    `quantity` INT,
                    `price` DECIMAL(18, 2), -- Змінено на DECIMAL
                    FOREIGN KEY (`order_id`) REFERENCES `orders`(`id`) ON DELETE CASCADE
                ) ENGINE=InnoDB;

                CREATE TABLE IF NOT EXISTS `users` (
                    `id` INT AUTO_INCREMENT PRIMARY KEY,
                    `username` VARCHAR(50) UNIQUE,
                    `password_hash` VARCHAR(255),
                    `role` VARCHAR(50)
                ) ENGINE=InnoDB;
            ";
                    cmd.CommandText = createTablesScript;
                    cmd.ExecuteNonQuery();

                    try
                    {
                        cmd.CommandText = "ALTER TABLE `users` CHANGE `password` `password_hash` VARCHAR(255);";
                        cmd.ExecuteNonQuery();
                    }
                    catch {}

                    try
                    {
                        cmd.CommandText = "ALTER TABLE `menuitems` MODIFY `price` DECIMAL(18, 2);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "ALTER TABLE `orders` MODIFY `TotalAmount` DECIMAL(18, 2);";
                        cmd.ExecuteNonQuery();
                    }
                    catch { }

                    cmd.CommandText = "SELECT COUNT(*) FROM `users`;";
                    long userCount = Convert.ToInt64(cmd.ExecuteScalar());
                    if (userCount == 0)
                    {
                        cmd.CommandText = "INSERT INTO `users` (`username`, `password_hash`, `role`) VALUES ('admin', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'Admin');";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка ініціалізації БД: " + ex.Message);
            }
        }

    }
}
  