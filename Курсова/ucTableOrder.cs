using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public partial class ucTableOrder : UserControl
    {
        public Button SelectedTableButton { get; set; }
        private double totalAmount = 0;
        private bool orderConfirmed = false;
        private Padding defaultPadding;

        public bool IsDelivery { get; set; } = false;

        public ucTableOrder()
        {
            InitializeComponent();
        }

        private void ucTableOrder_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            totalAmount = 0;
            orderConfirmed = false;

            flpProducts.SizeChanged += (s, ev) => AlignProductButtons();
            defaultPadding = flpProducts.Padding;

            LoadMenuFromDatabase();

            LoadCategoriesFromDatabase();

            btnAllCategories_Click(null, null);
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
                frmModernMsgBox.Show("Помилка завантаження меню з бази даних: " + ex.Message, "Помилка");
            }
        }

        private void LoadCategoriesFromDatabase()
        {
            if (flpCategories == null) return;

            flpCategories.Controls.Clear();

            Button btnAll = CreateCategoryButton("Всі страви", Color.Crimson);
            btnAll.Click += btnAllCategories_Click;
            flpCategories.Controls.Add(btnAll);

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "SELECT name FROM categories";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string catName = reader["name"].ToString();

                            Button btnCat = CreateCategoryButton(catName, Color.DarkSlateBlue);

                            btnCat.Click += (s, e) => FilterByCategory(catName);

                            flpCategories.Controls.Add(btnCat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                frmModernMsgBox.Show("Помилка завантаження категорій: " + ex.Message, "Помилка");
            }
        }

        private Button CreateCategoryButton(string text, Color bgColor)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(180, 60);
            btn.Margin = new Padding(5);
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            int radius = 15;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(btn.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(btn.Width - (radius * 2), btn.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(0, btn.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();
            btn.Region = new Region(path);

            return btn;
        }

        private void AddProductsByCategoryID(string categoryName)
        {
            var items = Form1.MenuList.Where(u => u.Category == categoryName).ToList();
            foreach (var item in items)
            {
                DisplayProductButton(item.Name, item.Price);
            }
        }

        private void AlignProductButtons()
        {
            if (flpProducts.Controls.Count == 0) return;

            Control first = flpProducts.Controls[0];
            int btnWidth = first.Width + first.Margin.Left + first.Margin.Right;
            int availableWidth = flpProducts.Width - SystemInformation.VerticalScrollBarWidth - flpProducts.Padding.Right;

            int columns = availableWidth / btnWidth;
            if (columns <= 0) columns = 1;

            int gridWidth = columns * btnWidth;
            int leftPad = (availableWidth - gridWidth) / 2;

            if (leftPad < 0) leftPad = 0;
            flpProducts.Padding = new Padding(leftPad, defaultPadding.Top, defaultPadding.Right, defaultPadding.Bottom);
        }

        private void DisplayProductButton(string name, double price)
        {
            Button btnItem = new Button();
            btnItem.Text = $"{name}\n{price} ₴";
            btnItem.Size = new Size(180, 180);
            btnItem.Margin = new Padding(5);
            btnItem.BackColor = Color.White;
            btnItem.FlatStyle = FlatStyle.Flat;
            btnItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnItem.Cursor = Cursors.Hand;
            btnItem.FlatAppearance.BorderSize = 0;

            int radius = 20;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btnItem.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btnItem.Width - radius, btnItem.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btnItem.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btnItem.Region = new Region(path);

            btnItem.Click += (s, ev) => {
                bool exists = false;

                for (int i = 0; i < lstCart.Items.Count; i++)
                {
                    if (lstCart.Items[i].ToString().Contains(name))
                    {
                        string currentText = lstCart.Items[i].ToString();
                        string[] parts = currentText.Split(new string[] { "x " }, 2, StringSplitOptions.None);

                        int qty = int.Parse(parts[0].Trim());
                        qty++;

                        lstCart.Items[i] = $"{qty}x {name} - {price} ₴";
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    lstCart.Items.Add($"1x {name} - {price} ₴");
                }

                totalAmount += price;
                lblPrice.Text = totalAmount.ToString("N2") + " ₴";
            };

            flpProducts.Controls.Add(btnItem);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (lstCart.SelectedIndex != -1)
            {
                int idx = lstCart.SelectedIndex;
                string line = lstCart.Items[idx].ToString();
                string[] parts = line.Split(new string[] { "x " }, 2, StringSplitOptions.None);

                int qty = int.Parse(parts[0].Trim());
                string itemPricePart = parts[1];
                string[] namePrice = itemPricePart.Split('-');

                string name = namePrice[0].Trim();
                double price = double.Parse(namePrice[1].Replace("₴", "").Trim());

                qty++;
                totalAmount += price;
                lstCart.Items[idx] = $"{qty}x {name} - {price} ₴";

                lblPrice.Text = totalAmount.ToString("N2") + " ₴";
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (lstCart.SelectedIndex != -1)
            {
                int idx = lstCart.SelectedIndex;
                string line = lstCart.Items[idx].ToString();
                string[] parts = line.Split(new string[] { "x " }, 2, StringSplitOptions.None);

                int qty = int.Parse(parts[0].Trim());
                string itemPricePart = parts[1];
                string[] namePrice = itemPricePart.Split('-');

                string name = namePrice[0].Trim();
                double price = double.Parse(namePrice[1].Replace("₴", "").Trim());

                if (qty == 1)
                {
                    if (frmModernMsgBox.Show($"Ви впевнені, що хочете видалити '{name}' з кошика?", "Видалення товару", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        totalAmount -= price;
                        if (totalAmount < 0) totalAmount = 0;
                        lstCart.Items.RemoveAt(idx);
                        lblPrice.Text = totalAmount.ToString("N2") + " ₴";
                    }
                }
                else
                {
                    qty--;
                    totalAmount -= price;
                    if (totalAmount < 0) totalAmount = 0;
                    lstCart.Items[idx] = $"{qty}x {name} - {price} ₴";
                    lblPrice.Text = totalAmount.ToString("N2") + " ₴";
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lstCart.Items.Count > 0)
            {
                if (frmModernMsgBox.Show("Ви впевнені, що хочете очистити кошик?", "Очистити", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lstCart.Items.Clear();
                    totalAmount = 0;
                    lblPrice.Text = "0.00 ₴";
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (lstCart.Items.Count == 0)
            {
                frmModernMsgBox.Show("Кошик порожній, будь ласка, виберіть товар!", "Увага");
                return;
            }

            if (this.IsDelivery)
            {
                Form1 mainForm = (Form1)this.FindForm();
                ucDeliveryDetails detailsPage = new ucDeliveryDetails();
                detailsPage.CartContent = string.Join("\n", lstCart.Items.Cast<object>());
                detailsPage.TotalAmount = totalAmount.ToString("N2") + " ₴";
                mainForm.ShowPage(detailsPage);
            }
            else
            {
                if (SelectedTableButton == null) { frmModernMsgBox.Show("Стіл не вибрано!", "Помилка"); return; }

                string tableName = SelectedTableButton.Text.Split('\n')[0];
                string orderDetails = string.Join("\n", lstCart.Items.Cast<object>());
                long newOrderId = 0;

                try
                {
                    using (MySqlConnection conn = DbHelper.GetConnection())
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string deleteQuery = "DELETE FROM Orders WHERE TableName = @tn AND Status = 'Pending'";
                        using (MySqlCommand delCmd = new MySqlCommand(deleteQuery, conn))
                        {
                            delCmd.Parameters.AddWithValue("@tn", tableName);
                            delCmd.ExecuteNonQuery();
                        }

                        string insertQuery = "INSERT INTO Orders (TableName, WaiterName, OrderDetails, TotalAmount, PaymentMethod, Status) " +
                                             "VALUES (@tn, @waiter, @details, @total, '-', 'Pending')";

                        using (MySqlCommand insCmd = new MySqlCommand(insertQuery, conn))
                        {
                            insCmd.Parameters.AddWithValue("@tn", tableName);
                            insCmd.Parameters.AddWithValue("@waiter", Form1.LoggedInUser);
                            insCmd.Parameters.AddWithValue("@details", orderDetails);
                            insCmd.Parameters.AddWithValue("@total", totalAmount);
                            insCmd.ExecuteNonQuery();

                            newOrderId = insCmd.LastInsertedId;
                        }

                        if (newOrderId > 0)
                        {
                            foreach (var item in lstCart.Items)
                            {
                                try
                                {
                                    string line = item.ToString();

                                    string[] parts = line.Split(new string[] { "x " }, 2, StringSplitOptions.None);
                                    int qty = int.Parse(parts[0].Trim());

                                    string itemPricePart = parts[1];
                                    int lastDashIndex = itemPricePart.LastIndexOf('-');

                                    string itemName = itemPricePart.Substring(0, lastDashIndex).Trim();
                                    double itemPrice = double.Parse(itemPricePart.Substring(lastDashIndex + 1).Replace("₴", "").Trim());

                                    string itemQuery = "INSERT INTO order_items (order_id, item_name, quantity, price) VALUES (@oid, @iname, @iqty, @iprice)";
                                    using (MySqlCommand itemCmd = new MySqlCommand(itemQuery, conn))
                                    {
                                        itemCmd.Parameters.AddWithValue("@oid", newOrderId);
                                        itemCmd.Parameters.AddWithValue("@iname", itemName);
                                        itemCmd.Parameters.AddWithValue("@iqty", qty);
                                        itemCmd.Parameters.AddWithValue("@iprice", itemPrice);
                                        itemCmd.ExecuteNonQuery();
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    frmModernMsgBox.Show("Помилка збереження: " + ex.Message, "Помилка");
                    return;
                }

                SelectedTableButton.BackColor = Color.Crimson;
                SelectedTableButton.ForeColor = Color.White;
                SelectedTableButton.Text = tableName + "\n\n" + totalAmount.ToString("N2") + " ₴";

                Form1.PendingOrders.Add(new Form1.KitchenOrder
                {
                    TableName = tableName,
                    WaiterName = Form1.LoggedInUser,
                    OrderDetails = orderDetails,
                    Time = DateTime.Now.ToString("HH:mm")
                });

                orderConfirmed = true;
                frmModernMsgBox.Show("Замовлення успішно відправлено на кухню!", "Підтверджено");

                _ = Task.Run(async () =>
                {
                    await Task.Delay(10000);

                    this.Invoke((MethodInvoker)delegate
                    {
                        frmModernMsgBox.Show($"🔔 Увага! Замовлення для столу '{tableName}' ГОТОВЕ! Можна забирати з кухні.", "Сповіщення з кухні");
                    });
                });
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (frmModernMsgBox.Show("Ви впевнені, що хочете скасувати та повернутися?", "Скасувати", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                GoBack();
            }
        }

        private void GoBack()
        {
            Form1 mainForm = (Form1)this.FindForm();
            mainForm.ShowPage(new ucTables());
        }

        public void LoadExistingOrders()
        {
            if (SelectedTableButton == null) return;
            string tableName = SelectedTableButton.Text.Split('\n')[0];

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "SELECT OrderDetails FROM Orders WHERE TableName = @tn AND Status = 'Pending'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tn", tableName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            bool hasOrders = false;

                            while (reader.Read())
                            {
                                hasOrders = true;
                                string details = reader["OrderDetails"].ToString();
                                string[] lines = details.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string line in lines)
                                {
                                    try
                                    {
                                        string[] parts = line.Split(new string[] { "x " }, 2, StringSplitOptions.None);
                                        int oldQty = int.Parse(parts[0].Trim());

                                        string[] namePrice = parts[1].Split('-');
                                        string name = namePrice[0].Trim();
                                        double unitPrice = double.Parse(namePrice[1].Replace("₴", "").Trim());

                                        bool exists = false;

                                        for (int i = 0; i < lstCart.Items.Count; i++)
                                        {
                                            if (lstCart.Items[i].ToString().Contains(name))
                                            {
                                                string currentText = lstCart.Items[i].ToString();
                                                string[] curParts = currentText.Split(new string[] { "x " }, 2, StringSplitOptions.None);
                                                int curQty = int.Parse(curParts[0].Trim());

                                                curQty += oldQty;
                                                lstCart.Items[i] = $"{curQty}x {name} - {unitPrice} ₴";
                                                exists = true;
                                                break;
                                            }
                                        }

                                        if (!exists)
                                        {
                                            lstCart.Items.Add($"{oldQty}x {name} - {unitPrice} ₴");
                                        }

                                        totalAmount += (oldQty * unitPrice);
                                    }
                                    catch { }
                                }
                            }

                            if (!hasOrders)
                            {
                                frmModernMsgBox.Show("Для цього столу немає активних замовлень.", "Інформація");
                            }
                            else
                            {
                                lblPrice.Text = totalAmount.ToString("N2") + " ₴";
                                frmModernMsgBox.Show("Попередні замовлення завантажено! Можете додавати нові.", "Успіх");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                frmModernMsgBox.Show("Помилка бази даних: " + ex.Message, "Помилка");
            }
        }

        private void FilterByCategory(string cat)
        {
            flpProducts.Controls.Clear();
            AddProductsByCategoryID(cat);
            AlignProductButtons();
        }

        private void btnAllCategories_Click(object sender, EventArgs e)
        {
            flpProducts.Controls.Clear();
            foreach (var item in Form1.MenuList.OrderBy(x => x.Category))
            {
                DisplayProductButton(item.Name, item.Price);
            }
            AlignProductButtons();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (lstCart.Items.Count == 0) return;

            if (!orderConfirmed && totalAmount > 0)
            {
                frmModernMsgBox.Show("Спочатку потрібно підтвердити замовлення!", "Увага");
                return;
            }

            flpProducts.Controls.Clear();

            Button btnCard = new Button { Text = "💳 КАРТКА", Size = new Size(220, 150), BackColor = Color.SkyBlue, Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCard.Click += (s, ev) => ProcessSale("КАРТКА");

            Button btnCash = new Button { Text = "💵 ГОТІВКА", Size = new Size(220, 150), BackColor = Color.LightGreen, Font = new Font("Segoe UI", 12, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCash.Click += (s, ev) => ProcessSale("ГОТІВКА");

            flpProducts.Controls.Add(btnCard);
            flpProducts.Controls.Add(btnCash);
            AlignProductButtons();
        }

        private void ProcessSale(string paymentType)
        {
            string tableName = SelectedTableButton != null ? SelectedTableButton.Text.Split('\n')[0] : "Замовлення";

            StringBuilder receipt = new StringBuilder();
            receipt.AppendLine("      ====================================");
            receipt.AppendLine("                MY RESTAURANT             ");
            receipt.AppendLine("      ====================================");
            receipt.AppendLine($" Дата     : {DateTime.Now:dd-MM-yyyy}");
            receipt.AppendLine($" Час      : {DateTime.Now:HH:mm}");
            receipt.AppendLine($" Стіл     : {tableName}");
            receipt.AppendLine($" Офіціант : {Form1.LoggedInUser}");
            receipt.AppendLine(" ------------------------------------------");
            receipt.AppendLine(string.Format(" {0,-20} {1,-5} {2,-8}", "Товар", "К-сть", "Сума"));
            receipt.AppendLine(" ------------------------------------------");

            foreach (var item in lstCart.Items)
            {
                receipt.AppendLine(item.ToString());
            }

            receipt.AppendLine(" ------------------------------------------");
            receipt.AppendLine($" РАЗОМ   : {totalAmount:N2} ₴");
            receipt.AppendLine($" ОПЛАТА  : {paymentType}");
            receipt.AppendLine(" ==========================================");

            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string updateQuery = "UPDATE Orders SET Status = 'Paid', PaymentMethod = @method WHERE TableName = @tn AND Status = 'Pending'";
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@method", paymentType);
                        cmd.Parameters.AddWithValue("@tn", tableName);
                        cmd.ExecuteNonQuery();
                    }

                    string resQuery = "UPDATE Reservations SET Status = 'Completed' WHERE TableName = @tn AND Status = 'Active'";
                    using (MySqlCommand resCmd = new MySqlCommand(resQuery, conn))
                    {
                        resCmd.Parameters.AddWithValue("@tn", tableName);
                        resCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                frmModernMsgBox.Show("DB Error: " + ex.Message, "Помилка");
                return;
            }

            Form1.SalesRecord record = new Form1.SalesRecord
            {
                Table = tableName,
                Waiter = Form1.LoggedInUser,
                Products = string.Join("\n", lstCart.Items.Cast<object>()),
                Total = totalAmount.ToString("N2") + " ₴",
                PaymentMethod = paymentType,
                FullReceiptText = receipt.ToString(),
                Date = DateTime.Now
            };

            Form1.SalesHistory.Add(record);

            if (GlobalData.ReservedTables.ContainsKey(tableName))
            {
                GlobalData.ReservedTables.Remove(tableName);
            }

            frmModernMsgBox.Show("Оплату прийнято, чек сформовано!", "Завершено");

            if (SelectedTableButton != null)
            {
                SelectedTableButton.BackColor = Color.White;
                SelectedTableButton.ForeColor = Color.Black;
                SelectedTableButton.Text = SelectedTableButton.Name.Replace("_", " ") + "\n\nВільний";
            }

            Form1 mainForm = (Form1)this.FindForm();
            mainForm.ShowPage(new ucHistory());
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void flpProducts_Paint(object sender, PaintEventArgs e) { }
        private void btnGoBack_Click(object sender, EventArgs e) { GoBack(); }
        private void listBox1_SelectedIndexChanged_2(object sender, EventArgs e) { }
        private void modernFlowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}