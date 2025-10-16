using System;
using System.Linq;
using System.Windows.Forms;

namespace CuoreUI.Controls.Internal
{
    public partial class DigitsOnlyTextBox : TextBox
    {
        public DigitsOnlyTextBox()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_CHAR = 0x0102;

            if (m.Msg == WM_CHAR)
            {
                char c = (char)m.WParam;

                // Allow Backspace
                if (c == (char)Keys.Back)
                {
                    base.WndProc(ref m);
                    return;
                }

                // Allow only digits
                if (!char.IsDigit(c))
                    return;

                int caretPositionBeforeFormat = SelectionStart;
                string newText = Text.Insert(caretPositionBeforeFormat, c.ToString());
                string cleaned = new string(newText.Where(char.IsDigit).Take(19).ToArray());

                string formatted = string.Join(" ", Enumerable.Range(0, (cleaned.Length + 3) / 4)
                    .Select(i => cleaned.Substring(i * 4, Math.Min(4, cleaned.Length - i * 4))));

                int digitsBeforeCaret = newText.Take(caretPositionBeforeFormat + 1).Count(char.IsDigit);
                int newCaretPosition = 0;
                int digitsCounted = 0;
                foreach (var part in formatted.Split(' '))
                {
                    if (digitsCounted + part.Length >= digitsBeforeCaret)
                    {
                        newCaretPosition += digitsBeforeCaret - digitsCounted;
                        break;
                    }
                    newCaretPosition += part.Length + 1;
                    digitsCounted += part.Length;
                }

                Text = formatted;
                SelectionStart = Math.Min(newCaretPosition, Text.Length);
                return;
            }

            base.WndProc(ref m);
        }
    }
}
