namespace Курсова
{
    partial class ucMainMenu
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMainMenu));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Clock = new System.Windows.Forms.Label();
            this.btnReservations = new Курсова.ModernButton();
            this.btnHistory = new Курсова.ModernButton();
            this.Tables = new Курсова.ModernButton();
            this.Menu = new Курсова.ModernButton();
            this.Accounts = new Курсова.ModernButton();
            this.Kitchen = new Курсова.ModernButton();
            this.Analyses = new Курсова.ModernButton();
            this.Delivery = new Курсова.ModernButton();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Clock
            // 
            this.Clock.AutoSize = true;
            this.Clock.Font = new System.Drawing.Font("Myanmar Text", 60F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clock.ForeColor = System.Drawing.SystemColors.Window;
            this.Clock.Location = new System.Drawing.Point(19, 241);
            this.Clock.Name = "Clock";
            this.Clock.Size = new System.Drawing.Size(313, 177);
            this.Clock.TabIndex = 7;
            this.Clock.Text = "15:34";
            this.Clock.Click += new System.EventHandler(this.Clock_Click);
            // 
            // btnReservations
            // 
            this.btnReservations.BackColor = System.Drawing.Color.Blue;
            this.btnReservations.BorderRadius = 80;
            this.btnReservations.FlatAppearance.BorderSize = 0;
            this.btnReservations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReservations.Font = new System.Drawing.Font("Myanmar Text", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReservations.ForeColor = System.Drawing.SystemColors.Window;
            this.btnReservations.GradientColor1 = System.Drawing.Color.Blue;
            this.btnReservations.GradientColor2 = System.Drawing.Color.Gray;
            this.btnReservations.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnReservations.Image = ((System.Drawing.Image)(resources.GetObject("btnReservations.Image")));
            this.btnReservations.Location = new System.Drawing.Point(1592, 466);
            this.btnReservations.Name = "btnReservations";
            this.btnReservations.Size = new System.Drawing.Size(325, 357);
            this.btnReservations.TabIndex = 10;
            this.btnReservations.Text = "Бронювання";
            this.btnReservations.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReservations.UseVisualStyleBackColor = false;
            this.btnReservations.Click += new System.EventHandler(this.btnReservations_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.BackColor = System.Drawing.Color.Blue;
            this.btnHistory.BorderRadius = 80;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistory.ForeColor = System.Drawing.SystemColors.Window;
            this.btnHistory.GradientColor1 = System.Drawing.Color.Blue;
            this.btnHistory.GradientColor2 = System.Drawing.Color.Gray;
            this.btnHistory.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnHistory.Image")));
            this.btnHistory.Location = new System.Drawing.Point(1592, 61);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(325, 357);
            this.btnHistory.TabIndex = 9;
            this.btnHistory.Text = "Історія";
            this.btnHistory.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // Tables
            // 
            this.Tables.BackColor = System.Drawing.Color.Blue;
            this.Tables.BorderRadius = 80;
            this.Tables.FlatAppearance.BorderSize = 0;
            this.Tables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tables.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tables.ForeColor = System.Drawing.SystemColors.Window;
            this.Tables.GradientColor1 = System.Drawing.Color.Blue;
            this.Tables.GradientColor2 = System.Drawing.Color.Gray;
            this.Tables.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.Tables.Image = ((System.Drawing.Image)(resources.GetObject("Tables.Image")));
            this.Tables.Location = new System.Drawing.Point(371, 75);
            this.Tables.Name = "Tables";
            this.Tables.Size = new System.Drawing.Size(325, 343);
            this.Tables.TabIndex = 8;
            this.Tables.Text = "Столи";
            this.Tables.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Tables.UseVisualStyleBackColor = false;
            this.Tables.Click += new System.EventHandler(this.btnTables_Click);
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.Color.Blue;
            this.Menu.BorderRadius = 80;
            this.Menu.FlatAppearance.BorderSize = 0;
            this.Menu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Menu.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Menu.ForeColor = System.Drawing.SystemColors.Window;
            this.Menu.GradientColor1 = System.Drawing.Color.Blue;
            this.Menu.GradientColor2 = System.Drawing.Color.Gray;
            this.Menu.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.Menu.Image = ((System.Drawing.Image)(resources.GetObject("Menu.Image")));
            this.Menu.Location = new System.Drawing.Point(788, 471);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(325, 357);
            this.Menu.TabIndex = 6;
            this.Menu.Text = "Меню";
            this.Menu.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Menu.UseVisualStyleBackColor = false;
            this.Menu.Click += new System.EventHandler(this.btnMenuManagement_Click);
            // 
            // Accounts
            // 
            this.Accounts.BackColor = System.Drawing.Color.Blue;
            this.Accounts.BorderRadius = 80;
            this.Accounts.FlatAppearance.BorderSize = 0;
            this.Accounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Accounts.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Accounts.ForeColor = System.Drawing.SystemColors.Window;
            this.Accounts.GradientColor1 = System.Drawing.Color.Blue;
            this.Accounts.GradientColor2 = System.Drawing.Color.Gray;
            this.Accounts.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.Accounts.Image = ((System.Drawing.Image)(resources.GetObject("Accounts.Image")));
            this.Accounts.Location = new System.Drawing.Point(1197, 471);
            this.Accounts.Name = "Accounts";
            this.Accounts.Size = new System.Drawing.Size(325, 357);
            this.Accounts.TabIndex = 4;
            this.Accounts.Text = "Акаунти";
            this.Accounts.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Accounts.UseVisualStyleBackColor = false;
            this.Accounts.Click += new System.EventHandler(this.btnAccounts_Click);
            // 
            // Kitchen
            // 
            this.Kitchen.BackColor = System.Drawing.Color.Blue;
            this.Kitchen.BorderRadius = 80;
            this.Kitchen.FlatAppearance.BorderSize = 0;
            this.Kitchen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Kitchen.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Kitchen.ForeColor = System.Drawing.SystemColors.Window;
            this.Kitchen.GradientColor1 = System.Drawing.Color.Blue;
            this.Kitchen.GradientColor2 = System.Drawing.Color.Gray;
            this.Kitchen.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.Kitchen.Image = ((System.Drawing.Image)(resources.GetObject("Kitchen.Image")));
            this.Kitchen.Location = new System.Drawing.Point(371, 485);
            this.Kitchen.Name = "Kitchen";
            this.Kitchen.Size = new System.Drawing.Size(325, 343);
            this.Kitchen.TabIndex = 3;
            this.Kitchen.Text = "Кухня";
            this.Kitchen.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Kitchen.UseVisualStyleBackColor = false;
            this.Kitchen.Click += new System.EventHandler(this.btnKitchen_Click);
            // 
            // Analyses
            // 
            this.Analyses.BackColor = System.Drawing.Color.Blue;
            this.Analyses.BorderRadius = 80;
            this.Analyses.FlatAppearance.BorderSize = 0;
            this.Analyses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Analyses.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analyses.ForeColor = System.Drawing.SystemColors.Window;
            this.Analyses.GradientColor1 = System.Drawing.Color.Blue;
            this.Analyses.GradientColor2 = System.Drawing.Color.Gray;
            this.Analyses.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.Analyses.Image = ((System.Drawing.Image)(resources.GetObject("Analyses.Image")));
            this.Analyses.Location = new System.Drawing.Point(1197, 61);
            this.Analyses.Name = "Analyses";
            this.Analyses.Size = new System.Drawing.Size(325, 357);
            this.Analyses.TabIndex = 2;
            this.Analyses.Text = "Аналітика";
            this.Analyses.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Analyses.UseVisualStyleBackColor = false;
            this.Analyses.Click += new System.EventHandler(this.btnAnalytics_Click);
            // 
            // Delivery
            // 
            this.Delivery.BackColor = System.Drawing.Color.Blue;
            this.Delivery.BorderRadius = 80;
            this.Delivery.FlatAppearance.BorderSize = 0;
            this.Delivery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Delivery.Font = new System.Drawing.Font("Myanmar Text", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delivery.ForeColor = System.Drawing.SystemColors.Window;
            this.Delivery.GradientColor1 = System.Drawing.Color.Blue;
            this.Delivery.GradientColor2 = System.Drawing.Color.Gray;
            this.Delivery.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.Delivery.Image = ((System.Drawing.Image)(resources.GetObject("Delivery.Image")));
            this.Delivery.Location = new System.Drawing.Point(788, 61);
            this.Delivery.Name = "Delivery";
            this.Delivery.Size = new System.Drawing.Size(325, 357);
            this.Delivery.TabIndex = 1;
            this.Delivery.Text = "Доставка";
            this.Delivery.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Delivery.UseVisualStyleBackColor = false;
            this.Delivery.Click += new System.EventHandler(this.btnDelivery_Click);
            // 
            // ucMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkBlue;
            this.Controls.Add(this.btnReservations);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.Tables);
            this.Controls.Add(this.Clock);
            this.Controls.Add(this.Menu);
            this.Controls.Add(this.Accounts);
            this.Controls.Add(this.Kitchen);
            this.Controls.Add(this.Analyses);
            this.Controls.Add(this.Delivery);
            this.Name = "ucMainMenu";
            this.Size = new System.Drawing.Size(1970, 847);
            this.Load += new System.EventHandler(this.ucMainMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private ModernButton Delivery;
        private ModernButton Analyses;
        private ModernButton Kitchen;
        private ModernButton Accounts;
        private ModernButton Menu;
        private System.Windows.Forms.Label Clock;
        private ModernButton Tables;
        private ModernButton btnHistory;
        private ModernButton btnReservations;
    }
}