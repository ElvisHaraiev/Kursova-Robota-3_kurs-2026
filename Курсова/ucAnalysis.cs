using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Data;

namespace Курсова
{
    public partial class ucAnalysis : UserControl
    {
        private FlowLayoutPanel pnlCategories;
        private Label lblCash, lblCard, lblOther, lblTotalRevenue, lblPeriodDate;
        private Label lblServiceCount, lblPackageCount, lblSafeTotal;
        private Panel pnlRevenue;

        public ucAnalysis()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            BuildDesign();

            SyncWithDatabase();
            UpdateData("Підсумок дня");
        }

        private void BuildDesign()
        {
            this.BackColor = Color.White;
            this.Size = new Size(1600, 900);

            Panel pnlTabs = new Panel() { Dock = DockStyle.Top, Height = 70 };
            string[] tabLabels = { "Підсумок дня", "Тижневий звіт", "Місячний звіт" };
            int tabXLocation = 50;

            foreach (var label in tabLabels)
            {
                Button btnTab = new Button()
                {
                    Text = label,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    Size = new Size(220, 50),
                    Location = new Point(tabXLocation, 10),
                    Cursor = Cursors.Hand
                };

                btnTab.FlatAppearance.BorderSize = 0;

                btnTab.Click += (s, e) => { UpdateData(((Button)s).Text); };

                pnlTabs.Controls.Add(btnTab);
                tabXLocation += 240;
            }
            this.Controls.Add(pnlTabs);

            lblServiceCount = CreateSummaryCard(50, 100, "Обслуговування", "🍽️", 420);
            lblPackageCount = CreateSummaryCard(490, 100, "Доставка", "📦", 420);
            lblSafeTotal = CreateSummaryCard(930, 100, "Каса", "💰", 420, true);

            pnlRevenue = new Panel()
            {
                Location = new Point(1150, 300),
                Size = new Size(400, 550),
                BackColor = Color.White
            };

            pnlRevenue.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                GraphicsPath path = RoundedRect(new Rectangle(0, 0, pnlRevenue.Width - 1, pnlRevenue.Height - 1), 35);
                e.Graphics.DrawPath(new Pen(Color.Cyan, 5), path);
            };

            Label lblRevenueTitle = new Label()
            {
                Text = "Дохід",
                Font = new Font("Segoe UI", 22, FontStyle.Bold | FontStyle.Underline),
                Location = new Point(40, 40),
                AutoSize = true
            };
            pnlRevenue.Controls.Add(lblRevenueTitle);

            lblCash = AddRevenueRow(pnlRevenue, "💵 Готівка", 140);
            lblCard = AddRevenueRow(pnlRevenue, "💳 Картка", 220);
            lblOther = AddRevenueRow(pnlRevenue, "💬 Інше", 300);

