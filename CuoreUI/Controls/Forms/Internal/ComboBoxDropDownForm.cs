using CuoreUI.Components;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CuoreUI.Controls.Forms.Internal
{
    public partial class ComboBoxDropDownForm : Form
    {
        private string[] Items = new string[] { };
        private Control TargetControl = null;

        // todo: expose these as properties
        internal int buttonHeight = 36;
        internal int buttonPadding = 6;
        internal int formPadding = 2;

        public ComboBoxDropDownForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        internal EventHandler SelectedItemChanged;
        cuiFormRounder formRounder;

        protected override void OnHandleCreated(EventArgs e)
        {
            formRounder = new cuiFormRounder();
            base.OnHandleCreated(e);
            formRounder.OutlineColor = Color.FromArgb(48, 128, 128, 128);
            formRounder.TargetForm = this;
        }

        internal bool canShow = true;

        public bool Show(Control attachToControl, params string[] comboBoxItems)
        {
            if (canShow == false)
            {
                return false;
            }

            canShow = false;
            Owner = attachToControl.FindForm();
            TargetControl = attachToControl;
            Items = comboBoxItems;

            CalculateNewLocation(attachToControl);
            if (formRounder != null)
            {
                formRounder.roundedFormObj?.BringToFront();
            }

            Show();
            Focus();
            LostFocus += ComboBoxDropDownForm_LostFocus;
            CalculateNewSize();

            return true;
        }

        private void CalculateNewLocation(Control attachToControl)
        {
            Location = attachToControl.PointToScreen(Point.Empty) + new Size(-formPadding, attachToControl.Height + 2 - formPadding);
        }

        private async void ComboBoxDropDownForm_LostFocus(object sender, System.EventArgs e)
        {
            if (formRounder != null)
            {
                formRounder.roundedFormObj?.Hide();
            }
            Hide();
            LostFocus -= ComboBoxDropDownForm_LostFocus;

            // prevents accidents
            await Task.Delay(250);
            canShow = true;
        }

        void CalculateNewSize()
        {
            int buttonOffsetSize = buttonHeight + buttonPadding - 2;
            int newHeight = -1 + buttonPadding + Items.Length * buttonOffsetSize;
            int doubleFormPadding = formPadding + formPadding;

            Size = new Size(TargetControl.Width + 1 + doubleFormPadding, newHeight + 1 + doubleFormPadding);
            formRounder.roundedFormObj.Region = null;
        }

        internal int _selectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                SelectedItemChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnClick(EventArgs e)
        {
            Point clickPoint = PointToClient(Cursor.Position);
            int buttonOffsetSize = buttonHeight + buttonPadding - 2;
            int currentItemY = buttonPadding;

            for (int i = 0; i < Items.Length; i++)
            {
                Rectangle itemRect = new Rectangle(
                    buttonPadding,
                    currentItemY,
                    Width - (buttonPadding * 2) - 1,
                    buttonHeight
                );

                if (itemRect.Contains(clickPoint))
                {
                    SelectedIndex = i;
                    break;
                }

                currentItemY += buttonOffsetSize;
            }

            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (TargetControl == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int currentItemY = buttonPadding;
            int buttonOffsetSize = buttonHeight + buttonPadding - 2;
            int doublePadding = buttonPadding + buttonPadding;

            Rectangle GetItemRect()
            {
                return new Rectangle(buttonPadding, currentItemY, Width - doublePadding - 1, buttonHeight);
            }

            Point cursorPosition = PointToClient(Cursor.Position);
            int i = 0;
            foreach (string item in Items)
            {
                Rectangle itemRect = GetItemRect();
                bool isSelected = SelectedIndex == i;

                // don't create path if not hovering or not selected
                if (itemRect.Contains(cursorPosition) || isSelected)
                {
                    using (SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(32, 128, 128, 128)))
                    using (GraphicsPath itemPath = Helpers.GeneralHelper.RoundRect(itemRect, new Padding(8)))
                    {
                        e.Graphics.FillPath(hoverBrush, itemPath);
                    }

                    if (isSelected)
                    {
                        Rectangle selectedIndicator = GetItemRect();
                        selectedIndicator.Inflate(0, -9);
                        selectedIndicator.Width = 3;

                        using (GraphicsPath selectedIndicatorPath = Helpers.GeneralHelper.RoundRect(selectedIndicator, new Padding(2)))
                        {
                            e.Graphics.FillPath(Brushes.Gray, selectedIndicatorPath);
                        }
                    }
                }

                Point textPosition = new Point(itemRect.X + 8, currentItemY + (itemRect.Height - Font.Height) / 2);
                TextRenderer.DrawText(e.Graphics, Items[i], Font, textPosition, ForeColor);

                currentItemY += buttonOffsetSize;
                i++;
            }

            //base.OnPaint(e);
        }
    }
}
