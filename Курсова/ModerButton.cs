using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Курсова
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public string NewRestaurantName { get; set; }
        public Image NewRestaurantLogo { get; set; }
    }

    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);



    public enum ButtonEffects
    {
        None,
        Bounce,
        ColorShift,
        BounceAndColor
    }

    public class ModernButton : Button
    {
        private int borderRadius = 20;
        private Color gradientColor1 = Color.MediumSlateBlue;
        private Color gradientColor2 = Color.MediumSlateBlue;

        private bool isHovered = false;
        private bool isPressed = false;
        private ButtonEffects currentEffect = ButtonEffects.BounceAndColor;


        [Category("🚀 Modern Design")]
        [Description("Butonun animasyon efektini seçin.")]
        public ButtonEffects HoverEffect
        {
            get { return currentEffect; }
            set { currentEffect = value; this.Invalidate(); }
        }

        [Category("🚀 Modern Design")]
        [Description("The border radius of the button.")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; this.Invalidate(); }
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


        public ModernButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Resize += (s, e) => this.Invalidate();
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            isPressed = true;

            if (currentEffect == ButtonEffects.Bounce || currentEffect == ButtonEffects.BounceAndColor)
            {
                this.Location = new Point(this.Location.X, this.Location.Y + 2);
            }
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isPressed = false;

            if (currentEffect == ButtonEffects.Bounce || currentEffect == ButtonEffects.BounceAndColor)
            {
                this.Location = new Point(this.Location.X, this.Location.Y - 2);
            }
            this.Invalidate();
        }


        private Color LightenColor(Color color, float amount)
        {
            return Color.FromArgb(color.A,
                (int)Math.Min(255, color.R + 255 * amount),
                (int)Math.Min(255, color.G + 255 * amount),
                (int)Math.Min(255, color.B + 255 * amount));
        }

        private Color DarkenColor(Color color, float amount)
        {
            return Color.FromArgb(color.A,
                (int)Math.Max(0, color.R - 255 * amount),
                (int)Math.Max(0, color.G - 255 * amount),
                (int)Math.Max(0, color.B - 255 * amount));
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectSurface = this.ClientRectangle;

            Color c1 = gradientColor1;
            Color c2 = gradientColor2;

            if (currentEffect == ButtonEffects.ColorShift || currentEffect == ButtonEffects.BounceAndColor)
            {
                if (isPressed)
                {
                    c1 = DarkenColor(c1, 0.2f);
                    c2 = DarkenColor(c2, 0.2f);
                }
                else if (isHovered)
                {
                    c1 = LightenColor(c1, 0.2f);
                    c2 = LightenColor(c2, 0.2f);
                }
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(rectSurface, c1, c2, LinearGradientMode.Vertical))
            {
                if (borderRadius > 2)
                {
                    using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                    using (Pen penSurface = new Pen(this.Parent != null ? this.Parent.BackColor : Color.White, 2))
                    {
                        pevent.Graphics.FillPath(brush, pathSurface);
                        this.Region = new Region(pathSurface);
                        pevent.Graphics.DrawPath(penSurface, pathSurface);
                    }
                }
                else
                {
                    pevent.Graphics.FillRectangle(brush, rectSurface);
                    this.Region = new Region(rectSurface);
                }
            }

            // Çizim Alanı (Padding dikkate alınır)
            Rectangle paddedRect = new Rectangle(
                rectSurface.X + this.Padding.Left,
                rectSurface.Y + this.Padding.Top,
                rectSurface.Width - this.Padding.Horizontal,
                rectSurface.Height - this.Padding.Vertical
            );

            // 2. DRAW IMAGE
            if (this.Image != null)
            {
                Point imgPoint = GetImageLocation(paddedRect, this.Image.Size);
                pevent.Graphics.DrawImage(this.Image, imgPoint.X, imgPoint.Y, this.Image.Width, this.Image.Height);
            }

            // 3. DRAW TEXT
            TextFormatFlags flags = GetTextAlignmentFlags() | TextFormatFlags.WordBreak;
            Rectangle textRect = paddedRect;

            if (this.Image != null && !string.IsNullOrEmpty(this.Text))
            {
                if (this.TextImageRelation == TextImageRelation.ImageBeforeText)
                {
                    int offset = this.Image.Width + 4;
                    textRect.X += offset;
                    textRect.Width -= offset;
                }
                else if (this.TextImageRelation == TextImageRelation.ImageAboveText)
                {
                    int offset = this.Image.Height + 4;
                    textRect.Y += offset;
                    textRect.Height -= offset;
                }
            }

            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, textRect, this.ForeColor, flags);
        }

        private TextFormatFlags GetTextAlignmentFlags()
        {
            TextFormatFlags flags = TextFormatFlags.Default;
            if (this.TextAlign == ContentAlignment.TopLeft || this.TextAlign == ContentAlignment.TopCenter || this.TextAlign == ContentAlignment.TopRight) flags |= TextFormatFlags.Top;
            else if (this.TextAlign == ContentAlignment.BottomLeft || this.TextAlign == ContentAlignment.BottomCenter || this.TextAlign == ContentAlignment.BottomRight) flags |= TextFormatFlags.Bottom;
            else flags |= TextFormatFlags.VerticalCenter;

            if (this.TextAlign == ContentAlignment.TopLeft || this.TextAlign == ContentAlignment.MiddleLeft || this.TextAlign == ContentAlignment.BottomLeft) flags |= TextFormatFlags.Left;
            else if (this.TextAlign == ContentAlignment.TopRight || this.TextAlign == ContentAlignment.MiddleRight || this.TextAlign == ContentAlignment.BottomRight) flags |= TextFormatFlags.Right;
            else flags |= TextFormatFlags.HorizontalCenter;
            return flags;
        }

        private Point GetImageLocation(Rectangle rect, Size imgSize)
        {
            int x = rect.X; int y = rect.Y;
            if (this.ImageAlign == ContentAlignment.TopCenter || this.ImageAlign == ContentAlignment.MiddleCenter || this.ImageAlign == ContentAlignment.BottomCenter) x = rect.X + (rect.Width - imgSize.Width) / 2;
            else if (this.ImageAlign == ContentAlignment.TopRight || this.ImageAlign == ContentAlignment.MiddleRight || this.ImageAlign == ContentAlignment.BottomRight) x = rect.Right - imgSize.Width;

            if (this.ImageAlign == ContentAlignment.MiddleLeft || this.ImageAlign == ContentAlignment.MiddleCenter || this.ImageAlign == ContentAlignment.MiddleRight) y = rect.Y + (rect.Height - imgSize.Height) / 2;
            else if (this.ImageAlign == ContentAlignment.BottomLeft || this.ImageAlign == ContentAlignment.BottomCenter || this.ImageAlign == ContentAlignment.BottomRight) y = rect.Bottom - imgSize.Height;
            return new Point(x, y);
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