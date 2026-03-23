using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Курсова;

namespace Курсова
{

    public interface ICustomerRepository
    {
        DataTable GetAllCustomers();
        void UpdateCustomer(int id, string phone, string name, string address, string type);
        void DeleteCustomer(int id);
    }

    public class CustomerRepository : ICustomerRepository
    {
        public DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "SELECT id, phone, name, address, client_type, total_orders FROM clients ORDER BY total_orders DESC";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void UpdateCustomer(int id, string phone, string name, string address, string type)
        {
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "UPDATE clients SET phone = @phone, name = @name, address = @address, client_type = @type WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomer(int id)
        {
            using (MySqlConnection conn = DbHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "DELETE FROM clients WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }


    public partial class ucCustomers : UserControl
    {
        private ICustomerRepository _customerRepo;
        private DataGridView dgvCustomers;
        private Panel pnlEdit;

        private TextBox txtPhone;
        private TextBox txtName;
        private TextBox txtAddress;
        private ComboBox cmbType;
        private Label lblDiscountStatus;

        private int selectedCustomerId = -1;

        public ucCustomers()
        {
            InitializeComponent();
            _customerRepo = new CustomerRepository();
        }

        private void ucCustomers_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            this.BackColor = Color.FromArgb(245, 246, 250);
            CreateUI();
            LoadData();
        }

        private void CreateUI()
        {

            pnlEdit = new Panel() { Width = 350, Dock = DockStyle.Right, BackColor = Color.White };
            this.Controls.Add(pnlEdit);

            Label lblEditTitle = new Label() { Text = "Редагувати Клієнта", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, AutoSize = true, Location = new Point(20, 20) };
            pnlEdit.Controls.Add(lblEditTitle);


            Label lblType = new Label() { Text = "Тип Клієнта:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 80), AutoSize = true };
            cmbType = new ComboBox() { Font = new Font("Segoe UI", 12), Location = new Point(20, 105), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbType.Items.AddRange(new string[] { "Доставка", "Резервація" });
            cmbType.SelectedIndexChanged += (s, e) => { txtAddress.Enabled = (cmbType.Text == "Доставка"); }; 
            pnlEdit.Controls.Add(lblType); pnlEdit.Controls.Add(cmbType);


            Label lblPhone = new Label() { Text = "Телефон:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 150), AutoSize = true };
            txtPhone = new TextBox() { Font = new Font("Segoe UI", 12), Location = new Point(20, 175), Width = 300 };
            pnlEdit.Controls.Add(lblPhone); pnlEdit.Controls.Add(txtPhone);


            Label lblName = new Label() { Text = "Ім'я клієнта:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 220), AutoSize = true };
            txtName = new TextBox() { Font = new Font("Segoe UI", 12), Location = new Point(20, 245), Width = 300 };
            pnlEdit.Controls.Add(lblName); pnlEdit.Controls.Add(txtName);


            Label lblAddress = new Label() { Text = "Адреса (Тільки для доставки):", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 290), AutoSize = true };
            txtAddress = new TextBox() { Font = new Font("Segoe UI", 12), Location = new Point(20, 315), Width = 300, Height = 60, Multiline = true };
            pnlEdit.Controls.Add(lblAddress); pnlEdit.Controls.Add(txtAddress);


            lblDiscountStatus = new Label() { Text = "Статус знижки: Очікування...", Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = Color.DeepPink, Location = new Point(20, 390), AutoSize = true };
            pnlEdit.Controls.Add(lblDiscountStatus);

  
            Button btnUpdate = new Button() { Text = "💾 Зберегти Зміни", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 440), Width = 300, Height = 45, Cursor = Cursors.Hand };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;
            pnlEdit.Controls.Add(btnUpdate);

            Button btnDelete = new Button() { Text = "🗑️ Видалити", Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 500), Width = 300, Height = 45, Cursor = Cursors.Hand };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            pnlEdit.Controls.Add(btnDelete);


            Label lblHeader = new Label() { Text = "👥 Управління Клієнтами (CRM)", Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, AutoSize = true, Location = new Point(20, 20) };
            this.Controls.Add(lblHeader);

            dgvCustomers = new DataGridView()
            {
                Location = new Point(20, 80),
                Width = this.Width - pnlEdit.Width - 40,
                Height = this.Height - 100,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 12), SelectionBackColor = Color.MediumSlateBlue },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 12, FontStyle.Bold), BackColor = Color.FromArgb(240, 240, 240) }
            };
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.CellClick += DgvCustomers_CellClick;
            this.Controls.Add(dgvCustomers);
        }

        private void LoadData()
        {
            try
            {
                DataTable data = _customerRepo.GetAllCustomers();
                dgvCustomers.DataSource = data;

                dgvCustomers.Columns["id"].HeaderText = "ID";
                dgvCustomers.Columns["id"].Width = 50;
                dgvCustomers.Columns["phone"].HeaderText = "Телефон";
                dgvCustomers.Columns["name"].HeaderText = "Ім'я";
                dgvCustomers.Columns["address"].HeaderText = "Адреса";
                dgvCustomers.Columns["client_type"].HeaderText = "Тип";
                dgvCustomers.Columns["total_orders"].HeaderText = "Замовлення";
            }
            catch { }
        }

        private void DgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                selectedCustomerId = Convert.ToInt32(row.Cells["id"].Value);

                txtPhone.Text = row.Cells["phone"].Value?.ToString();
                txtName.Text = row.Cells["name"].Value?.ToString();
                txtAddress.Text = row.Cells["address"].Value?.ToString();

                string type = row.Cells["client_type"].Value?.ToString();
                if (cmbType.Items.Contains(type)) cmbType.SelectedItem = type;
                else cmbType.SelectedIndex = 0;

                int orders = Convert.ToInt32(row.Cells["total_orders"].Value);
                if (orders > 0)
                {
                    lblDiscountStatus.Text = "🎟️ Статус знижки: ВИКОРИСТАНА (20%)";
                    lblDiscountStatus.ForeColor = Color.MediumSeaGreen;
                }
                else
                {
                    lblDiscountStatus.Text = "🎟️ Статус знижки: ДОСТУПНА (Новий)";
                    lblDiscountStatus.ForeColor = Color.DarkOrange;
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == -1) return;
            if (string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Телефон та Ім'я не можуть бути порожніми!", "Увага");
                return;
            }

            try
            {
                _customerRepo.UpdateCustomer(selectedCustomerId, txtPhone.Text.Trim(), txtName.Text.Trim(), txtAddress.Text.Trim(), cmbType.Text);
                MessageBox.Show("Дані успішно оновлено!", "Успіх");
                LoadData();
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == -1) return;

            if (MessageBox.Show("Ви впевнені, що хочете видалити цього клієнта?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _customerRepo.DeleteCustomer(selectedCustomerId);
                    MessageBox.Show("Клієнта видалено!", "Успіх");
                    txtPhone.Clear(); txtName.Clear(); txtAddress.Clear(); selectedCustomerId = -1;
                    lblDiscountStatus.Text = "Статус знижки: Очікування...";
                    LoadData();
                }
                catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
            }
        }
    }
}