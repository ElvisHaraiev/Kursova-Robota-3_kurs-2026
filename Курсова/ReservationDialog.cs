using System;
using System.Drawing;
using System.Windows.Forms;

namespace Курсова
{
    public class ReservationDialog : Form
    {
        public DateTime SelectedDateTime { get; private set; }
        private DateTimePicker dtPicker;
        private Button btnConfirm;

        public ReservationDialog(string tableName)
        {
            this.Text = "Бронювання: " + tableName;
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblInfo = new Label()
            {
                Text = "Оберіть дату та час для: " + tableName,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(300, 25)
            };
            this.Controls.Add(lblInfo);

            dtPicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 70),
                Size = new Size(290, 30)
            };
            this.Controls.Add(dtPicker);

            btnConfirm = new Button()
            {
                Text = "Підтвердити",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 130),
                Size = new Size(290, 45)
            };
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            SelectedDateTime = dtPicker.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}