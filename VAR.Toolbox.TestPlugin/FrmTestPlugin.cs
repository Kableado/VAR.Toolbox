using System.Windows.Forms;
using VAR.Toolbox.UI;

namespace VAR.Toolbox.TestPlugin
{
    public partial class FrmTestPlugin : Form, IToolForm
    {
        public FrmTestPlugin()
        {
            InitializeComponent();
        }

        public string ToolName => "Test";

        public bool HasIcon => false;
    }
}