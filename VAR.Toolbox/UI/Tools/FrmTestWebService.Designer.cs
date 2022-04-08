namespace VAR.Toolbox.UI.Tools
{
    partial class FrmTestWebService
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
            this.tabWebServices = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnTestSoap = new VAR.Toolbox.Controls.CButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtResultSoap = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtParametersSoap = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtMethodSoap = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtNamespaceUrlSoap = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtUrlSoap = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtResultRest = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.btnTestRest = new VAR.Toolbox.Controls.CButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblUrlApiMethod = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtBodyRest = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtParametersRest = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtUrlApiMethodRest = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.txtUrlRest = new VAR.Toolbox.Controls.TextBoxMonospace();
            this.tabWebServices.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabWebServices
            // 
            this.tabWebServices.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabWebServices.Controls.Add(this.tabPage1);
            this.tabWebServices.Controls.Add(this.tabPage2);
            this.tabWebServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabWebServices.Location = new System.Drawing.Point(0, 0);
            this.tabWebServices.Name = "tabWebServices";
            this.tabWebServices.SelectedIndex = 0;
            this.tabWebServices.Size = new System.Drawing.Size(682, 521);
            this.tabWebServices.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage1.Controls.Add(this.btnTestSoap);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtResultSoap);
            this.tabPage1.Controls.Add(this.txtParametersSoap);
            this.tabPage1.Controls.Add(this.txtMethodSoap);
            this.tabPage1.Controls.Add(this.txtNamespaceUrlSoap);
            this.tabPage1.Controls.Add(this.txtUrlSoap);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(674, 492);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SoapService";
            // 
            // btnTestSoap
            // 
            this.btnTestSoap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestSoap.Location = new System.Drawing.Point(596, 190);
            this.btnTestSoap.Name = "btnTestSoap";
            this.btnTestSoap.Size = new System.Drawing.Size(75, 23);
            this.btnTestSoap.TabIndex = 21;
            this.btnTestSoap.Text = "Test";
            this.btnTestSoap.Click += new System.EventHandler(this.BtnTestSoap_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Result";
            // 
            // txtResultSoap
            // 
            this.txtResultSoap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResultSoap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtResultSoap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResultSoap.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtResultSoap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtResultSoap.Location = new System.Drawing.Point(6, 219);
            this.txtResultSoap.Multiline = true;
            this.txtResultSoap.Name = "txtResultSoap";
            this.txtResultSoap.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultSoap.Size = new System.Drawing.Size(662, 265);
            this.txtResultSoap.TabIndex = 19;
            // 
            // txtParametersSoap
            // 
            this.txtParametersSoap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParametersSoap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtParametersSoap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParametersSoap.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtParametersSoap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtParametersSoap.Location = new System.Drawing.Point(100, 81);
            this.txtParametersSoap.Multiline = true;
            this.txtParametersSoap.Name = "txtParametersSoap";
            this.txtParametersSoap.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtParametersSoap.Size = new System.Drawing.Size(571, 103);
            this.txtParametersSoap.TabIndex = 18;
            // 
            // txtMethodSoap
            // 
            this.txtMethodSoap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethodSoap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMethodSoap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMethodSoap.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtMethodSoap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtMethodSoap.Location = new System.Drawing.Point(100, 55);
            this.txtMethodSoap.Name = "txtMethodSoap";
            this.txtMethodSoap.Size = new System.Drawing.Size(571, 17);
            this.txtMethodSoap.TabIndex = 17;
            // 
            // txtNamespaceUrlSoap
            // 
            this.txtNamespaceUrlSoap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNamespaceUrlSoap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNamespaceUrlSoap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNamespaceUrlSoap.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtNamespaceUrlSoap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtNamespaceUrlSoap.Location = new System.Drawing.Point(100, 29);
            this.txtNamespaceUrlSoap.Name = "txtNamespaceUrlSoap";
            this.txtNamespaceUrlSoap.Size = new System.Drawing.Size(571, 17);
            this.txtNamespaceUrlSoap.TabIndex = 16;
            // 
            // txtUrlSoap
            // 
            this.txtUrlSoap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlSoap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUrlSoap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrlSoap.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtUrlSoap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtUrlSoap.Location = new System.Drawing.Point(100, 6);
            this.txtUrlSoap.Name = "txtUrlSoap";
            this.txtUrlSoap.Size = new System.Drawing.Size(571, 17);
            this.txtUrlSoap.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Parameters";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Method";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "NamespaceUrl";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "URL";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtResultRest);
            this.tabPage2.Controls.Add(this.btnTestRest);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.lblUrlApiMethod);
            this.tabPage2.Controls.Add(this.lblURL);
            this.tabPage2.Controls.Add(this.txtBodyRest);
            this.tabPage2.Controls.Add(this.txtParametersRest);
            this.tabPage2.Controls.Add(this.txtUrlApiMethodRest);
            this.tabPage2.Controls.Add(this.txtUrlRest);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(674, 492);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "RestService";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Result";
            // 
            // txtResultRest
            // 
            this.txtResultRest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResultRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtResultRest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResultRest.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtResultRest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtResultRest.Location = new System.Drawing.Point(8, 213);
            this.txtResultRest.Multiline = true;
            this.txtResultRest.Name = "txtResultRest";
            this.txtResultRest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultRest.Size = new System.Drawing.Size(658, 271);
            this.txtResultRest.TabIndex = 20;
            // 
            // btnTestRest
            // 
            this.btnTestRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestRest.Location = new System.Drawing.Point(591, 184);
            this.btnTestRest.Name = "btnTestRest";
            this.btnTestRest.Size = new System.Drawing.Size(75, 23);
            this.btnTestRest.TabIndex = 19;
            this.btnTestRest.Text = "Test";
            this.btnTestRest.Click += new System.EventHandler(this.BtnTestRest_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Body";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Parameters";
            // 
            // lblUrlApiMethod
            // 
            this.lblUrlApiMethod.AutoSize = true;
            this.lblUrlApiMethod.Location = new System.Drawing.Point(8, 39);
            this.lblUrlApiMethod.Name = "lblUrlApiMethod";
            this.lblUrlApiMethod.Size = new System.Drawing.Size(71, 13);
            this.lblUrlApiMethod.TabIndex = 16;
            this.lblUrlApiMethod.Text = "UrlApiMethod";
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(8, 12);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(29, 13);
            this.lblURL.TabIndex = 15;
            this.lblURL.Text = "URL";
            // 
            // txtBodyRest
            // 
            this.txtBodyRest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBodyRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBodyRest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBodyRest.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtBodyRest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtBodyRest.Location = new System.Drawing.Point(119, 119);
            this.txtBodyRest.Multiline = true;
            this.txtBodyRest.Name = "txtBodyRest";
            this.txtBodyRest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBodyRest.Size = new System.Drawing.Size(547, 59);
            this.txtBodyRest.TabIndex = 14;
            // 
            // txtParametersRest
            // 
            this.txtParametersRest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParametersRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtParametersRest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParametersRest.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtParametersRest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtParametersRest.Location = new System.Drawing.Point(119, 63);
            this.txtParametersRest.Multiline = true;
            this.txtParametersRest.Name = "txtParametersRest";
            this.txtParametersRest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtParametersRest.Size = new System.Drawing.Size(547, 50);
            this.txtParametersRest.TabIndex = 13;
            // 
            // txtUrlApiMethodRest
            // 
            this.txtUrlApiMethodRest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlApiMethodRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUrlApiMethodRest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrlApiMethodRest.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtUrlApiMethodRest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtUrlApiMethodRest.Location = new System.Drawing.Point(119, 36);
            this.txtUrlApiMethodRest.Name = "txtUrlApiMethodRest";
            this.txtUrlApiMethodRest.Size = new System.Drawing.Size(547, 17);
            this.txtUrlApiMethodRest.TabIndex = 12;
            // 
            // txtUrlRest
            // 
            this.txtUrlRest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUrlRest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrlRest.Font = new System.Drawing.Font("Consolas", 6F);
            this.txtUrlRest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtUrlRest.Location = new System.Drawing.Point(119, 9);
            this.txtUrlRest.Name = "txtUrlRest";
            this.txtUrlRest.Size = new System.Drawing.Size(547, 17);
            this.txtUrlRest.TabIndex = 11;
            // 
            // FrmTestWebService
            // 
            this.ClientSize = new System.Drawing.Size(682, 521);
            this.Controls.Add(this.tabWebServices);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmTestWebService";
            this.Text = "FrmTestWebService";
            this.tabWebServices.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabWebServices;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private VAR.Toolbox.Controls.CButton btnTestSoap;
        private System.Windows.Forms.Label label5;
        private VAR.Toolbox.Controls.TextBoxMonospace txtResultSoap;
        private VAR.Toolbox.Controls.TextBoxMonospace txtParametersSoap;
        private VAR.Toolbox.Controls.TextBoxMonospace txtMethodSoap;
        private VAR.Toolbox.Controls.TextBoxMonospace txtNamespaceUrlSoap;
        private VAR.Toolbox.Controls.TextBoxMonospace txtUrlSoap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private VAR.Toolbox.Controls.TextBoxMonospace txtResultRest;
        private VAR.Toolbox.Controls.CButton btnTestRest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblUrlApiMethod;
        private System.Windows.Forms.Label lblURL;
        private VAR.Toolbox.Controls.TextBoxMonospace txtBodyRest;
        private VAR.Toolbox.Controls.TextBoxMonospace txtParametersRest;
        private VAR.Toolbox.Controls.TextBoxMonospace txtUrlApiMethodRest;
        private VAR.Toolbox.Controls.TextBoxMonospace txtUrlRest;
    }
}