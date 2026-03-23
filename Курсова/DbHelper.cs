using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 

namespace Курсова
{
    public static class DbHelper
    {
        // XAMPP varsayılan şifresiz root bağlantısı
        public static string connectionString = "Server=localhost;Database=restaurant_db;Uid=root;Pwd=;";

        // Veritabanı bağlantısını açıp döndüren metot
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
    }
}