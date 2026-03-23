using System;
using System.ComponentModel; // 🚀 Required for Properties panel
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Курсова
{
    public class ModernFlowLayoutPanel : FlowLayoutPanel
    {
        private int borderRadius = 0;
        private Color gradientColor1 = Color.FromArgb(40, 40, 50);
        private Color gradientColor2 = Color.FromArgb(40, 40, 50);

        private float gradientAngle = 90F;

        [Category("Modern Design")]
        [Description("The border radius of the panel. Set to 0 for a sharp square.")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; this.Invalidate(); }
        }

        [Category("Modern Design")]
        [Description("The first (top/left) color of the gradient.")]
        public Color GradientColor1
        {
            get { return gradientColor1; }
            set { gradientColor1 = value; this.Invalidate(); }
        }

        [Category("Modern Design")]
        [Description("The second (bottom/right) color of the gradient.")]
        public Color GradientColor2
        {
            get { return gradientColor2; }
            set { gradientColor2 = value; this.Invalidate(); }
        }

        [Category("Modern Design")]
        [Description("The angle of the color gradient (90 for Vertical, 0 for Horizontal).")]
        public float GradientAngle
        {
            get { return gradientAngle; }
            set { gradientAngle = value; this.Invalidate(); }
        }
        public ModernFlowLayoutPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
            this.Resize += (s, e) => this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = this.ClientRectangle;

            if (rectSurface.Width > 0 && rectSurface.Height > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(rectSurface, gradientColor1, gradientColor2, gradientAngle))
                {
                    if (borderRadius > 2)
                    {
                        using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                        {
                            this.Region = new Region(pathSurface);
                            e.Graphics.FillPath(brush, pathSurface);
                        }
                    }
                    else
                    {
                        this.Region = new Region(rectSurface);
                        e.Graphics.FillRectangle(brush, rectSurface);
                    }
                }
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