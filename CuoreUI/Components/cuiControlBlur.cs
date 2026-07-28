using CuoreUI.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    [Description("Blur effect on a control")]
    [ToolboxBitmap(typeof(Panel))]
    public partial class cuiControlBlur : Component
    {
        private Bitmap cachedBitmap;
        public cuiControlBlur(IContainer container)
        {
            container.Add(this);
        }

        private Control privateTargetControl;

        [Category("CuoreUI")]
        public Control TargetControl
        {
            get
            {
                return privateTargetControl;
            }
            set
            {
                if (TargetControl is Form || value is Form || value == null)
                {
                    privateTargetControl = null;
                    cachedBitmap?.Dispose();
                    cachedBitmap = null;
                    if ((Debugger.IsAttached || DesignMode) && value != null)
                    {
                        MessageBox.Show($"Cannot set TargetControl to type Form in this cuiControlBlur instance.\nBlurring the whole form would be too expensive for winforms, sorry.", "CuoreUI");
                    }
                    return;
                }

                if (privateTargetControl != null)
                {
                    privateTargetControl.Paint -= TargetControl_Paint;
                    privateTargetControl.Invalidated -= TargetControl_Invalidated;
                }
                value.Parent?.Invalidate();

                privateTargetControl = value;
                if (privateTargetControl != null)
                {
                    privateTargetControl.Paint += TargetControl_Paint;
                    privateTargetControl.Invalidated += TargetControl_Invalidated;
                }
                cachedBitmap?.Dispose();
                cachedBitmap = null;
                privateTargetControl?.Invalidate();
            }
        }

        private float privateBlurAmount = 1.5f;

        [Category("CuoreUI")]
        public float BlurAmount
        {
            get
            {
                return privateBlurAmount;
            }
            set
            {
                if (value > 0)
                {
                    privateBlurAmount = value;
                }
                cachedBitmap?.Dispose();
                cachedBitmap = null;
                privateTargetControl?.Invalidate();
            }
        }

        private void TargetControl_Invalidated(object sender, InvalidateEventArgs e)
        {
            cachedBitmap?.Dispose();
            cachedBitmap = null;
        }

        private void TargetControl_Paint(object sender, PaintEventArgs e)
        {
            if (privateTargetControl == null ||
                privateTargetControl.IsDisposed ||
                !privateTargetControl.IsHandleCreated ||
                privateTargetControl.Width <= 0 ||
                privateTargetControl.Height <= 0)
            {
                return;
            }

            try
            {
                if (cachedBitmap == null ||
                    cachedBitmap.Width != privateTargetControl.Width ||
                    cachedBitmap.Height != privateTargetControl.Height)
                {
                    cachedBitmap?.Dispose();

                    cachedBitmap = new Bitmap(
                        privateTargetControl.Width,
                        privateTargetControl.Height);

                    privateTargetControl.DrawToBitmap(
                        cachedBitmap,
                        new Rectangle(
                            0,
                            0,
                            privateTargetControl.Width,
                            privateTargetControl.Height));

                    DrawingHelper.Imaging.ImageBlurs.QuadraticBlur.Apply(
                        ref cachedBitmap,
                        BlurAmount);
                }

                e.Graphics.DrawImage(
                    cachedBitmap,
                    privateTargetControl.ClientRectangle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                cachedBitmap = null;
                cachedBitmap?.Dispose();
                TargetControl.Paint -= TargetControl_Paint;
                TargetControl.Invalidated -= TargetControl_Invalidated;
                TargetControl.Invalidate();
                TargetControl = null;
            }
            base.Dispose(disposing);
        }
    }
}
