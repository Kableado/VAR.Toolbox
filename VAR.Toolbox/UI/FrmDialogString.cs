using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmDialogString : Form
    {
        public FrmDialogString()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return base.Text; }
            set { Text = value; }
        }

        public string Description
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }

        public string Value
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }
    }
}