            lblPeriodDate = new Label()
            {
                Text = "🗓️ Дата: --",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(40, 370),
                Size = new Size(320, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlRevenue.Controls.Add(lblPeriodDate);

            Panel pnlTotalBar = new Panel() { BackColor = Color.FromArgb(230, 230, 230), Size = new Size(340, 100), Location = new Point(20, 430) };
            Label lblTotalLabelText = new Label() { Text = "Всього", Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(20, 30), AutoSize = true };
            lblTotalRevenue = new Label() { Text = "0.0 ₴", Font = new Font("Segoe UI", 24, FontStyle.Bold), Location = new Point(130, 25), Size = new Size(180, 50), TextAlign = ContentAlignment.MiddleRight };

            pnlTotalBar.Controls.Add(lblTotalLabelText);
            pnlTotalBar.Controls.Add(lblTotalRevenue);
            pnlRevenue.Controls.Add(pnlTotalBar);
            this.Controls.Add(pnlRevenue);

            Label lblCategorySectionTitle = new Label()
            {
                Text = "Аналіз категорій (продано шт.)",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(50, 280),
                AutoSize = true,
                ForeColor = Color.FromArgb(40, 40, 40)
            };
            this.Controls.Add(lblCategorySectionTitle);

            pnlCategories = new FlowLayoutPanel()
            {
                Location = new Point(50, 340),
                Size = new Size(1050, 500),
                AutoScroll = true
            };
            this.Controls.Add(pnlCategories);
        }

        private Label AddRevenueRow(Panel parent, string title, int yPos)
        {
            Label lblT = new Label() { Text = title, Font = new Font("Segoe UI", 16), Location = new Point(40, yPos), AutoSize = true };
            Label lblV = new Label() { Text = "0", Font = new Font("Segoe UI", 16, FontStyle.Bold), Size = new Size(130, 40), TextAlign = ContentAlignment.MiddleRight, Location = new Point(210, yPos - 5) };
            parent.Controls.Add(lblT);
            parent.Controls.Add(lblV);
            return lblV;
        }

        private Label CreateSummaryCard(int x, int y, string title, string icon, int width, bool isSafeCard = false)
        {
            Panel cardPanel = new Panel() { Location = new Point(x, y), Size = new Size(width, 160), BackColor = Color.White };
            cardPanel.Paint += (s, e) => { e.Graphics.DrawPath(new Pen(Color.LightGray, 3), RoundedRect(new Rectangle(0, 0, cardPanel.Width - 1, cardPanel.Height - 1), 30)); };

            Label lblTitle = new Label() { Text = title, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.Crimson, Location = new Point(30, 25), AutoSize = true };
            Label lblIcon = new Label() { Text = icon, Font = new Font("Segoe UI", 45), Location = new Point(width - 110, 35), Size = new Size(100, 100) };
            Label lblVal = new Label() { Text = "0", Font = new Font("Segoe UI", 32, FontStyle.Bold), Location = new Point(30, 85), AutoSize = true };

            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(lblIcon);
            cardPanel.Controls.Add(lblVal);
            this.Controls.Add(cardPanel);
            return lblVal;
        }

        private void SyncWithDatabase()
        {
            try
            {
                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    if (Form1.MenuList.Count == 0)
                    {
                        string menuQuery = "SELECT Name, Price, Category FROM menuitems";
                        using (MySqlCommand cmdMenu = new MySqlCommand(menuQuery, conn))
                        using (MySqlDataReader readerMenu = cmdMenu.ExecuteReader())
                        {
                            while (readerMenu.Read())
                            {
                                Form1.MenuList.Add(new Form1.MenuItem
                                {
                                    Name = readerMenu["Name"].ToString(),
                                    Price = Convert.ToDouble(readerMenu["Price"]),
                                    Category = readerMenu["Category"].ToString()
                                });
                            }
                        }
                    }

                    Form1.SalesHistory.Clear();
                    string salesQuery = "SELECT * FROM Orders WHERE Status = 'Paid' OR Status = 'Completed'";

                    using (MySqlCommand cmdSales = new MySqlCommand(salesQuery, conn))
                    using (MySqlDataReader readerSales = cmdSales.ExecuteReader())
                    {
                        while (readerSales.Read())
                        {
                            DateTime orderDate = DateTime.Now;
                            try { orderDate = Convert.ToDateTime(readerSales["Date"]); } catch { try { orderDate = Convert.ToDateTime(readerSales["OrderDate"]); } catch { } }

                            Form1.SalesHistory.Add(new Form1.SalesRecord
                            {
                                Table = readerSales["TableName"].ToString(),
                                Waiter = readerSales["WaiterName"].ToString(),
                                Products = readerSales["OrderDetails"].ToString(),
                                Total = readerSales["TotalAmount"].ToString() + " ₴",
                                PaymentMethod = readerSales["PaymentMethod"].ToString(),
                                Date = orderDate
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Sync Error: " + ex.Message);
            }
        }

        private void UpdateData(string timePeriod)
        {
            if (Form1.SalesHistory == null) return;

            DateTime anchorDate = DateTime.Now.Date;

            var filteredSales = Form1.SalesHistory.Where(record => {
                if (timePeriod == "Підсумок дня") return record.Date.Date == anchorDate;
                if (timePeriod == "Тижневий звіт") return record.Date.Date >= anchorDate.AddDays(-7);
                return record.Date.Date >= anchorDate.AddDays(-30);
            }).ToList();

            CultureInfo culture = new CultureInfo("uk-UA");

            if (timePeriod == "Підсумок дня")
                lblPeriodDate.Text = "🗓️ День: " + anchorDate.ToString("dd.MM.yyyy");
            else if (timePeriod == "Тижневий звіт")
                lblPeriodDate.Text = "🗓️ Тиждень: " + anchorDate.AddDays(-7).ToString("dd.MM") + " - " + anchorDate.ToString("dd.MM.yyyy");
            else
                lblPeriodDate.Text = "🗓️ Місяць: " + anchorDate.ToString("MMMM yyyy", culture);

            pnlCategories.Controls.Clear();
            var distinctCategories = Form1.MenuList.Select(m => m.Category).Distinct().ToList();

            foreach (var category in distinctCategories)
            {
                int totalSoldQty = 0;
                foreach (var sale in filteredSales)
                {
                    var productLines = sale.Products.Split('\n');
                    foreach (var line in productLines)
                    {
                        var itemMatch = Form1.MenuList.FirstOrDefault(m => line.Contains(m.Name));
                        if (itemMatch != null && itemMatch.Category == category)
                        {
                            int qty = 1;
                            if (line.Contains("x")) int.TryParse(line.Split('x')[0], out qty);
                            totalSoldQty += qty;
                        }
                    }
                }

                Panel catPanel = new Panel() { Size = new Size(330, 130), Margin = new Padding(20), BackColor = Color.FromArgb(248, 248, 248) };
                catPanel.Paint += (s, e) => { e.Graphics.DrawPath(new Pen(Color.Gainsboro, 3), RoundedRect(new Rectangle(0, 0, catPanel.Width - 1, catPanel.Height - 1), 20)); };

                Panel statusDot = new Panel() { Size = new Size(20, 20), Location = new Point(20, 20), BackColor = Color.FromArgb((category.GetHashCode() & 0x7F) + 120, (category.GetHashCode() & 0x7F00) >> 8, (category.GetHashCode() & 0x7F0000) >> 16) };
                Label lName = new Label() { Text = category, Location = new Point(45, 17), AutoSize = true, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.DimGray };
                Label lCount = new Label() { Text = totalSoldQty.ToString() + " шт.", Location = new Point(45, 55), AutoSize = true, Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor = Color.Black };

                catPanel.Controls.Add(statusDot);
                catPanel.Controls.Add(lName);
                catPanel.Controls.Add(lCount);
                pnlCategories.Controls.Add(catPanel);
            }

            double cashSum = 0, cardSum = 0, otherSum = 0;
            int serviceTotal = 0, deliveryTotal = 0;

            foreach (var record in filteredSales)
            {
                double parsedAmount = 0;
                string amountRaw = record.Total.Replace("TL", "").Replace("₺", "").Replace("$", "").Replace("₴", "").Trim();
                double.TryParse(amountRaw, out parsedAmount);

                string method = record.PaymentMethod.ToUpper();

                if (method.Contains("ГОТІВКА") || method.Contains("CASH")) cashSum += parsedAmount;
                else if (method.Contains("КАРТКА") || method.Contains("CARD")) cardSum += parsedAmount;
                else if (method.Contains("ПРИ ОТРИМАННІ") || method.Contains("ОТРИМАННІ")) otherSum += parsedAmount;
                else otherSum += parsedAmount;

                string tableNameToCheck = record.Table.ToUpper();
                if (tableNameToCheck.Contains("ДОСТАВКА") || tableNameToCheck.Contains("📦") || tableNameToCheck.Contains("PAKET") || tableNameToCheck.Contains("DELIVERY"))
                {
                    deliveryTotal++;
                }
                else
                {
                    serviceTotal++;
                }
            }

            lblCash.Text = cashSum.ToString("N2") + " ₴";
            lblCard.Text = cardSum.ToString("N2") + " ₴";
            lblOther.Text = otherSum.ToString("N2") + " ₴";

            double revenueGrandTotal = cashSum + cardSum + otherSum;
            lblTotalRevenue.Text = revenueGrandTotal.ToString("N1") + " ₴";

            lblServiceCount.Text = serviceTotal.ToString();
            lblPackageCount.Text = deliveryTotal.ToString();
            lblSafeTotal.Text = revenueGrandTotal.ToString("N1") + " ₴";
        }

        public GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ucAnalizler_Load(object sender, EventArgs e)
        {
        }
    }
}