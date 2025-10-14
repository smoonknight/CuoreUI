using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Helpers
{
    public static class GeneralHelper
    {
        static GeneralHelper()
        {
            HandCursorFix.EnableModernCursor();
        }

        public static GraphicsPath RoundHexagon(Rectangle bounds, float rounding)
        {
            GraphicsPath path = new GraphicsPath();

            // so it doesnt crash (negative values also dont crash so thats cool)
            rounding = Math.Min(rounding, Math.Min(bounds.Width, bounds.Height) / 4f);

            PointF[] points = new PointF[6];
            float width = bounds.Width;
            float height = bounds.Height;

            points[0] = new PointF(bounds.X + width / 2, bounds.Y); // t
            points[1] = new PointF(bounds.X + width, bounds.Y + height / 4); // tr
            points[2] = new PointF(bounds.X + width, bounds.Y + 3 * height / 4); // br
            points[3] = new PointF(bounds.X + width / 2, bounds.Y + height); // b
            points[4] = new PointF(bounds.X, bounds.Y + 3 * height / 4); // bl
            points[5] = new PointF(bounds.X, bounds.Y + height / 4); // tl

            for (int i = 0; i < points.Length; i++)
            {
                // i did Beziers becuase Arcs are PAINN

                PointF current = points[i];
                PointF previous = points[(i - 1 + points.Length) % points.Length];
                PointF next = points[(i + 1) % points.Length];

                PointF dirToPrev = Normalize(new PointF(previous.X - current.X, previous.Y - current.Y));
                PointF dirToNext = Normalize(new PointF(next.X - current.X, next.Y - current.Y));

                PointF arcStart = new PointF(current.X + dirToPrev.X * rounding, current.Y + dirToPrev.Y * rounding);
                PointF arcEnd = new PointF(current.X + dirToNext.X * rounding, current.Y + dirToNext.Y * rounding);

                PointF control1 = new PointF(current.X + dirToPrev.X * (rounding / 2), current.Y + dirToPrev.Y * (rounding / 2));
                PointF control2 = new PointF(current.X + dirToNext.X * (rounding / 2), current.Y + dirToNext.Y * (rounding / 2));

                if (i == 0)
                {
                    path.StartFigure();
                }

                path.AddBezier(arcStart, control1, control2, arcEnd);
            }

            path.CloseFigure();
            return path;
        }

        public static PointF Normalize(PointF point)
        {
            float length = (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
            return new PointF(point.X / length, point.Y / length);
        }

        public static GraphicsPath RoundRect(Rectangle rectangle, int borderRadius)
        {
            return RoundRect(rectangle, new Padding(borderRadius));
        }

        public static GraphicsPath RoundRect(RectangleF rectangle, int borderRadius)
        {
            return RoundRect(rectangle, new Padding(borderRadius));
        }

        public static GraphicsPath RoundRect(RectangleF rectangle, Padding borderRadius)
        {
            GraphicsPath path = new GraphicsPath();

            float diameter1 = (float)checked(borderRadius.Top * 2);
            AddArc(rectangle.X, rectangle.Y, diameter1, 180f, 90f);

            float diameter2 = (float)checked(borderRadius.Left * 2);
            AddArc(rectangle.Right - diameter2, rectangle.Y, diameter2, 270f, 90f);

            float diameter3 = (float)checked(borderRadius.Bottom * 2);
            AddArc(rectangle.Right - diameter3, rectangle.Bottom - diameter3, diameter3, 0.0f, 90f);

            float diameter4 = (float)checked(borderRadius.Right * 2);
            AddArc(rectangle.X, rectangle.Bottom - diameter4, diameter4, 90f, 90f);

            path.CloseFigure();
            return path;

            void AddArc(float x, float y, float diameter, float startAngle, float sweepAngle)
            {
                if ((double)diameter > 0.0)
                {
                    RectangleF rect = new RectangleF(x, y, diameter, diameter);
                    path.AddArc(rect, startAngle, sweepAngle);
                }
                else
                    path.AddLine(x, y, x + 0.01f, y);
            }
        }

        public static GraphicsPath Checkmark(Rectangle area)
        {
            GraphicsPath path = new GraphicsPath();

            Point[] points = new Point[]
            {
            new Point(area.Left + (int)(area.Width * 0.25), area.Top + (int)(area.Height * 0.5)),
            new Point(area.Left + (int)(area.Width * 0.45), area.Top + (int)(area.Height * 0.7)),
            new Point(area.Right - (int)(area.Width * 0.3), area.Top + (int)(area.Height * 0.3))
            };

            path.AddLines(points);

            return path;
        }

        public static GraphicsPath Checkmark(RectangleF area, Point symbolsOffset)
        {
            GraphicsPath path = new GraphicsPath();

            area.Offset(symbolsOffset);

            PointF[] points = new PointF[]
            {
            new PointF(area.Left + (int)(area.Width * 0.25), area.Top + (int)(area.Height * 0.5)),
            new PointF(area.Left + (int)(area.Width * 0.45), area.Top + (int)(area.Height * 0.7)),
            new PointF(area.Right - (int)(area.Width * 0.3), area.Top + (int)(area.Height * 0.3))
            };

            path.AddLines(points);

            return path;
        }

        public static GraphicsPath Crossmark(Rectangle rect)
        {
            Rectangle area = rect;
            int WidthBeforeScale = area.Width;
            area.Width = (int)Math.Round(area.Width * 0.7f, 0);
            area.Height = area.Width;

            int WidthAfterScale = area.Width;
            int WidthDifference = WidthBeforeScale - WidthAfterScale;

            area.Offset(WidthDifference / 2, 1 + WidthDifference / 2);

            GraphicsPath path = new GraphicsPath();

            Point[] points = new Point[]
            {
            new Point(area.Left, area.Top),
            new Point(area.Right, area.Bottom)
            };

            path.AddLines(points);

            GraphicsPath path2 = new GraphicsPath();

            Point[] points2 = new Point[]
            {
            new Point(area.Left, area.Bottom),
            new Point(area.Right, area.Top)
            };

            path2.AddLines(points2);

            path.AddPath(path2, false);

            return path;
        }

        public static GraphicsPath Crossmark(RectangleF rect, Point symbolsOffset)
        {
            RectangleF area = rect;
            area.Offset(symbolsOffset);
            float WidthBeforeScale = area.Width;
            area.Width = (int)Math.Round(area.Width * 0.7f, 0);
            area.Height = area.Width;

            float WidthAfterScale = area.Width;
            float WidthDifference = WidthBeforeScale - WidthAfterScale;

            area.Offset(WidthDifference / 2, 1 + WidthDifference / 2);

            GraphicsPath path = new GraphicsPath();

            PointF[] points = new PointF[]
            {
            new PointF(area.Left, area.Top),
            new PointF(area.Right, area.Bottom)
            };

            path.AddLines(points);

            GraphicsPath path2 = new GraphicsPath();

            PointF[] points2 = new PointF[]
            {
            new PointF(area.Left, area.Bottom),
            new PointF(area.Right, area.Top)
            };

            path2.AddLines(points2);

            path.AddPath(path2, false);

            return path;
        }

        public static GraphicsPath Plus(Rectangle rect)
        {
            Rectangle area = rect;
            int widthBeforeScale = area.Width;
            area.Width = (int)Math.Round(area.Width * 0.7f, 0);
            area.Height = area.Width;

            int widthAfterScale = area.Width;
            int widthDifference = widthBeforeScale - widthAfterScale;

            area.Offset(widthDifference / 2, 1 + widthDifference / 2);

            GraphicsPath path = new GraphicsPath();

            Point[] horizontalPoints = new Point[]
{
        new Point(area.Left, area.Top + area.Height / 2),
        new Point(area.Right, area.Top + area.Height / 2)
};

            path.AddLines(horizontalPoints);

            GraphicsPath path2 = new GraphicsPath();

            Point[] verticalPoints = new Point[]
{
        new Point(area.Left + area.Width / 2, area.Top),
        new Point(area.Left + area.Width / 2, area.Bottom)
};

            path2.AddLines(verticalPoints);
            path.AddPath(path2, false);

            return path;
        }

        public static GraphicsPath LeftArrow(Rectangle rectangle)
        {
            GraphicsPath path = new GraphicsPath();

            Point[] points =
            {
            new Point(rectangle.Right, rectangle.Top),
            new Point(rectangle.Left, rectangle.Top + rectangle.Height / 2),
            new Point(rectangle.Right, rectangle.Bottom)
        };

            path.AddPolygon(points);

            return path;
        }

        public static GraphicsPath DownArrow(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();

            Point[] points =
            {
            new Point(rect.Left, rect.Top),
            new Point(rect.Left + rect.Width / 2, rect.Bottom),
            new Point(rect.Right, rect.Top)
        };

            path.AddPolygon(points);

            return path;
        }

        public static GraphicsPath Star(float centerX, float centerY, float outerRadius, float innerRadius, int numPoints)
        {
            if (numPoints % 2 == 0 || numPoints < 5)
            {
                throw new ArgumentException("Number of points must be an odd number and greater than or equal to 5.");
            }

            var path = new GraphicsPath();
            float angleIncrement = 360f / numPoints;
            float currentAngle = -90f;
            PointF[] points = new PointF[numPoints * 2];

            for (int i = 0; i < numPoints * 2; i += 2)
            {
                points[i] = PointOnCircle(centerX, centerY, outerRadius, currentAngle);
                points[i + 1] = PointOnCircle(centerX, centerY, innerRadius, currentAngle + angleIncrement / 2);
                currentAngle += angleIncrement;
            }

            path.AddPolygon(points);

            return path;
        }

        private static PointF PointOnCircle(float centerX, float centerY, float radius, float angleInDegrees)
        {
            float angleInRadians = (float)(angleInDegrees * Math.PI / 180.0);
            float x = centerX + radius * (float)Math.Cos(angleInRadians);
            float y = centerY + radius * (float)Math.Sin(angleInRadians);
            return new PointF(x, y);
        }

        public static PointF ClosestPointOnSegment(PointF p, PointF a, PointF b)
        {
            var ap = new PointF(p.X - a.X, p.Y - a.Y);
            var ab = new PointF(b.X - a.X, b.Y - a.Y);
            float ab2 = ab.X * ab.X + ab.Y * ab.Y;
            float dot = ap.X * ab.X + ap.Y * ab.Y;
            float t = Math.Max(0, Math.Min(1, ab2 == 0 ? 0 : dot / ab2));
            return new PointF(a.X + ab.X * t, a.Y + ab.Y * t);
        }

        public static PointF ClosestPointOnTriangle(PointF p, PointF a, PointF b, PointF c)
        {
            var ab = ClosestPointOnSegment(p, a, b);
            var bc = ClosestPointOnSegment(p, b, c);
            var ca = ClosestPointOnSegment(p, c, a);

            float d1 = DistanceSquared(p, ab);
            float d2 = DistanceSquared(p, bc);
            float d3 = DistanceSquared(p, ca);

            return d1 < d2 && d1 < d3 ? ab : d2 < d3 ? bc : ca;
        }

        public static float DistanceSquared(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        public static PointF RotatePoint(PointF origin, PointF point, float angleDegrees)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;
            double cosA = Math.Cos(angleRadians);
            double sinA = Math.Sin(angleRadians);

            float dx = point.X - origin.X;
            float dy = point.Y - origin.Y;

            float xNew = (float)(dx * cosA - dy * sinA) + origin.X;
            float yNew = (float)(dx * sinA + dy * cosA) + origin.Y;

            return new PointF(xNew, yNew);
        }

        public static bool PointInTriangle(PointF p, PointF p0, PointF p1, PointF p2)
        {
            float s = p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y;
            float t = p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y;

            if (s < 0 != t < 0)
                return false;

            float A = -p1.Y * p2.X + p0.Y * (p2.X - p1.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y;
            return A < 0 ? s <= 0 && s + t >= A : s >= 0 && s + t <= A;
        }

        public static (double X, double Y, double Z) BarycentricCoords(PointF p, PointF a, PointF b, PointF c)
        {
            double denom = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            double w1 = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denom;
            double w2 = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denom;
            double w3 = 1 - w1 - w2;
            return (w1, w2, w3);
        }
    }
}

