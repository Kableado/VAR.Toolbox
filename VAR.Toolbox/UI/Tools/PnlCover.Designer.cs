namespace VAR.Toolbox.UI
{
    partial class PnlCover
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpCover = new VAR.Toolbox.Controls.CGroupBox();
            this.btnCover = new VAR.Toolbox.Controls.CButton();
            this.numInactive = new System.Windows.Forms.NumericUpDown();
            this.lblInactive = new System.Windows.Forms.Label();
            this.chkAutoCover = new System.Windows.Forms.CheckBox();
            this.timTicker = new System.Windows.Forms.Timer(this.components);
            this.grpCover.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInactive)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCover
            // 
            this.grpCover.Controls.Add(this.btnCover);
            this.grpCover.Controls.Add(this.numInactive);
            this.grpCover.Controls.Add(this.lblInactive);
            this.grpCover.Controls.Add(this.chkAutoCover);
            this.grpCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCover.Location = new System.Drawing.Point(0, 0);
            this.grpCover.Margin = new System.Windows.Forms.Padding(4);
            this.grpCover.Name = "grpCover";
            this.grpCover.Padding = new System.Windows.Forms.Padding(4);
            this.grpCover.Size = new System.Drawing.Size(200, 123);
            this.grpCover.TabIndex = 14;
            this.grpCover.TabStop = false;
            this.grpCover.Text = "Cover";
            // 
            // btnCover
            // 
            this.btnCover.Location = new System.Drawing.Point(7, 66);
            this.btnCover.Margin = new System.Windows.Forms.Padding(4);
            this.btnCover.Name = "btnCover";
            this.btnCover.Size = new System.Drawing.Size(161, 28);
            this.btnCover.TabIndex = 7;
            this.btnCover.Text = "Cover";
            this.btnCover.Click += new System.EventHandler(this.BtnCover_Click);
            // 
            // numInactive
            // 
            this.numInactive.Location = new System.Drawing.Point(116, 22);
            this.numInactive.Margin = new System.Windows.Forms.Padding(4);
            this.numInactive.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numInactive.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInactive.Name = "numInactive";
            this.numInactive.Size = new System.Drawing.Size(52, 22);
            this.numInactive.TabIndex = 10;
            this.numInactive.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // lblInactive
            // 
            this.lblInactive.AutoSize = true;
            this.lblInactive.Location = new System.Drawing.Point(7, 46);
            this.lblInactive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInactive.Name = "lblInactive";
            this.lblInactive.Size = new System.Drawing.Size(70, 17);
            this.lblInactive.TabIndex = 8;
            this.lblInactive.Text = "lblInactive";
            // 
            // chkAutoCover
            // 
            this.chkAutoCover.AutoSize = true;
            this.chkAutoCover.Checked = true;
            this.chkAutoCover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoCover.Location = new System.Drawing.Point(7, 23);
            this.chkAutoCover.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoCover.Name = "chkAutoCover";
            this.chkAutoCover.Size = new System.Drawing.Size(96, 21);
            this.chkAutoCover.TabIndex = 9;
            this.chkAutoCover.Text = "AutoCover";
            // 
            // timTicker
            // 
            this.timTicker.Enabled = true;
            this.timTicker.Tick += new System.EventHandler(this.TimTicker_Tick);
            // 
            // PnlCover
            // 
            this.Controls.Add(this.grpCover);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PnlCover";
            this.Size = new System.Drawing.Size(200, 123);
            this.grpCover.ResumeLayout(false);
            this.grpCover.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInactive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VAR.Toolbox.Controls.CGroupBox grpCover;
        private VAR.Toolbox.Controls.CButton btnCover;
        private System.Windows.Forms.NumericUpDown numInactive;
        private System.Windows.Forms.Label lblInactive;
        private System.Windows.Forms.CheckBox chkAutoCover;
        private System.Windows.Forms.Timer timTicker;
    }
}
