using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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

                    // 2. Veritabanını kullanmaya başla
                    cmd.CommandText = "USE `restaurant_db`;";
                    cmd.ExecuteNonQuery();

                    string createTablesScript = @"
                        -- Kategoriler Tablosu
                        CREATE TABLE IF NOT EXISTS `categories` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `name` VARCHAR(100) NOT NULL
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        -- Müşteriler (CRM) Tablosu
                        CREATE TABLE IF NOT EXISTS `clients` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `phone` VARCHAR(20) UNIQUE,
                            `name` VARCHAR(100),
                            `address` TEXT,
                            `total_orders` INT DEFAULT 0,
                            `client_type` VARCHAR(50)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        -- Menü Ürünleri Tablosu
                        CREATE TABLE IF NOT EXISTS `menuitems` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `name` VARCHAR(100) NOT NULL,
                            `price` DOUBLE,
                            `category` VARCHAR(100),
                            `is_available` TINYINT(1) DEFAULT 1,
                            `image_path` TEXT
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        -- Siparişler Tablosu
                        CREATE TABLE IF NOT EXISTS `orders` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `TableName` VARCHAR(100),
                            `WaiterName` VARCHAR(100),
                            `OrderDetails` TEXT,
                            `TotalAmount` DOUBLE,
                            `PaymentMethod` VARCHAR(50),
                            `Status` VARCHAR(50),
                            `OrderDate` DATETIME DEFAULT CURRENT_TIMESTAMP
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        -- Sipariş Detayları (Order Items) Tablosu
                        CREATE TABLE IF NOT EXISTS `order_items` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `order_id` INT,
                            `item_name` VARCHAR(100),
                            `quantity` INT,
                            `price` DOUBLE
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        -- Rezervasyonlar Tablosu
                        CREATE TABLE IF NOT EXISTS `reservations` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `client_name` VARCHAR(100),
                            `phone` VARCHAR(20),
                            `table_name` VARCHAR(50),
                            `res_date` DATE,
                            `res_time` VARCHAR(20),
                            `guests` INT,
                            `status` VARCHAR(50) DEFAULT 'Active',
                            `notes` TEXT
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

                        -- Kullanıcılar (Personel) Tablosu
                        CREATE TABLE IF NOT EXISTS `users` (
                            `id` INT AUTO_INCREMENT PRIMARY KEY,
                            `username` VARCHAR(50) UNIQUE,
                            `password` VARCHAR(255),
                            `role` VARCHAR(50)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
                    ";
                    cmd.CommandText = createTablesScript;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT COUNT(*) FROM `users`;";
                    long userCount = (long)cmd.ExecuteScalar();
                    if (userCount == 0)
                    {
                        cmd.CommandText = "INSERT INTO `users` (`username`, `password`, `role`) VALUES ('admin', '1234', 'Admin');";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}