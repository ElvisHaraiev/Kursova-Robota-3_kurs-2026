namespace Курсова
{
    partial class ucDeliveryChoose
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
            this.btnOrderHistory = new Курсова.ModernButton();
            this.btnCreateOrder = new Курсова.ModernButton();
            this.SuspendLayout();
            // 
            // btnOrderHistory
            // 
            this.btnOrderHistory.BackColor = System.Drawing.Color.Blue;
            this.btnOrderHistory.BorderRadius = 20;
            this.btnOrderHistory.FlatAppearance.BorderSize = 0;
            this.btnOrderHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrderHistory.Font = new System.Drawing.Font("Myanmar Text", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrderHistory.ForeColor = System.Drawing.Color.White;
            this.btnOrderHistory.GradientColor1 = System.Drawing.Color.Blue;
            this.btnOrderHistory.GradientColor2 = System.Drawing.Color.Gray;
            this.btnOrderHistory.Location = new System.Drawing.Point(946, 160);
            this.btnOrderHistory.Name = "btnOrderHistory";
            this.btnOrderHistory.Size = new System.Drawing.Size(822, 448);
            this.btnOrderHistory.TabIndex = 1;
            this.btnOrderHistory.Text = "Історія";
            this.btnOrderHistory.UseVisualStyleBackColor = false;
            this.btnOrderHistory.Click += new System.EventHandler(this.btnOrderHistory_Click);
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.BackColor = System.Drawing.Color.Blue;
            this.btnCreateOrder.BorderRadius = 20;
            this.btnCreateOrder.FlatAppearance.BorderSize = 0;
            this.btnCreateOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateOrder.Font = new System.Drawing.Font("Myanmar Text", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateOrder.ForeColor = System.Drawing.Color.White;
            this.btnCreateOrder.GradientColor1 = System.Drawing.Color.Blue;
            this.btnCreateOrder.GradientColor2 = System.Drawing.Color.Gray;
            this.btnCreateOrder.Location = new System.Drawing.Point(63, 160);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(822, 448);
            this.btnCreateOrder.TabIndex = 0;
            this.btnCreateOrder.Text = "Створюй Доставку";
            this.btnCreateOrder.UseVisualStyleBackColor = false;
            this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            // 
            // ucPaketSecim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.Controls.Add(this.btnOrderHistory);
            this.Controls.Add(this.btnCreateOrder);
            this.Name = "ucPaketSecim";
            this.Size = new System.Drawing.Size(1854, 1007);
            this.Load += new System.EventHandler(this.ucPaketSecim_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ModernButton btnCreateOrder;
        private ModernButton btnOrderHistory;
    }
}
