namespace Курсова
{
    partial class ucLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ucLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Name = "ucLogin";
            this.Size = new System.Drawing.Size(1855, 800);
            this.Load += new System.EventHandler(this.ucLogin_Load);
            this.ResumeLayout(false);

        }
    }
}