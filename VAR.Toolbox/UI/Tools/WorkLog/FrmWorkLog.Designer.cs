namespace VAR.Toolbox.UI.Tools.WorkLog
{
    partial class FrmWorkLog
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
            this.splitWindow = new System.Windows.Forms.SplitContainer();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.cboImporters = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNextDay = new System.Windows.Forms.Button();
            this.btnPreviousDay = new System.Windows.Forms.Button();
            this.dtToday = new System.Windows.Forms.DateTimePicker();
            this.lsbWorkLog = new System.Windows.Forms.ListBox();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtActivity = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitWindow)).BeginInit();
            this.splitWindow.Panel1.SuspendLayout();
            this.splitWindow.Panel2.SuspendLayout();
            this.splitWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitWindow
            // 
            this.splitWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitWindow.Location = new System.Drawing.Point(0, 0);
            this.splitWindow.Name = "splitWindow";
            // 
            // splitWindow.Panel1
            // 
            this.splitWindow.Panel1.Controls.Add(this.btnExport);
            this.splitWindow.Panel1.Controls.Add(this.btnImport);
            this.splitWindow.Panel1.Controls.Add(this.cboImporters);
            this.splitWindow.Panel1.Controls.Add(this.txtName);
            this.splitWindow.Panel1.Controls.Add(this.btnLoad);
            this.splitWindow.Panel1.Controls.Add(this.btnSave);
            this.splitWindow.Panel1.Controls.Add(this.btnNextDay);
            this.splitWindow.Panel1.Controls.Add(this.btnPreviousDay);
            this.splitWindow.Panel1.Controls.Add(this.dtToday);
            this.splitWindow.Panel1.Controls.Add(this.lsbWorkLog);
            // 
            // splitWindow.Panel2
            // 
            this.splitWindow.Panel2.Controls.Add(this.dtEnd);
            this.splitWindow.Panel2.Controls.Add(this.dtStart);
            this.splitWindow.Panel2.Controls.Add(this.txtDescription);
            this.splitWindow.Panel2.Controls.Add(this.txtActivity);
            this.splitWindow.Panel2.Controls.Add(this.btnDelete);
            this.splitWindow.Panel2.Controls.Add(this.btnAdd);
            this.splitWindow.Size = new System.Drawing.Size(721, 603);
            this.splitWindow.SplitterDistance = 442;
            this.splitWindow.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(365, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(40, 21);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Exp";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(319, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(40, 21);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Imp";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cboImporters
            // 
            this.cboImporters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImporters.FormattingEnabled = true;
            this.cboImporters.Location = new System.Drawing.Point(224, 6);
            this.cboImporters.Name = "cboImporters";
            this.cboImporters.Size = new System.Drawing.Size(89, 21);
            this.cboImporters.TabIndex = 6;
            // 
            // txtName
            // 
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtName.Location = new System.Drawing.Point(3, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(104, 20);
            this.txtName.TabIndex = 5;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(113, 7);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(43, 20);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(162, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(41, 20);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNextDay
            // 
            this.btnNextDay.Location = new System.Drawing.Point(154, 33);
            this.btnNextDay.Name = "btnNextDay";
            this.btnNextDay.Size = new System.Drawing.Size(33, 20);
            this.btnNextDay.TabIndex = 2;
            this.btnNextDay.Text = "->";
            this.btnNextDay.UseVisualStyleBackColor = true;
            this.btnNextDay.Click += new System.EventHandler(this.btnNextDay_Click);
            // 
            // btnPreviousDay
            // 
            this.btnPreviousDay.Location = new System.Drawing.Point(3, 33);
            this.btnPreviousDay.Name = "btnPreviousDay";
            this.btnPreviousDay.Size = new System.Drawing.Size(33, 20);
            this.btnPreviousDay.TabIndex = 1;
            this.btnPreviousDay.Text = "<-";
            this.btnPreviousDay.UseVisualStyleBackColor = true;
            this.btnPreviousDay.Click += new System.EventHandler(this.btnPreviousDay_Click);
            // 
            // dtToday
            // 
            this.dtToday.CustomFormat = "yyyy-MM-dd";
            this.dtToday.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtToday.Location = new System.Drawing.Point(42, 33);
            this.dtToday.Name = "dtToday";
            this.dtToday.Size = new System.Drawing.Size(106, 20);
            this.dtToday.TabIndex = 0;
            this.dtToday.ValueChanged += new System.EventHandler(this.dtToday_ValueChanged);
            // 
            // lsbWorkLog
            // 
            this.lsbWorkLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbWorkLog.BackColor = System.Drawing.Color.Black;
            this.lsbWorkLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbWorkLog.ForeColor = System.Drawing.Color.Gray;
            this.lsbWorkLog.FormattingEnabled = true;
            this.lsbWorkLog.Location = new System.Drawing.Point(3, 60);
            this.lsbWorkLog.Name = "lsbWorkLog";
            this.lsbWorkLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsbWorkLog.Size = new System.Drawing.Size(436, 537);
            this.lsbWorkLog.TabIndex = 0;
            this.lsbWorkLog.SelectedIndexChanged += new System.EventHandler(this.lsbWorkLog_SelectedIndexChanged);
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(4, 60);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.ShowUpDown = true;
            this.dtEnd.Size = new System.Drawing.Size(155, 20);
            this.dtEnd.TabIndex = 5;
            this.dtEnd.ValueChanged += new System.EventHandler(this.dtEnd_ValueChanged);
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(4, 33);
            this.dtStart.Name = "dtStart";
            this.dtStart.ShowUpDown = true;
            this.dtStart.Size = new System.Drawing.Size(155, 20);
            this.dtStart.TabIndex = 4;
            this.dtStart.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(4, 119);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(259, 472);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // txtActivity
            // 
            this.txtActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivity.Location = new System.Drawing.Point(3, 93);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.Size = new System.Drawing.Size(259, 20);
            this.txtActivity.TabIndex = 2;
            this.txtActivity.TextChanged += new System.EventHandler(this.txtActivity_TextChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(84, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // FrmWorkLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 603);
            this.Controls.Add(this.splitWindow);
            this.Name = "FrmWorkLog";
            this.Text = "WorkLog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWorkLog_FormClosing);
            this.splitWindow.Panel1.ResumeLayout(false);
            this.splitWindow.Panel1.PerformLayout();
            this.splitWindow.Panel2.ResumeLayout(false);
            this.splitWindow.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitWindow)).EndInit();
            this.splitWindow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitWindow;
        private System.Windows.Forms.DateTimePicker dtToday;
        private System.Windows.Forms.ListBox lsbWorkLog;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtActivity;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Button btnNextDay;
        private System.Windows.Forms.Button btnPreviousDay;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ComboBox cboImporters;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
    }
}