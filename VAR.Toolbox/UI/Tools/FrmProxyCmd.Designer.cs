﻿namespace VAR.Toolbox.UI
{
    partial class FrmProxyCmd
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
            this.splitMain = new VAR.Toolbox.Controls.CSplitContainer();
            this.txtOutput = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtInput = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.ddlCurrentConfig = new VAR.Toolbox.Controls.CComboBox();
            this.btnConfig = new VAR.Toolbox.Controls.CButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitMain.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitMain.Location = new System.Drawing.Point(0, 28);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.txtOutput);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.txtInput);
            this.splitMain.Size = new System.Drawing.Size(413, 418);
            this.splitMain.SplitterDistance = 353;
            this.splitMain.SplitterWidth = 10;
            this.splitMain.TabIndex = 3;
            this.splitMain.TabStop = false;
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.ForeColor = System.Drawing.Color.Silver;
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(413, 353);
            this.txtOutput.TabIndex = 1;
            this.txtOutput.TabStop = false;
            // 
            // txtInput
            // 
            this.txtInput.BackColor = System.Drawing.Color.Black;
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInput.ForeColor = System.Drawing.Color.Silver;
            this.txtInput.Location = new System.Drawing.Point(0, 0);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(413, 55);
            this.txtInput.TabIndex = 0;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtInput_KeyDown);
            // 
            // ddlCurrentConfig
            // 
            this.ddlCurrentConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlCurrentConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCurrentConfig.FormattingEnabled = true;
            this.ddlCurrentConfig.Location = new System.Drawing.Point(0, 1);
            this.ddlCurrentConfig.Name = "ddlCurrentConfig";
            this.ddlCurrentConfig.Size = new System.Drawing.Size(342, 21);
            this.ddlCurrentConfig.TabIndex = 4;
            this.ddlCurrentConfig.SelectedIndexChanged += new System.EventHandler(this.DdlCurrentConfig_SelectedIndexChanged);
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig.Location = new System.Drawing.Point(348, 1);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(65, 23);
            this.btnConfig.TabIndex = 5;
            this.btnConfig.Text = "Config";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // FrmProxyCmd
            // 
            this.ClientSize = new System.Drawing.Size(413, 446);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.ddlCurrentConfig);
            this.Controls.Add(this.splitMain);
            this.Name = "FrmProxyCmd";
            this.Text = "ProxyCmd";
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VAR.Toolbox.Controls.CSplitContainer splitMain;
        private VAR.Toolbox.Controls.TextBoxMonospace txtOutput;
        private VAR.Toolbox.Controls.TextBoxMonospace txtInput;
        private VAR.Toolbox.Controls.CComboBox ddlCurrentConfig;
        private VAR.Toolbox.Controls.CButton btnConfig;
    }
}