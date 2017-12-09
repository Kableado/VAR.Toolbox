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
            this.grpCover = new System.Windows.Forms.GroupBox();
            this.btnCover = new System.Windows.Forms.Button();
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
            this.grpCover.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCover.Name = "grpCover";
            this.grpCover.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCover.Size = new System.Drawing.Size(262, 160);
            this.grpCover.TabIndex = 14;
            this.grpCover.TabStop = false;
            this.grpCover.Text = "Cover";
            // 
            // btnCover
            // 
            this.btnCover.Location = new System.Drawing.Point(8, 83);
            this.btnCover.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCover.Name = "btnCover";
            this.btnCover.Size = new System.Drawing.Size(181, 35);
            this.btnCover.TabIndex = 7;
            this.btnCover.Text = "Cover";
            this.btnCover.UseVisualStyleBackColor = true;
            this.btnCover.Click += new System.EventHandler(this.btnCover_Click);
            // 
            // numInactive
            // 
            this.numInactive.Location = new System.Drawing.Point(131, 27);
            this.numInactive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.numInactive.Size = new System.Drawing.Size(58, 26);
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
            this.lblInactive.Location = new System.Drawing.Point(8, 58);
            this.lblInactive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInactive.Name = "lblInactive";
            this.lblInactive.Size = new System.Drawing.Size(79, 20);
            this.lblInactive.TabIndex = 8;
            this.lblInactive.Text = "lblInactive";
            // 
            // chkAutoCover
            // 
            this.chkAutoCover.AutoSize = true;
            this.chkAutoCover.Checked = true;
            this.chkAutoCover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoCover.Location = new System.Drawing.Point(8, 29);
            this.chkAutoCover.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAutoCover.Name = "chkAutoCover";
            this.chkAutoCover.Size = new System.Drawing.Size(110, 24);
            this.chkAutoCover.TabIndex = 9;
            this.chkAutoCover.Text = "AutoCover";
            this.chkAutoCover.UseVisualStyleBackColor = true;
            // 
            // timTicker
            // 
            this.timTicker.Enabled = true;
            this.timTicker.Tick += new System.EventHandler(this.timTicker_Tick);
            // 
            // PnlCover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpCover);
            this.Name = "PnlCover";
            this.Size = new System.Drawing.Size(262, 160);
            this.grpCover.ResumeLayout(false);
            this.grpCover.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInactive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCover;
        private System.Windows.Forms.Button btnCover;
        private System.Windows.Forms.NumericUpDown numInactive;
        private System.Windows.Forms.Label lblInactive;
        private System.Windows.Forms.CheckBox chkAutoCover;
        private System.Windows.Forms.Timer timTicker;
    }
}
