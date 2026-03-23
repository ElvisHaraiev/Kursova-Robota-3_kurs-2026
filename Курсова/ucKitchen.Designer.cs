namespace Курсова
{
    partial class ucKitchen
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
            this.flpOrders = new Курсова.ModernFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpOrders
            // 
            this.flpOrders.BackColor = System.Drawing.Color.Transparent;
            this.flpOrders.BorderRadius = 0;
            this.flpOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpOrders.GradientAngle = 90F;
            this.flpOrders.GradientColor1 = System.Drawing.Color.MidnightBlue;
            this.flpOrders.GradientColor2 = System.Drawing.Color.Blue;
            this.flpOrders.Location = new System.Drawing.Point(0, 0);
            this.flpOrders.Name = "flpOrders";
            this.flpOrders.Size = new System.Drawing.Size(1454, 632);
            this.flpOrders.TabIndex = 0;
            this.flpOrders.Paint += new System.Windows.Forms.PaintEventHandler(this.flpOrders_Paint);
            // 
            // ucKitchen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkMagenta;
            this.Controls.Add(this.flpOrders);
            this.Name = "ucKitchen";
            this.Size = new System.Drawing.Size(1454, 632);
            this.ResumeLayout(false);

        }

        #endregion

        private ModernFlowLayoutPanel flpOrders;
    }
}
