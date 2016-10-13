using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;

namespace MOFFIS
{
    static class Program
    {
        //static public DSSesion.UsuarioDataTable usuario = new DSSesion.UsuarioDataTable();
        //static public bool RutaConfigDesarrollo = true;
        //static public FrmBuscar ultimocheque;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int IRetorno;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-PA", false);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-PA");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //IRetorno = BemaFI32.Bematech_FI_CierraCupon("Tarjeta", "A", "%", "0000", "00", "LUIS ES UN EXPLOTADOR!");
            //BemaFI32.Analisa_iRetorno(IRetorno);
            //BemaFI32.Analisa_RetornoImpresora();

            //IRetorno = BemaFI32.Bematech_FI_VerificaImpresoraPrendida();
            //BemaFI32.Analisa_iRetorno(IRetorno);
            //BemaFI32.Analisa_RetornoImpresora();

            ////string Alicuotas = new string('\x20', 79);
            ////IRetorno = BemaFI32.Bematech_FI_RetornoAlicuotas(ref Alicuotas);
            ////BemaFI32.Analisa_iRetorno(IRetorno);
            ////BemaFI32.Analisa_RetornoImpresora();
            ////MessageBox.Show("Alicuotas Programadas : " + Alicuotas, "Bematech", MessageBoxButtons.OK);

            IRetorno = 1;

            if (IRetorno > 0)
            {
                Application.Run(new frmLogin());
            }

            
           

        }
    }
}
