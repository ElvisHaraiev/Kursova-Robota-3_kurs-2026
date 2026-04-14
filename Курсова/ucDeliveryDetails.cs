using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсова
{
    public partial class ucDeliveryDetails : UserControl
    {
        public string CartContent { get; set; }
        public string TotalAmount { get; set; }

        private Panel pnlWrapper;
        private TextBox txtFullName;
        private MaskedTextBox mtbPhone;
        private TextBox txtStreet;
        private TextBox txtBuilding;
        private TextBox txtApt;
        private TextBox txtPromo;
        private Label lblOrderSummary;

        private decimal originalTotal = 0m;
        private decimal currentTotal = 0m;
        private decimal discountAmount = 0m;
        private bool isDiscountApplied = false;
        private string appliedPromoOrReason = "";

        public ucDeliveryDetails()
        {
            InitializeComponent();
        }

        private void ucDeliveryDetails_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;

            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Controls.Clear();

            if (!string.IsNullOrEmpty(TotalAmount))
            {
                decimal.TryParse(TotalAmount.Replace("₴", "").Trim(), out originalTotal);
                currentTotal = originalTotal;
            }

            CreateDeliveryUI();
            UpdateSummaryLabel();
        }

        private void CreateDeliveryUI()
        {
            pnlWrapper = new Panel();
            pnlWrapper.Size = new Size(1000, 650);
            this.Controls.Add(pnlWrapper);

            this.Resize += (s, e) => {
                pnlWrapper.Location = new Point((this.Width - pnlWrapper.Width) / 2, (this.Height - pnlWrapper.Height) / 2);
            };
            pnlWrapper.Location = new Point((this.Width - pnlWrapper.Width) / 2, (this.Height - pnlWrapper.Height) / 2);

            Label lblHeader = new Label() { Text = "📦 Інформація про доставку", Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, AutoSize = true, Location = new Point(0, 10) };
            pnlWrapper.Controls.Add(lblHeader);

            Label lblName = new Label() { Text = "👤 Ім'я та Прізвище:", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(0, 80) };
            txtFullName = new TextBox() { Font = new Font("Segoe UI", 16), Width = 400, Location = new Point(0, 110) };
            pnlWrapper.Controls.Add(lblName); pnlWrapper.Controls.Add(txtFullName); ModernizeControl(txtFullName, 10);

            Label lblPhone = new Label() { Text = "📞 Номер телефону:", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(0, 170) };
            mtbPhone = new MaskedTextBox() { Mask = "+38 (000) 000-00-00", Font = new Font("Segoe UI", 16), Width = 400, Location = new Point(0, 200) };
            pnlWrapper.Controls.Add(lblPhone); pnlWrapper.Controls.Add(mtbPhone); ModernizeControl(mtbPhone, 10);

            Label lblStreet = new Label() { Text = "🛣️ Вулиця:", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(0, 260) };
            txtStreet = new TextBox() { Font = new Font("Segoe UI", 16), Width = 400, Location = new Point(0, 290) };
            pnlWrapper.Controls.Add(lblStreet); pnlWrapper.Controls.Add(txtStreet); ModernizeControl(txtStreet, 10);

            Label lblBldg = new Label() { Text = "🏠 Будинок:", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(0, 350) };
            txtBuilding = new TextBox() { Font = new Font("Segoe UI", 16), Width = 150, Location = new Point(0, 380) };
            pnlWrapper.Controls.Add(lblBldg); pnlWrapper.Controls.Add(txtBuilding); ModernizeControl(txtBuilding, 10);

            Label lblApt = new Label() { Text = "🏢 Кв / Поверх:", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(200, 350) };
            txtApt = new TextBox() { Font = new Font("Segoe UI", 16), Width = 200, Location = new Point(200, 380) };
            pnlWrapper.Controls.Add(lblApt); pnlWrapper.Controls.Add(txtApt); ModernizeControl(txtApt, 10);

            Label lblPromo = new Label() { Text = "🎟️ Промокод:", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(0, 440) };
            txtPromo = new TextBox() { Font = new Font("Segoe UI", 16), Width = 250, Location = new Point(0, 470) };
            pnlWrapper.Controls.Add(lblPromo); pnlWrapper.Controls.Add(txtPromo); ModernizeControl(txtPromo, 10);

            Button btnApplyPromo = new Button() { Text = "Застосувати", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(130, 48), Location = new Point(270, 460), Cursor = Cursors.Hand };
            btnApplyPromo.FlatAppearance.BorderSize = 0;
            btnApplyPromo.Paint += (s, e) => PaintGradientButton((Button)s, e, Color.DarkOrange, Color.Orange, 10);
            btnApplyPromo.Click += BtnApplyPromo_Click;
            pnlWrapper.Controls.Add(btnApplyPromo);

            lblOrderSummary = new Label() { Font = new Font("Courier New", 12), Width = 450, Height = 430, Location = new Point(500, 80), BackColor = Color.White };
            pnlWrapper.Controls.Add(lblOrderSummary); ModernizeControl(lblOrderSummary, 15);

            Button btnCompleteOrder = new Button() { Text = "Завершити Замовлення", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(250, 60), Location = new Point(0, 560), Cursor = Cursors.Hand };
            btnCompleteOrder.FlatAppearance.BorderSize = 0;
            btnCompleteOrder.Click += btnCompleteOrder_Click;
            btnCompleteOrder.Paint += (s, e) => PaintGradientButton((Button)s, e, Color.MidnightBlue, Color.Blue, 15);
            pnlWrapper.Controls.Add(btnCompleteOrder);

            Button btnCancel = new Button() { Text = "Скасувати", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(150, 60), Location = new Point(260, 560), Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += btnCancel_Click;
            btnCancel.Paint += (s, e) => PaintGradientButton((Button)s, e, Color.DarkRed, Color.Red, 15);
            pnlWrapper.Controls.Add(btnCancel);
        }

        private void BtnApplyPromo_Click(object sender, EventArgs e)
        {
            IDiscountStrategy discountStrategy;

            if (txtPromo.Text.Trim().ToUpper() == "WELCOME2026")
            {
                if (!isDiscountApplied)
                {
                    discountStrategy = new PercentageDiscountStrategy(0.20m);
                    isDiscountApplied = true;
                    appliedPromoOrReason = "WELCOME2026";
                    discountAmount = discountStrategy.CalculateDiscountAmount(originalTotal);
                    currentTotal = originalTotal - discountAmount;

                    UpdateSummaryLabel();

                    ShowModernAlert("Знижка за промокодом (20%) успішно застосована!", "Успіх", false);
                }
                else
                {
                    ShowModernAlert("Знижка вже застосована!", "Увага", false);
                }
            }
            else
            {
                discountStrategy = new NoDiscountStrategy();
                ShowModernAlert("Недійсний або прострочений промокод!", "Помилка", false);
            }
        }

        private void UpdateSummaryLabel()
        {
            if (lblOrderSummary != null)
            {
                string discountText = isDiscountApplied
                    ? $"\nЗНИЖКА ({appliedPromoOrReason}): -{discountAmount:N2} ₴\n-------------------\nДО СПЛАТИ: {currentTotal:N2} ₴"
                    : $"\n-------------------\nРАЗОМ: {currentTotal:N2} ₴";

                lblOrderSummary.Text = "--- ДЕТАЛІ ЗАМОВЛЕННЯ ---\n\n" + CartContent + "\n" + discountText;
            }
        }

        private void PaintGradientButton(Button btn, PaintEventArgs e, Color c1, Color c2, int radius)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(btn.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(btn.Width - (radius * 2), btn.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(0, btn.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);

            using (LinearGradientBrush brush = new LinearGradientBrush(btn.ClientRectangle, c1, c2, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillPath(brush, path);
            }
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void btnCompleteOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || !mtbPhone.MaskCompleted || string.IsNullOrWhiteSpace(txtStreet.Text) || string.IsNullOrWhiteSpace(txtBuilding.Text))
            {
                ShowModernAlert("Будь ласка, заповніть усі необхідні поля!", "Увага", false);
                return;
            }

            string phone = mtbPhone.Text.Trim();
            string clientName = txtFullName.Text.Trim();
            string fullAddress = $"{txtStreet.Text}, Буд. {txtBuilding.Text}, Кв/Пов: {txtApt.Text}";

            IDiscountStrategy discountStrategy = new NoDiscountStrategy();
            bool isNewClient = false;
            string extraDiscountMsg = "";
            string deliveryName = "";
            long newOrderId = 0;
            string receiptDiscountText = "";
            string combinedDetails = "";

            Form1 mainForm = (Form1)this.FindForm();

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
                        string insQuery = "INSERT INTO clients (phone, name, address, client_type, total_orders) VALUES (@phone, @name, @address, 'Доставка', 1)";
                        using (MySqlCommand insCmd = new MySqlCommand(insQuery, conn))
                        {
                            insCmd.Parameters.AddWithValue("@phone", phone);
                            insCmd.Parameters.AddWithValue("@name", clientName);
                            insCmd.Parameters.AddWithValue("@address", fullAddress);
                            insCmd.ExecuteNonQuery();
                        }

                        if (!isDiscountApplied)
                        {
                            discountStrategy = new PercentageDiscountStrategy(0.20m);
                            decimal newDiscount = discountStrategy.CalculateDiscountAmount(originalTotal);
                            discountAmount += newDiscount;
                            currentTotal = originalTotal - discountAmount;
                            isDiscountApplied = true;
                            appliedPromoOrReason = "новий клієнт";
                            extraDiscountMsg = "\n🎉 Знижка 20% (Новий клієнт) успішно застосована!";
                        }
                    }
                    else
                    {
                        string updQuery = "UPDATE clients SET total_orders = total_orders + 1, address = @address, name = @name, client_type = 'Доставка' WHERE phone = @phone";
                        using (MySqlCommand updCmd = new MySqlCommand(updQuery, conn))
                        {
                            updCmd.Parameters.AddWithValue("@phone", phone);
                            updCmd.Parameters.AddWithValue("@address", fullAddress);
                            updCmd.Parameters.AddWithValue("@name", clientName);
                            updCmd.ExecuteNonQuery();
                        }
                    }

                    Random rnd = new Random();
                    int orderID = rnd.Next(1000, 10000);
                    deliveryName = "Доставка #" + orderID;

                    receiptDiscountText = isDiscountApplied ? $"\nЗНИЖКА ({appliedPromoOrReason}): -{discountAmount:N2} ₴\nДО СПЛАТИ: {currentTotal:N2} ₴" : $"\nРАЗОМ: {currentTotal:N2} ₴";
                    combinedDetails = $"Клієнт: {txtFullName.Text}\nТелефон: {mtbPhone.Text}\nАДРЕСА: {fullAddress}\n\n{CartContent}\n{receiptDiscountText}";

                    string query = "INSERT INTO Orders (TableName, WaiterName, OrderDetails, TotalAmount, PaymentMethod, Status) " +
                                   "VALUES (@table, @waiter, @details, @total, @method, 'Completed')";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", "📦 " + deliveryName);
                        cmd.Parameters.AddWithValue("@waiter", Form1.LoggedInUser);
                        cmd.Parameters.AddWithValue("@details", combinedDetails);
                        cmd.Parameters.AddWithValue("@total", currentTotal);
                        cmd.Parameters.AddWithValue("@method", "При Отриманні");
                        cmd.ExecuteNonQuery();

                        newOrderId = cmd.LastInsertedId;
                    }

                    if (newOrderId > 0 && !string.IsNullOrEmpty(CartContent))
                    {
                        string[] cartLines = CartContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string line in cartLines)
                        {
                            try
                            {
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

                    Form1.KitchenOrder kitchenTicket = new Form1.KitchenOrder
                    {
                        TableName = "📦 " + deliveryName,
                        WaiterName = Form1.LoggedInUser,
                        Time = DateTime.Now.ToString("HH:mm"),
                        OrderDetails = combinedDetails
                    };
                    Form1.PendingOrders.Add(kitchenTicket);

                    if (Form1.SalesHistory == null) Form1.SalesHistory = new System.Collections.Generic.List<Form1.SalesRecord>();
                    Form1.SalesRecord newSalesRecord = new Form1.SalesRecord
                    {
                        Table = "📦 " + deliveryName,
                        Waiter = Form1.LoggedInUser,
                        Date = DateTime.Now,
                        Total = $"{currentTotal:N2} ₴",
                        PaymentMethod = "При Отриманні",
                        Products = CartContent,
                        FullReceiptText = "Delivery ID: " + orderID + "\nName: " + txtFullName.Text + "\nPhone: " + mtbPhone.Text + "\nAddress: " + fullAddress + receiptDiscountText
                    };
                    Form1.SalesHistory.Add(newSalesRecord);
                }

                string safeDeliveryName = "📦 " + deliveryName;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(5000);

                    var order = Form1.PendingOrders.FirstOrDefault(o => o.TableName == safeDeliveryName);
                    if (order != null) order.IsReady = true;

                    if (mainForm != null && !mainForm.IsDisposed)
                    {
                        mainForm.ShowOrderReadyNotification($"🔔 Увага! Замовлення '{deliveryName}' ГОТОВЕ!\nКур'єр може забирати з кухні.");
                    }
                });

                ShowModernAlert($"Замовлення на доставку прийнято!{extraDiscountMsg}\nВаш номер замовлення: {newOrderId}", "Замовлення підтверджено", false);

                mainForm.ShowPage(new ucMainMenu());
            }
            catch (Exception ex)
            {
                ShowModernAlert("Помилка: " + ex.Message, "Помилка", false);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult response = ShowModernAlert("Ви впевнені, що хочете скасувати замовлення?", "Підтвердження скасування", true);

            if (response == DialogResult.Yes)
            {
                Form1 mainForm = (Form1)this.FindForm();
                if (mainForm != null) mainForm.ShowPage(new ucMainMenu());
            }
        }

        private DialogResult ShowModernAlert(string message, string title, bool isQuestion)
        {
            Form alertForm = new Form();
            alertForm.FormBorderStyle = FormBorderStyle.None;
            alertForm.StartPosition = FormStartPosition.CenterParent;
            alertForm.Size = new Size(550, 280);
            alertForm.BackColor = Color.FromArgb(30, 30, 40);

            Label lblTitle = new Label() { Text = title, ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(30, 30), AutoSize = true };
            Label lblMsg = new Label() { Text = message, ForeColor = Color.Silver, Font = new Font("Segoe UI", 12), Location = new Point(30, 85), MaximumSize = new Size(500, 0), AutoSize = true };

            alertForm.Controls.Add(lblTitle);
            alertForm.Controls.Add(lblMsg);

            if (isQuestion)
            {
                Button btnYes = new Button() { Text = "ТАК", BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Size = new Size(200, 60), Location = new Point(80, 200), DialogResult = DialogResult.Yes, Cursor = Cursors.Hand };
                btnYes.FlatAppearance.BorderSize = 0;
                Button btnNo = new Button() { Text = "НІ", BackColor = Color.FromArgb(70, 70, 80), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Size = new Size(200, 60), Location = new Point(310, 200), DialogResult = DialogResult.No, Cursor = Cursors.Hand };
                btnNo.FlatAppearance.BorderSize = 0;
                alertForm.Controls.Add(btnYes); alertForm.Controls.Add(btnNo);
            }
            else
            {
                Button btnOk = new Button() { Text = "ОК", BackColor = Color.MediumSlateBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Size = new Size(200, 60), Location = new Point(310, 200), DialogResult = DialogResult.OK, Cursor = Cursors.Hand };
                btnOk.FlatAppearance.BorderSize = 0;
                alertForm.Controls.Add(btnOk);
            }

            alertForm.Paint += (s, e) => { e.Graphics.DrawRectangle(new Pen(Color.MediumSlateBlue, 3), 0, 0, alertForm.Width - 1, alertForm.Height - 1); };
            return alertForm.ShowDialog();
        }

        private void ModernizeControl(Control control, int radius)
        {
            if (control is TextBox txt) { txt.BorderStyle = BorderStyle.None; txt.BackColor = Color.White; txt.Font = new Font("Segoe UI", 14); }
            if (control is MaskedTextBox mtb) { mtb.BorderStyle = BorderStyle.None; mtb.BackColor = Color.White; mtb.Font = new Font("Segoe UI", 14); }
            if (control is Label lbl) { lbl.BackColor = Color.White; }

            Panel containerPanel = new Panel();
            containerPanel.Size = new Size(control.Width + 20, control.Height + 20);
            containerPanel.Location = new Point(control.Location.X - 10, control.Location.Y - 10);
            containerPanel.BackColor = Color.White;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(containerPanel.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(containerPanel.Width - (radius * 2), containerPanel.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(0, containerPanel.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();
            containerPanel.Region = new Region(path);

            containerPanel.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Color.Gainsboro, 2), path);
            };

            control.Parent.Controls.Add(containerPanel);
            containerPanel.Controls.Add(control);

            control.Location = new Point(10, 10);
            control.Width = containerPanel.Width - 20;
            if (control is TextBox t && t.Multiline || control is Label)
            {
                control.Height = containerPanel.Height - 20;
            }
        }
    }
}