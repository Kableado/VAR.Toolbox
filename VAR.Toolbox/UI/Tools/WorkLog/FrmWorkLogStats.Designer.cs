namespace VAR.Toolbox.UI.Tools.WorkLog
{
    partial class FrmWorkLogStats
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
            this.lsbDays = new VAR.Toolbox.Controls.ListBoxNormal();
            this.lblDateStart = new System.Windows.Forms.Label();
            this.txtActivity = new VAR.Toolbox.Controls.TextBoxNormal();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDateEnd = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lsbDays
            // 
            this.lsbDays.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbDays.FormattingEnabled = true;
            this.lsbDays.Location = new System.Drawing.Point(12, 113);
            this.lsbDays.Name = "lsbDays";
            this.lsbDays.Size = new System.Drawing.Size(393, 290);
            this.lsbDays.TabIndex = 0;
            // 
            // lblDateStart
            // 
            this.lblDateStart.Location = new System.Drawing.Point(12, 87);
            this.lblDateStart.Name = "lblDateStart";
            this.lblDateStart.Size = new System.Drawing.Size(186, 23);
            this.lblDateStart.TabIndex = 1;
            this.lblDateStart.Text = "label1";
            // 
            // txtActivity
            // 
            this.txtActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivity.Location = new System.Drawing.Point(12, 12);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.Size = new System.Drawing.Size(325, 20);
            this.txtActivity.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(343, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(62, 20);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(330, 413);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDateEnd
            // 
            this.lblDateEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateEnd.Location = new System.Drawing.Point(234, 87);
            this.lblDateEnd.Name = "lblDateEnd";
            this.lblDateEnd.Size = new System.Drawing.Size(171, 23);
            this.lblDateEnd.TabIndex = 5;
            this.lblDateEnd.Text = "label2";
            this.lblDateEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Location = new System.Drawing.Point(12, 406);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(35, 13);
            this.lblTotalTime.TabIndex = 6;
            this.lblTotalTime.Text = "label1";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(15, 38);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(141, 20);
            this.dtpStart.TabIndex = 7;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(264, 38);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(141, 20);
            this.dtpEnd.TabIndex = 8;
            // 
            // FrmWorkLogStats
            // 
            this.ClientSize = new System.Drawing.Size(417, 448);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.lblTotalTime);
            this.Controls.Add(this.lblDateEnd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtActivity);
            this.Controls.Add(this.lblDateStart);
            this.Controls.Add(this.lsbDays);
            this.Name = "FrmWorkLogStats";
            this.Text = "WorkLogStats";
            this.Load += new System.EventHandler(this.FrmWorkLogStats_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VAR.Toolbox.Controls.ListBoxNormal lsbDays;
        private System.Windows.Forms.Label lblDateStart;
        private VAR.Toolbox.Controls.TextBoxNormal txtActivity;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblDateEnd;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
    }
}