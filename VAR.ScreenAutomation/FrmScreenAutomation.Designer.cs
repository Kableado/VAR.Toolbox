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
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.ddlAutomationBot = new System.Windows.Forms.ComboBox();
            this.btnStartEnd = new System.Windows.Forms.Button();
            this.btnAutomationBotConfig = new System.Windows.Forms.Button();
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
            this.picCapturer.Location = new System.Drawing.Point(3, 3);
            this.picCapturer.Name = "picCapturer";
            this.picCapturer.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.picCapturer.Size = new System.Drawing.Size(319, 649);
            this.picCapturer.TabIndex = 0;
            this.picCapturer.TabStop = false;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.splitOutput);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.picCapturer);
            this.splitMain.Size = new System.Drawing.Size(566, 663);
            this.splitMain.SplitterDistance = 232;
            this.splitMain.SplitterWidth = 3;
            this.splitMain.TabIndex = 3;
            // 
            // splitOutput
            // 
            this.splitOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitOutput.Location = new System.Drawing.Point(0, 0);
            this.splitOutput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitOutput.Panel1
            // 
            this.splitOutput.Panel1.Controls.Add(this.picPreview);
            // 
            // splitOutput.Panel2
            // 
            this.splitOutput.Panel2.Controls.Add(this.btnAutomationBotConfig);
            this.splitOutput.Panel2.Controls.Add(this.numFPS);
            this.splitOutput.Panel2.Controls.Add(this.ddlAutomationBot);
            this.splitOutput.Panel2.Controls.Add(this.btnStartEnd);
            this.splitOutput.Panel2.Controls.Add(this.ctrOutput);
            this.splitOutput.Size = new System.Drawing.Size(232, 663);
            this.splitOutput.SplitterDistance = 229;
            this.splitOutput.SplitterWidth = 3;
            this.splitOutput.TabIndex = 4;
            // 
            // numFPS
            // 
            this.numFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFPS.Location = new System.Drawing.Point(186, 41);
            this.numFPS.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.numFPS.Size = new System.Drawing.Size(44, 20);
            this.numFPS.TabIndex = 5;
            this.numFPS.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // ddlAutomationBot
            // 
            this.ddlAutomationBot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlAutomationBot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAutomationBot.FormattingEnabled = true;
            this.ddlAutomationBot.Location = new System.Drawing.Point(10, 3);
            this.ddlAutomationBot.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ddlAutomationBot.Name = "ddlAutomationBot";
            this.ddlAutomationBot.Size = new System.Drawing.Size(171, 21);
            this.ddlAutomationBot.TabIndex = 4;
            this.ddlAutomationBot.SelectedIndexChanged += new System.EventHandler(this.DdlAutomationBot_SelectedIndexChanged);
            // 
            // btnStartEnd
            // 
            this.btnStartEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartEnd.Location = new System.Drawing.Point(11, 28);
            this.btnStartEnd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStartEnd.Name = "btnStartEnd";
            this.btnStartEnd.Size = new System.Drawing.Size(170, 32);
            this.btnStartEnd.TabIndex = 3;
            this.btnStartEnd.Text = "Start";
            this.btnStartEnd.UseVisualStyleBackColor = true;
            this.btnStartEnd.Click += new System.EventHandler(this.BtnStartEnd_Click);
            // 
            // btnAutomationBotConfig
            // 
            this.btnAutomationBotConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutomationBotConfig.Location = new System.Drawing.Point(187, 3);
            this.btnAutomationBotConfig.Name = "btnAutomationBotConfig";
            this.btnAutomationBotConfig.Size = new System.Drawing.Size(45, 23);
            this.btnAutomationBotConfig.TabIndex = 6;
            this.btnAutomationBotConfig.Text = "Cfg.";
            this.btnAutomationBotConfig.UseVisualStyleBackColor = true;
            this.btnAutomationBotConfig.Click += new System.EventHandler(this.btnAutomationBotConfig_Click);
            // 
            // picPreview
            // 
            this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.ImageShow = null;
            this.picPreview.Location = new System.Drawing.Point(9, 10);
            this.picPreview.Margin = new System.Windows.Forms.Padding(2);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(221, 217);
            this.picPreview.TabIndex = 1;
            this.picPreview.TabStop = false;
            // 
            // ctrOutput
            // 
            this.ctrOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrOutput.Location = new System.Drawing.Point(9, 64);
            this.ctrOutput.Margin = new System.Windows.Forms.Padding(2);
            this.ctrOutput.Name = "ctrOutput";
            this.ctrOutput.Size = new System.Drawing.Size(221, 356);
            this.ctrOutput.TabIndex = 2;
            this.ctrOutput.Text = "ctrOutput1";
            // 
            // FrmScreenAutomation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 663);
            this.Controls.Add(this.splitMain);
            this.Name = "FrmScreenAutomation";
            this.Text = "ScreenAutomation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmScreenAutomation_FormClosing);
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
        private System.Windows.Forms.Button btnAutomationBotConfig;
    }
}

