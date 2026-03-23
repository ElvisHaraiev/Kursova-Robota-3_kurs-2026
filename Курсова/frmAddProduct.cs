using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public class frmAddProduct : Form
    {
        public Form1.MenuItem NewItem { get; private set; }
        private Form1.MenuItem ItemToEdit;
        private string originalName = "";

        private TextBox txtName;
        private TextBox txtPrice;
        private ComboBox cmbCategory;

        public frmAddProduct(Form1.MenuItem existingItem = null)
        {
            ItemToEdit = existingItem;
            if (existingItem != null) originalName = existingItem.Name;

            this.Text = existingItem == null ? "Додати новий товар" : "Редагувати товар";

            this.Size = new Size(500, 600);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = Color.WhiteSmoke;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlCenter = new Panel();
            pnlCenter.Dock = DockStyle.Fill;
            this.Controls.Add(pnlCenter);

            Label lblHeader = new Label() { Text = existingItem == null ? "Додати новий товар" : "Редагувати товар", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.DarkViolet, Location = new Point(40, 20), AutoSize = true };
            pnlCenter.Controls.Add(lblHeader);

            Label lblName = new Label() { Text = "Назва товару:", Location = new Point(40, 90), AutoSize = true, Font = new Font("Segoe UI", 12) };
            txtName = new TextBox() { Location = new Point(40, 120), Width = 400, Font = new Font("Segoe UI", 18) };
            pnlCenter.Controls.Add(lblName); pnlCenter.Controls.Add(txtName);

            Label lblPrice = new Label() { Text = "Ціна (₴):", Location = new Point(40, 190), AutoSize = true, Font = new Font("Segoe UI", 12) };
            txtPrice = new TextBox() { Location = new Point(40, 220), Width = 400, Font = new Font("Segoe UI", 18) };
            pnlCenter.Controls.Add(lblPrice); pnlCenter.Controls.Add(txtPrice);

            Label lblCategory = new Label() { Text = "Категорія:", Location = new Point(40, 290), AutoSize = true, Font = new Font("Segoe UI", 12) };
            pnlCenter.Controls.Add(lblCategory);

            cmbCategory = new ComboBox() { Location = new Point(40, 320), Width = 290, Font = new Font("Segoe UI", 18), DropDownStyle = ComboBoxStyle.DropDownList };
            pnlCenter.Controls.Add(cmbCategory);

            Button btnAddCategory = new Button() { Text = "+", Location = new Point(340, 320), Width = 45, Height = 38, BackColor = Color.MediumSeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 14, FontStyle.Bold), Cursor = Cursors.Hand, TextAlign = ContentAlignment.MiddleCenter };
            btnAddCategory.FlatAppearance.BorderSize = 0;
            btnAddCategory.Click += BtnAddCategory_Click;
            pnlCenter.Controls.Add(btnAddCategory);

            Button btnRemoveCategory = new Button() { Text = "-", Location = new Point(395, 320), Width = 45, Height = 38, BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 16, FontStyle.Bold), Cursor = Cursors.Hand, TextAlign = ContentAlignment.MiddleCenter };
            btnRemoveCategory.FlatAppearance.BorderSize = 0;
            btnRemoveCategory.Click += BtnRemoveCategory_Click;
            pnlCenter.Controls.Add(btnRemoveCategory);

            LoadCategoriesIntoComboBox();

            Button btnCancel = new Button() { Text = "Скасувати", Location = new Point(40, 430), Width = 190, Height = 50, BackColor = Color.LightGray, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12), Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            pnlCenter.Controls.Add(btnCancel);

            Button btnSave = new Button() { Text = existingItem == null ? "Зберегти" : "Оновити", Location = new Point(250, 430), Width = 190, Height = 50, BackColor = Color.DeepPink, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            pnlCenter.Controls.Add(btnSave);

            if (ItemToEdit != null)
            {
                txtName.Text = ItemToEdit.Name;
                txtPrice.Text = ItemToEdit.Price.ToString();
                if (cmbCategory.Items.Contains(ItemToEdit.Category)) cmbCategory.SelectedItem = ItemToEdit.Category;
            }
            else { if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0; }
        }

        private void LoadCategoriesIntoComboBox()
        {
            if (cmbCategory == null) return;
            cmbCategory.Items.Clear();

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string query = "SELECT name FROM categories ORDER BY name ASC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCategory.Items.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            catch { }
        }

        private void BtnRemoveCategory_Click(object sender, EventArgs e)
        {
            string catToDelete = ShowCategoryDeleteDialog();

            if (!string.IsNullOrWhiteSpace(catToDelete))
            {
                DialogResult confirm = MessageBox.Show($"⚠️ УВАГА!\nВи збираєтесь видалити категорію '{catToDelete}'.\nУСІ СТРАВИ, які належать до цієї категорії, також будуть ВИДАЛЕНІ!\n\nВи впевнені?", "Небезпечна дія", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = DbHelper.GetConnection())
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();

                            string delItemsQuery = "DELETE FROM menuitems WHERE Category = @cat";
                            using (MySqlCommand cmdItems = new MySqlCommand(delItemsQuery, conn))
                            {
                                cmdItems.Parameters.AddWithValue("@cat", catToDelete);
                                cmdItems.ExecuteNonQuery();
                            }

                            string delCatQuery = "DELETE FROM categories WHERE name = @cat";
                            using (MySqlCommand cmdCat = new MySqlCommand(delCatQuery, conn))
                            {
                                cmdCat.Parameters.AddWithValue("@cat", catToDelete);
                                cmdCat.ExecuteNonQuery();
                            }
                        }

                        Form1.MenuList.RemoveAll(x => x.Category == catToDelete);

                        MessageBox.Show($"Категорію '{catToDelete}' та всі її страви успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadCategoriesIntoComboBox();
                        if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;
                        else cmbCategory.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка видалення: " + ex.Message, "Помилка");
                    }
                }
            }
        }

        private string ShowCategoryDeleteDialog()
        {
            Form promptForm = new Form() { Width = 450, Height = 250, FormBorderStyle = FormBorderStyle.FixedDialog, Text = "Видалити категорію", StartPosition = FormStartPosition.CenterParent, MaximizeBox = false, MinimizeBox = false, BackColor = Color.White };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Оберіть категорію для видалення:", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DarkSlateBlue };

            ComboBox cmbDel = new ComboBox() { Left = 20, Top = 60, Width = 390, Font = new Font("Segoe UI", 14), DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var item in cmbCategory.Items) cmbDel.Items.Add(item);
            if (cmbDel.Items.Count > 0) cmbDel.SelectedIndex = 0;

            Label warningLabel = new Label() { Left = 20, Top = 100, Text = "Будьте обережні! Видалення неможливо скасувати.", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = Color.DimGray };

            Button confirmation = new Button() { Text = "Видалити", Left = 270, Width = 140, Height = 45, Top = 140, DialogResult = DialogResult.OK, BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Click += (sender, ev) => { promptForm.Close(); };

            Button cancellation = new Button() { Text = "Скасувати", Left = 110, Width = 140, Height = 45, Top = 140, DialogResult = DialogResult.Cancel, BackColor = Color.LightGray, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12), Cursor = Cursors.Hand };
            cancellation.FlatAppearance.BorderSize = 0;
            cancellation.Click += (sender, ev) => { promptForm.Close(); };

            promptForm.Controls.Add(cmbDel);
            promptForm.Controls.Add(confirmation);
            promptForm.Controls.Add(cancellation);
            promptForm.Controls.Add(textLabel);
            promptForm.Controls.Add(warningLabel);

            return promptForm.ShowDialog() == DialogResult.OK ? cmbDel.Text : "";
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            string newCat = ShowInputBox("Введіть назву нової категорії:", "Нова категорія");

            if (!string.IsNullOrWhiteSpace(newCat))
            {
                try
                {
                    using (MySqlConnection conn = DbHelper.GetConnection())
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string checkQuery = "SELECT COUNT(*) FROM categories WHERE LOWER(name) = LOWER(@cat)";
                        using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@cat", newCat);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (count > 0)
                            {
                                MessageBox.Show("Ця категорія вже існує!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        string insQuery = "INSERT INTO categories (name) VALUES (@cat)";
                        using (MySqlCommand insCmd = new MySqlCommand(insQuery, conn))
                        {
                            insCmd.Parameters.AddWithValue("@cat", newCat);
                            insCmd.ExecuteNonQuery();
                        }
                    }

                    LoadCategoriesIntoComboBox();
                    cmbCategory.SelectedItem = newCat;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка додавання категорії: " + ex.Message, "Помилка");
                }
            }
        }

        private string ShowInputBox(string prompt, string title)
        {
            Form promptForm = new Form() { Width = 400, Height = 220, FormBorderStyle = FormBorderStyle.FixedDialog, Text = title, StartPosition = FormStartPosition.CenterParent, MaximizeBox = false, MinimizeBox = false, BackColor = Color.White };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = prompt, AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DarkSlateBlue };
            TextBox textBox = new TextBox() { Left = 20, Top = 60, Width = 340, Font = new Font("Segoe UI", 14) };
            Button confirmation = new Button() { Text = "ОК", Left = 260, Width = 100, Height = 45, Top = 110, DialogResult = DialogResult.OK, BackColor = Color.MediumSlateBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Click += (sender, ev) => { promptForm.Close(); };

            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(confirmation);
            promptForm.Controls.Add(textLabel);
            promptForm.AcceptButton = confirmation;

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля та оберіть категорію!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sanitizedPrice = txtPrice.Text.Replace(".", ",");

            if (!double.TryParse(sanitizedPrice, out double parsedPrice))
            {
                MessageBox.Show("Будь ласка, введіть коректну ціну!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newName = txtName.Text.Trim();
            string selectedCategory = cmbCategory.SelectedItem.ToString();

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    if (ItemToEdit != null)
                    {
                        string query = "UPDATE menuitems SET Name = @newName, Price = @newPrice, Category = @newCat WHERE Name = @oldName";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@newName", newName);
                            cmd.Parameters.AddWithValue("@newPrice", parsedPrice);
                            cmd.Parameters.AddWithValue("@newCat", selectedCategory);
                            cmd.Parameters.AddWithValue("@oldName", originalName);
                            cmd.ExecuteNonQuery();
                        }

                        ItemToEdit.Name = newName;
                        ItemToEdit.Price = parsedPrice;
                        ItemToEdit.Category = selectedCategory;
                        ItemToEdit.ImagePath = "";
                    }
                    else
                    {
                        string query = "INSERT INTO menuitems (Name, Price, Category) VALUES (@name, @price, @cat)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", newName);
                            cmd.Parameters.AddWithValue("@price", parsedPrice);
                            cmd.Parameters.AddWithValue("@cat", selectedCategory);
                            cmd.ExecuteNonQuery();
                        }

                        NewItem = new Form1.MenuItem
                        {
                            Name = newName,
                            Price = parsedPrice,
                            Category = selectedCategory,
                            ImagePath = ""
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка бази даних:\n" + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}