namespace VAR.Toolbox.UI
{
    partial class FrmDialogString
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
            this.txtValue = new VAR.Toolbox.Controls.TextBoxNormal();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tblLayout.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.Location = new System.Drawing.Point(3, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(410, 114);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "label1";
            // 
            // tblLayout
            // 
            this.tblLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblLayout.ColumnCount = 1;
            this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayout.Controls.Add(this.txtValue, 0, 1);
            this.tblLayout.Controls.Add(this.lblDescription, 0, 0);
            this.tblLayout.Controls.Add(this.flowButtons, 0, 2);
            this.tblLayout.Location = new System.Drawing.Point(12, 12);
            this.tblLayout.Name = "tblLayout";
            this.tblLayout.RowCount = 3;
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblLayout.Size = new System.Drawing.Size(416, 260);
            this.tblLayout.TabIndex = 1;
            // 
            // txtValue
            // 
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValue.Location = new System.Drawing.Point(3, 117);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(410, 108);
            this.txtValue.TabIndex = 2;
            // 
            // flowButtons
            // 
            this.flowButtons.Controls.Add(this.btnCancel);
            this.flowButtons.Controls.Add(this.btnAccept);
            this.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowButtons.Location = new System.Drawing.Point(3, 231);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowButtons.Size = new System.Drawing.Size(410, 26);
            this.flowButtons.TabIndex = 3;
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point(251, 3);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(332, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmDialogString
            // 
            this.ClientSize = new System.Drawing.Size(440, 284);
            this.Controls.Add(this.tblLayout);
            this.Name = "FrmDialogString";
            this.Text = "DialogString";
            this.tblLayout.ResumeLayout(false);
            this.tblLayout.PerformLayout();
            this.flowButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TableLayoutPanel tblLayout;
        private VAR.Toolbox.Controls.TextBoxNormal txtValue;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
    }
}