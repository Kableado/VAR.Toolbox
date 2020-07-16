namespace VAR.Toolbox.UI.Tools
{
    partial class PnlActivity
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpActivity = new System.Windows.Forms.GroupBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.lblActiveWindowTitle = new System.Windows.Forms.Label();
            this.txtCurrentActivity = new VAR.Toolbox.Controls.TextBoxNormal();
            this.timTicker = new System.Windows.Forms.Timer(this.components);
            this.grpActivity.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpActivity
            // 
            this.grpActivity.Controls.Add(this.lblActive);
            this.grpActivity.Controls.Add(this.lblActiveWindowTitle);
            this.grpActivity.Controls.Add(this.txtCurrentActivity);
            this.grpActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpActivity.Location = new System.Drawing.Point(0, 0);
            this.grpActivity.Name = "grpActivity";
            this.grpActivity.Size = new System.Drawing.Size(200, 125);
            this.grpActivity.TabIndex = 0;
            this.grpActivity.TabStop = false;
            this.grpActivity.Text = "Activity";
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Location = new System.Drawing.Point(7, 109);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(37, 13);
            this.lblActive.TabIndex = 2;
            this.lblActive.Text = "Active";
            // 
            // lblActiveWindowTitle
            // 
            this.lblActiveWindowTitle.AutoSize = true;
            this.lblActiveWindowTitle.Location = new System.Drawing.Point(7, 81);
            this.lblActiveWindowTitle.Name = "lblActiveWindowTitle";
            this.lblActiveWindowTitle.Size = new System.Drawing.Size(96, 13);
            this.lblActiveWindowTitle.TabIndex = 1;
            this.lblActiveWindowTitle.Text = "ActiveWindowTitle";
            // 
            // txtCurrentActivity
            // 
            this.txtCurrentActivity.Location = new System.Drawing.Point(7, 20);
            this.txtCurrentActivity.Multiline = true;
            this.txtCurrentActivity.Name = "txtCurrentActivity";
            this.txtCurrentActivity.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCurrentActivity.Size = new System.Drawing.Size(187, 58);
            this.txtCurrentActivity.TabIndex = 0;
            // 
            // timTicker
            // 
            this.timTicker.Enabled = true;
            this.timTicker.Interval = 1000;
            this.timTicker.Tick += new System.EventHandler(this.TimTicker_Tick);
            // 
            // PnlActivity
            // 
            this.Controls.Add(this.grpActivity);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PnlActivity";
            this.Size = new System.Drawing.Size(200, 125);
            this.grpActivity.ResumeLayout(false);
            this.grpActivity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpActivity;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Label lblActiveWindowTitle;
        private VAR.Toolbox.Controls.TextBoxNormal txtCurrentActivity;
        private System.Windows.Forms.Timer timTicker;
    }
}
