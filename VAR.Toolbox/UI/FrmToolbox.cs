﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmToolbox : Form
    {
        public FrmToolbox()
        {
            InitializeComponent();

            MouseDown += DragWindow_MouseDown;
            lblToolbox.MouseDown += DragWindow_MouseDown;
        }

        private void FrmToolbox_Load(object sender, EventArgs e)
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void btnBase64_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmBase64));
        }

        private void btnProxyCmd_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmProxyCmd));
        }

        private void btnWebcam_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmWebcam));
        }

        private void btnTunnelTCP_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmTunnelTCP));
        }

        private void DragWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, User32.WM_NCLBUTTONDOWN, User32.HT_CAPTION, 0);
            }
        }
        #region Window handling

        private Form CreateWindow(Type type)
        {
            Form frm = Activator.CreateInstance(type) as Form;
            if(frm== null)
            {
                return null;
            }
            _forms.Add(frm);
            frm.FormClosing += frmChild_FormClosing;
            if ((frm is IFormWithIcon) == false)
            {
                frm.Icon = Icon;
            }
            frm.Show();
            return frm;
        }
        
        private List<Form> _forms = new List<Form>();

        private void frmChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            _forms.Remove((Form)sender);
        }

        private void FrmToolbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (_forms.Count > 0)
            {
                _forms[0].Close();
            }
        }

        private bool _wasMinimized = false;

        private void FrmToolbox_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                _wasMinimized = true;
                foreach(Form frm in _forms)
                {
                    frm.Hide();
                }
            }
            if (FormWindowState.Normal == WindowState && _wasMinimized)
            {
                _wasMinimized = false;
                foreach (Form frm in _forms)
                {
                    frm.Show();
                }
            }
        }

        #endregion Window handling
    }
}
