using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсова
{
    public partial class frmModernMsgBox : Form
    {

        private Label lblTitle;
        private Label lblMessage;
        private Button btnYes;
        private Button btnNo;
        private Button btnOk;

        public DialogResult CustomResult { get; private set; }

        public frmModernMsgBox(string message, string title, MessageBoxButtons buttons)
        {
            InitializeComponent();
            CreateLayout(message, title, buttons);
        }

        private void frmModernMsgBox_Load(object sender, EventArgs e)
        {
        }

        private void CreateLayout(string message, string title, MessageBoxButtons buttons)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 250);
            this.BackColor = Color.FromArgb(40, 40, 45);
            this.ShowInTaskbar = false;

            this.Paint += (s, e) =>
            {
                GraphicsPath path = GetRoundedRectPath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 20);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(Color.MediumSlateBlue, 2), path);
                this.Region = new Region(path);
            };

            lblTitle = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            lblMessage = new Label()
            {
                Text = message,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.LightGray,
                Location = new Point(20, 65),
                Size = new Size(this.Width - 40, this.Height - 120),
                TextAlign = ContentAlignment.TopLeft
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblMessage);


            int btnY = this.Height - 60;

            if (buttons == MessageBoxButtons.YesNo)
            {
                btnNo = CreateCustomButton("Ні", Color.Crimson, new Point(this.Width - 240, btnY));
                btnNo.Click += (s, e) => { CustomResult = DialogResult.No; this.Close(); };

                btnYes = CreateCustomButton("Так", Color.MediumSeaGreen, new Point(this.Width - 130, btnY));
                btnYes.Click += (s, e) => { CustomResult = DialogResult.Yes; this.Close(); };

                this.Controls.Add(btnNo);
                this.Controls.Add(btnYes);
            }
            else
            {
                btnOk = CreateCustomButton("ОК", Color.MediumSlateBlue, new Point(this.Width - 110, btnY));
                btnOk.Click += (s, e) => { CustomResult = DialogResult.OK; this.Close(); };
                this.Controls.Add(btnOk);
            }
        }

        private Button CreateCustomButton(string text, Color backColor, Point location)
        {
            Button btn = new Button()
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(95, 40),
                Location = location,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
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

        public static DialogResult Show(string message, string title = "Інформація", MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            using (var msgBox = new frmModernMsgBox(message, title, buttons))
            {
                msgBox.ShowDialog();
                return msgBox.CustomResult;
            }
        }
    }
}