﻿namespace VAR.Toolbox.UI
{
    partial class PnlSuspension
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
            this.chkSuspendAtCustom = new System.Windows.Forms.CheckBox();
            this.numOffset = new System.Windows.Forms.NumericUpDown();
            this.btnRandOffset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCustomSuspenedNow = new System.Windows.Forms.Button();
            this.ddlCustomHour = new System.Windows.Forms.ComboBox();
            this.ddlCustomMinute = new System.Windows.Forms.ComboBox();
            this.grpSuspension = new System.Windows.Forms.GroupBox();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.timTicker = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numOffset)).BeginInit();
            this.grpSuspension.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSuspendAtCustom
            // 
            this.chkSuspendAtCustom.AutoSize = true;
            this.chkSuspendAtCustom.Location = new System.Drawing.Point(8, 29);
            this.chkSuspendAtCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSuspendAtCustom.Name = "chkSuspendAtCustom";
            this.chkSuspendAtCustom.Size = new System.Drawing.Size(170, 24);
            this.chkSuspendAtCustom.TabIndex = 5;
            this.chkSuspendAtCustom.Text = "SuspendAtCustom";
            this.chkSuspendAtCustom.UseVisualStyleBackColor = true;
            // 
            // numOffset
            // 
            this.numOffset.Location = new System.Drawing.Point(90, 101);
            this.numOffset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numOffset.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numOffset.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOffset.Name = "numOffset";
            this.numOffset.Size = new System.Drawing.Size(58, 26);
            this.numOffset.TabIndex = 11;
            this.numOffset.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // btnRandOffset
            // 
            this.btnRandOffset.Location = new System.Drawing.Point(8, 101);
            this.btnRandOffset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRandOffset.Name = "btnRandOffset";
            this.btnRandOffset.Size = new System.Drawing.Size(74, 35);
            this.btnRandOffset.TabIndex = 12;
            this.btnRandOffset.Text = "Rand";
            this.btnRandOffset.UseVisualStyleBackColor = true;
            this.btnRandOffset.Click += new System.EventHandler(this.btnRandOffset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(158, 108);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Secs.";
            // 
            // btnCustomSuspenedNow
            // 
            this.btnCustomSuspenedNow.Location = new System.Drawing.Point(181, 60);
            this.btnCustomSuspenedNow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCustomSuspenedNow.Name = "btnCustomSuspenedNow";
            this.btnCustomSuspenedNow.Size = new System.Drawing.Size(34, 35);
            this.btnCustomSuspenedNow.TabIndex = 13;
            this.btnCustomSuspenedNow.Text = "N";
            this.btnCustomSuspenedNow.UseVisualStyleBackColor = true;
            this.btnCustomSuspenedNow.Click += new System.EventHandler(this.btnCustomSuspenedNow_Click);
            // 
            // ddlCustomHour
            // 
            this.ddlCustomHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCustomHour.FormattingEnabled = true;
            this.ddlCustomHour.Location = new System.Drawing.Point(37, 63);
            this.ddlCustomHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddlCustomHour.Name = "ddlCustomHour";
            this.ddlCustomHour.Size = new System.Drawing.Size(61, 28);
            this.ddlCustomHour.TabIndex = 14;
            // 
            // ddlCustomMinute
            // 
            this.ddlCustomMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCustomMinute.FormattingEnabled = true;
            this.ddlCustomMinute.Location = new System.Drawing.Point(109, 63);
            this.ddlCustomMinute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddlCustomMinute.Name = "ddlCustomMinute";
            this.ddlCustomMinute.Size = new System.Drawing.Size(61, 28);
            this.ddlCustomMinute.TabIndex = 15;
            // 
            // grpSuspension
            // 
            this.grpSuspension.Controls.Add(this.lblCountdown);
            this.grpSuspension.Controls.Add(this.ddlCustomMinute);
            this.grpSuspension.Controls.Add(this.ddlCustomHour);
            this.grpSuspension.Controls.Add(this.btnCustomSuspenedNow);
            this.grpSuspension.Controls.Add(this.label1);
            this.grpSuspension.Controls.Add(this.btnRandOffset);
            this.grpSuspension.Controls.Add(this.numOffset);
            this.grpSuspension.Controls.Add(this.chkSuspendAtCustom);
            this.grpSuspension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSuspension.Location = new System.Drawing.Point(0, 0);
            this.grpSuspension.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpSuspension.Name = "grpSuspension";
            this.grpSuspension.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpSuspension.Size = new System.Drawing.Size(261, 213);
            this.grpSuspension.TabIndex = 12;
            this.grpSuspension.TabStop = false;
            this.grpSuspension.Text = "Suspension";
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountdown.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblCountdown.Location = new System.Drawing.Point(8, 141);
            this.lblCountdown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(199, 37);
            this.lblCountdown.TabIndex = 16;
            this.lblCountdown.Text = "00:00:00:00";
            // 
            // timTicker
            // 
            this.timTicker.Enabled = true;
            this.timTicker.Tick += new System.EventHandler(this.timTicker_Tick);
            // 
            // PnlSuspension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpSuspension);
            this.Name = "PnlSuspension";
            this.Size = new System.Drawing.Size(261, 213);
            this.Load += new System.EventHandler(this.PnlSuspension_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numOffset)).EndInit();
            this.grpSuspension.ResumeLayout(false);
            this.grpSuspension.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSuspendAtCustom;
        private System.Windows.Forms.NumericUpDown numOffset;
        private System.Windows.Forms.Button btnRandOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCustomSuspenedNow;
        private System.Windows.Forms.ComboBox ddlCustomHour;
        private System.Windows.Forms.ComboBox ddlCustomMinute;
        private System.Windows.Forms.GroupBox grpSuspension;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Timer timTicker;
    }
}
