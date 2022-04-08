using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public partial class FrmDialogString : Frame
    {
        public FrmDialogString()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => base.Text;
            set => Text = value;
        }

        public string Description
        {
            get => lblDescription.Text;
            set => lblDescription.Text = value;
        }

        public string Value
        {
            get => txtValue.Text;
            set => txtValue.Text = value;
        }
    }
}