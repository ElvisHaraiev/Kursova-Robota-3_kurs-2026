using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Курсова
{
    public partial class ucDeliveryChoose : UserControl
    {
        public ucDeliveryChoose()
        {
            InitializeComponent();
        }

        private void ucPaketSecim_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            RoundButtonCorners(btnOrderHistory);
            RoundButtonCorners(btnCreateOrder);
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null)
            {
                ucTableOrder orderPage = new ucTableOrder();

                orderPage.IsDelivery = true;

                mainForm.ShowPage(orderPage);
            }
        }

        private void btnOrderHistory_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null)
            {
                mainForm.ShowPage(new ucHistory());
            }
        }

        private void RoundButtonCorners(Button btn)
        {
            if (btn == null) return;

            int radius = 20;

            if (btn.Width <= radius * 2 || btn.Height <= radius * 2) return;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(btn.Width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(btn.Width - (radius * 2), btn.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(0, btn.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();

            btn.Region = new Region(path);
        }
    }
}