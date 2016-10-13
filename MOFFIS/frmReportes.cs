using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TFHKADIR;

namespace MOFFIS
{
    public partial class frmReportes : Form
    {
        private int IRetorno;
        private int IRetornoImpresora;

        int handler;
        char FS = Convert.ToChar(28);
        char etx = Convert.ToChar(3);
        char FS2 = Convert.ToChar(128);
        int init;

        public Tfhka Tf
        {
            get { return frmPrincipal.tf; }
            set { frmPrincipal.tf = value; }
        }

        public frmReportes()
        {
            InitializeComponent();
        }

        private bool ValidarImpresora()
        {
            string error;
            int err;
            PrinterStatus StatusError;
            StatusError = Tf.getPrinterStatus();
            err = StatusError.PrinterErrorCode;
            error = StatusError.PrinterErrorDescription;
            if (err.Equals(0))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Error: " + error);
                return false;
            }
            return false;
        }

        private bool ValidarReporteZ()
        {
            return true;
        }

        private void btnGenerarReporteX_Click(object sender, EventArgs e)
        {
            handler = frmPrincipal.handlerM;
            string respuesta;
            string[] CadResp;
            string[] status;
            string mensaje, mensaje1, mensaje2, SImp, SFis;
            mensaje = HASAR.MandaPaqueteFiscal(handler, "9∟X∟X").ToString();
            if (mensaje == "0")
            {
                respuesta = HASAR.LeerDoc();
                CadResp = respuesta.Split(etx);
                status = CadResp[0].Split(FS);
                SImp = status[1];
                SFis = status[2];

                mensaje1 = HASAR.error_SF(SImp, 1);
                if (mensaje1 != "0")
                {
                    MessageBox.Show("Errores: " + mensaje);
                }

                mensaje2 = HASAR.error_SF(SFis, 2);
                if (mensaje2 != "0")
                {
                    MessageBox.Show("Errores: " + mensaje);
                }

                if ((mensaje1 == "0") && (mensaje2 == "0"))
                {
                    MessageBox.Show("Reducción X impresa correctamente", "Reducción X", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Error en generación de reducción X", "Error en reducción X", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGenerarReporteZ_Click(object sender, EventArgs e)
        {
            handler = frmPrincipal.handlerM;
            string respuesta;
            string[] CadResp;
            string[] status;
            string mensaje, mensaje1, mensaje2, SImp, SFis;
            mensaje = HASAR.MandaPaqueteFiscal(handler, "9∟Z∟S").ToString();
            if (mensaje == "0")
            {
                respuesta = HASAR.LeerDoc();
                CadResp = respuesta.Split(etx);
                status = CadResp[0].Split(FS);
                SImp = status[1];
                SFis = status[2];

                mensaje1 = HASAR.error_SF(SImp, 1);
                if (mensaje1 != "0")
                {
                    MessageBox.Show("Errores: " + mensaje1);
                }

                mensaje2 = HASAR.error_SF(SFis, 2);
                if (mensaje2 != "0")
                {
                    MessageBox.Show("Errores: " + mensaje2);
                }

                if ((mensaje1 == "0") && (mensaje2 == "0"))
                {
                    MessageBox.Show("Reducción Z impresa correctamente", "Reducción Z", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Error en generación de reducción Z", "Error en reducción Z", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
