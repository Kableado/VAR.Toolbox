using System.Drawing;

namespace VAR.Toolbox.Controls
{
    public class CSplitContainer : System.Windows.Forms.SplitContainer
    {
        public CSplitContainer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(255, 32, 32, 32);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
        }
    }
}