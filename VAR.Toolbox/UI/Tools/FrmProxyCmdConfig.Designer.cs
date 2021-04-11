namespace VAR.Toolbox.UI
{
    partial class FrmProxyCmdConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lsvCmdProxyConfigs = new VAR.Toolbox.Controls.ListBoxNormal();
            this.txtCmdProxyConfigName = new VAR.Toolbox.Controls.TextBoxNormal();
            this.btnSave = new VAR.Toolbox.Controls.CButton();
            this.btnDelete = new VAR.Toolbox.Controls.CButton();
            this.btnNew = new VAR.Toolbox.Controls.CButton();
            this.txtCmdProxyConfigContent = new VAR.Toolbox.Controls.TextBoxNormal();
            this.SuspendLayout();
            // 
            // lsvCmdProxyConfigs
            // 
            this.lsvCmdProxyConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsvCmdProxyConfigs.FormattingEnabled = true;
            this.lsvCmdProxyConfigs.Location = new System.Drawing.Point(13, 13);
            this.lsvCmdProxyConfigs.Name = "lsvCmdProxyConfigs";
            this.lsvCmdProxyConfigs.Size = new System.Drawing.Size(149, 355);
            this.lsvCmdProxyConfigs.TabIndex = 0;
            this.lsvCmdProxyConfigs.SelectedIndexChanged += new System.EventHandler(this.LsvCmdProxyConfigs_SelectedIndexChanged);
            // 
            // txtCmdProxyConfigName
            // 
            this.txtCmdProxyConfigName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCmdProxyConfigName.Location = new System.Drawing.Point(169, 13);
            this.txtCmdProxyConfigName.Name = "txtCmdProxyConfigName";
            this.txtCmdProxyConfigName.Size = new System.Drawing.Size(315, 20);
            this.txtCmdProxyConfigName.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(409, 351);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(328, 351);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(247, 351);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // txtCmdProxyConfigContent
            // 
            this.txtCmdProxyConfigContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCmdProxyConfigContent.Location = new System.Drawing.Point(168, 39);
            this.txtCmdProxyConfigContent.Multiline = true;
            this.txtCmdProxyConfigContent.Name = "txtCmdProxyConfigContent";
            this.txtCmdProxyConfigContent.Size = new System.Drawing.Size(316, 306);
            this.txtCmdProxyConfigContent.TabIndex = 6;
            // 
            // FrmProxyCmdConfig
            // 
            this.ClientSize = new System.Drawing.Size(496, 386);
            this.Controls.Add(this.txtCmdProxyConfigContent);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtCmdProxyConfigName);
            this.Controls.Add(this.lsvCmdProxyConfigs);
            this.MinimumSize = new System.Drawing.Size(440, 250);
            this.Name = "FrmProxyCmdConfig";
            this.Text = "ProxyCmdConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VAR.Toolbox.Controls.ListBoxNormal lsvCmdProxyConfigs;
        private VAR.Toolbox.Controls.TextBoxNormal txtCmdProxyConfigName;
        private VAR.Toolbox.Controls.CButton btnSave;
        private VAR.Toolbox.Controls.CButton btnDelete;
        private VAR.Toolbox.Controls.CButton btnNew;
        private VAR.Toolbox.Controls.TextBoxNormal txtCmdProxyConfigContent;
    }
}