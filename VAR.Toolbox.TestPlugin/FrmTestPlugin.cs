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

        public string ToolName { get { return "Test"; } }

        public bool HasIcon { get { return false; } }
    }
}
