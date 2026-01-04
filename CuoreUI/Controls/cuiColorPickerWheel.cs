using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CuoreUI.Helpers.DrawingHelper;
using static CuoreUI.Helpers.GeneralHelper;

namespace CuoreUI.Controls
{
    [Description("HSV Color picker wheel, triangle inside")]
    [DefaultEvent("SelectedColor")]
    public partial class cuiColorPickerWheel : UserControl
    {
        private Bitmap privateHueBitmap;
        private Bitmap privateTriangleBitmap;

        public cuiColorPickerWheel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, false);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            privateHueBitmap?.Dispose();
            privateHueBitmap = null;
            privateTriangleBitmap?.Dispose();
            privateTriangleBitmap = null;
            UpdateClickedRectangleFromColor();
            Invalidate();
        }

        private int privateWheelThickness = 16;
        [Category("CuoreUI")]
        [Description("The Hue ring's thickness. The bigger it is, the smaller the triangle inside.")]
        public int WheelThickness
        {
            get
            {
                return privateWheelThickness;
            }
            set
            {
                privateWheelThickness = value;
                privateHueBitmap?.Dispose();
                privateHueBitmap = null;
                privateTriangleBitmap?.Dispose();
                privateTriangleBitmap = null;
                Invalidate();
            }
        }

        #region hue ring & sat/val triangle
        private void GenerateHueBitmap()
        {

            int size = Math.Min(Width, Height);
            if (size <= 0)
            {
                return;
            }

            int outerRadius = size / 2 - 1;
            int innerRadius = outerRadius - WheelThickness;
            Point center = new Point(size / 2, size / 2);
            Rectangle rect = new Rectangle(0, 0, size, size);

            BitmapData bmpData = null;
            try
            {
                privateHueBitmap?.Dispose();
                privateHueBitmap = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bmpData = privateHueBitmap.LockBits(rect, ImageLockMode.WriteOnly, privateHueBitmap.PixelFormat);
            }
            catch
            {
                // Most likely that bitmap is already locked
                return;
            }

            int bytesPerPixel = 4;
            int stride = bmpData.Stride;
            IntPtr ptr = bmpData.Scan0;
            int length = stride * size;
            byte[] pixels = new byte[length];

            Parallel.For(0, size, y =>
            {
                for (int x = 0; x < size; x++)
                {
                    int dx = x - center.X;
                    int dy = y - center.Y;
                    double dist = Math.Sqrt(dx * dx + dy * dy);

                    int index = y * stride + x * bytesPerPixel;

                    if (dist >= innerRadius && dist <= outerRadius)
                    {
                        double angle = Math.Atan2(dy, dx) * (180.0 / Math.PI);
                        if (angle < 0) angle += 360;
                        Color color = ColorFromHSV(angle, 1, 1);
                        pixels[index] = color.B;
                        pixels[index + 1] = color.G;
                        pixels[index + 2] = color.R;
                        pixels[index + 3] = 255;
                    }
                    else
                    {
                        pixels[index] = 0;
                        pixels[index + 1] = 0;
                        pixels[index + 2] = 0;
                        pixels[index + 3] = 0;
                    }
                }
            });

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, length);
            privateHueBitmap.UnlockBits(bmpData);
        }

        private void GenerateTriangleBitmap(double hue, int size, int innerRadius)
        {
            BitmapData bmpData = null;
            try
            {
                privateTriangleBitmap?.Dispose();
                privateTriangleBitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
                bmpData = privateTriangleBitmap.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.WriteOnly, privateTriangleBitmap.PixelFormat);
            }
            catch
            {
                // Most likely that bitmap is already locked
                return;
            }

            Point center = new Point(size / 2, size / 2);

            // vertices of all sides even triangle inside innerRadius
            PointF pHue = new PointF(center.X, center.Y - innerRadius); // top vertex (hue, sat=1, val=1)
            PointF pWhite = RotatePoint(center, new PointF(center.X, center.Y - innerRadius), 120); // bottom-left
            PointF pBlack = RotatePoint(center, new PointF(center.X, center.Y - innerRadius), 240); // bottom-right

            int bytesPerPixel = 4;
            int stride = bmpData.Stride;
            IntPtr ptr = bmpData.Scan0;
            int length = stride * size;
            byte[] pixels = new byte[length];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    PointF p = new PointF(x, y);
                    if (PointInTriangle(p, pHue, pWhite, pBlack))
                    {
                        var barycentric = BarycentricCoords(p, pHue, pWhite, pBlack);

                        // bary.X = hue
                        // bary.Y = white
                        // bary.Z = black

                        // interpolate sat and val
                        double saturation = barycentric.X;
                        double value = barycentric.X + barycentric.Y;

                        Color color = ColorFromHSV(hue, saturation, value);

                        int idx = y * stride + x * bytesPerPixel;
                        pixels[idx] = color.B;
                        pixels[idx + 1] = color.G;
                        pixels[idx + 2] = color.R;
                        pixels[idx + 3] = 255;
                    }
                    else
                    {
                        int idx = y * stride + x * bytesPerPixel;
                        pixels[idx] = 0;
                        pixels[idx + 1] = 0;
                        pixels[idx + 2] = 0;
                        pixels[idx + 3] = 0;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, length);
            privateTriangleBitmap.UnlockBits(bmpData);
        }
        #endregion

        double previouslyPaintedHue = 0;
        double privateHue = 0;
        double privateSaturation = 0;
        double privateValue = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int size = Math.Min(Width, Height);
            int x = (Width - size) / 2;
            int y = (Height - size) / 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // hue ring
            if (privateHueBitmap == null)
            {
                GenerateHueBitmap();
            }

            try
            {
                e.Graphics.DrawImage(privateHueBitmap, x, y, size, size);
            }
            catch
            {
                // Most likely hue ring bitmap is locked and it shouldn't be touched right now
                return;
            }

            int outerRadius = size / 2 - 1;
            int innerRadius = outerRadius - WheelThickness;

            // value/sat triangle
            if (privateTriangleBitmap == null || previouslyPaintedHue != privateHue)
            {
                previouslyPaintedHue = privateHue;
                GenerateTriangleBitmap((int)privateHue, size, innerRadius - 1);
                //GenerateHueBitmap();
            }

            using (Pen antialiasPen = new Pen(BackColor, 4))
            using (Pen whereClickPen1 = new Pen(Color.FromArgb(128, 0, 0, 0), 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                Rectangle modifiedCR = ClientRectangle;
                modifiedCR.Size = new Size(size, size);
                modifiedCR.X = x;
                modifiedCR.Y = y;
                modifiedCR.Inflate(-1, -1);

                e.Graphics.DrawEllipse(antialiasPen, modifiedCR);
                modifiedCR.Inflate(-WheelThickness, -WheelThickness);
                e.Graphics.DrawEllipse(antialiasPen, modifiedCR);

                try
                {
                    e.Graphics.DrawImage(privateTriangleBitmap, x, y, size, size);
                }
                catch
                {
                    // Most likely sat/val triangle bitmap is locked and it shouldn't be touched right now
                    return;
                }

                int centerX = Width / 2;
                int centerY = Height / 2;

                float r = innerRadius;

                // triangle with all sides even
                // the center and left are offset because when the control's
                // width is odd there is a small visual glitch without this code
                // (because we fake antialias for the bitmaps)
                PointF p1 = new PointF(centerX, centerY - r);
                PointF p1v2 = new PointF(centerX - 1, centerY - r);
                PointF p2 = new PointF(centerX + r * (float)Math.Sin(Math.PI / 3) - 1, centerY + r * (float)Math.Cos(Math.PI / 3) - 1);
                PointF p3 = new PointF(centerX - r * (float)Math.Sin(Math.PI / 3), centerY + r * (float)Math.Cos(Math.PI / 3) - 1);

                PointF[] trianglePoints = { p1, p2, p3 };
                PointF[] trianglePointsv2 = { p1v2, p2, p3 };

                antialiasPen.Width = 2;
                e.Graphics.DrawPolygon(antialiasPen, trianglePoints);
                e.Graphics.DrawPolygon(antialiasPen, trianglePointsv2);

                // hue is range <0 - 360)
                double radians = privateHue * 3.14d / 180.0;
                float startX = centerX + (float)(((Width > Height ? centerY : centerX) - 2) * Math.Cos(radians));
                float startY = centerY + (float)((centerY - 2) * Math.Sin(radians));

                PointF p1hueSelectorPoint = new PointF(startX, startY);
                PointF p2hueSelectorPoint = PointTowardsCenter(p1hueSelectorPoint, centerX, centerY, privateWheelThickness);

                e.Graphics.DrawEllipse(whereClickPen1, clickRectangle);
                whereClickPen1.Width = 4f;
                e.Graphics.DrawLine(whereClickPen1, p1hueSelectorPoint.X, p1hueSelectorPoint.Y, p2hueSelectorPoint.X, p2hueSelectorPoint.Y);
                whereClickPen1.Width = 0.4f;
                whereClickPen1.Color = Color.White;
                e.Graphics.DrawEllipse(whereClickPen1, clickRectangle);
                whereClickPen1.Width = 3f;
                e.Graphics.DrawLine(whereClickPen1, p1hueSelectorPoint.X, p1hueSelectorPoint.Y, p2hueSelectorPoint.X, p2hueSelectorPoint.Y);

                // e.Graphics.DrawString(privateHue.ToString(), Font, Brushes.Black, Point.Empty);
            }
        }

        private PointF PointTowardsCenter(PointF inputPoint, float centerX, float centerY, double distance)
        {
            double dx = centerX - inputPoint.X;
            double dy = centerY - inputPoint.Y;
            double len = Math.Sqrt(dx * dx + dy * dy);

            if (len == 0.0) // ??
            {
                return inputPoint;
            }
            if (distance >= len)
            {
                return new PointF(centerX, centerY);
            }

            double ux = dx / len;
            double uy = dy / len;

            return new PointF(
                (float)(inputPoint.X + ux * distance),
                (float)(inputPoint.Y + uy * distance)
            );
        }


        // WHERE THE USER IS CLICKING
        // 0 - normal
        // 1 - hue ring
        // 2 - sat/val triangle
        byte state = 0;

        [Category("CuoreUI")]
        [Description("Any change in hue, brightness or saturation will invoke this event.")]
        public event EventHandler ContentChanged;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ColorToHSV(privateContent, out privateHue, out privateSaturation, out privateValue);
            UpdateClickedRectangleFromColor();
        }

        private Color privateContent = Color.Red;
        [Category("CuoreUI")]
        public Color Content
        {
            get
            {
                return privateContent;
            }
            set
            {
                float oldHue = privateContent.GetHue();
                privateContent = value;

                if (DesignMode)
                {
                    ColorToHSV(value, out privateHue, out _, out _);
                }
                else
                {
                    float newHue = value.GetHue();
                    if (oldHue != newHue)
                    {
                        privateHue = newHue;
                        privateHueBitmap = null;
                    }

                    // if color changes, but mouse is not over this wheel, fire the SelectedColor event
                    // (means the color was changed programatically, and not by the user)
                    // do not use ClientRectangle.Contains(PointToClient(Cursor.Position))
                    // since if the user were to click a control on TOP OF this wheel, the event wouldn't fire
                    if (!isMouseOnControl)
                    {
                        SelectedColor?.Invoke(this, EventArgs.Empty);
                    }
                }

                if (state == 0)
                {
                    UpdateClickedRectangleFromColor();
                }

                ContentChanged?.Invoke(null, EventArgs.Empty);
                Invalidate();
            }
        }

        private void UpdateClickedRectangleFromColor()
        {
            int size = Math.Min(Width, Height);
            int centerX = Width / 2;
            int centerY = Height / 2;
            int outerRadius = size / 2 - 1;
            int innerRadius = outerRadius - WheelThickness;

            float r = innerRadius - 1;
            PointF center = new PointF(centerX, centerY);
            PointF pHue = new PointF(center.X, center.Y - r);
            PointF pWhite = RotatePoint(center, pHue, 120);
            PointF pBlack = RotatePoint(center, pHue, 240);

            ColorToHSV(privateContent, out double h, out double s, out double v);

            double w1 = s;
            double w2 = v - s;
            double w3 = 1.0 - w1 - w2;

            float x = (float)(w1 * pHue.X + w2 * pWhite.X + w3 * pBlack.X);
            float y = (float)(w1 * pHue.Y + w2 * pWhite.Y + w3 * pBlack.Y);

            PointF candidate = new PointF(x, y);

            // clamp candidate inside triangle
            if (!PointInTriangle(candidate, pHue, pWhite, pBlack))
            {
                candidate = ClosestPointOnTriangle(candidate, pHue, pWhite, pBlack);
            }

            clickRectangle = new RectangleF(candidate.X - 4, candidate.Y - 4, 8, 8);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        RectangleF clickRectangle = new RectangleF(-8, -8, 8, 8);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int size = Math.Min(Width, Height);
                PointF center = new PointF(Width / 2f, Height / 2f);
                float rOuter = size / 2f - 1;
                float rInner = rOuter - WheelThickness;

                byte currentAlpha = Content.A;

                // changing hue (ring)
                if (state == 1)
                {
                    float dx = e.X - center.X;
                    float dy = e.Y - center.Y;
                    float angle = (float)(Math.Atan2(dy, dx) * 180.0 / Math.PI);
                    if (angle < 0) angle += 360;

                    privateHue = (int)angle;
                    privateTriangleBitmap?.Dispose();
                    privateTriangleBitmap = null;
                    Content = ColorFromHSV(privateHue, privateSaturation, privateValue, currentAlpha);
                }

                // changing saturation or value (triangle)
                else if (state == 2)
                {
                    float r = rInner - 1;
                    PointF p1 = new PointF(center.X, center.Y - r);
                    PointF p2 = new PointF(center.X + r * (float)Math.Sin(Math.PI / 3) - 1, center.Y + r * (float)Math.Cos(Math.PI / 3) - 1);
                    PointF p3 = new PointF(center.X - r * (float)Math.Sin(Math.PI / 3), center.Y + r * (float)Math.Cos(Math.PI / 3) - 1);

                    PointF p = e.Location;
                    if (!PointInTriangle(p, p1, p2, p3))
                    {
                        // nearest point in triangle from mouse cursor location
                        p = ClosestPointOnTriangle(p, p1, p2, p3);
                    }

                    var bary = BarycentricCoords(p, p1, p2, p3);

                    //

                    clickRectangle.X = (int)p.X - 4;
                    clickRectangle.Y = (int)p.Y - 4;

                    //

                    privateSaturation = bary.X;
                    privateValue = bary.X + bary.Y;

                    // don't replace privateContent with Content
                    // Content calculates new hue values with GetHue,
                    // but we don't want to change the hue while the user is changing the saturation and value
                    privateContent = ColorFromHSV(privateHue, privateSaturation, privateValue, currentAlpha);
                    ContentChanged?.Invoke(null, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsInHueRing(e.Location))
            {
                state = 1;
            }
            else if (IsInValueTriangle(e.Location))
            {
                state = 2;
            }
            else
            {
                state = 0;
            }
        }

        [Category("CuoreUI")]
        [Description("Gets invoked whenever the user releases their mouse, and the color has changed.")]
        public event EventHandler SelectedColor;

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (state != 0)
            {
                SelectedColor?.Invoke(this, EventArgs.Empty);
            }

            state = 0;
            base.OnMouseUp(e);
        }

        private bool IsInHueRing(Point point)
        {
            int size = Math.Min(Width, Height);
            Point center = new Point(Width / 2, Height / 2);

            int dx = point.X - center.X;
            int dy = point.Y - center.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            int outerRadius = size / 2 - 1;
            int innerRadius = outerRadius - WheelThickness;

            return dist >= innerRadius && dist <= outerRadius;
        }

        private bool IsInValueTriangle(Point point)
        {
            int size = Math.Min(Width, Height);
            int innerRadius = size / 2 - 1 - WheelThickness;
            PointF center = new PointF(Width / 2f, Height / 2f);
            float r = innerRadius;

            PointF p1 = new PointF(center.X, center.Y - r);
            PointF p2 = new PointF(center.X + r * (float)Math.Sin(Math.PI / 3) - 1, center.Y + r * (float)Math.Cos(Math.PI / 3) - 1);
            PointF p3 = new PointF(center.X - r * (float)Math.Sin(Math.PI / 3), center.Y + r * (float)Math.Cos(Math.PI / 3) - 1);

            return PointInTriangle(point, p1, p2, p3);
        }

        private bool isMouseOnControl = false;

        protected override void OnMouseEnter(EventArgs e)
        {
            isMouseOnControl = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isMouseOnControl = false;
            base.OnMouseLeave(e);
        }
    }
}