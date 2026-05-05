using System.ComponentModel;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public partial class FrmDialogString : Frame
    {
        public FrmDialogString()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get => base.Text;
            set => Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Description
        {
            get => lblDescription.Text;
            set => lblDescription.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get => txtValue.Text;
            set => txtValue.Text = value;
        }
    }
}