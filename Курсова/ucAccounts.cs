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
    public partial class ucAccounts : UserControl
    {
        private DataGridView dgvAccounts;
        private Panel pnlPopup;
        private TextBox txtUsername, txtPassword;
        private ComboBox cmbRole;
        private Label lblPasswordTitle;
        private string actionType = "";
        private int selectedAccountId = 0;

        public ucAccounts()
        {
            try { InitializeComponent(); } catch { }

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            InitializeLayout();
            RefreshAccountList();
        }

        private void ucAccounts_Load(object sender, EventArgs e)
        {
        }

        private void InitializeLayout()
        {
            Panel pnlTop = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.White };
            Label lblTitle = new Label() { Text = "👥 Керування акаунтами", Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 50), Location = new Point(30, 20), AutoSize = true };
            pnlTop.Controls.Add(lblTitle);

            Panel pnlRightButton = new Panel() { Dock = DockStyle.Right, Width = 260, BackColor = Color.Transparent };
            Button btnAddNew = new Button()
            {
                Text = "➕ Додати персонал",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(20, 17),
                Cursor = Cursors.Hand
            };
            btnAddNew.FlatAppearance.BorderSize = 0;

            int radius = 20;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btnAddNew.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btnAddNew.Width - radius, btnAddNew.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btnAddNew.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btnAddNew.Region = new Region(path);

            btnAddNew.Click += (s, e) => OpenPopup("Add", 0, "", "", "");
            pnlRightButton.Controls.Add(btnAddNew);
            pnlTop.Controls.Add(pnlRightButton);
            this.Controls.Add(pnlTop);

            dgvAccounts = new DataGridView();
            dgvAccounts.Dock = DockStyle.Fill;
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.BorderStyle = BorderStyle.None;
            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAccounts.RowHeadersVisible = false;

            dgvAccounts.ReadOnly = true;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;

            dgvAccounts.AllowUserToResizeRows = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.DefaultCellStyle.Font = new Font("Segoe UI", 12F);
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            dgvAccounts.RowTemplate.Height = 60;
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.ColumnHeadersHeight = 60;
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 247);
            dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 249, 251);

            dgvAccounts.CellClick += DgvAccounts_CellClick;
            dgvAccounts.CellFormatting += DgvAccounts_CellFormatting;

            this.Controls.Add(dgvAccounts);
            dgvAccounts.BringToFront();

            SetupPopup();
        }

        private void SetupPopup()
        {
            pnlPopup = new Panel() { Size = new Size(450, 450), BackColor = Color.FromArgb(240, 240, 245), Visible = false, BorderStyle = BorderStyle.FixedSingle };

            Label lblPopupTitle = new Label() { Text = "Дані персоналу", Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };

            Label l1 = new Label() { Text = "Ім'я користувача:", Font = new Font("Segoe UI", 12), Location = new Point(30, 80), AutoSize = true };
            txtUsername = new TextBox() { Font = new Font("Segoe UI", 16), Location = new Point(30, 110), Width = 390 };

            lblPasswordTitle = new Label() { Text = "Новий пароль (залиште пустим):", Font = new Font("Segoe UI", 12), Location = new Point(30, 160), AutoSize = true };
            txtPassword = new TextBox() { Font = new Font("Segoe UI", 16), Location = new Point(30, 190), Width = 390, PasswordChar = '•' };

            Label l3 = new Label() { Text = "Посада / Права:", Font = new Font("Segoe UI", 12), Location = new Point(30, 240), AutoSize = true };
            cmbRole = new ComboBox() { Font = new Font("Segoe UI", 16), Location = new Point(30, 270), Width = 390, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRole.Items.AddRange(new string[] { "Admin", "Працивнік", "Кухар" });

            Button btnSave = new Button() { Text = "ЗБЕРЕГТИ", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(185, 50), Location = new Point(30, 350), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button() { Text = "СКАСУВАТИ", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(185, 50), Location = new Point(235, 350), Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { pnlPopup.Visible = false; dgvAccounts.Enabled = true; };

            pnlPopup.Controls.AddRange(new Control[] { lblPopupTitle, l1, txtUsername, lblPasswordTitle, txtPassword, l3, cmbRole, btnSave, btnCancel });
            this.Controls.Add(pnlPopup);
        }

        private void OpenPopup(string type, int id, string user, string pass, string role)
        {
            actionType = type;
            selectedAccountId = id;

            if (type == "Edit")
            {
                lblPasswordTitle.Text = "Новий пароль (залиште пустим):";
                txtUsername.Text = user;
                txtPassword.Text = "";

                if (cmbRole.Items.Contains(role)) cmbRole.SelectedItem = role;
                else cmbRole.SelectedIndex = 1;
            }
            else
            {
                lblPasswordTitle.Text = "Пароль:";
                txtUsername.Clear();
                txtPassword.Clear();
                cmbRole.SelectedIndex = 1;
            }

            txtUsername.Enabled = true;
            cmbRole.Enabled = true;

            pnlPopup.Location = new Point((this.Width - pnlPopup.Width) / 2, (this.Height - pnlPopup.Height) / 2);
            pnlPopup.Visible = true;
            pnlPopup.BringToFront();
            dgvAccounts.Enabled = false;
        }

        private void RefreshAccountList()
        {
            dgvAccounts.Columns.Clear();
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn == null || conn.State != ConnectionState.Open) return;

                // ДОДАНО: ORDER BY Id ASC, щоб черга завжди йшла від початку
                string query = "SELECT Id, Username AS Користувач, password_hash AS Пароль, Role AS Посада FROM Users ORDER BY Id ASC";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns.Add("№", typeof(int)).SetOrdinal(0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["№"] = i + 1;
                    }

                    dgvAccounts.DataSource = dt;
                }
            }

            AddActionButton("Edit", "✎", Color.Orange);
            AddActionButton("Delete", "🗑", Color.Crimson);

            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvAccounts.Columns["№"] != null)
            {
                dgvAccounts.Columns["№"].Width = 50;
                dgvAccounts.Columns["№"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvAccounts.Columns["Id"] != null) dgvAccounts.Columns["Id"].Visible = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, заповніть обов'язкові поля!", "Помилка");
                return;
            }

            if (actionType == "Add" && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Для нового користувача пароль обов'язковий!", "Помилка");
                return;
            }

            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn == null || conn.State != ConnectionState.Open) return;

                string query = "";
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());

                    if (actionType == "Add")
                    {
                        query = "INSERT INTO Users (Username, password_hash, Role) VALUES (@user, @pass, @role)";
                        cmd.Parameters.AddWithValue("@pass", SecurityHelper.HashPassword(txtPassword.Text.Trim()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@id", selectedAccountId);

                        if (string.IsNullOrWhiteSpace(txtPassword.Text))
                        {
                            query = "UPDATE Users SET Username=@user, Role=@role WHERE Id=@id";
                        }
                        else
                        {
                            query = "UPDATE Users SET Username=@user, password_hash=@pass, Role=@role WHERE Id=@id";
                            cmd.Parameters.AddWithValue("@pass", SecurityHelper.HashPassword(txtPassword.Text.Trim()));
                        }
                    }

                    cmd.CommandText = query;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Дані успішно збережено!", "Успіх");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка збереження!\n" + ex.Message, "Помилка");
                        return;
                    }
                }
            }

            pnlPopup.Visible = false;
            dgvAccounts.Enabled = true;
            RefreshAccountList();
        }

        private void DgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            int id = Convert.ToInt32(dgvAccounts.Rows[e.RowIndex].Cells["Id"].Value);
            string user = dgvAccounts.Rows[e.RowIndex].Cells["Користувач"].Value.ToString();
            string role = dgvAccounts.Rows[e.RowIndex].Cells["Посада"].Value.ToString();
            string clickedCol = dgvAccounts.Columns[e.ColumnIndex].HeaderText;

            if (clickedCol == "Редагувати") OpenPopup("Edit", id, user, "", role);
            else if (clickedCol == "Видалити")
            {
                if (role == "Admin" && CountAdmins() <= 1)
                {
                    MessageBox.Show("Повинен залишитися хоча б один Admin!", "Помилка");
                    return;
                }
                if (MessageBox.Show($"Видалити {user}?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    DeleteUser(id);
            }
        }

        private void DeleteUser(int id)
        {
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn == null || conn.State != ConnectionState.Open) return;
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Users WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            RefreshAccountList();
        }

        private int CountAdmins()
        {
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn == null || conn.State != ConnectionState.Open) return 0;
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'Admin'", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void AddActionButton(string name, string icon, Color color)
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = name;
            btn.HeaderText = (name == "Edit") ? "Редагувати" : "Видалити";
            btn.Text = icon;
            btn.UseColumnTextForButtonValue = true;
            btn.Width = 100;
            btn.FlatStyle = FlatStyle.Flat;
            btn.DefaultCellStyle.ForeColor = color;
            dgvAccounts.Columns.Add(btn);
        }

        private void DgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAccounts.Columns[e.ColumnIndex].Name == "Посада" && e.Value != null)
            {
                string role = e.Value.ToString();
                if (role == "Admin") e.CellStyle.ForeColor = Color.Crimson;
                else if (role == "Кухар") e.CellStyle.ForeColor = Color.DarkOrange;
                else e.CellStyle.ForeColor = Color.Teal;
            }
        }
    }
}