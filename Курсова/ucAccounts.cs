using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        private string actionType = "";

        private int selectedAccountId = 0;
        private string selectedAccountName = "";

        public ucAccounts()
        {
            try { InitializeComponent(); } catch { }

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            InitializeLayout();
            RefreshAccountList();
        }

        private void ucAccounts_Load(object sender, EventArgs e) { }

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
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
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
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToResizeRows = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.DefaultCellStyle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
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
            pnlPopup = new Panel() { Size = new Size(400, 450), BackColor = Color.FromArgb(240, 240, 245), Visible = false, BorderStyle = BorderStyle.FixedSingle };

            Label lblPopupTitle = new Label() { Text = "Дані персоналу", Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };

            Label l1 = new Label() { Text = "Ім'я користувача:", Font = new Font("Segoe UI", 12), Location = new Point(30, 80), AutoSize = true };
            txtUsername = new TextBox() { Font = new Font("Segoe UI", 16), Location = new Point(30, 110), Width = 340 };

            Label l2 = new Label() { Text = "Пароль:", Font = new Font("Segoe UI", 12), Location = new Point(30, 160), AutoSize = true };
            txtPassword = new TextBox() { Font = new Font("Segoe UI", 16), Location = new Point(30, 190), Width = 340 };

            Label l3 = new Label() { Text = "Посада / Права:", Font = new Font("Segoe UI", 12), Location = new Point(30, 240), AutoSize = true };
            cmbRole = new ComboBox() { Font = new Font("Segoe UI", 16), Location = new Point(30, 270), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

            // 🚀 AŞÇI (КУХАР) ROLÜ EKLENDİ!
            cmbRole.Items.AddRange(new string[] { "Admin", "Працівник", "Кухар" });

            Button btnSave = new Button() { Text = "ЗБЕРЕГТИ", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(160, 50), Location = new Point(30, 350), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button() { Text = "СКАСУВАТИ", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(160, 50), Location = new Point(210, 350), Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { pnlPopup.Visible = false; dgvAccounts.Enabled = true; };

            pnlPopup.Controls.AddRange(new Control[] { lblPopupTitle, l1, txtUsername, l2, txtPassword, l3, cmbRole, btnSave, btnCancel });
            this.Controls.Add(pnlPopup);
        }

        private void OpenPopup(string type, int id, string user, string pass, string role)
        {
            actionType = type;
            selectedAccountId = id;

            if (type == "Edit")
            {
                txtUsername.Text = user;
                txtPassword.Text = pass;

                if (cmbRole.Items.Contains(role)) cmbRole.SelectedItem = role;
                else cmbRole.SelectedIndex = 1;
            }
            else
            {
                txtUsername.Clear();
                txtPassword.Clear();
                cmbRole.SelectedIndex = 1;
            }

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

                string query = "SELECT Id, Username AS Користувач, Password AS Пароль, Role AS Посада FROM Users";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvAccounts.DataSource = dt;
                }
            }

            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.Columns["Id"].Visible = false;

            foreach (DataGridViewColumn col in dgvAccounts.Columns)
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            AddActionButton("Edit", "✏️", Color.Orange);
            AddActionButton("Delete", "🗑️", Color.Crimson);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || cmbRole.SelectedIndex == -1)
            {
                frmModernMsgBox.Show("Будь ласка, заповніть усі поля!", "Помилка");
                return;
            }

            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn == null || conn.State != ConnectionState.Open) return;

                string query = "";
                if (actionType == "Add")
                {
                    query = "INSERT INTO Users (Username, Password, Role) VALUES (@user, @pass, @role)";
                }
                else if (actionType == "Edit")
                {
                    query = "UPDATE Users SET Username=@user, Password=@pass, Role=@role WHERE Id=@id";
                }

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());

                    if (actionType == "Edit")
                        cmd.Parameters.AddWithValue("@id", selectedAccountId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        frmModernMsgBox.Show("Помилка збереження! Можливо, це ім'я вже існує.\n" + ex.Message, "Помилка");
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
            string pass = dgvAccounts.Rows[e.RowIndex].Cells["Пароль"].Value.ToString();
            string role = dgvAccounts.Rows[e.RowIndex].Cells["Посада"].Value.ToString();

            string clickedCol = dgvAccounts.Columns[e.ColumnIndex].HeaderText;

            if (clickedCol == "Редагувати")
            {
                OpenPopup("Edit", id, user, pass, role);
            }
            else if (clickedCol == "Видалити")
            {
                if (role == "Admin" && CountAdmins() <= 1)
                {
                    frmModernMsgBox.Show("У системі повинен залишитися хоча б один Admin! Ви не можете видалити цей акаунт.", "Помилка безпеки");
                    return;
                }

                if (frmModernMsgBox.Show($"Ви впевнені, що хочете видалити акаунт {user}?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteUser(id);
                }
            }
        }

        private void DeleteUser(int id)
        {
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn == null || conn.State != ConnectionState.Open) return;

                string query = "DELETE FROM Users WHERE Id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            RefreshAccountList();
        }

        private int CountAdmins()
        {
            int count = 0;
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    string query = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            return count;
        }

        private void AddActionButton(string name, string icon, Color color)
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = name;
            btn.HeaderText = name == "Edit" ? "Редагувати" : "Видалити";
            btn.Text = icon;
            btn.UseColumnTextForButtonValue = true;
            btn.Width = 80;
            btn.MinimumWidth = 80;
            btn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
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