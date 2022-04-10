using System.ComponentModel;

namespace VAR.Toolbox.UI.Tools
{
    partial class FrmVirtualMouse
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.btnStartStop = new VAR.Toolbox.Controls.CButton();
            this.lsbInputs = new VAR.Toolbox.Controls.ListBoxMonospace();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(335, 331);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(92, 58);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lsbInputs
            // 
            this.lsbInputs.BackColor = System.Drawing.Color.Black;
            this.lsbInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsbInputs.Font = new System.Drawing.Font("Consolas", 9F);
            this.lsbInputs.ForeColor = System.Drawing.Color.Gray;
            this.lsbInputs.FormattingEnabled = true;
            this.lsbInputs.ItemHeight = 14;
            this.lsbInputs.Location = new System.Drawing.Point(12, 23);
            this.lsbInputs.Name = "lsbInputs";
            this.lsbInputs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsbInputs.Size = new System.Drawing.Size(224, 282);
            this.lsbInputs.TabIndex = 1;
            // 
            // FrmVirtualMouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lsbInputs);
            this.Controls.Add(this.btnStartStop);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmVirtualMouse";
            this.Text = "FrmVirtualMouse";
            this.ResumeLayout(false);
        }

        private VAR.Toolbox.Controls.ListBoxMonospace lsbInputs;

        private VAR.Toolbox.Controls.CButton btnStartStop;

        #endregion
    }
}