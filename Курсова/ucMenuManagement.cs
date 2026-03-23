using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public partial class ucMenuManagement : UserControl
    {
        private TextBox txtDynamicSearch;

        public ucMenuManagement()
        {
            InitializeComponent();

            EventHandler searchBoxEvent = (s, e) =>
            {
                if (txtDynamicSearch != null)
                {
                    txtDynamicSearch.Visible = (this.Visible && this.Parent != null);
                }
            };

            this.VisibleChanged += searchBoxEvent;
            this.ParentChanged += searchBoxEvent;

            this.Load += ucMenuManagement_Load;
        }

        public frmAddProduct frmAddProduct { get => default; set { } }

        private void ucMenuManagement_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            AddSearchBoxToTopBar();
            LoadMenuFromDatabase();
            RefreshList();
        }

        private void LoadMenuFromDatabase()
        {
            Form1.MenuList.Clear();
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "SELECT Name, Price, Category FROM menuitems";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Form1.MenuList.Add(new Form1.MenuItem
                                {
                                    Name = reader["Name"].ToString(),
                                    Price = Convert.ToDouble(reader["Price"]),
                                    Category = reader["Category"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowModernAlert("Помилка завантаження меню: " + ex.Message, "Помилка", false);
            }
        }

        private void DeleteItemFromDatabase(string itemName, string categoryName)
        {
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "DELETE FROM menuitems WHERE Name = @name";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", itemName);
                        cmd.ExecuteNonQuery();
                    }

                    string checkCatQuery = "SELECT COUNT(*) FROM menuitems WHERE Category = @catName";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkCatQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@catName", categoryName);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            string delCatQuery = "DELETE FROM categories WHERE name = @catName";
                            using (MySqlCommand delCatCmd = new MySqlCommand(delCatQuery, conn))
                            {
                                delCatCmd.Parameters.AddWithValue("@catName", categoryName);
                                delCatCmd.ExecuteNonQuery();
                            }

                            if (Form1.Categories.Contains(categoryName))
                            {
                                Form1.Categories.Remove(categoryName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowModernAlert("Помилка видалення: " + ex.Message, "Помилка", false);
            }
        }

        private void AddSearchBoxToTopBar()
        {
            Form1 mainForm = (Form1)this.FindForm();

            if (mainForm != null)
            {
                Panel topBar = mainForm.Controls.OfType<Panel>().FirstOrDefault(p => p.Dock == DockStyle.Top);
                Control targetContainer = topBar != null ? (Control)topBar : (Control)mainForm;
                var oldBox = targetContainer.Controls.Find("TopSearchBox", true).FirstOrDefault();

                if (oldBox != null)
                {
                    targetContainer.Controls.Remove(oldBox);
                }

                txtDynamicSearch = new TextBox();
                txtDynamicSearch.Name = "TopSearchBox";
                txtDynamicSearch.Font = new Font("Segoe UI", 16);
                txtDynamicSearch.Width = 350;
                txtDynamicSearch.Location = new Point(targetContainer.Width - 800, (targetContainer.Height - txtDynamicSearch.Height) / 2);
                txtDynamicSearch.TextChanged += (s, ev) => RefreshList();

                targetContainer.Controls.Add(txtDynamicSearch);
                txtDynamicSearch.BringToFront();

                this.HandleDestroyed += (s, ev) =>
                {
                    if (txtDynamicSearch != null && targetContainer.Controls.Contains(txtDynamicSearch))
                    {
                        targetContainer.Controls.Remove(txtDynamicSearch);
                    }
                };
            }
        }

        public void RefreshList()
        {
            dgvMenu.DataSource = null;
            dgvMenu.Columns.Clear();

            var searchText = txtDynamicSearch?.Text?.ToLower()?.Trim() ?? "";

            var displayedList = Form1.MenuList
                .Where(x => x.Name.ToLower().Contains(searchText))
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Name = x.Name,
                    Price = x.Price,
                    Category = x.Category
                })
                .ToList();

            if (displayedList.Count > 0)
            {
                dgvMenu.DataSource = displayedList;
                dgvMenu.AllowUserToResizeRows = true;
                dgvMenu.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                dgvMenu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                dgvMenu.RowTemplate.Height = 60;

                dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvMenu.Columns["Name"].HeaderText = "Назва товару";
                dgvMenu.Columns["Price"].HeaderText = "Ціна (₴)";
                dgvMenu.Columns["Category"].HeaderText = "Категорія";

                foreach (DataGridViewColumn col in dgvMenu.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dgvMenu.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvMenu.BackgroundColor = Color.White;
                dgvMenu.BorderStyle = BorderStyle.None;
                dgvMenu.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgvMenu.GridColor = Color.FromArgb(220, 220, 220);
                dgvMenu.RowsDefaultCellStyle.BackColor = Color.White;
                dgvMenu.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                dgvMenu.RowHeadersVisible = true;
                dgvMenu.RowHeadersWidth = 50;
                dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvMenu.EnableHeadersVisualStyles = false;
                dgvMenu.ColumnHeadersHeight = 55;
                dgvMenu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 247);

                AddModernButton("Edit", "Редагувати", "✏️", Color.Orange);
                AddModernButton("Delete", "Видалити", "🗑️", Color.Crimson);
            }
        }

        private void AddModernButton(string name, string header, string icon, Color color)
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = name;
            btn.HeaderText = header;
            btn.Text = icon;
            btn.UseColumnTextForButtonValue = true;
            btn.Width = 60;
            btn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            btn.FlatStyle = FlatStyle.Flat;
            btn.DefaultCellStyle.ForeColor = color;
            dgvMenu.Columns.Add(btn);
        }

        private void dgvMenu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvMenu.Columns[e.ColumnIndex].Name == "Category" && e.Value != null)
            {
                string categoryStr = e.Value.ToString();
                if (categoryStr == "Головні страви") e.CellStyle.ForeColor = Color.DarkBlue;
                else if (categoryStr == "Напої") e.CellStyle.ForeColor = Color.Teal;
                else if (categoryStr == "Десерти") e.CellStyle.ForeColor = Color.Purple;
                else if (categoryStr == "Стейки") e.CellStyle.ForeColor = Color.DarkRed;
                e.CellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            }
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmAddProduct frm = new frmAddProduct())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadMenuFromDatabase();
                    RefreshList();
                    ShowModernAlert($"{frm.NewItem.Name} успішно додано!", "Інформація", false);
                }
            }
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string itemName = dgvMenu.Rows[e.RowIndex].Cells["Name"].Value.ToString();
            string itemCategory = dgvMenu.Rows[e.RowIndex].Cells["Category"].Value.ToString();
            double itemPrice = Convert.ToDouble(dgvMenu.Rows[e.RowIndex].Cells["Price"].Value);

            string clickedColumn = dgvMenu.Columns[e.ColumnIndex].Name;

            if (clickedColumn == "Delete")
            {
                if (ShowModernAlert($"{itemName} видалити? Ви впевнені?", "Підтвердження", true) == DialogResult.Yes)
                {
                    DeleteItemFromDatabase(itemName, itemCategory);
                    LoadMenuFromDatabase();
                    RefreshList();
                }
            }
            else if (clickedColumn == "Edit")
            {
                Form1.MenuItem selectedProduct = new Form1.MenuItem { Name = itemName, Price = itemPrice, Category = itemCategory };

                using (frmAddProduct frm = new frmAddProduct(selectedProduct))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadMenuFromDatabase();
                        RefreshList();
                        ShowModernAlert($"{itemName} успішно оновлено!", "Інформація", false);
                    }
                }
            }
        }

        private void dgvMenu_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var index = (e.RowIndex + 1).ToString();
            var font = new Font("Segoe UI", 10, FontStyle.Bold);
            var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgvMenu.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(index, font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private DialogResult ShowModernAlert(string message, string title, bool isQuestion)
        {
            Form alertForm = new Form();
            alertForm.FormBorderStyle = FormBorderStyle.None;
            alertForm.Size = new Size(550, 280);
            alertForm.BackColor = Color.FromArgb(30, 30, 40);
            alertForm.StartPosition = FormStartPosition.CenterParent;

            Label lblTitle = new Label() { Text = title, ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(30, 30), AutoSize = true };
            Label lblMessage = new Label() { Text = message, ForeColor = Color.Silver, Font = new Font("Segoe UI", 12), Location = new Point(30, 85), MaximumSize = new Size(500, 0), AutoSize = true };

            alertForm.Controls.Add(lblTitle);
            alertForm.Controls.Add(lblMessage);

            if (isQuestion)
            {
                Button btnYes = new Button() { Text = "ТАК", BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(200, 60), Location = new Point(80, 200), DialogResult = DialogResult.Yes };
                Button btnNo = new Button() { Text = "СКАСУВАТИ", BackColor = Color.FromArgb(70, 70, 80), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(200, 60), Location = new Point(310, 200), DialogResult = DialogResult.No };
                alertForm.Controls.Add(btnYes);
                alertForm.Controls.Add(btnNo);
            }
            else
            {
                Button btnOK = new Button() { Text = "ОК", BackColor = Color.MediumSlateBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(200, 60), Location = new Point(310, 200), DialogResult = DialogResult.OK };
                alertForm.Controls.Add(btnOK);
            }

            alertForm.Paint += (s, ev) => { ev.Graphics.DrawRectangle(new Pen(Color.MediumSlateBlue, 3), 0, 0, alertForm.Width - 1, alertForm.Height - 1); };

            return alertForm.ShowDialog();
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) { }
        private void cmbModern_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void pnlMain_Paint(object sender, PaintEventArgs e) { }
    }
}