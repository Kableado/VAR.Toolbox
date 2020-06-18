using System.Collections.Generic;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmListBoxDialog : Form
    {
        public FrmListBoxDialog()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return base.Text; }
            set { Text = value; }
        }

        public void LoadItems(List<string> items)
        {
            lsbItems.Items.Clear();
            lsbItems.Items.AddRange(items.ToArray());
        }

        public string Value
        {
            get { return (lsbItems.SelectedItem as string) ?? string.Empty; }
        }

        private void lsbItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
