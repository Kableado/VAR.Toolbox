using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using VAR.ScreenAutomation.Code;

namespace VAR.ScreenAutomation
{
    public partial class FrmAutomationBotParams : Form
    {
        private FileBackedConfiguration _config = null;

        private BindingList<Pair> pairs = null;

        private DataGridView dgvParams = null;

        public FrmAutomationBotParams(FileBackedConfiguration config)
        {
            _config = config;

            InitializeComponent();
        }

        private void FrmAutomationBotParams_Load(object sender, EventArgs e)
        {
            pairs = new BindingList<Pair>();
            foreach (string key in _config.GetKeys())
            {
                pairs.Add(new Pair { Key = key, Value = _config.Get(key, string.Empty), });
            }
            pairs.AddingNew += (s, a) =>
            {
                a.NewObject = new Pair { Parent = pairs };
            };
            dgvParams = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = pairs
            };

            Controls.Add(dgvParams);
        }

        private void FrmAutomationBotParams_FormClosing(object sender, FormClosingEventArgs e)
        {
            var parms = new Dictionary<string, string>();
            foreach (Pair pair in pairs)
            {
                if (string.IsNullOrEmpty(pair.Key)) { continue; }
                _config.Set(pair.Key, pair.Value);
            }
            _config.Save();
        }

        internal class Pair : IDataErrorInfo
        {
            internal IList<Pair> Parent { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }

            string IDataErrorInfo.Error
            {
                get { return string.Empty; }
            }

            string IDataErrorInfo.this[string columnName]
            {
                get
                {
                    if (columnName == "Key" && Parent != null && Parent.Any(
                        x => x.Key == this.Key && !ReferenceEquals(x, this)))
                    {
                        return "duplicate key";
                    }
                    return string.Empty;
                }
            }
        }
    }
}
