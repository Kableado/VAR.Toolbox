using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public partial class FrmListBoxDialog : Frame
    {
        public FrmListBoxDialog()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => base.Text;
            set => Text = value;
        }

        public void LoadItems(List<string> items)
        {
            lsbItems.Items.Clear();
            lsbItems.Items.AddRange(items.ToArray<object>());
        }

        public string Value => (lsbItems.SelectedItem as string) ?? string.Empty;

        private void lsbItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}