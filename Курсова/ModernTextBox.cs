using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Курсова
{
    public class ModernTextBox : Control
    {
        private TextBox baseTextBox;
        private int borderRadius = 20;
        private Color gradientColor1 = Color.MidnightBlue;
        private Color gradientColor2 = Color.Blue;
        private float gradientAngle = 90F;


        [Category("🚀 Modern Design")]
        [Description("The text color inside the box.")]
        public override Color ForeColor
        {
            get { return baseTextBox != null ? baseTextBox.ForeColor : Color.White; }
            set { if (baseTextBox != null) baseTextBox.ForeColor = value; }
        }

        [Category("🚀 Modern Design")]
        [Description("The font inside the box.")]
        public override Font Font
        {
            get { return baseTextBox != null ? baseTextBox.Font : new Font("Segoe UI", 12); }
            set { if (baseTextBox != null) baseTextBox.Font = value; }
        }

        [Category("🚀 Modern Design")]
        [Description("The actual text content.")]
        public override string Text
        {
            get { return baseTextBox != null ? baseTextBox.Text : ""; }
            set { if (baseTextBox != null) baseTextBox.Text = value; }
        }

        [Category("Modern Design")]
        [Description("The first (top) color of the gradient.")]
        public Color GradientColor1
        {
            get { return gradientColor1; }
            set { gradientColor1 = value; this.Invalidate(); }
        }

        [Category("Modern Design")]
        [Description("The second (bottom) color of the gradient.")]
        public Color GradientColor2
        {
            get { return gradientColor2; }
            set { gradientColor2 = value; this.Invalidate(); }
        }

        [Category("Modern Design")]
        [Description("The angle of the color gradient.")]
        public float GradientAngle
        {
            get { return gradientAngle; }
            set { gradientAngle = value; this.Invalidate(); }
        }


        public ModernTextBox()
        {
            baseTextBox = new TextBox();
            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.BackColor = Color.MidnightBlue;
            baseTextBox.ForeColor = Color.White;
            baseTextBox.Font = new Font("Segoe UI", 12);
            baseTextBox.Location = new Point(10, 10);
            baseTextBox.Size = new Size(this.Width - 20, this.Height - 20);
            baseTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(baseTextBox);
            this.Size = new Size(200, 40);
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = this.ClientRectangle;

            // Arka planı boya (Gradient)
            using (LinearGradientBrush brush = new LinearGradientBrush(rectSurface, gradientColor1, gradientColor2, gradientAngle))
            {
                e.Graphics.FillPath(brush, GetFigurePath(rectSurface, borderRadius));
            }
        }

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
    }
}