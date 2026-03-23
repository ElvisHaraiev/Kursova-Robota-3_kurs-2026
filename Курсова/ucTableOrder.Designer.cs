namespace Курсова
{
    partial class ucTableOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTableOrder));
            this.panel3 = new System.Windows.Forms.Panel();
            this.flpProducts = new Курсова.ModernFlowLayoutPanel();
            this.lblPrice = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPayment = new Курсова.ModernButton();
            this.btnCancel = new Курсова.ModernButton();
            this.btnConfirm = new Курсова.ModernButton();
            this.lstCart = new System.Windows.Forms.ListBox();
            this.btnClear = new Курсова.ModernButton();
            this.btnMinus = new Курсова.ModernButton();
            this.btnPlus = new Курсова.ModernButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flpCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.flpProducts);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(438, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(933, 840);
            this.panel3.TabIndex = 1;
            // 
            // flpProducts
            // 
            this.flpProducts.AutoScroll = true;
            this.flpProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpProducts.BorderRadius = 0;
            this.flpProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpProducts.ForeColor = System.Drawing.Color.MidnightBlue;
            this.flpProducts.GradientAngle = 90F;
            this.flpProducts.GradientColor1 = System.Drawing.Color.DarkBlue;
            this.flpProducts.GradientColor2 = System.Drawing.Color.Blue;
            this.flpProducts.Location = new System.Drawing.Point(0, 0);
            this.flpProducts.Name = "flpProducts";
            this.flpProducts.Size = new System.Drawing.Size(933, 840);
            this.flpProducts.TabIndex = 0;
            this.flpProducts.Paint += new System.Windows.Forms.PaintEventHandler(this.modernFlowLayoutPanel1_Paint);
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.SystemColors.Window;
            this.lblPrice.Location = new System.Drawing.Point(112, 765);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(186, 75);
            this.lblPrice.TabIndex = 0;
            this.lblPrice.Text = "0.00 ₴";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel1.Controls.Add(this.btnPayment);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnConfirm);
            this.panel1.Controls.Add(this.lblPrice);
            this.panel1.Controls.Add(this.lstCart);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnMinus);
            this.panel1.Controls.Add(this.btnPlus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 840);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnPayment
            // 
            this.btnPayment.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnPayment.BorderRadius = 20;
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.GradientColor1 = System.Drawing.Color.DarkBlue;
            this.btnPayment.GradientColor2 = System.Drawing.Color.Blue;
            this.btnPayment.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnPayment.Image = ((System.Drawing.Image)(resources.GetObject("btnPayment.Image")));
            this.btnPayment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPayment.Location = new System.Drawing.Point(51, 652);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(320, 80);
            this.btnPayment.TabIndex = 7;
            this.btnPayment.Text = "Платити";
            this.btnPayment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkRed;
            this.btnCancel.BorderRadius = 20;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.GradientColor1 = System.Drawing.Color.DarkRed;
            this.btnCancel.GradientColor2 = System.Drawing.Color.Red;
            this.btnCancel.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(47, 533);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(324, 88);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Скасувати Замовлення";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.DarkGreen;
            this.btnConfirm.BorderRadius = 20;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.GradientColor1 = System.Drawing.Color.DarkGreen;
            this.btnConfirm.GradientColor2 = System.Drawing.Color.Green;
            this.btnConfirm.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("btnConfirm.Image")));
            this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfirm.Location = new System.Drawing.Point(47, 414);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(320, 88);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "Підтвердіть Замовлення";
            this.btnConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lstCart
            // 
            this.lstCart.Font = new System.Drawing.Font("Myanmar Text", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstCart.FormattingEnabled = true;
            this.lstCart.ItemHeight = 53;
            this.lstCart.Location = new System.Drawing.Point(0, 0);
            this.lstCart.Name = "lstCart";
            this.lstCart.Size = new System.Drawing.Size(438, 322);
            this.lstCart.TabIndex = 0;
            this.lstCart.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged_2);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnClear.BorderRadius = 20;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.GradientColor1 = System.Drawing.Color.Orange;
            this.btnClear.GradientColor2 = System.Drawing.Color.Gold;
            this.btnClear.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnClear.Location = new System.Drawing.Point(293, 328);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(139, 59);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Видал.";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnMinus.BorderRadius = 20;
            this.btnMinus.FlatAppearance.BorderSize = 0;
            this.btnMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMinus.ForeColor = System.Drawing.Color.White;
            this.btnMinus.GradientColor1 = System.Drawing.Color.DarkRed;
            this.btnMinus.GradientColor2 = System.Drawing.Color.Red;
            this.btnMinus.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnMinus.Location = new System.Drawing.Point(145, 328);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(139, 59);
            this.btnMinus.TabIndex = 9;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnPlus.BorderRadius = 20;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPlus.ForeColor = System.Drawing.Color.White;
            this.btnPlus.GradientColor1 = System.Drawing.Color.LimeGreen;
            this.btnPlus.GradientColor2 = System.Drawing.Color.Green;
            this.btnPlus.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnPlus.Location = new System.Drawing.Point(0, 328);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(139, 59);
            this.btnPlus.TabIndex = 8;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel2.Controls.Add(this.flpCategories);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1165, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 840);
            this.panel2.TabIndex = 1;
            // 
            // flpCategories
            // 
            this.flpCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCategories.Location = new System.Drawing.Point(0, 0);
            this.flpCategories.Name = "flpCategories";
            this.flpCategories.Size = new System.Drawing.Size(206, 840);
            this.flpCategories.TabIndex = 0;
            // 
            // ucTableOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkMagenta;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "ucTableOrder";
            this.Size = new System.Drawing.Size(1371, 840);
            this.Load += new System.EventHandler(this.ucTableOrder_Load);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private ModernFlowLayoutPanel flpProducts;
        private ModernButton btnConfirm;
        private ModernButton btnCancel;
        private ModernButton btnPayment;
        private System.Windows.Forms.Label lblPrice;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lstCart;
        private ModernButton btnClear;
        private ModernButton btnMinus;
        private ModernButton btnPlus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
    }
}
