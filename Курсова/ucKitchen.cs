using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсова
{
    public partial class ucKitchen : UserControl
    {
        private static Dictionary<string, DateTime> OrderTimers = new Dictionary<string, DateTime>();

        public ucKitchen()
        {
            InitializeComponent();
            DrawOrders();
        }

        private void ucKitchen_Load(object sender, EventArgs e)
        {
            DrawOrders();
        }

        private void DrawOrders()
        {
            flpOrders.Controls.Clear();

            foreach (var order in Form1.PendingOrders)
            {
                bool isDelivery = order.TableName.ToUpper().Contains("ДОСТАВКА") || order.TableName.ToUpper().Contains("PAKET");
                string cleanName = isDelivery ? order.TableName.Replace("📦", "").Replace("Доставка", "").Replace("Paket", "").Replace("SERVİS", "").Trim() : order.TableName;

                string productsToShow = order.OrderDetails;
                int addressIndex = productsToShow.IndexOf("--- ДАНІ ДОСТАВКИ");

                if (addressIndex == -1) addressIndex = productsToShow.IndexOf("--- ДОСТАВКА");

                if (addressIndex != -1)
                {
                    productsToShow = productsToShow.Substring(0, addressIndex).Trim();
                }

                Panel pnlCard = new Panel
                {
                    Width = 280,
                    Height = 350,
                    Margin = new Padding(15),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White
                };

                Panel pnlHeader = new Panel
                {
                    Height = isDelivery ? 135 : 110,
                    Dock = DockStyle.Top,
                    BackColor = isDelivery ? Color.FromArgb(255, 140, 0) : Color.FromArgb(43, 52, 75)
                };

                string headerText = isDelivery ? "Номер замовлення : " + cleanName : "Стіл : " + cleanName;
                Label lblTable = new Label { Text = headerText, ForeColor = Color.White, Location = new Point(10, 10), Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true };
                Label lblWaiter = new Label { Text = "Офіціант : " + order.WaiterName, ForeColor = Color.LightGray, Location = new Point(10, 40), Font = new Font("Segoe UI", 10), AutoSize = true };
                Label lblTime = new Label { Text = "Час замовлення : " + order.Time, ForeColor = Color.LightGray, Location = new Point(10, 65), Font = new Font("Segoe UI", 10), AutoSize = true };

                pnlHeader.Controls.Add(lblTable);
                pnlHeader.Controls.Add(lblWaiter);
                pnlHeader.Controls.Add(lblTime);

                if (isDelivery)
                {
                    Label lblDeliveryType = new Label
                    {
                        Text = "ТИП : ДОСТАВКА",
                        ForeColor = Color.Yellow,
                        Location = new Point(10, 90),
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        AutoSize = true
                    };
                    pnlHeader.Controls.Add(lblDeliveryType);
                }

                Button btnPrint = new Button
                {
                    Dock = DockStyle.Bottom,
                    Height = 45,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    ForeColor = Color.White
                };
                btnPrint.FlatAppearance.BorderSize = 0;


                string uniqueOrderKey = order.TableName + "|" + order.Time + "|" + productsToShow.Length;

                if (!OrderTimers.ContainsKey(uniqueOrderKey))
                {
                    OrderTimers[uniqueOrderKey] = DateTime.Now;
                }


                TimeSpan elapsed = DateTime.Now - OrderTimers[uniqueOrderKey];

                if (elapsed.TotalSeconds >= 10)
                {
                    btnPrint.BackColor = Color.MediumSeaGreen;
                    btnPrint.Text = "Друк (ГОТОВО)";
                }
                else
                {
                    btnPrint.BackColor = Color.FromArgb(255, 105, 135);
                    btnPrint.Text = "Друк";

                    int remainingWaitTime = 10000 - (int)elapsed.TotalMilliseconds;
                    if (remainingWaitTime > 0)
                    {
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(remainingWaitTime);


                            try
                            {
                                if (!btnPrint.IsDisposed)
                                {
                                    btnPrint.Invoke((MethodInvoker)delegate
                                    {
                                        btnPrint.BackColor = Color.MediumSeaGreen;
                                        btnPrint.Text = "Друк (ГОТОВО)";
                                    });
                                }
                            }
                            catch {}
                        });
                    }
                }


                btnPrint.Click += (s, ev) =>
                {
                    PrintDocument pdKitchen = new PrintDocument();
                    pdKitchen.PrintPage += (senderP, eP) =>
                    {
                        Graphics gr = eP.Graphics;
                        Font titleFont = new Font("Times New Roman", 22, FontStyle.Bold);
                        Font subTitleFont = new Font("Times New Roman", 14, FontStyle.Bold);
                        Font regFont = new Font("Times New Roman", 12);
                        Font itemBoldFont = new Font("Times New Roman", 12, FontStyle.Bold);

                        int y = 70;
                        int xMargin = 100;

                        gr.DrawString("КУХОННИЙ ЧЕК", titleFont, Brushes.Black, 220, y); // 🇺🇦
                        y += 70;

                        if (isDelivery)
                        {
                            gr.DrawString("ДОСТАВКА", subTitleFont, Brushes.OrangeRed, 250, y);
                            y += 40;
                        }

                        gr.DrawString("Дата / Час :", subTitleFont, Brushes.Black, xMargin, y);
                        gr.DrawString(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), regFont, Brushes.Black, 250, y);
                        y += 35;

                        if (isDelivery) gr.DrawString("Номер замовл. :", subTitleFont, Brushes.Black, xMargin, y);
                        else gr.DrawString("Назва столу :", subTitleFont, Brushes.Black, xMargin, y);

                        gr.DrawString(cleanName, regFont, Brushes.Black, 250, y);
                        y += 35;

                        gr.DrawString("Офіціант :", subTitleFont, Brushes.Black, xMargin, y);
                        gr.DrawString(order.WaiterName, regFont, Brushes.Black, 250, y);
                        y += 50;

                        gr.DrawLine(new Pen(Color.Black, 2), xMargin, y, 750, y);
                        y += 20;

                        gr.DrawString("К-СТЬ ТА НАЗВА ТОВАРУ", subTitleFont, Brushes.Black, xMargin, y); // 🇺🇦
                        y += 35;

                        string[] itemLines = productsToShow.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string line in itemLines)
                        {
                            string cleanProduct = line;
                            if (line.Contains("-")) cleanProduct = line.Split('-')[0].Trim();

                            gr.DrawString(cleanProduct, itemBoldFont, Brushes.Black, xMargin, y);
                            y += 30;
                        }

                        y += 20;
                        gr.DrawLine(new Pen(Color.Black, 2), xMargin, y, 750, y);
                        y += 30;
                        gr.DrawString("КОПІЯ ДЛЯ КУХНІ", regFont, Brushes.Black, 350, y); // 🇺🇦
                    };

                    PrintPreviewDialog ppDialog = new PrintPreviewDialog();
                    ppDialog.Document = pdKitchen;
                    ppDialog.Width = 800;
                    ppDialog.Height = 1000;
                    ppDialog.ShowDialog();
                };

                Panel pnlProductContainer = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(10)
                };

                Label lblProductDetails = new Label
                {
                    Text = productsToShow,
                    AutoSize = true,
                    MaximumSize = new Size(240, 0),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    ForeColor = Color.Black
                };

                pnlProductContainer.Controls.Add(lblProductDetails);

                pnlCard.Controls.Add(pnlProductContainer);
                pnlCard.Controls.Add(pnlHeader);
                pnlCard.Controls.Add(btnPrint);

                pnlProductContainer.BringToFront();

                flpOrders.Controls.Add(pnlCard);
            }
        }

        private void flpOrders_Paint(object sender, PaintEventArgs e) { }
    }
}