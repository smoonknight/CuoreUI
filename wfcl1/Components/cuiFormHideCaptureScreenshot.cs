using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    [ToolboxBitmap(typeof(MaskedTextBox))]
    [Description("Hides your form from screenshots and screen recordings.")]
    public partial class cuiFormHideCaptureScreenshot : Component
    {
        public enum ExclusionTypeEnum
        {
            None = 0,
            Black = 1,
            Disappear = 17
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        public cuiFormHideCaptureScreenshot()
        {
            InitializeComponent();
        }

        internal void SetExclusionType(ExclusionTypeEnum targetExclusionType)
        {
            if (!DesignMode)
            {
                if (privateTargetForm != null && !privateTargetForm.IsDisposed)
                {
                    if (privateTargetForm != null && !privateTargetForm.IsDisposed)
                    {
                        cuiFormRounder formRounder = FormsRegisteredByRounder.GetRounderByForm(privateTargetForm);
                        if (formRounder != null)
                        {
                            Form roundedForm = formRounder.roundedFormObj;
                            if (roundedForm != null && !roundedForm.IsDisposed)
                            {
                                SetWindowDisplayAffinity(roundedForm.Handle, (uint)ExclusionTypeEnum.None);
                                SetWindowDisplayAffinity(roundedForm.Handle, (uint)targetExclusionType);
                            }
                        }

                        // Switching from Black -> Disappear or Disappear -> Black didn't always work
                        // This line fixes that issue for some reason..
                        SetWindowDisplayAffinity(TargetForm.Handle, (uint)ExclusionTypeEnum.None);
                        SetWindowDisplayAffinity(TargetForm.Handle, (uint)targetExclusionType);
                    }
                }
            }
        }

        private ExclusionTypeEnum privateExclusionType = ExclusionTypeEnum.Disappear;
        public ExclusionTypeEnum ExclusionType
        {
            get
            {
                return privateExclusionType;
            }
            set
            {
                privateExclusionType = value;
                SetExclusionType(value);
            }
        }

        private Form privateTargetForm = null;
        public Form TargetForm
        {
            get
            {
                return privateTargetForm;
            }
            set
            {
                if (privateTargetForm != null)
                {
                    SetExclusionType(ExclusionTypeEnum.None);
                    privateTargetForm.HandleCreated -= RefreshExclusionType;
                    privateTargetForm.Load -= RefreshExclusionType;
                }

                privateTargetForm = value;

                if (privateTargetForm != null)
                {
                    SetExclusionType(ExclusionType);
                    privateTargetForm.HandleCreated += RefreshExclusionType;
                    privateTargetForm.Load += RefreshExclusionType;
                }
            }
        }

        public void RefreshExclusionType(object sender = null, EventArgs e = null)
        {
            SetExclusionType(ExclusionType);
        }
    }
}
