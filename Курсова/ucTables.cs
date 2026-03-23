using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public partial class ucTables : UserControl
    {
        public bool IsReservationMode { get; set; } = false;

        public ucTables()
        {
            InitializeComponent();
        }

        private void ucTables_Load(object sender, EventArgs e)
        {
            LoadTablesFromDatabase();
        }

        public void LoadTablesFromDatabase()
        {
            flowLayoutPanel1.Controls.Clear();

            Dictionary<string, double> unpaidTables = new Dictionary<string, double>();
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        string query = "SELECT TableName, SUM(TotalAmount) as Total FROM Orders WHERE Status = 'Pending' GROUP BY TableName";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string tName = reader["TableName"].ToString();
                                    double tTotal = Convert.ToDouble(reader["Total"]);
                                    unpaidTables[tName] = tTotal;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Error: " + ex.Message);
            }

            for (int i = 1; i <= 40; i++)
            {
                Button btnTable = new Button();
                string tableNameUI = $"Стіл {i}";

                btnTable.Name = "Table_" + i;
                btnTable.Tag = tableNameUI;
                btnTable.Size = new Size(180, 180);
                btnTable.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                btnTable.FlatStyle = FlatStyle.Flat;
                btnTable.FlatAppearance.BorderSize = 0;
                btnTable.Margin = new Padding(15);
                btnTable.Cursor = Cursors.Hand;

                if (unpaidTables.ContainsKey(tableNameUI))
                {
                    btnTable.BackColor = Color.Crimson;
                    btnTable.ForeColor = Color.White;
                    btnTable.Text = $"{tableNameUI}\n\n{unpaidTables[tableNameUI]:N2} ₴";
                }
                else if (GlobalData.ReservedTables.ContainsKey(tableNameUI))
                {
                    btnTable.BackColor = Color.Gold;
                    btnTable.ForeColor = Color.Black;
                    DateTime resTime = GlobalData.ReservedTables[tableNameUI].ResDate;
                    btnTable.Text = $"{tableNameUI}\n\nРезерв:\n{resTime.ToString("dd.MM HH:mm")}";
                }
                else
                {
                    btnTable.BackColor = Color.White;
                    btnTable.ForeColor = Color.Black;
                    btnTable.Text = $"{tableNameUI}\n\nВільний";
                }

                int radius = 20;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btnTable.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btnTable.Width - radius, btnTable.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btnTable.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();
                btnTable.Region = new Region(path);

                btnTable.Click += Table_Click;
                flowLayoutPanel1.Controls.Add(btnTable);
            }
        }

        private void Table_Click(object sender, EventArgs e)
        {
            Button clickedTable = (Button)sender;
            string actualTableName = clickedTable.Tag.ToString();
            Form1 mainForm = (Form1)this.FindForm();

            if (IsReservationMode)
            {
                if (clickedTable.BackColor == Color.Crimson || clickedTable.BackColor == Color.Gold)
                {
                    MessageBox.Show("Цей стіл вже зайнятий або заброньований!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ReservationDialog dialog = new ReservationDialog(actualTableName);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DateTime selectedDate = dialog.SelectedDateTime;

                    GlobalData.ReservedTables[actualTableName] = new ReservationData()
                    {
                        CustomerName = "Гість",
                        Phone = "-",
                        ResDate = selectedDate
                    };
                    MessageBox.Show($"{actualTableName} успішно заброньовано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    IsReservationMode = false;
                }
            }
            else
            {
                ucTableOrder orderPage = new ucTableOrder();
                orderPage.SelectedTableButton = clickedTable;
                mainForm.ShowPage(orderPage);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}