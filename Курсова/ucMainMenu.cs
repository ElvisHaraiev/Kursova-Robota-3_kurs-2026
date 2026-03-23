using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Курсова
{
    public partial class ucMainMenu : UserControl
    {
        private Label lblLiveDate;

        public ucMainMenu()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void ucMainMenu_Load(object sender, EventArgs e)
        {
            lblLiveDate = new Label();
            lblLiveDate.AutoSize = true;
            lblLiveDate.Font = new Font("Segoe UI", 20, FontStyle.Italic);
            lblLiveDate.ForeColor = Color.LightGray;

            if (Clock != null)
            {
                lblLiveDate.Location = new Point(Clock.Location.X, Clock.Bottom + 5);
            }

            this.Controls.Add(lblLiveDate);
            lblLiveDate.BringToFront();

            UpdateLiveDateTime();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Clock != null) Clock.Text = DateTime.Now.ToString("HH:mm");

            UpdateLiveDateTime();
        }

        private void UpdateLiveDateTime()
        {
            if (lblLiveDate != null)
            {
                lblLiveDate.Text = DateTime.Now.ToString("dd MMMM yyyy, dddd", new CultureInfo("uk-UA"));
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null)
            {
                Form1.LoggedInUser = "";
                mainForm.ShowPage(new ucLogin());
            }
        }

        private void btnTables_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucTables());
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucKitchen());
        }

        private void btnMenuManagement_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucMenuManagement());
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucDeliveryChoose());
        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucAnalysis());
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucAccounts());
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucHistory());
        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            Form1 mainForm = (Form1)this.FindForm();
            if (mainForm != null) mainForm.ShowPage(new ucReservations());
        }

        private void lblClock_Click(object sender, EventArgs e) { }
        private void Clock_Click(object sender, EventArgs e) { }
    }
}