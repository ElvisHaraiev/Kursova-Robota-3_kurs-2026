using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public static class DbHelper
    {
        public static string connectionString = "Server=localhost;Database=restaurant_db;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try { conn.Open(); }
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
    }
}