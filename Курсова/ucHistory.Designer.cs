namespace Курсова
{
    partial class ucHistory
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
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.btnBack = new Курсова.ModernButton();
            this.btnViewReceipt = new Курсова.ModernButton();
            this.cmbFilter = new Курсова.ModernComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHistory
            // 
            this.dgvHistory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(416, -4);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.RowHeadersWidth = 51;
            this.dgvHistory.RowTemplate.Height = 24;
            this.dgvHistory.Size = new System.Drawing.Size(1335, 1006);
            this.dgvHistory.TabIndex = 0;
            this.dgvHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellContentClick);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Blue;
            this.btnBack.BorderRadius = 80;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Myanmar Text", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.GradientColor1 = System.Drawing.Color.MidnightBlue;
            this.btnBack.GradientColor2 = System.Drawing.Color.Blue;
            this.btnBack.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnBack.Location = new System.Drawing.Point(6, 177);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(407, 191);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "⬅ НА ГОЛОВНУ";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnViewReceipt
            // 
            this.btnViewReceipt.BackColor = System.Drawing.Color.White;
            this.btnViewReceipt.BorderRadius = 80;
            this.btnViewReceipt.FlatAppearance.BorderSize = 0;
            this.btnViewReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewReceipt.Font = new System.Drawing.Font("Myanmar Text", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewReceipt.ForeColor = System.Drawing.Color.White;
            this.btnViewReceipt.GradientColor1 = System.Drawing.Color.MidnightBlue;
            this.btnViewReceipt.GradientColor2 = System.Drawing.Color.Blue;
            this.btnViewReceipt.HoverEffect = Курсова.ButtonEffects.BounceAndColor;
            this.btnViewReceipt.Location = new System.Drawing.Point(3, 443);
            this.btnViewReceipt.Name = "btnViewReceipt";
            this.btnViewReceipt.Size = new System.Drawing.Size(410, 191);
            this.btnViewReceipt.TabIndex = 4;
            this.btnViewReceipt.Text = "ПЕРЕГЛЯНУТИ ЧЕК/ ДРУКУВАТИ ЧЕК";
            this.btnViewReceipt.UseVisualStyleBackColor = false;
            this.btnViewReceipt.Click += new System.EventHandler(this.btnViewReceipt_Click);
            // 
            // cmbFilter
            // 
            this.cmbFilter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cmbFilter.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.cmbFilter.BorderRadius = 20;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "Весь час",
            "Сьогодні (щоденно)",
            "Цей тиждень (щотижня)",
            "Цей місяць (щомісяця)"});
            this.cmbFilter.Location = new System.Drawing.Point(1757, 0);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(167, 31);
            this.cmbFilter.TabIndex = 3;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // ucHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnViewReceipt);
            this.Controls.Add(this.dgvHistory);
            this.Name = "ucHistory";
            this.Size = new System.Drawing.Size(2002, 908);
            this.Load += new System.EventHandler(this.ucHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHistory;
        private ModernComboBox cmbFilter;
        private ModernButton btnViewReceipt;
        private ModernButton btnBack;
    }
}
