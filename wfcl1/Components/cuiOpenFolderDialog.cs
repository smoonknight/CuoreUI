using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    [Description("The proper OpenFolderDialog, because of the outdated FolderBrowserDialog")]
    [ToolboxItem(typeof(FolderBrowserDialog))]
    public partial class cuiOpenFolderDialog : Component
    {
        public cuiOpenFolderDialog()
        {
            InitializeComponent();
        }

        public string Title
        {
            get;
            set;
        }

        public bool Multiselect
        {
            get;
            set;
        }

        public string OkButtonLabel
        {
            get;
            set;
        }

        public string FolderName
        {
            get;
            private set;
        }

        public IReadOnlyList<string> FolderNames
        {
            get;
            private set;
        }

        public DialogResult ShowDialog()
        {
            using (OpenFolderDialog ofd = new OpenFolderDialog())
            {
                ofd.Title = Title;
                ofd.Multiselect = Multiselect;
                ofd.OkButtonLabel = OkButtonLabel;

                DialogResult ofdResult = ofd.ShowDialog();

                if (ofdResult != DialogResult.Cancel)
                {
                    FolderName = ofd.FolderName;
                    FolderNames = ofd.FolderNames;
                }

                return ofdResult;
            }
        }
    }
}
