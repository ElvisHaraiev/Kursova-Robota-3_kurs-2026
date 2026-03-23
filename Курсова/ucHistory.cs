using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Курсова
{
    public partial class ucHistory : UserControl
    {
        PrintDocument pdHistory = new PrintDocument();
        Form1.SalesRecord selectedRecord;

        public ucHistory()
        {
            InitializeComponent();
        }

        private void ucHistory_Load(object sender, EventArgs e)
        {
            dgvHistory.Visible = true;
            dgvHistory.BringToFront();
            if (dgvHistory.Width < 50) dgvHistory.Size = new Size(800, 500);

            RefreshHistoryList();

            if (cmbFilter != null)
            {
                cmbFilter.Items.Clear();
                cmbFilter.Items.Add("Усі замовлення");
                cmbFilter.Items.Add("Замовлення на доставку");
                cmbFilter.Items.Add("Замовлення за столами");
                cmbFilter.SelectedIndex = 0;
            }
        }

        public void RefreshHistoryList()
        {
            dgvHistory.DataSource = null;
            if (Form1.SalesHistory != null)
            {
                dgvHistory.DataSource = Form1.SalesHistory;
                SetupGridColumns();
            }
        }

        private void SetupGridColumns()
        {
            dgvHistory.BackgroundColor = Color.White;
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.ReadOnly = true;
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.RowTemplate.Height = 40;

            if (dgvHistory.Columns.Count == 0) return;

            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            string[] hiddenColumns = { "FullReceiptText", "Products" };
            foreach (string col in hiddenColumns)
            {
                if (dgvHistory.Columns[col] != null)
                    dgvHistory.Columns[col].Visible = false;
            }

            if (dgvHistory.Columns["Table"] != null) dgvHistory.Columns["Table"].HeaderText = "Стіл / № Замовлення";
            if (dgvHistory.Columns["Waiter"] != null) dgvHistory.Columns["Waiter"].HeaderText = "Офіціант";
            if (dgvHistory.Columns["Date"] != null) dgvHistory.Columns["Date"].HeaderText = "Дата та час";
            if (dgvHistory.Columns["Total"] != null) dgvHistory.Columns["Total"].HeaderText = "Сума (₴)";
            if (dgvHistory.Columns["PaymentMethod"] != null) dgvHistory.Columns["PaymentMethod"].HeaderText = "Тип оплати";
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.SelectedItem == null || Form1.SalesHistory == null) return;

            string selection = cmbFilter.SelectedItem.ToString();

            var sortedList = Form1.SalesHistory.OrderByDescending(s => s.Date).ToList();

            if (selection == "Замовлення na доставку" || selection == "Замовлення на доставку")
            {
                sortedList = Form1.SalesHistory
                    .OrderBy(s => (s.Table != null && (s.Table.Contains("Paket") || s.Table.Contains("📦") || s.Table.Contains("Доставка"))) ? 0 : 1)
                    .ThenByDescending(s => s.Date)
                    .ToList();
            }
            else if (selection == "Замовлення за столами")
            {
                sortedList = Form1.SalesHistory
                    .OrderBy(s => (s.Table != null && (s.Table.Contains("Paket") || s.Table.Contains("📦") || s.Table.Contains("Доставка"))) ? 1 : 0)
                    .ThenByDescending(s => s.Date)
                    .ToList();
            }

            dgvHistory.DataSource = null;
            dgvHistory.DataSource = sortedList;
            SetupGridColumns();
        }

        private void btnViewReceipt_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count > 0)
            {
                selectedRecord = (Form1.SalesRecord)dgvHistory.SelectedRows[0].DataBoundItem;

                pdHistory.PrintPage -= DrawReceiptToPage;
                pdHistory.PrintPage += DrawReceiptToPage;

                PrintPreviewDialog ppd = new PrintPreviewDialog { Document = pdHistory, Width = 800, Height = 1000 };
                ppd.ShowDialog();
            }
            else MessageBox.Show("Будь ласка, виберіть замовлення зі списку!", "Увага");
        }


        private void DrawReceiptToPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontHeader = new Font("Times New Roman", 24, FontStyle.Bold);
            Font fontSubHeader = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontBody = new Font("Times New Roman", 10);
            Font fontFooter = new Font("Times New Roman", 9, FontStyle.Italic);

            int currentY = 60;
            int marginX = 80;
            Pen linePen = new Pen(Color.Black, 1.5f);

            bool isDelivery = selectedRecord.Table != null && (selectedRecord.Table.Contains("Paket") || selectedRecord.Table.Contains("📦") || selectedRecord.Table.Contains("Доставка"));

            if (isDelivery)
            {
                g.DrawString("📦 ELVIS ДОСТАВКА", fontHeader, Brushes.Black, 220, currentY);
                currentY += 50;
                g.DrawString("Чек на доставку", fontSubHeader, Brushes.Black, 330, currentY);
            }
            else
            {
                g.DrawString("ELVIS RESTAURANT", fontHeader, Brushes.Black, 240, currentY);
                currentY += 50;
                g.DrawString("Клієнтський чек", fontSubHeader, Brushes.Black, 330, currentY);
            }
            currentY += 60;

            g.DrawString($"Дата : {selectedRecord.Date:dd.MM.yyyy}", fontBody, Brushes.Black, marginX, currentY);

            if (isDelivery)
            {
                string cleanID = selectedRecord.Table.Replace("📦", "").Replace("Доставка", "").Trim();
                g.DrawString($"№ Замовлення : {cleanID}", fontBody, Brushes.Black, 400, currentY);
            }
            else
            {
                g.DrawString($"Стіл : {selectedRecord.Table}", fontBody, Brushes.Black, 400, currentY);
            }

            currentY += 25;
            g.DrawString($"Час : {selectedRecord.Date:HH:mm}", fontBody, Brushes.Black, marginX, currentY);
            g.DrawString($"Офіціант : {selectedRecord.Waiter}", fontBody, Brushes.Black, 400, currentY);
            currentY += 40;

            g.DrawLine(linePen, marginX, currentY, 750, currentY);
            currentY += 10;
            g.DrawString("НАЗВА ТОВАРУ", fontSubHeader, Brushes.Black, marginX, currentY);
            g.DrawString("К-СТЬ", fontSubHeader, Brushes.Black, 450, currentY);
            g.DrawString("ЦІНА", fontSubHeader, Brushes.Black, 550, currentY);
            g.DrawString("ВСЬОГО", fontSubHeader, Brushes.Black, 670, currentY);
            currentY += 25;
            g.DrawLine(linePen, marginX, currentY, 750, currentY);
            currentY += 15;

            string[] items = selectedRecord.Products.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            double rawTotalSum = 0;

            foreach (string line in items)
            {
                try
                {
                    int xMark = line.IndexOf("x ");
                    int dashMark = line.LastIndexOf("-");

                    if (xMark != -1 && dashMark != -1)
                    {
                        string qty = line.Substring(0, xMark).Trim();
                        string name = line.Substring(xMark + 2, dashMark - xMark - 2).Trim();
                        string priceStr = line.Substring(dashMark + 1).Replace("TL", "").Replace("₺", "").Replace("₴", "").Trim();

                        double unitPrice = double.Parse(priceStr);
                        double lineTotal = int.Parse(qty) * unitPrice;

                        rawTotalSum += lineTotal;

                        g.DrawString(name.Length > 35 ? name.Substring(0, 32) + "..." : name, fontBody, Brushes.Black, marginX, currentY);
                        g.DrawString(qty, fontBody, Brushes.Black, 455, currentY);
                        g.DrawString(unitPrice.ToString("N2") + " ₴", fontBody, Brushes.Black, 545, currentY);
                        g.DrawString(lineTotal.ToString("N2") + " ₴", fontBody, Brushes.Black, 665, currentY);
                    }
                }
                catch
                {

                }
                currentY += 25;

                if (currentY > 1000) break;
            }

            currentY += 20;
            g.DrawLine(linePen, marginX, currentY, 750, currentY);
            currentY += 20;

            double finalPaidTotal = 0;
            if (!string.IsNullOrEmpty(selectedRecord.Total))
            {
                double.TryParse(selectedRecord.Total.Replace("₴", "").Trim(), out finalPaidTotal);
            }

            if (rawTotalSum > finalPaidTotal && (rawTotalSum - finalPaidTotal) > 0.5)
            {
                double discountAmount = rawTotalSum - finalPaidTotal;

                g.DrawString("СУМА :", fontSubHeader, Brushes.DimGray, 550, currentY);
                g.DrawString(rawTotalSum.ToString("N2") + " ₴", fontSubHeader, Brushes.DimGray, 660, currentY);
                currentY += 25;

                g.DrawString("ЗНИЖКА (WELCOME2026) :", new Font("Times New Roman", 10, FontStyle.Italic | FontStyle.Bold), Brushes.Crimson, 460, currentY);
                g.DrawString("-" + discountAmount.ToString("N2") + " ₴", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Crimson, 660, currentY);
                currentY += 30;

                g.DrawString("ДО СПЛАТИ :", fontSubHeader, Brushes.Black, 530, currentY);
                g.DrawString(finalPaidTotal.ToString("N2") + " ₴", fontSubHeader, Brushes.Black, 660, currentY);
            }
            else
            {
                g.DrawString("ЗАГАЛЬНА СУМА :", fontSubHeader, Brushes.Black, 500, currentY);
                g.DrawString(selectedRecord.Total, fontSubHeader, Brushes.Black, 660, currentY);
            }

            currentY += 35;
            g.DrawString("ТИП ОПЛАТИ :", fontSubHeader, Brushes.Black, 515, currentY);
            g.DrawString(selectedRecord.PaymentMethod, fontBody, Brushes.Black, 660, currentY);

            currentY += 60;
            g.DrawString("Дякуємо, що обрали MY RESTAURANT!", fontFooter, Brushes.Black, 280, currentY);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            mainForm.ShowPage(new ucMainMenu());
        }
        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cmbFilter_SelectedIndexChanged_1(object sender, EventArgs e) { }
    }
}