using CuoreUI.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(Button))]
    [DefaultEvent("Click")]
    public partial class cuiButton : UserControl
    {
        public static class ButtonStates
        {
            public const int Normal = 1;
            public const int Hovered = 2;
            public const int Pressed = 3;
        }

        public cuiButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ForeColor = Color.Black;
            Font = new Font("Microsoft Sans Serif", 9.75f);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private DialogResult privateDialogResult = DialogResult.None;

        [Category("CuoreUI")]
        public DialogResult DialogResult
        {
            get
            {
                return privateDialogResult;
            }
            set
            {
                privateDialogResult = value;
                Invalidate();
            }
        }

        private string privateContent = "Your text here!";

        [Category("CuoreUI")]
        public string Content
        {
            get
            {
                return privateContent;
            }
            set
            {
                privateContent = value;
                Invalidate();
            }
        }

        private Padding privateRounding = new Padding(8, 8, 8, 8);

        [Category("CuoreUI")]
        public Padding Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                privateRounding = value;
                Invalidate();
            }
        }

        private Color privateNormalBackground = Color.White;

        [Category("CuoreUI")]
        public Color NormalBackground
        {
            get
            {
                return privateNormalBackground;
            }
            set
            {
                privateNormalBackground = value;
                Invalidate();
            }
        }

        private Color privateHoverBackground = Color.White;

        [Category("CuoreUI")]
        public Color HoverBackground
        {
            get
            {
                return privateHoverBackground;
            }
            set
            {
                privateHoverBackground = value;
                Invalidate();
            }
        }

        private Color privatePressedBackground = Color.WhiteSmoke;

        [Category("CuoreUI")]
        public Color PressedBackground
        {
            get
            {
                return privatePressedBackground;
            }
            set
            {
                privatePressedBackground = value;
                Invalidate();
            }
        }

        private Color privateNormalOutline = Color.FromArgb(64, 128, 128, 128);

        [Category("CuoreUI")]
        public Color NormalOutline
        {
            get
            {
                return privateNormalOutline;
            }
            set
            {
                privateNormalOutline = value;
                Invalidate();
            }
        }

        private Color privateHoverOutline = Color.FromArgb(32, 128, 128, 128);

        [Category("CuoreUI")]
        public Color HoverOutline
        {
            get
            {
                return privateHoverOutline;
            }
            set
            {
                privateHoverOutline = value;
                Invalidate();
            }
        }

        private Color privatePressedOutline = Color.FromArgb(64, 128, 128, 128);

        [Category("CuoreUI")]
        public Color PressedOutline
        {
            get
            {
                return privatePressedOutline;
            }
            set
            {
                privatePressedOutline = value;
                Invalidate();
            }
        }

        private bool privateCheckButton = false;

        [Category("CuoreUI")]
        public bool CheckButton
        {

            get
            {
                return privateCheckButton;
            }
            set
            {
                privateCheckButton = value;
                Invalidate();
            }
        }

        private bool privateChecked = false;

        [Category("CuoreUI")]
        public bool Checked
        {
            get
            {
                return privateChecked;
            }
            set
            {
                privateChecked = value;
                Invalidate();
            }
        }

        private int state = ButtonStates.Normal;
        private SolidBrush privateBrush = new SolidBrush(Color.Black);
        private Pen privatePen = new Pen(Color.Black) { Alignment = PenAlignment.Inset };
        StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Center };

        private Image privateImage = null;

        [Category("CuoreUI")]
        public Image Image
        {
            get
            {
                return privateImage;
            }
            set
            {
                privateImage = value;
                Invalidate();
            }
        }

        private Color privateCheckedBackground = Helpers.DrawingHelper.PrimaryColor;

        [Category("CuoreUI")]
        public Color CheckedBackground
        {
            get
            {
                return privateCheckedBackground;
            }
            set
            {
                privateCheckedBackground = value;
                Invalidate();
            }
        }

        private Color privateCheckedOutline = Helpers.DrawingHelper.PrimaryColor;

        [Category("CuoreUI")]
        public Color CheckedOutline
        {
            get
            {
                return privateCheckedOutline;
            }
            set
            {
                privateCheckedOutline = value;
                Invalidate();
            }
        }

        private bool privateImageAutoCenter = true;

        [Category("CuoreUI")]
        public bool ImageAutoCenter
        {
            get
            {
                return privateImageAutoCenter;
            }
            set
            {
                privateImageAutoCenter = value;
                Invalidate();
            }
        }

        private StringAlignment privateTextAlignment = StringAlignment.Center;

        [Category("CuoreUI")]
        public StringAlignment TextAlignment
        {
            get
            {
                return privateTextAlignment;
            }

            set
            {
                privateTextAlignment = value;
                Invalidate();
            }
        }

        private float privateOutlineThickness = 1f;

        [Category("CuoreUI")]
        public float OutlineThickness
        {
            get
            {
                return privateOutlineThickness;
            }
            set
            {
                privateOutlineThickness = Math.Max(value, 0);
                privatePen.Width = value;
            }
        }

        private Point privateImageExpand = Point.Empty;

        [Category("CuoreUI")]
        public Point ImageExpand
        {
            get
            {
                return privateImageExpand;
            }
            set
            {
                privateImageExpand = value;
                Invalidate();
            }
        }

        Color privateCheckedForeColor = Color.White;

        [Category("CuoreUI")]
        public Color CheckedForeColor
        {
            get
            {
                return privateCheckedForeColor;
            }
            set
            {
                privateCheckedForeColor = value;
                Invalidate();
            }
        }

        Color privatePressedForeColor = Color.FromArgb(32, 32, 32);

        [Category("CuoreUI")]
        public Color PressedForeColor
        {
            get
            {
                return privatePressedForeColor;
            }
            set
            {
                privatePressedForeColor = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public Color NormalForeColor
        {
            get
            {
                return ForeColor;
            }
            set
            {
                ForeColor = value;
            }
        }

        Color privateHoverForeColor = Color.Black;

        [Category("CuoreUI")]
        public Color HoverForeColor
        {
            get
            {
                return privateHoverForeColor;
            }
            set
            {
                privateHoverForeColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            stringFormat.Trimming = StringTrimming.None;
            stringFormat.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;
            stringFormat.Alignment = TextAlignment;

            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Inflate(-1, -1);

            Color renderedBackgroundColor = Color.Empty;
            Color renderedOutlineColor = Color.Empty;
            Color renderedTint = NormalImageTint;
            Color renderedForeColor = Color.Empty;

            if (CheckButton && Checked)
            {
                renderedBackgroundColor = CheckedBackground;
                renderedOutlineColor = CheckedOutline;
                renderedTint = CheckedImageTint;
                renderedForeColor = CheckedForeColor;
            }
            else
            {
                switch (state)
                {
                    case ButtonStates.Normal:
                        renderedBackgroundColor = NormalBackground;
                        renderedOutlineColor = NormalOutline;
                        renderedForeColor = NormalForeColor;
                        renderedTint = NormalImageTint;
                        break;

                    case ButtonStates.Hovered:
                        renderedBackgroundColor = HoverBackground;
                        renderedOutlineColor = HoverOutline;
                        renderedTint = HoverImageTint;
                        renderedForeColor = HoverForeColor;
                        break;

                    case ButtonStates.Pressed:
                        renderedBackgroundColor = PressedBackground;
                        renderedOutlineColor = PressedOutline;
                        renderedTint = PressedImageTint;
                        renderedForeColor = PressedForeColor;
                        break;
                }
            }

            privateBrush.Color = renderedBackgroundColor;
            privatePen.Color = renderedOutlineColor;
            privatePen.Width = OutlineThickness;

            GraphicsPath roundBackground = GeneralHelper.RoundRect(modifiedCR, Rounding);
            e.Graphics.FillPath(privateBrush, roundBackground);

            if (OutlineThickness > 0)
            {
                // need to adjust modifiedCR, or else the outline's left & top borders won't be in the expected places
                modifiedCR.Width += 1;
                modifiedCR.Height += 1;
                modifiedCR.Offset(-1, -1);
                roundBackground.Dispose();
                roundBackground = GeneralHelper.RoundRect(modifiedCR, Rounding);

                // PixelOffsetMode.HighQuality's antialiasing looks ugly
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                e.Graphics.DrawPath(privatePen, roundBackground);
            }
            roundBackground?.Dispose();

            Rectangle textRectangle = ClientRectangle;
            int textY = (Height / 2) - (Font.Height / 2);
            textRectangle.Location = new Point(0, textY);

            Rectangle imageRectangle = textRectangle;
            imageRectangle.Height = Font.Height;
            imageRectangle.Width = imageRectangle.Height;
            imageRectangle.Inflate(ImageExpand.X, ImageExpand.Y);

            if (ImageAutoCenter && privateImage != null)
            {
                imageRectangle.X = (Width / 2) - (imageRectangle.Width / 2);
                int TextOffsetFromImage = (int)e.Graphics.MeasureString(Content, Font, textRectangle.Width, stringFormat).Width;
                imageRectangle.X -= TextOffsetFromImage / 2;
                textRectangle.X += imageRectangle.Width / 2;
            }
            else if (privateImage != null)
            {
                textRectangle.X += imageRectangle.Width;
            }

            textRectangle.Offset(privateTextOffset);
            imageRectangle.Offset(privateImageOffset);

            using (SolidBrush brush = new SolidBrush(renderedForeColor))
            {
                e.Graphics.DrawString(privateContent, Font, brush, textRectangle, stringFormat);
            }

            if (privateImage != null)
            {
                if (renderedTint == Color.White)
                {
                    e.Graphics.DrawImage(
                    privateImage,
                    imageRectangle,
                    0, 0, privateImage.Width, privateImage.Height,
                    GraphicsUnit.Pixel);
                }
                else
                {
                    if (colorMatrix == null || renderedTint != lastImageTint)
                    {
                        float tintR = renderedTint.R / 255f;
                        float tintG = renderedTint.G / 255f;
                        float tintB = renderedTint.B / 255f;
                        float tintA = renderedTint.A / 255f;

                        // Create a color matrix that will apply the tint color
                        colorMatrix = new ColorMatrix(new float[][]
                        {
            new float[] {tintR, 0, 0, 0, 0},
            new float[] {0, tintG, 0, 0, 0},
            new float[] {0, 0, tintB, 0, 0},
            new float[] {0, 0, 0, tintA, 0},
            new float[] {0, 0, 0, 0, 1}
                        });
                    }

                    if (imageAttributes == null)
                    {
                        imageAttributes = new ImageAttributes();
                    }
                    imageAttributes.SetColorMatrix(colorMatrix);

                    // Draw the image with the tint
                    e.Graphics.DrawImage(
                        privateImage,
                        imageRectangle,
                        0, 0, privateImage.Width, privateImage.Height,
                        GraphicsUnit.Pixel,
                        imageAttributes);
                }

                lastImageTint = renderedTint;
            }

            base.OnPaint(e);
        }

        private Color lastImageTint = Color.Empty;
        private ColorMatrix colorMatrix = null;
        private ImageAttributes imageAttributes = null;

        private Color privateImageTint = Color.White;

        [Category("CuoreUI")]
        public Color NormalImageTint
        {
            get
            {
                return privateImageTint;
            }
            set
            {
                privateImageTint = value;
                Invalidate();
            }
        }

        private Color privateHoverImageTint = Color.White;

        [Category("CuoreUI")]
        public Color HoverImageTint
        {
            get
            {
                return privateHoverImageTint;
            }
            set
            {
                privateHoverImageTint = value;
                Invalidate();
            }
        }

        private Color privateCheckedImageTint = Color.White;

        [Category("CuoreUI")]
        public Color CheckedImageTint
        {
            get
            {
                return privateCheckedImageTint;
            }
            set
            {
                privateCheckedImageTint = value;
                Invalidate();
            }
        }

        private Color privatePressedImageTint = Color.White;

        [Category("CuoreUI")]
        public Color PressedImageTint
        {
            get
            {
                return privatePressedImageTint;
            }
            set
            {
                privatePressedImageTint = value;
                Invalidate();
            }
        }

        private Point privateImageOffset = new Point(0, 0);

        [Category("CuoreUI")]
        public Point ImageOffset
        {
            get
            {
                return privateImageOffset;
            }
            set
            {
                privateImageOffset = value;
                Invalidate();
            }
        }

        private Point privateTextOffset = new Point(0, 0);

        [Category("CuoreUI")]
        public Point TextOffset
        {
            get
            {
                return privateTextOffset;
            }
            set
            {
                privateTextOffset = value;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (ClientRectangle.Contains(e.Location))
            {
                if (state == ButtonStates.Pressed)
                {
                    if (CheckButton)
                    {
                        Checked = !Checked;
                    }
                }

                if (state != ButtonStates.Normal)
                {
                    state = ButtonStates.Hovered;
                }
                Invalidate();
            }
            else
            {
                state = ButtonStates.Normal;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            state = ButtonStates.Normal;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            state = ButtonStates.Hovered;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            state = ButtonStates.Pressed;

            if (privateDialogResult != DialogResult.None)
            {
                Form parentForm = FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = privateDialogResult;
                }
            }

            Focus();
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            state = ButtonStates.Normal;
            base.OnLostFocus(e);
        }
    }
}
