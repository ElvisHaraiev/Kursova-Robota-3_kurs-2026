using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Курсова
{
    public class ModernComboBox : ComboBox
    {
        // --- AYARLANABİLİR ÖZELLİKLER ---
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 1;
        private int borderRadius = 15; // Mobbin tarzı yumuşak köşe
        private Color iconColor = Color.Gray;
        private Color fillColor = Color.WhiteSmoke;

        [Category("Modern Tasarım")]
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }

        [Category("Modern Tasarım")]
        public int BorderRadius { get => borderRadius; set { borderRadius = value; Invalidate(); } }

        public ModernComboBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DropDownStyle = ComboBoxStyle.DropDownList; // Mobbin tarzı için sadece seçim modu
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);
        }

        // Köşeleri yuvarlatmak için yol oluşturucu
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectContour = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            Rectangle rectIcon = new Rectangle(this.Width - 30, 0, 30, this.Height);

            using (GraphicsPath pathContour = GetFigurePath(rectContour, borderRadius))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            using (SolidBrush brushFill = new SolidBrush(fillColor))
            {
                // 1. Arka planı ve bölgeyi (Region) çiz
                this.Region = new Region(pathContour);
                g.FillPath(brushFill, pathContour);

                // 2. Kenarlığı çiz
                g.DrawPath(penBorder, pathContour);

                // 3. Dropdown Okunu Çiz (Minimalist Ok)
                Point[] arrowPoints = new Point[] {
                    new Point(this.Width - 20, this.Height / 2 - 2),
                    new Point(this.Width - 15, this.Height / 2 + 3),
                    new Point(this.Width - 10, this.Height / 2 - 2)
                };
                using (Pen penIcon = new Pen(iconColor, 2))
                {
                    g.DrawLines(penIcon, arrowPoints);
                }

                // 4. Seçili metni yazdır
                string text = this.SelectedIndex >= 0 ? this.Items[this.SelectedIndex].ToString() : this.Text;
                g.DrawString(text, this.Font, Brushes.Black, 10, (this.Height - g.MeasureString(text, this.Font).Height) / 2);
            }
        }
    }
}