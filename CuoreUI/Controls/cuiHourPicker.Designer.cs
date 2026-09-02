namespace CuoreUI.Controls
{
    partial class cuiHourPicker
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.hourTextBox = new CuoreUI.Controls.cuiTextBox();
            this.minuteTextBox = new CuoreUI.Controls.cuiTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(54, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 35;
            this.label1.Text = ":";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hourTextBox
            // 
            this.hourTextBox.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(37)))), ((int)(((byte)(42)))));
            this.hourTextBox.Content = "";
            this.hourTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.hourTextBox.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(37)))), ((int)(((byte)(42)))));
            this.hourTextBox.FocusImageTint = System.Drawing.Color.White;
            this.hourTextBox.FocusOutlineColor = System.Drawing.Color.Gray;
            this.hourTextBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hourTextBox.ForeColor = System.Drawing.Color.Gray;
            this.hourTextBox.Image = null;
            this.hourTextBox.ImageExpand = new System.Drawing.Point(0, 0);
            this.hourTextBox.ImageOffset = new System.Drawing.Point(0, 0);
            this.hourTextBox.Location = new System.Drawing.Point(2, -1);
            this.hourTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.hourTextBox.Multiline = false;
            this.hourTextBox.Name = "hourTextBox";
            this.hourTextBox.NormalImageTint = System.Drawing.Color.White;
            this.hourTextBox.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.hourTextBox.Padding = new System.Windows.Forms.Padding(16, 2, 16, 0);
            this.hourTextBox.PasswordChar = false;
            this.hourTextBox.PlaceholderColor = System.Drawing.Color.LightGray;
            this.hourTextBox.PlaceholderText = "00";
            this.hourTextBox.Rounding = new System.Windows.Forms.Padding(8);
            this.hourTextBox.Size = new System.Drawing.Size(49, 21);
            this.hourTextBox.TabIndex = 36;
            this.hourTextBox.TextOffset = new System.Drawing.Size(0, 0);
            this.hourTextBox.UnderlinedStyle = true;
            // 
            // minuteTextBox
            // 
            this.minuteTextBox.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(37)))), ((int)(((byte)(42)))));
            this.minuteTextBox.Content = "";
            this.minuteTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.minuteTextBox.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(37)))), ((int)(((byte)(42)))));
            this.minuteTextBox.FocusImageTint = System.Drawing.Color.White;
            this.minuteTextBox.FocusOutlineColor = System.Drawing.Color.Gray;
            this.minuteTextBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minuteTextBox.ForeColor = System.Drawing.Color.Gray;
            this.minuteTextBox.Image = null;
            this.minuteTextBox.ImageExpand = new System.Drawing.Point(0, 0);
            this.minuteTextBox.ImageOffset = new System.Drawing.Point(0, 0);
            this.minuteTextBox.Location = new System.Drawing.Point(69, -1);
            this.minuteTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.minuteTextBox.Multiline = false;
            this.minuteTextBox.Name = "minuteTextBox";
            this.minuteTextBox.NormalImageTint = System.Drawing.Color.White;
            this.minuteTextBox.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.minuteTextBox.Padding = new System.Windows.Forms.Padding(16, 2, 16, 0);
            this.minuteTextBox.PasswordChar = false;
            this.minuteTextBox.PlaceholderColor = System.Drawing.Color.LightGray;
            this.minuteTextBox.PlaceholderText = "00";
            this.minuteTextBox.Rounding = new System.Windows.Forms.Padding(8);
            this.minuteTextBox.Size = new System.Drawing.Size(49, 21);
            this.minuteTextBox.TabIndex = 37;
            this.minuteTextBox.TextOffset = new System.Drawing.Size(0, 0);
            this.minuteTextBox.UnderlinedStyle = true;
            // 
            // cuiHourPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.minuteTextBox);
            this.Controls.Add(this.hourTextBox);
            this.Controls.Add(this.label1);
            this.Name = "cuiHourPicker";
            this.Size = new System.Drawing.Size(120, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private cuiTextBox hourTextBox;
        private cuiTextBox minuteTextBox;
    }
}
