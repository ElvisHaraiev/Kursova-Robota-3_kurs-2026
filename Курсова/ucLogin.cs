using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Курсова
{
    public partial class ucLogin : UserControl
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblError;
        private Panel pnlCard;

        public ucLogin()
        {
            try { InitializeComponent(); } catch { }
            this.DoubleBuffered = true;

            CreateDesign();
        }

        private void ucLogin_Load(object sender, EventArgs e)
        {
        }

        private void CreateDesign()
        {
            this.Dock = DockStyle.Fill;

            this.Paint += (s, e) => {
                Color color1 = Color.MidnightBlue;
                Color color2 = Color.Blue;

                using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color2, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            pnlCard = new Panel();
            pnlCard.Size = new Size(500, 550);
            pnlCard.Anchor = AnchorStyles.None;
            pnlCard.BackColor = Color.Transparent;
            pnlCard.Paint += (s, e) => {
                GraphicsPath path = RoundedRect(new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1), 30);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (SolidBrush cardBrush = new SolidBrush(Color.FromArgb(30, 50, 100)))
                {
                    e.Graphics.FillPath(cardBrush, path);
                }
            };

            this.Resize += (s, e) => {
                pnlCard.Location = new Point((this.Width - pnlCard.Width) / 2, (this.Height - pnlCard.Height) / 2);
                this.Invalidate();
            };

            Label lblTitle = new Label()
            {
                Text = "Ласкаво просимо до\nMY RESTAURANT",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(pnlCard.Width, 80),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblSubtitle = new Label()
            {
                Text = "Будь ласка, введіть свої дані",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(pnlCard.Width, 30),
                Location = new Point(0, 105),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblUserTag = new Label() { Text = "👤 Ім'я користувача", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Location = new Point(75, 170), AutoSize = true };
            txtUsername = new TextBox() { Font = new Font("Segoe UI", 18), Location = new Point(75, 205), Width = 350, BorderStyle = BorderStyle.FixedSingle };

            Label lblPassTag = new Label() { Text = "🔒 Пароль", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.White, Location = new Point(75, 270), AutoSize = true };
            txtPassword = new TextBox() { Font = new Font("Segoe UI", 18), Location = new Point(75, 305), Width = 350, BorderStyle = BorderStyle.FixedSingle, PasswordChar = '•' };

            lblError = new Label()
            {
                Text = "Неправильне ім'я користувача або пароль!",
                ForeColor = Color.FromArgb(255, 100, 100),
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                AutoSize = false,
                Size = new Size(pnlCard.Width, 25),
                Location = new Point(0, 360),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            Button btnLogin = new Button();
            btnLogin.Text = "УВІЙТИ";
            btnLogin.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(350, 60);
            btnLogin.Location = new Point(75, 400);
            btnLogin.Cursor = Cursors.Hand;

            btnLogin.Paint += BtnLogin_Paint;

            btnLogin.MouseDown += (s, e) => { btnLogin.Location = new Point(btnLogin.Location.X, btnLogin.Location.Y + 2); };
            btnLogin.MouseUp += (s, e) => { btnLogin.Location = new Point(btnLogin.Location.X, btnLogin.Location.Y - 2); };

            btnLogin.Click += btnLogin_Click;

            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnLogin_Click(null, null); };
            txtUsername.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtPassword.Focus(); };

            pnlCard.Controls.Add(lblTitle);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblUserTag);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblPassTag);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(btnLogin);

            this.Controls.Add(pnlCard);
        }

        private void BtnLogin_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int radius = 20;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            btn.Region = new Region(path);

            using (LinearGradientBrush brush = new LinearGradientBrush(btn.ClientRectangle, Color.MidnightBlue, Color.Blue, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillPath(brush, path);
            }

            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredUser = txtUsername.Text.Trim();
            string enteredPass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(enteredUser) || string.IsNullOrEmpty(enteredPass))
            {
                lblError.Text = "Будь ласка, заповніть усі поля!";
                lblError.Visible = true;
                return;
            }

            try
            {
                string hashedPass = SecurityHelper.HashPassword(enteredPass);

                using (MySqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn == null || conn.State != ConnectionState.Open) return;

                    string query = "SELECT Role FROM Users WHERE Username = @user AND password_hash = @hash";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", enteredUser);
                        cmd.Parameters.AddWithValue("@hash", hashedPass);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["Role"].ToString();

                                Form1 mainForm = (Form1)this.FindForm();
                                if (mainForm != null)
                                {
                                    Form1.LoggedInUser = $"{enteredUser} ({role})";

                                    Label lblDisplay = mainForm.Controls.Find("lblUsername", true).FirstOrDefault() as Label;
                                    if (lblDisplay != null)
                                    {
                                        lblDisplay.Text = "👤 " + Form1.LoggedInUser;
                                    }

                                    mainForm.ShowPage(new ucMainMenu());
                                }
                            }
                            else
                            {

                                lblError.Text = "Неправильне ім'я користувача або пароль!";
                                lblError.Visible = true;
                                txtPassword.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка бази даних:\n" + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;AdjustableArrowCap on;
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
    }
}