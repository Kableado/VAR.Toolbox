namespace VAR.ScreenAutomation
{
    partial class FrmScreenAutomation
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
            this.picCapturer = new System.Windows.Forms.PictureBox();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitOutput = new System.Windows.Forms.SplitContainer();
            this.ddlAutomationBot = new System.Windows.Forms.ComboBox();
            this.btnStartEnd = new System.Windows.Forms.Button();
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.picPreview = new VAR.ScreenAutomation.Controls.CtrImageViewer();
            this.ctrOutput = new VAR.ScreenAutomation.Controls.CtrOutput();
            ((System.ComponentModel.ISupportInitialize)(this.picCapturer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitOutput)).BeginInit();
            this.splitOutput.Panel1.SuspendLayout();
            this.splitOutput.Panel2.SuspendLayout();
            this.splitOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // picCapturer
            // 
            this.picCapturer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picCapturer.Location = new System.Drawing.Point(4, 4);
            this.picCapturer.Margin = new System.Windows.Forms.Padding(4);
            this.picCapturer.Name = "picCapturer";
            this.picCapturer.Padding = new System.Windows.Forms.Padding(10);
            this.picCapturer.Size = new System.Drawing.Size(501, 799);
            this.picCapturer.TabIndex = 0;
            this.picCapturer.TabStop = false;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Margin = new System.Windows.Forms.Padding(10);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.splitOutput);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.picCapturer);
            this.splitMain.Size = new System.Drawing.Size(754, 816);
            this.splitMain.SplitterDistance = 232;
            this.splitMain.TabIndex = 3;
            // 
            // splitOutput
            // 
            this.splitOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitOutput.Location = new System.Drawing.Point(0, 0);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitOutput.Panel1
            // 
            this.splitOutput.Panel1.Controls.Add(this.picPreview);
            // 
            // splitOutput.Panel2
            // 
            this.splitOutput.Panel2.Controls.Add(this.numFPS);
            this.splitOutput.Panel2.Controls.Add(this.ddlAutomationBot);
            this.splitOutput.Panel2.Controls.Add(this.btnStartEnd);
            this.splitOutput.Panel2.Controls.Add(this.ctrOutput);
            this.splitOutput.Size = new System.Drawing.Size(232, 816);
            this.splitOutput.SplitterDistance = 283;
            this.splitOutput.TabIndex = 4;
            // 
            // ddlAutomationBot
            // 
            this.ddlAutomationBot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlAutomationBot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAutomationBot.FormattingEnabled = true;
            this.ddlAutomationBot.Location = new System.Drawing.Point(13, 4);
            this.ddlAutomationBot.Name = "ddlAutomationBot";
            this.ddlAutomationBot.Size = new System.Drawing.Size(216, 24);
            this.ddlAutomationBot.TabIndex = 4;
            this.ddlAutomationBot.SelectedIndexChanged += new System.EventHandler(this.DdlAutomationBot_SelectedIndexChanged);
            // 
            // btnStartEnd
            // 
            this.btnStartEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartEnd.Location = new System.Drawing.Point(15, 34);
            this.btnStartEnd.Name = "btnStartEnd";
            this.btnStartEnd.Size = new System.Drawing.Size(149, 39);
            this.btnStartEnd.TabIndex = 3;
            this.btnStartEnd.Text = "Start";
            this.btnStartEnd.UseVisualStyleBackColor = true;
            this.btnStartEnd.Click += new System.EventHandler(this.BtnStartEnd_Click);
            // 
            // numFPS
            // 
            this.numFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFPS.Location = new System.Drawing.Point(170, 50);
            this.numFPS.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFPS.Name = "numFPS";
            this.numFPS.Size = new System.Drawing.Size(59, 22);
            this.numFPS.TabIndex = 5;
            this.numFPS.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // picPreview
            // 
            this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.ImageShow = null;
            this.picPreview.Location = new System.Drawing.Point(12, 12);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(217, 268);
            this.picPreview.TabIndex = 1;
            this.picPreview.TabStop = false;
            // 
            // ctrOutput
            // 
            this.ctrOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrOutput.Location = new System.Drawing.Point(12, 79);
            this.ctrOutput.Name = "ctrOutput";
            this.ctrOutput.Size = new System.Drawing.Size(217, 437);
            this.ctrOutput.TabIndex = 2;
            this.ctrOutput.Text = "ctrOutput1";
            // 
            // FrmScreenAutomation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 816);
            this.Controls.Add(this.splitMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmScreenAutomation";
            this.Text = "ScreenAutomation";
            this.Load += new System.EventHandler(this.FrmScreenAutomation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCapturer)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitOutput.Panel1.ResumeLayout(false);
            this.splitOutput.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitOutput)).EndInit();
            this.splitOutput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numFPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picCapturer;
        private Controls.CtrImageViewer picPreview;
        private Controls.CtrOutput ctrOutput;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.SplitContainer splitOutput;
        private System.Windows.Forms.Button btnStartEnd;
        private System.Windows.Forms.ComboBox ddlAutomationBot;
        private System.Windows.Forms.NumericUpDown numFPS;
    }
}

