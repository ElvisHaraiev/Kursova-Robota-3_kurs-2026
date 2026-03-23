using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Курсова
{
    public class ModernPanel : Panel
    {
        private Color gradientColor1 = Color.MidnightBlue;
        private Color gradientColor2 = Color.Blue;
        private float gradientAngle = 90F;

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
        [Description("The angle of the color gradient (90 = Vertical, 0 = Horizontal).")]
        public float GradientAngle
        {
            get { return gradientAngle; }
            set { gradientAngle = value; this.Invalidate(); }
        }

        public ModernPanel()
        {
            this.DoubleBuffered = true;
            this.Resize += (s, e) => this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.ClientRectangle.Width > 0 && this.ClientRectangle.Height > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, gradientColor1, gradientColor2, gradientAngle))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }
    }
}