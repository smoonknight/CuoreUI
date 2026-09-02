using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiHourPicker : UserControl
    {
        private TimeSpan privateValue = TimeSpan.Zero;
        private bool updating;

        [Category("CuoreUI")]
        [Browsable(true)]
        public TimeSpan Value
        {
            get => privateValue;
            set
            {
                SetValue(value);

                if (IsHandleCreated)
                {
                    UpdateTextBoxes();
                }
            }
        }

        [Category("CuoreUI")]
        [Browsable(true)]
        public event EventHandler ValueChanged;

        [Category("CuoreUI")]
        [Browsable(true)]
        public new Color ForeColor
        {
            get => hourTextBox.ForeColor;
            set
            {
                hourTextBox.ForeColor = value;
                minuteTextBox.ForeColor = value;
            }
        }

        [Category("CuoreUI")]
        [Browsable(true)]
        public Color FocusForeColor
        {
            get => focusForeColor;
            set
            {
                focusForeColor = value;
                UpdateForeColors();
            }
        }

        private Color focusForeColor;

        private void UpdateForeColors()
        {
            hourTextBox.ForeColor =
                hourTextBox.Focused
                    ? focusForeColor
                    : ForeColor;

            minuteTextBox.ForeColor =
                minuteTextBox.Focused
                    ? focusForeColor
                    : ForeColor;
        }

        [Category("CuoreUI")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        public cuiHourPicker()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TabStop = true;

            hourTextBox.ContentChanged += HourTextBox_ContentChanged;
            minuteTextBox.ContentChanged += MinuteTextBox_ContentChanged;

            hourTextBox.KeyPress += TextBox_KeyPress;
            minuteTextBox.KeyPress += TextBox_KeyPress;

            UpdateTextBoxes();
        }

        private void UpdateTextBoxes()
        {
            updating = true;

            hourTextBox.Text = privateValue.Hours.ToString("D2");
            minuteTextBox.Text = privateValue.Minutes.ToString("D2");

            updating = false;
        }

        private void HourTextBox_ContentChanged(object sender, EventArgs e)
        {
            if (ReadOnly)
            {
                UpdateTextBoxes();
                return;
            }

            if (updating || hourTextBox.Text.Length < 2)
            {
                return;
            }

            if (!int.TryParse(hourTextBox.Text, out int hour) || hour > 23)
            {
                SystemSounds.Beep.Play();

                updating = true;
                hourTextBox.Text = "";
                updating = false;

                hourTextBox.Focus();
                return;
            }

            if (int.TryParse(minuteTextBox.Text, out int minute))
            {
                UpdateValue(hour, minute);
            }

            minuteTextBox.Focus();
        }

        private void MinuteTextBox_ContentChanged(object sender, EventArgs e)
        {
            if (ReadOnly)
            {
                UpdateTextBoxes();
                return;
            }

            if (updating || minuteTextBox.Text.Length < 2)
            {
                return;
            }

            if (!int.TryParse(minuteTextBox.Text, out int minute) || minute > 59)
            {
                SystemSounds.Beep.Play();

                updating = true;
                minuteTextBox.Text = "";
                updating = false;

                minuteTextBox.Focus();
                return;
            }

            if (int.TryParse(hourTextBox.Text, out int hour))
            {
                UpdateValue(hour, minute);
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ReadOnly)
            {
                e.Handled = true;
                return;
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
            }
        }

        private void UpdateValue(int hour, int minute)
        {
            SetValue(new TimeSpan(hour, minute, 0));
        }

        private void SetValue(TimeSpan value)
        {
            if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
            {
                value = TimeSpan.Zero;
            }
            else
            {
                value = new TimeSpan(value.Hours, value.Minutes, 0);
            }

            if (privateValue == value)
            {
                return;
            }

            privateValue = value;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}