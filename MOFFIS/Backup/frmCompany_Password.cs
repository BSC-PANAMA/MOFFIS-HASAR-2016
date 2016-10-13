using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSSDK
{
    public partial class frmCompany_Password : Form
    {
        public frmCompany_Password()
        {
            InitializeComponent();
        }


        private void frmCompany_Password_Load(object sender, EventArgs e)
        {

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

    }
}
