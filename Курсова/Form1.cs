using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Курсова
{
    public partial class Form1 : Form
    {
        public static string LoggedInUser = "";

        public string RestaurantName { get; set; } = "MY RESTAURANT";
        public Image RestaurantLogo { get; set; } = null;

        private PictureBox pbHeaderLogo;
        private Label lblHeaderName;

        private Button btnLogout;
        public Button btnExistingOrders;
        private Stack<UserControl> pageHistory = new Stack<UserControl>();
        private Button btnDynamicBack;

        private Button btnBurger;
        private FlowLayoutPanel pnlSideMenu;
        private bool isMenuOpen = false;

        public class UserAccount { public string Username { get; set; } public string Password { get; set; } public string Role { get; set; } }
        public static List<UserAccount> UserList = new List<UserAccount>();

        public class SalesRecord { public string Table { get; set; } public string Waiter { get; set; } public string Products { get; set; } public string Total { get; set; } public string PaymentMethod { get; set; } public string FullReceiptText { get; set; } public DateTime Date { get; set; } }
        public static List<SalesRecord> SalesHistory = new List<SalesRecord>();

        public class KitchenOrder { public string TableName { get; set; } public string WaiterName { get; set; } public string Time { get; set; } public string OrderDetails { get; set; } }
        public static List<KitchenOrder> PendingOrders = new List<KitchenOrder>();

        public class MenuItem { public string Name { get; set; } public double Price { get; set; } public string Category { get; set; } public string ImagePath { get; set; } }
        public static List<string> Categories = new List<string> { "Головні страви", "Напої", "Десерти", "Салати", "Піца", "Стейки" };
        public static List<MenuItem> MenuList = new List<MenuItem>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            try
            {
                this.RestaurantLogo = Properties.Resources._93e267ba_d2c1_4ccf_b217_b266430e1c7a_removalai_preview;
            }
            catch
            {
                this.RestaurantLogo = null;
            }

            InitializeHeader();
            AddLogoutButtonToTopBar();
            AddExistingOrdersButtonToTopBar();
            CreateBurgerMenu();

            ShowPage(new ucLogin());
        }

        private void InitializeHeader()
        {
            if (lblUsername == null || lblUsername.Parent == null) return;
            Panel topBar = (Panel)lblUsername.Parent;

            pbHeaderLogo = new PictureBox();
            pbHeaderLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pbHeaderLogo.Image = RestaurantLogo;
            pbHeaderLogo.Size = new Size(120, 100);
            pbHeaderLogo.BackColor = Color.Transparent;

            lblHeaderName = new Label();
            lblHeaderName.Text = RestaurantName;
            lblHeaderName.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblHeaderName.ForeColor = Color.White;
            lblHeaderName.AutoSize = true;
            lblHeaderName.BackColor = Color.Transparent;

            topBar.Controls.Add(pbHeaderLogo);
            topBar.Controls.Add(lblHeaderName);

            Action centerHeaderElements = () =>
            {
                int textOffset = -60;
                int logoOffset = -250;

                int textX = ((topBar.Width - lblHeaderName.Width) / 2) + textOffset;
                lblHeaderName.Location = new Point(textX, (topBar.Height - lblHeaderName.Height) / 2);

                if (RestaurantLogo != null)
                {
                    pbHeaderLogo.Visible = true;
                    int logoX = ((topBar.Width - pbHeaderLogo.Width) / 2) + logoOffset;
                    pbHeaderLogo.Location = new Point(logoX, (topBar.Height - pbHeaderLogo.Height) / 2);
                }
                else
                {
                    pbHeaderLogo.Visible = false;
                }
            };

            centerHeaderElements();
            topBar.Resize += (s, e) => centerHeaderElements();

            pbHeaderLogo.BringToFront();
            lblHeaderName.BringToFront();
        }

        private void AddLogoutButtonToTopBar()
        {
            if (lblUsername == null || lblUsername.Parent == null) return;
            Panel topBar = (Panel)lblUsername.Parent;

            btnLogout = new Button();
            btnLogout.Text = "Вихід";
            btnLogout.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogout.BackColor = Color.Crimson;
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Size = new Size(110, 45);
            btnLogout.Cursor = Cursors.Hand;

            int radius = 20;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btnLogout.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btnLogout.Width - radius, btnLogout.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btnLogout.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btnLogout.Region = new Region(path);

            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Location = new Point(topBar.Width - btnLogout.Width - 20, (topBar.Height - btnLogout.Height) / 2);

            btnLogout.Click += (s, e) => { LoggedInUser = ""; ShowPage(new ucLogin()); };
            topBar.Controls.Add(btnLogout);
            btnLogout.BringToFront();
        }

        private void AddExistingOrdersButtonToTopBar()
        {
            if (lblUsername == null || lblUsername.Parent == null) return;
            Panel topBar = (Panel)lblUsername.Parent;

            btnExistingOrders = new Button();
            btnExistingOrders.Text = "Замовлення столу";
            btnExistingOrders.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnExistingOrders.BackColor = Color.MidnightBlue;
            btnExistingOrders.ForeColor = Color.White;
            btnExistingOrders.FlatStyle = FlatStyle.Flat;
            btnExistingOrders.FlatAppearance.BorderSize = 0;
            btnExistingOrders.Size = new Size(240, 40);
            btnExistingOrders.Cursor = Cursors.Hand;
            btnExistingOrders.Visible = false;
            btnExistingOrders.Click += BtnExistingOrders_Click;

            int radius = 15;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btnExistingOrders.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btnExistingOrders.Width - radius, btnExistingOrders.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btnExistingOrders.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btnExistingOrders.Region = new Region(path);

            topBar.Controls.Add(btnExistingOrders);
        }

        private void CreateBurgerMenu()
        {
            if (lblUsername == null || lblUsername.Parent == null) return;
            Panel topBar = (Panel)lblUsername.Parent;

            var oldLabels = topBar.Controls.OfType<Label>().Where(l => l.Text.Contains("Історія") || l.Name.Contains("History")).ToList();
            foreach (var lbl in oldLabels) topBar.Controls.Remove(lbl);

            btnBurger = new Button();
            btnBurger.Text = "☰";
            btnBurger.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            btnBurger.ForeColor = Color.White;
            btnBurger.BackColor = Color.Transparent;
            btnBurger.FlatStyle = FlatStyle.Flat;
            btnBurger.FlatAppearance.BorderSize = 0;
            btnBurger.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
            btnBurger.Size = new Size(60, 60);
            btnBurger.Location = new Point(10, (topBar.Height - 60) / 2);
            btnBurger.Cursor = Cursors.Hand;
            btnBurger.Click += (s, e) => ToggleSidebar();

            topBar.Controls.Add(btnBurger);
            btnBurger.BringToFront();

            btnDynamicBack = new Button();
            btnDynamicBack.Text = "⬅ НАЗАД";
            btnDynamicBack.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnDynamicBack.BackColor = Color.Blue;
            btnDynamicBack.ForeColor = Color.White;
            btnDynamicBack.FlatStyle = FlatStyle.Flat;
            btnDynamicBack.FlatAppearance.BorderSize = 0;
            btnDynamicBack.Size = new Size(120, 40);
            btnDynamicBack.Location = new Point(btnBurger.Right + 10, (topBar.Height - 40) / 2);
            btnDynamicBack.Cursor = Cursors.Hand;
            btnDynamicBack.Visible = false;
            btnDynamicBack.Click += BtnDynamicBack_Click;

            int r = 15;
            GraphicsPath p = new GraphicsPath();
            p.AddArc(0, 0, r, r, 180, 90);
            p.AddArc(btnDynamicBack.Width - r, 0, r, r, 270, 90);
            p.AddArc(btnDynamicBack.Width - r, btnDynamicBack.Height - r, r, r, 0, 90);
            p.AddArc(0, btnDynamicBack.Height - r, r, r, 90, 90);
            p.CloseAllFigures();
            btnDynamicBack.Region = new Region(p);

            topBar.Controls.Add(btnDynamicBack);

            pnlSideMenu = new FlowLayoutPanel();
            pnlSideMenu.FlowDirection = FlowDirection.TopDown;
            pnlSideMenu.WrapContents = false;
            pnlSideMenu.Width = 280;
            pnlSideMenu.Height = 1000;
            pnlSideMenu.Location = new Point(0, topBar.Height);
            pnlSideMenu.Visible = false;

            this.Controls.Add(pnlSideMenu);
            pnlSideMenu.BringToFront();

            ApplyGradientToSidebar();

            AddSidebarButton("📜 Історія", (s, e) => { ShowPage(new ucHistory()); ToggleSidebar(); });
            AddSidebarButton("🍽️ Столи", (s, e) => { ShowPage(new ucTables()); ToggleSidebar(); });
            AddSidebarButton("🛵 Доставка", (s, e) => { ShowPage(new ucDeliveryChoose()); ToggleSidebar(); });
            AddSidebarButton("👨‍🍳 Кухня", (s, e) => { ShowPage(new ucKitchen()); ToggleSidebar(); });
            AddSidebarButton("📋 Меню", (s, e) => { ShowPage(new ucMenuManagement()); ToggleSidebar(); });
            AddSidebarButton("📈 Аналітика", (s, e) => { ShowPage(new ucAnalysis()); ToggleSidebar(); });
            AddSidebarButton("👥 Акаунти", (s, e) => { ShowPage(new ucAccounts()); ToggleSidebar(); });
            AddSidebarButton("📅 Бронювання", (s, e) => { ShowPage(new ucReservations()); ToggleSidebar(); });
            AddSidebarButton("🤝 Клієнти (CRM)", (s, e) => { ShowPage(new ucCustomers()); ToggleSidebar(); });

            this.Resize += (s, e) => {
                if (pnlSideMenu != null)
                {
                    pnlSideMenu.Height = this.Height;
                    ApplyGradientToSidebar();
                }
            };
        }

        private void ApplyGradientToSidebar()
        {
            if (pnlSideMenu.Width > 0 && pnlSideMenu.Height > 0)
            {
                Bitmap bmp = new Bitmap(pnlSideMenu.Width, pnlSideMenu.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Color c1 = Color.MidnightBlue;
                    Color c2 = Color.Blue;
                    using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, bmp.Width, bmp.Height), c1, c2, LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                    }
                }
                pnlSideMenu.BackgroundImage = bmp;
                pnlSideMenu.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private bool IsAdminSilently()
        {
            return LoggedInUser.Contains("Admin") || LoggedInUser.Contains("Адмін");
        }

        private bool IsChefSilently()
        {
            return LoggedInUser.Contains("Кухар") || LoggedInUser.Contains("Chef");
        }

        private void ToggleSidebar()
        {
            isMenuOpen = !isMenuOpen;

            bool isChef = IsChefSilently();
            bool isAdmin = IsAdminSilently();

            foreach (Control ctrl in pnlSideMenu.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (isAdmin)
                    {
                        btn.Visible = true;
                    }
                    else if (isChef)
                    {
                        if (btn.Text.Contains("Кухня") || btn.Text.Contains("Меню") || btn.Text.Contains("Столи") || btn.Text.Contains("Доставка"))
                            btn.Visible = true;
                        else
                            btn.Visible = false;
                    }
                    else
                    {
                        if (btn.Text.Contains("Аналітика") || btn.Text.Contains("Акаунти") || btn.Text.Contains("CRM") || btn.Text.Contains("Меню"))
                            btn.Visible = false;
                        else
                            btn.Visible = true;
                    }
                }
            }

            pnlSideMenu.Visible = isMenuOpen;
            if (isMenuOpen) pnlSideMenu.BringToFront();
        }

        private void AddSidebarButton(string text, EventHandler onClick)
        {
            Button btn = new Button();
            btn.Text = "  " + text;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.Transparent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
            btn.Size = new Size(260, 65);
            btn.Margin = new Padding(0);
            btn.Cursor = Cursors.Hand;
            btn.Click += onClick;

            pnlSideMenu.Controls.Add(btn);
        }

        public bool IsAdmin()
        {
            if (!IsAdminSilently())
            {
                frmModernMsgBox.Show("У вас немає прав доступу до цього розділу!\n(Тільки для адміністраторів)", "Доступ заборонено");
                return false;
            }
            return true;
        }

        public void ShowPage(UserControl uc, bool isGoingBack = false)
        {
            if (pnlContainer == null) return;

            bool isChef = IsChefSilently();
            bool isAdmin = IsAdminSilently();
            bool isWaiter = !isAdmin && !isChef;


            if (uc is ucAnalysis || uc is ucAccounts || uc is ucCustomers)
            {
                if (!isAdmin)
                {
                    frmModernMsgBox.Show("У вас немає доступу до цього розділу!\n(Тільки для Адміністраторів)", "Доступ заборонено");
                    return;
                }
            }

            else if (uc is ucMenuManagement)
            {
                if (!isAdmin && !isChef)
                {
                    frmModernMsgBox.Show("У вас немає доступу до цього розділу!\n(Тільки для Адміністраторів та Кухарів)", "Доступ заборонено");
                    return;
                }
            }

            else if (uc is ucHistory || uc is ucReservations)
            {
                if (!isAdmin && !isWaiter)
                {
                    frmModernMsgBox.Show("У вас немає доступу до цього розділу!\n(Тільки для Адміністраторів та Офіціантів)", "Доступ заборонено");
                    return;
                }
            }


            if (isMenuOpen) ToggleSidebar();

            if (!isGoingBack && pnlContainer.Controls.Count > 0)
            {
                UserControl currentUC = pnlContainer.Controls[0] as UserControl;
                if (currentUC != null && !(currentUC is ucLogin) && !(currentUC is ucMainMenu))
                {
                    pageHistory.Push(currentUC);
                }
            }

            if (uc is ucMainMenu || uc is ucLogin)
            {
                pageHistory.Clear();
            }

            pnlContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(uc);

            bool isLoginScreen = (uc is ucLogin);
            bool isMainMenu = (uc is ucMainMenu);

            if (pbHeaderLogo != null) pbHeaderLogo.Visible = true;
            if (lblHeaderName != null) lblHeaderName.Visible = true;

            if (lblUsername != null && lblUsername.Parent != null)
            {
                Panel topBar = (Panel)lblUsername.Parent;

                foreach (Control ctrl in topBar.Controls)
                {
                    if (ctrl.Text == "Reports" || ctrl.Text == "Звіти" || ctrl.Name.ToLower() == "lblreports")
                    {
                        ctrl.Visible = false;
                    }
                }
            }

            if (btnBurger != null) btnBurger.Visible = !isLoginScreen;
            if (btnDynamicBack != null) btnDynamicBack.Visible = !isMainMenu && !isLoginScreen;

            if (lblUsername != null)
            {
                lblUsername.Visible = !isLoginScreen;
                if (btnLogout != null && lblUsername.Visible)
                {
                    lblUsername.Location = new Point(btnLogout.Left - lblUsername.Width - 20, lblUsername.Location.Y);
                }

                if (btnExistingOrders != null)
                {
                    btnExistingOrders.Visible = (uc is ucTableOrder);

                    if (btnExistingOrders.Visible && lblUsername.Parent != null)
                    {
                        Panel topBar = (Panel)lblUsername.Parent;
                        btnExistingOrders.Location = new Point(lblUsername.Left - btnExistingOrders.Width - 20, (topBar.Height - btnExistingOrders.Height) / 2);
                        btnExistingOrders.BringToFront();
                    }
                }
            }

            if (btnLogout != null)
            {
                btnLogout.Visible = !isLoginScreen;
                btnLogout.BringToFront();
            }

            if (btnBack != null) btnBack.Visible = false;

            if (btnAddProduct != null && lblUsername != null && lblUsername.Parent != null)
            {
                Panel topBar = (Panel)lblUsername.Parent;
                btnAddProduct.Visible = (uc is ucMenuManagement);

                if (btnAddProduct.Visible)
                {
                    Control searchBox = topBar.Controls.Find("TopSearchBox", true).FirstOrDefault();
                    if (searchBox != null)
                    {
                        btnAddProduct.Location = new Point(searchBox.Location.X - btnAddProduct.Width - 15, (topBar.Height - btnAddProduct.Height) / 2);
                    }
                    else
                    {
                        btnAddProduct.Location = new Point(topBar.Width - 650, (topBar.Height - btnAddProduct.Height) / 2);
                    }
                }
                btnAddProduct.BringToFront();
            }
        }

        private void BtnDynamicBack_Click(object sender, EventArgs e)
        {
            if (pageHistory.Count > 0)
            {
                UserControl previousPage = pageHistory.Pop();
                ShowPage(previousPage, true);
            }
            else
            {
                ShowPage(new ucMainMenu(), true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (lblUsername != null)
                lblUsername.Text = "Користувач: " + LoggedInUser;
        }

        private void btnBack_Click(object sender, EventArgs e) => BtnDynamicBack_Click(sender, e);
        private void btnHistory_Click(object sender, EventArgs e) { }
        private void panelContainer_Paint(object sender, PaintEventArgs e) { }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            foreach (Control c in pnlContainer.Controls)
            {
                if (c is ucMenuManagement menuPage)
                {
                    menuPage.btnAdd_Click(sender, e);
                    break;
                }
            }
        }

        private void BtnExistingOrders_Click(object sender, EventArgs e)
        {
            if (pnlContainer.Controls.Count > 0 && pnlContainer.Controls[0] is ucTableOrder orderPage)
            {
                orderPage.LoadExistingOrders();
            }
        }

        public ucLogin ucLogin { get => default; set { } }
        public ucMainMenu ucMainMenu { get => default; set { } }
        public ucMenuManagement ucMenuManagement { get => default; set { } }
        public ucTableOrder ucTableOrder { get => default; set { } }
        public ucTables ucTables { get => default; set { } }
        public ucDeliveryChoose ucDeliveryChoose { get => default; set { } }
        public ucDeliveryDetails ucDeliveryDetails { get => default; set { } }
        public ucHistory ucHistory { get => default; set { } }
        public ucKitchen ucKitchen { get => default; set { } }
        public ucAnalysis ucAnalysis { get => default; set { } }
        public ucAccounts ucAccounts { get => default; set { } }
        public ucReservations ucReservations { get => default; set { } }
        public SaleReport SaleReport { get => default; set { } }
    }
}