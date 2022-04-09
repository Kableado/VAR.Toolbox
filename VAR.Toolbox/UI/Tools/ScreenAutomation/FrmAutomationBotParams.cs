using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using VAR.Toolbox.Code.Configuration;
using VAR.Toolbox.Controls;

namespace VAR.ScreenAutomation
{
    public partial class FrmAutomationBotParams : Frame
    {
        private readonly FileBackedConfiguration _config;

        private BindingList<Pair> _pairs;

        private DataGridView _dgvParams;

        public FrmAutomationBotParams(FileBackedConfiguration config)
        {
            _config = config;

            InitializeComponent();
        }

        private void FrmAutomationBotParams_Load(object sender, EventArgs e)
        {
            _pairs = new BindingList<Pair>();
            foreach (string key in _config.GetKeys())
            {
                _pairs.Add(new Pair { Key = key, Value = _config.Get(key, string.Empty), });
            }

            _pairs.AddingNew += (s, a) =>
            {
                a.NewObject = new Pair { Parent = _pairs };
            };
            _dgvParams = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = _pairs
            };

            Controls.Add(_dgvParams);
        }

        private void FrmAutomationBotParams_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Pair pair in _pairs)
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

            string IDataErrorInfo.Error => string.Empty;

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