using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public partial class ucReservations : UserControl
    {
        private Panel pnlLeft;
        private Panel pnlMain;
        private FlowLayoutPanel flpTables;
        private FlowLayoutPanel flpReservedBottom;

        private Label lblSelectedTable;
        private TextBox txtCustomerName;
        private MaskedTextBox mtbPhone;
        private TextBox txtEmail;
        private NumericUpDown numGuestCount;
        private DateTimePicker dtpDate;
        private ComboBox cmbTime;
        private Button btnReserve;
        private Button btnCancel;
        private Button btnSaveChanges;

        private string _selectedTableName = "";

        public ucReservations()
        {
            this.Size = new Size(1200, 800);
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            CreateModernUI();

            typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(flpTables, true, null);
            typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(flpReservedBottom, true, null);

            InitializeTablesOnce();
            SyncWithDatabase();
            UpdateTableVisuals();
        }

        public ReservationDialog ReservationDialog
        {
            get => default;
            set
            {
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.MidnightBlue, Color.Blue, LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void CreateModernUI()
        {
            pnlLeft = new Panel();
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Width = 350;
            pnlLeft.BackColor = Color.FromArgb(80, 0, 0, 0);
            this.Controls.Add(pnlLeft);

            Label lblTitle = new Label() { Text = "БРОНЮВАННЯ", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 20), AutoSize = true, BackColor = Color.Transparent };
            pnlLeft.Controls.Add(lblTitle);

            lblSelectedTable = new Label() { Text = "Стіл: Не обрано", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.Orange, Location = new Point(20, 70), AutoSize = true, BackColor = Color.Transparent };
            pnlLeft.Controls.Add(lblSelectedTable);

            txtCustomerName = new TextBox() { Font = new Font("Segoe UI", 14), Location = new Point(20, 150), Width = 300 };
            pnlLeft.Controls.Add(new Label() { Text = "Ім'я та Прізвище:", ForeColor = Color.White, Font = new Font("Segoe UI", 11), Location = new Point(20, 120), AutoSize = true, BackColor = Color.Transparent });
            pnlLeft.Controls.Add(txtCustomerName);

            mtbPhone = new MaskedTextBox() { Mask = "+38 (000) 000-00-00", Font = new Font("Segoe UI", 14), Location = new Point(20, 230), Width = 300 };
            pnlLeft.Controls.Add(new Label() { Text = "Номер телефону:", ForeColor = Color.White, Font = new Font("Segoe UI", 11), Location = new Point(20, 200), AutoSize = true, BackColor = Color.Transparent });
            pnlLeft.Controls.Add(mtbPhone);

            txtEmail = new TextBox() { Font = new Font("Segoe UI", 14), Location = new Point(20, 310), Width = 300 };
            pnlLeft.Controls.Add(new Label() { Text = "Email (не обов'язково):", ForeColor = Color.White, Font = new Font("Segoe UI", 11), Location = new Point(20, 280), AutoSize = true, BackColor = Color.Transparent });
            pnlLeft.Controls.Add(txtEmail);

            Label lblGuests = new Label() { Text = "Кількість осіб (2-10):", ForeColor = Color.White, Font = new Font("Segoe UI", 11), Location = new Point(20, 360), AutoSize = true, BackColor = Color.Transparent };
            numGuestCount = new NumericUpDown() { Minimum = 2, Maximum = 10, Value = 2, Font = new Font("Segoe UI", 14), Location = new Point(20, 390), Width = 300 };
            pnlLeft.Controls.Add(lblGuests); pnlLeft.Controls.Add(numGuestCount);

            dtpDate = new DateTimePicker() { Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 14), Location = new Point(20, 460), Width = 160 };
            cmbTime = new ComboBox() { Font = new Font("Segoe UI", 14), Location = new Point(190, 460), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            for (int h = 10; h <= 22; h++) { cmbTime.Items.Add($"{h}:00"); cmbTime.Items.Add($"{h}:30"); }
            cmbTime.SelectedIndex = 0;
            pnlLeft.Controls.Add(dtpDate); pnlLeft.Controls.Add(cmbTime);

            btnReserve = new Button() { Text = "Забронювати стіл", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, Size = new Size(300, 60), Location = new Point(20, 530), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnReserve.FlatAppearance.BorderSize = 0;
            btnReserve.Paint += BtnGradient_Paint;
            btnReserve.Click += BtnReserve_Click;
            pnlLeft.Controls.Add(btnReserve);

            btnSaveChanges = new Button() { Text = "Зберегти зміни", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.SeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(300, 45), Location = new Point(20, 530), Visible = false, Cursor = Cursors.Hand };
            btnSaveChanges.FlatAppearance.BorderSize = 0;
            btnSaveChanges.Click += BtnSaveChanges_Click;
            RoundButtonCorners(btnSaveChanges, 15);
            pnlLeft.Controls.Add(btnSaveChanges);

            btnCancel = new Button() { Text = "Скасувати бронювання", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(300, 45), Location = new Point(20, 590), Visible = false, Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            RoundButtonCorners(btnCancel, 15);
            pnlLeft.Controls.Add(btnCancel);

            pnlMain = new Panel() { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            this.Controls.Add(pnlMain); pnlMain.BringToFront();

            flpReservedBottom = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 150, BackColor = Color.FromArgb(60, 0, 0, 0), AutoScroll = true, Padding = new Padding(15) };
            pnlMain.Controls.Add(flpReservedBottom);

            flpTables = new FlowLayoutPanel() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20), BackColor = Color.Transparent };
            pnlMain.Controls.Add(flpTables);
        }

        private void InitializeTablesOnce()
        {
            flpTables.SuspendLayout();
            for (int i = 1; i <= 40; i++)
            {
                Button btnTable = new Button();
                btnTable.Name = $"Стіл {i}";
                btnTable.Size = new Size(110, 110);
                btnTable.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                btnTable.FlatStyle = FlatStyle.Flat;
                btnTable.FlatAppearance.BorderSize = 0;
                btnTable.Margin = new Padding(10);
                btnTable.Cursor = Cursors.Hand;

                RoundButtonCorners(btnTable, 15);
                btnTable.Click += Table_Click;

                flpTables.Controls.Add(btnTable);
            }
            flpTables.ResumeLayout();
        }

        private void UpdateTableVisuals()
        {
            flpTables.SuspendLayout();
            flpReservedBottom.SuspendLayout();
            flpReservedBottom.Controls.Clear();

            foreach (Control ctrl in flpTables.Controls)
            {
                if (ctrl is Button btnTable)
                {
                    string tableName = btnTable.Name;
                    bool isReserved = GlobalData.ReservedTables.ContainsKey(tableName);
                    bool isSelected = (tableName == _selectedTableName);

                    if (isSelected)
                    {
                        btnTable.BackColor = Color.LightGreen;
                        btnTable.ForeColor = Color.Black;
                        btnTable.Text = $"{tableName}\n(Обрано)";
                    }
                    else if (isReserved)
                    {
                        btnTable.BackColor = Color.Gold;
                        btnTable.ForeColor = Color.Black;
                        btnTable.Text = $"{tableName}\nРезерв";

                        AddReservedCardToBottom(tableName, GlobalData.ReservedTables[tableName]);
                    }
                    else
                    {
                        btnTable.BackColor = Color.White;
                        btnTable.ForeColor = Color.Black;
                        btnTable.Text = tableName;
                    }
                }
            }

            flpReservedBottom.ResumeLayout();
            flpTables.ResumeLayout();
        }

        private void SyncWithDatabase()
        {
            GlobalData.ReservedTables.Clear();
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "SELECT * FROM Reservations WHERE Status = 'Active'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GlobalData.ReservedTables[reader["TableName"].ToString()] = new ReservationData
                            {
                                CustomerName = reader["CustomerName"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                GuestCount = Convert.ToInt32(reader["GuestCount"]),
                                ResDate = Convert.ToDateTime(reader["ReservationDate"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("DB Sync Error: " + ex.Message); }
        }

        private void Table_Click(object sender, EventArgs e)
        {
            Button clickedBtn = (Button)sender;
            _selectedTableName = clickedBtn.Name;
            lblSelectedTable.Text = $"Стіл: {_selectedTableName}";

            if (GlobalData.ReservedTables.ContainsKey(_selectedTableName))
            {
                ReservationData data = GlobalData.ReservedTables[_selectedTableName];
                txtCustomerName.Text = data.CustomerName;
                mtbPhone.Text = data.Phone;
                txtEmail.Text = data.Email;
                numGuestCount.Value = data.GuestCount;
                dtpDate.Value = data.ResDate.Date;
                cmbTime.SelectedItem = data.ResDate.ToString("HH:mm");

                btnReserve.Visible = false;
                btnSaveChanges.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                txtCustomerName.Clear(); mtbPhone.Clear(); txtEmail.Clear();
                numGuestCount.Value = 2; dtpDate.Value = DateTime.Now; cmbTime.SelectedIndex = 0;

                btnReserve.Visible = true;
                btnSaveChanges.Visible = false;
                btnCancel.Visible = false;
            }

            UpdateTableVisuals();
        }

        private void BtnReserve_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedTableName) || !mtbPhone.MaskCompleted || string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                frmModernMsgBox.Show("Оберіть стіл та введіть номер телефону з ім'ям!", "Увага"); return;
            }

            DateTime selectedDate = dtpDate.Value.Date;
            string[] timeParts = cmbTime.SelectedItem.ToString().Split(':');
            selectedDate = selectedDate.AddHours(int.Parse(timeParts[0])).AddMinutes(int.Parse(timeParts[1]));

            string phone = mtbPhone.Text.Trim();
            string clientName = txtCustomerName.Text.Trim();
            bool isNewClient = false;

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string checkQuery = "SELECT id FROM clients WHERE phone = @phone";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@phone", phone);
                        using (MySqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.HasRows) isNewClient = true;
                        }
                    }

                    if (isNewClient)
                    {
                        string insClientQuery = "INSERT INTO clients (phone, name, address, client_type, total_orders) VALUES (@phone, @name, '-', 'Резервація', 1)";
                        using (MySqlCommand insCmd = new MySqlCommand(insClientQuery, conn))
                        {
                            insCmd.Parameters.AddWithValue("@phone", phone);
                            insCmd.Parameters.AddWithValue("@name", clientName);
                            insCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updClientQuery = "UPDATE clients SET total_orders = total_orders + 1, name = @name, client_type = 'Резервація' WHERE phone = @phone";
                        using (MySqlCommand updCmd = new MySqlCommand(updClientQuery, conn))
                        {
                            updCmd.Parameters.AddWithValue("@phone", phone);
                            updCmd.Parameters.AddWithValue("@name", clientName);
                            updCmd.ExecuteNonQuery();
                        }
                    }

                    string query = "INSERT INTO Reservations (TableName, CustomerName, Phone, Email, GuestCount, ReservationDate) VALUES (@tn, @cn, @ph, @em, @gc, @rd)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tn", _selectedTableName);
                    cmd.Parameters.AddWithValue("@cn", clientName);
                    cmd.Parameters.AddWithValue("@ph", phone);
                    cmd.Parameters.AddWithValue("@em", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@gc", (int)numGuestCount.Value);
                    cmd.Parameters.AddWithValue("@rd", selectedDate);
                    cmd.ExecuteNonQuery();
                }

                string msg = isNewClient ? "Успішно заброньовано!\n🎉 Новий клієнт доданий до бази. (Доступна знижка 20% на замовлення)" : "Успішно заброньовано!";
                frmModernMsgBox.Show(msg, "Успіх");

                SyncWithDatabase();
                UpdateTableVisuals();
            }
            catch (Exception ex) { frmModernMsgBox.Show("Error: " + ex.Message, "DB Error"); }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedTableName) || !mtbPhone.MaskCompleted || string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                frmModernMsgBox.Show("Заповніть коректно всі обов'язкові поля!", "Увага"); return;
            }

            DateTime selectedDate = dtpDate.Value.Date;
            string[] timeParts = cmbTime.SelectedItem.ToString().Split(':');
            selectedDate = selectedDate.AddHours(int.Parse(timeParts[0])).AddMinutes(int.Parse(timeParts[1]));

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = @"UPDATE Reservations 
                                     SET CustomerName = @cn, 
                                         Phone = @ph, 
                                         Email = @em, 
                                         GuestCount = @gc, 
                                         ReservationDate = @rd 
                                     WHERE TableName = @tn AND Status = 'Active'";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cn", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ph", mtbPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@em", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@gc", (int)numGuestCount.Value);
                    cmd.Parameters.AddWithValue("@rd", selectedDate);
                    cmd.Parameters.AddWithValue("@tn", _selectedTableName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        frmModernMsgBox.Show("Дані бронювання успішно оновлено!", "Успіх");
                        SyncWithDatabase();
                        UpdateTableVisuals();
                    }
                    else
                    {
                        frmModernMsgBox.Show("Не вдалося оновити дані. Можливо, бронювання скасовано.", "Помилка");
                    }
                }
            }
            catch (Exception ex) { frmModernMsgBox.Show("Error: " + ex.Message, "DB Error"); }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Скасувати бронь?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DbHelper.GetConnection())
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        string query = "UPDATE Reservations SET Status = 'Cancelled' WHERE TableName = @tn AND Status = 'Active'";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@tn", _selectedTableName);
                        cmd.ExecuteNonQuery();
                    }

                    _selectedTableName = "";
                    lblSelectedTable.Text = "Стіл: Не обрано";
                    txtCustomerName.Clear(); mtbPhone.Clear(); txtEmail.Clear();

                    // Скидаємо кнопки
                    btnCancel.Visible = false;
                    btnSaveChanges.Visible = false;
                    btnReserve.Visible = true;

                    SyncWithDatabase();
                    UpdateTableVisuals();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void AddReservedCardToBottom(string tableName, ReservationData data)
        {
            Button card = new Button() { Size = new Size(200, 100), BackColor = Color.Gold, FlatStyle = FlatStyle.Flat, Margin = new Padding(10), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            card.Text = $"{tableName}\n👤 {data.CustomerName}\n👥 {data.GuestCount} осіб\n🕒 {data.ResDate:HH:mm}";
            card.Click += (s, e) => { _selectedTableName = tableName; Table_Click(card, e); };
            RoundButtonCorners(card, 15);
            flpReservedBottom.Controls.Add(card);
        }

        private void RoundButtonCorners(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90); path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90); path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            btn.Region = new Region(path);
        }

        private void BtnGradient_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            using (LinearGradientBrush brush = new LinearGradientBrush(btn.ClientRectangle, Color.MidnightBlue, Color.Blue, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, btn.ClientRectangle);
            }
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}