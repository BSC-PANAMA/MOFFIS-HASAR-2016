using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using Interop.PeachwServer;
using TFHKADIR;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MOFFIS
{
    public partial class frmPrincipal : System.Windows.Forms.Form
    {
        //Declaración de constantes necesarias (valores en hexadecimal)
        private const int MF_BYPOSITION = 0x400;
        private const int MF_REMOVE = 0x1000;
        private const int MF_DISABLED = 0x2;

        //Quitar elementos del menú de sistema    
        [DllImport("user32.Dll")]
        public static extern IntPtr RemoveMenu(int hMenu, int nPosition,int wFlags);

        //Obtener el menú de sistema
        [DllImport("User32.Dll")]
        public static extern IntPtr GetSystemMenu(int hWnd, bool bRevert);

        //Obtener el número de elementos del menú de sistema
        [DllImport("User32.Dll")]
        public static extern IntPtr GetMenuItemCount(int hMenu);

        //Redibujar la barra de título de la ventana
        [DllImport("User32.Dll")]
        public static extern IntPtr DrawMenuBar(int hwnd);

        static public Tfhka tf;
        static public string Puerto = "";

        private Interop.PeachwServer.Import importer;
        private ConectarPT ptApp = new ConectarPT();

        private int ControladorError;

        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;

        static public string rolUsuario;
        static public string pathCompaniaUsuario;
        static public string error;

        //nuevo multiempresa

        static public string PuertoImpresora;
        static public string IDcomp;


        private string PathMoffis;

        static public int handlerM;
        char FS = Convert.ToChar(28);
        char etx = Convert.ToChar(3);
        int init;

        public int HANDLER
        {
            get { return frmPrincipal.handlerM; }
            set { frmPrincipal.handlerM = value; }
        }

        public string ROL
        {
            get { return frmPrincipal.rolUsuario; }
            set { frmPrincipal.rolUsuario = value; }
        }

        public string ERROR
        {
            get { return frmPrincipal.error; }
            set { frmPrincipal.error = value; }
        }

        public string PathCompania
        {
            get { return frmPrincipal.pathCompaniaUsuario; }
            set { frmPrincipal.pathCompaniaUsuario = value; }
        }

        public string PuertoImp
        {
            get { return frmPrincipal.PuertoImpresora; }
            set { frmPrincipal.PuertoImpresora = value; }
        }


        public string IDcompania
        {
            get { return frmPrincipal.IDcomp; }
            set { frmPrincipal.IDcomp = value; }
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
          
        }

        //Método que desactiva el botón X (cerrar)
        public void DisableCloseButton(int hWnd)
        {
            IntPtr hMenu;
            IntPtr menuItemCount;

            //Obtener el manejador del menú de sistema del formulario
            hMenu = GetSystemMenu(hWnd, false);
            //Obtener la cuenta de los ítems del menú de sistema.
            //Es el menú que aparece al pulsar sobre el icono a la izquierda
            //de la Barra de título de la ventana, consta de los ítems: Restaurar, Mover,
            //Tamaño,Minimizar, Maximizar, Separador, Cerrar
            menuItemCount = GetMenuItemCount(hMenu.ToInt32());

            //Quitar el ítem Close (Cerrar), que es el último de ese menú
            RemoveMenu(hMenu.ToInt32(), menuItemCount.ToInt32() - 1,MF_DISABLED | MF_BYPOSITION);

            //Quitar el ítem Separador, el penúltimo de ese menú, entre Maximizar y Cerrar
            RemoveMenu(hMenu.ToInt32(), menuItemCount.ToInt32() - 2, MF_DISABLED | MF_BYPOSITION);

            //Redibujar la barra de menú
            DrawMenuBar(hWnd);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenido a MOFFIS, ha iniciado correctamente la sesión.", "Bienvenido a MOFFIS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            int Puerto = 0;
            //Puerto = Convert.ToInt32(ConfigurationSettings.AppSettings["PuertoImpresora"].ToString());

            if (ROL == "BSC")
            {
                Puerto = Convert.ToInt32(ConfigurationSettings.AppSettings["PuertoImpresora"].ToString());
            }
            else
            {
                Puerto = Convert.ToInt32(PuertoImpresora);
            }


            HASAR.SetModoEpson();
            handlerM = HASAR.OpenComFiscal(Puerto, 0);
            init = HASAR.InitFiscal(handlerM);

            if (handlerM >= 0)
            {
                MessageBox.Show("Impresora validada correctamente", "Impresora validada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (rolUsuario == "BSC")
                {
                    this.Reportes.Visible = false;
                    this.BSCMenu.Visible = true;
                    this.MantenimientoMenu.Visible = false;
                    this.ComprobantesFiscMenu.Visible = false;
                    this.ComprobantesNoFiscMenu.Visible = false;
                }
                if (rolUsuario == "REPORTE")
                {
                    this.Reportes.Visible = true;
                    this.BSCMenu.Visible = false;
                    this.MantenimientoMenu.Visible = false;
                    this.ComprobantesFiscMenu.Visible = false;
                    this.ComprobantesNoFiscMenu.Visible = false;
                }
                else
                    if (rolUsuario == "ADMIN")
                    {
                        this.Reportes.Visible = false;
                        this.BSCMenu.Visible = false;
                        this.MantenimientoMenu.Visible = true;
                        this.ComprobantesFiscMenu.Visible = false;
                        this.ComprobantesNoFiscMenu.Visible = false;
                    }
                    else
                        if ((rolUsuario == "CAJERO_1") || (rolUsuario == "CAJERO_2"))
                        {
                            this.Reportes.Visible = false;
                            this.BSCMenu.Visible = false;
                            this.MantenimientoMenu.Visible = false;
                            this.ComprobantesFiscMenu.Visible = true;
                            //this.ComprobantesNoFiscMenu.Visible = true;
                        }
            }
            else
            {
                MessageBox.Show("Error en la apertura del puerto de la impresora, revise que se encuentre conectada correctamente en el puerto asignado el dia de su instalacion o verifique que la impresora se encuentra correctamente conectada o encendida, verifique y vuelva a abrir MOFFIS", "Error con comunicación con impresora", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void mitMantenimiento_Click(object sender, EventArgs e)
        {
            MOFFIS.frmMantenimientoCompania_Usuarios MantenimientoCU = new MOFFIS.frmMantenimientoCompania_Usuarios();
            MantenimientoCU.MdiParent = this;
            MantenimientoCU.Show();
        }

        private void mitCrearInvoice_Click(object sender, EventArgs e)
        {
            if (this.ValidarImpresora())
            {
                if (this.ValidarReporteZ())
                {
                    if (this.ValidarPeachtree())
                    {
                        if (this.ValidarStatusError(1))
                        {
                            MOFFIS.frmCrearInvoices.PuertoImpresora = PuertoImpresora;
                            MOFFIS.frmCrearInvoices.IDcomp = IDcomp;

                            MOFFIS.frmCrearInvoices.DefInstance.MdiParent = this;
                            MOFFIS.frmCrearInvoices.DefInstance.Show();
                        }
                    }
                }
            }
        }

        private void mitNotasCredito_Click(object sender, EventArgs e)
        {
            if (this.ValidarImpresora())
            {
                if (this.ValidarReporteZ())
                {
                    if (this.ValidarPeachtree())
                    {
                        if (this.ValidarStatusError(1))
                        {
                            MOFFIS.frmNotasCredito.PuertoImpresora = PuertoImpresora;
                            MOFFIS.frmNotasCredito.IDcomp = IDcomp;
                            MOFFIS.frmNotasCredito.DefInstance.MdiParent = this;
                            MOFFIS.frmNotasCredito.DefInstance.Show();
                        }
                    }
                }
            }
        }

        private void mitNotasDebito_Click(object sender, EventArgs e)
        {
            if (this.ValidarImpresora())
            {
                if (this.ValidarReporteZ())
                {
                    if (this.ValidarPeachtree())
                    {
                        if (this.ValidarStatusError(1))
                        {
                            MOFFIS.frmNotasDebito.PuertoImpresora = PuertoImpresora;
                            MOFFIS.frmNotasDebito.IDcomp = IDcomp;
                            MOFFIS.frmNotasDebito.DefInstance.MdiParent = this;
                            MOFFIS.frmNotasDebito.DefInstance.Show();
                        }
                    }
                }
            }
        }

        private void mitImprimirUltimaFactura_Click(object sender, EventArgs e)
        {
            if (this.ValidarImpresora())
            {
                if (this.ValidarReporteZ())
                {
                    if (this.ValidarPeachtree())
                    {
                        if (this.ValidarStatusError(1))
                        {
                            MOFFIS.frmNoFiscal_Re_Imprimir ReImprimir = new frmNoFiscal_Re_Imprimir();
                            ReImprimir.MdiParent = this;
                            ReImprimir.Show();
                        }
                    }
                }
            }
        }

        private void mantenimientoPeachtreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.ValidarImpresora())
            //{
                if (this.ValidarPeachtree())
                {
                    if (this.ValidarStatusError(1))
                    {
                        MOFFIS.frmMantenimiento Mantenimiento = new MOFFIS.frmMantenimiento();
                        Mantenimiento.MdiParent = this;

                        Mantenimiento.Id_compañia = IDcomp;
                        Mantenimiento.Puerto = PuertoImpresora;
                        Mantenimiento.Show();
                    }
                }
            //}
        }

        private bool ValidarImpresora()
        {
            try
            {
                string mensaje, mensaje1, mensaje2, SImp, SFis;
                string respuesta;
                string[] CadResp;
                string[] status;
                HASAR.LimpiarDoc();
                mensaje = HASAR.MandaPaqueteFiscal(handlerM, "*").ToString();
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
                        return false;
                    }

                    mensaje2 = HASAR.error_SF(SFis, 2);
                    if (mensaje2 != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje2);
                        return false;
                    }

                    if ((mensaje1 == "0") && (mensaje2 == "0"))
                    {
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Error Immpresora, revise que la impresora este correctamente encendida y conectada.", "Error en Impresora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Immpresora, revise que la impresora este correctamente encendida y conectada", "Error en Impresora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }           
        }

        private bool ValidarReporteZ()
        {
            return true;
        }

        private bool ValidarStatusError(int mostrarMessage)
        {
            // validar q archivo de status de error exista
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\BSC\IndicadorError.xml";

            if (System.IO.File.Exists(PathListado))
            {
                string sNumStatusError = "";

                imp = new XmlImplementation();
                doc = imp.CreateDocument();
                doc.Load(PathListado);

                reader = doc.GetElementsByTagName("IndicadorError");
                sNumStatusError = reader.Item(0).InnerText;

                imp = null;
                doc = null;
                reader = null;

                if ((sNumStatusError.Trim() == "F0") || (sNumStatusError.Trim() == "F2") || (sNumStatusError.Trim() == "F4"))
                {
                    return true;
                    ERROR = "0";
                }
                else
                {
                    if (sNumStatusError.Trim() == "F1")
                    {
                        ERROR = "1";
                    }
                    else
                        if (sNumStatusError.Trim() == "F3")
                        {
                            ERROR = "3";
                        }
                        else
                            if (sNumStatusError.Trim() == "F5")
                            {
                                ERROR = "5";
                            }
                    if (mostrarMessage == 1)
                    {
                        MessageBox.Show("Al parecer ocurrio un hecho reciente que no permitio concluir un proceso de generacion de comprobante fiscal, dirijase al menu opciones y proceda a anular el ultimo comprobante fiscal..Gracias", "Error en Comprobante fiscal", MessageBoxButtons.OK);
                    }                    
                    return false;
                }
            }
            else
            {
                MessageBox.Show("El Archivo de status de error no existe en la ruta definida, para crear dicho archivo dirijase a la seccion de creacion de documento Estatus de Error del manual de MOFFIS", "Archivo No Existe", MessageBoxButtons.OK);
                return false;
            }
        }

        private void mitCambiarDeUsuario_Click(object sender, EventArgs e)
        {
            handlerM = HASAR.CloseComFiscal(handlerM);
            frmLogin.sCambiarUsuario = "SI";
            this.Close();
        }

        private bool ValidarPeachtree()
        {
            try
            {
                string appPath = ptApp.app.ApplicationPath;
                bool company = ptApp.app.CompanyIsOpen;
                string companyPath = ptApp.app.CompanyPath;
                string companyName = ptApp.app.CurrentCompanyName;

                int esCompaniaUsuario = String.Compare(Path.GetFullPath(companyPath).TrimEnd('\\'), Path.GetFullPath(pathCompaniaUsuario).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase);
                if (esCompaniaUsuario.Equals(0))
                {

                }
                else
                {
                    MessageBox.Show("Al parecer cambio la compañia en Peachtree, el sistema moffis esta configurado para otra compañia. Porfavor regrese a la compañia original", "Compañia Cambiada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Al parecer cerro el programa Peachtree o cerro la compañia. Debera iniciar nuevamente desde el login del sistema", "Peachtree Cerrado o Compañia Cerrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void mitAnularDocumento_Click(object sender, EventArgs e)
        {
            if (this.ValidarImpresora())
            {
                if (this.ValidarPeachtree())
                {
                    if (this.ValidarStatusError(0))
                    {
                        //tf.SendCmd("7");
                        //this.Importfile("A");
                    }
                    else
                    {
                        ControladorError = 0;

                        if (tf.SendCmd("7"))
                        {
                            MessageBox.Show("El ultimo documento ha sido anulado correctamente", "Documento Anulado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (ERROR == "1")
                            {
                                this.ImportFileF("A");
                            }
                            else
                                if (ERROR == "3")
                                {
                                    this.ImportFileNC("A");
                                }
                                else
                                    if (ERROR == "5")
                                    {
                                        this.ImportFileND("A");
                                    }

                            this.IndicadorError("F0");
                        }
                        else
                        {
                            MessageBox.Show("No se pudo anular el documento", "Anulación Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }            
        }

        private void IndicadorError(string Estado)
        {
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\BSC\IndicadorError.xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);
            Writer.WriteStartElement("Indicador_Error");
            Writer.WriteStartElement("IndicadorError");
            Writer.WriteString(Estado);
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.Close();
        }

        private void ImportFileF(string Tipo)
        {
            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjSalesJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMID);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnitPrice);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnits);

            //peachwIEObjSalesJournalField_enUMID
            //peachwIEObjSalesJournalField_enUMStockingUnitPrice
            //peachwIEObjSalesJournalField_enUMStockingUnits
            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\Factura\NuevaFactura.xml";
                string PathListado2 = PathMoffis + @"\XML\Factura\AnularFactura.xml";

                if (Tipo == "N")
                {
                    importer.SetFilename(PathListado);
                }
                else
                {
                    importer.SetFilename(PathListado2);
                }

                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Factura almacenada correctamente en Peachtree");
            }
            catch (System.Exception e)
            {
                ControladorError = 1;
                MessageBox.Show(e.Message);
            }
        }

        private void ImportFileNC(string Tipo)
        {

            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjSalesJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToInvoiceDistNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToInvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToSalesOrder);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_BeginningBalanceTransaction);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_COSTAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CostOfSalesAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CostOfSalesAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DateClearedInAccountRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DateDue);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DiscountAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DiscountDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DisplayedTerms);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DropShip);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enApplyToProposal);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enCOSAcntDateClearedInBankRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enGL_DateClearedInBankRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enInvAcntDateClearedInBankRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enProgressBillingInvoice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRecur);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRecurNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRetainagePercent);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enSerialNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enStockingQuantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnits);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enVoidedBy);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_INVAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InventoryAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceDistNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNote);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNote2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_JobGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_JobId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NotePrintsAfterLineItems);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumFields);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quote);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_QuoteGoodThruDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_QuoteNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ReceiptNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ReturnAuthorization);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderDistNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepresentativeGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxAuthority);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxCode);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipByDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCountry);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_StatementNote);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_StatementNotePrintsBeforeInvoiceRef);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionPeriod);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UPCSKU);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Weight);


            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\NotaCredito\NewNotaCredito.xml";
                string PathListado2 = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";

                if (Tipo == "N")
                {
                    importer.SetFilename(PathListado);
                }
                else
                {
                    importer.SetFilename(PathListado2);
                }
                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Nota Credito almacenada correctamente en Peachtree");
            }
            catch (System.Exception e)
            {
                ControladorError = 1;
                MessageBox.Show(e.Message);
            }
        }

        private void ImportFileND(string Tipo)
        {
            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjSalesJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);

            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = "";

                if (Tipo == "N")
                {
                    PathListado = PathMoffis + @"\XML\NotaDebito\NuevaNotaDebito.xml";
                    importer.SetFilename(PathListado);
                }
                else
                {
                    PathListado = PathMoffis + @"\XML\NotaDebito\AnularNotaDebito.xml";
                    importer.SetFilename(PathListado);
                }

                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Nota de Debito almacenada correctamente en Peachtree", "Guardar Nota Debito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception e)
            {
                if (e.Message.Substring(0, 7) == "WARNING")
                {
                    ControladorError = 1;
                    MessageBox.Show(e.Message);
                }
                else
                {
                    MessageBox.Show("Nota de Debito almacenada correctamente en Peachtree", "Guardar Nota Debito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void mantenimientoEmpresaUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MOFFIS.frmMantenimientoCompania_Usuarios MantenimientoCU = new MOFFIS.frmMantenimientoCompania_Usuarios();
            MantenimientoCU.MdiParent = this;
            MantenimientoCU.Show();
        }

        private void generarReportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MOFFIS.frmReportes GenerarRep = new MOFFIS.frmReportes();
            GenerarRep.MdiParent = this;
            GenerarRep.Show();
        }

        private void mitSoporteTecnico_Click(object sender, EventArgs e)
        {
            MOFFIS.frmSoporteTecnico SoporteTec = new MOFFIS.frmSoporteTecnico();
            SoporteTec.MdiParent = this;
            SoporteTec.Show();
        }

        private void setearVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndicadorError("F0");
            MessageBox.Show("Cambio aplicado");
        }


        public void Importfile_factura(string Tipo)
        {
            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjSalesJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);

            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = "";
                if (Tipo == "N")
                {
                    PathListado = PathMoffis + @"\XML\Factura\NuevaFactura.xml";
                    importer.SetFilename(PathListado);
                }
                else
                {
                    PathListado = PathMoffis + @"\XML\Factura\AnularFactura.xml";
                    importer.SetFilename(PathListado);
                }

                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Factura almacenada correctamente en Peachtree", "Guardar factura", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception e)
            {
                if (e.Message.Substring(0, 7) == "WARNING")
                {
                    ControladorError = 1;
                    MessageBox.Show(e.Message);
                }
                else
                {
                    MessageBox.Show("Factura almacenada correctamente en Peachtree", "Guardar factura", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void Importfile_Recibo()
        {
            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjCashReceiptsJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Amount);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_CashAccountId);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_CashAmount);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_CustomerId);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_CustomerName);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Date);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_DepositTicketId);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Description);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_DiscountAmount);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_enStockingQuantity);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_enUMStockingUnitPrice);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_enUMStockingUnits);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_GLAccountId);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_InvoicePaid);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_NumberOfDistributions);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_PayMethod);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Prepayment);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Quantity);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_ReceiptNumber);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Reference);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_SalesRepId);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_SalesTaxCode);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_TaxType);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_TotalPaidOnInvoices);//**
            //importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_TransactionPeriod);
            //importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_TransactionNumber);
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_UnitPrice);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_VendorReceipt);//**
            importer.AddToImportFieldList((short)PeachwIEObjCashReceiptsJournalField.peachwIEObjCashReceiptsJournalField_Weight);//**


            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathNuevoRecibo = PathMoffis + @"\XML\Factura\NuevaFacturaRecibo.xml";
            //string PathListado2 = PathMoffis + @"\XML\Factura\Cotizaciones\DetalleCotizacion2.xml";
            try
            {
                importer.SetFilename(PathNuevoRecibo);
                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Recibo almacenado correctamente en Peachtree");
            }
            catch (System.Exception e)
            {
                ControladorError = 1;
                MessageBox.Show(e.Message);
            }
        }

        public void Importfile_NC(string Tipo)
        {
            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjSalesJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToInvoiceDistNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToInvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToSalesOrder);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_BeginningBalanceTransaction);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_COSTAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CostOfSalesAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CostOfSalesAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DateClearedInAccountRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DateDue);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DiscountAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DiscountDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DisplayedTerms);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DropShip);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enApplyToProposal);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enCOSAcntDateClearedInBankRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enGL_DateClearedInBankRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enInvAcntDateClearedInBankRec);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enProgressBillingInvoice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRecur);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRecurNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRetainagePercent);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enSerialNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enStockingQuantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnits);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enVoidedBy);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_INVAccountGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InventoryAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceDistNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNote);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNote2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_JobGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_JobId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NotePrintsAfterLineItems);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumFields);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quote);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_QuoteGoodThruDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_QuoteNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ReceiptNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ReturnAuthorization);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderDistNum);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepresentativeGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxAuthority);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxCode);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipByDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCountry);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_StatementNote);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_StatementNotePrintsBeforeInvoiceRef);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionGUID);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionPeriod);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UPCSKU);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Weight);

            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\NotaCredito\NewNotaCredito.xml";
                string PathListado2 = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";

                if (Tipo == "N")
                {
                    importer.SetFilename(PathListado);
                }
                else
                {
                    importer.SetFilename(PathListado2);
                }
                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Nota Credito almacenada correctamente en Peachtree");
            }
            catch (System.Exception e)
            {
                ControladorError = 1;
                MessageBox.Show(e.Message);
            }
        }

        public void Importfile_ND(string Tipo)
        {
            importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjSalesJournal);
            importer.ClearImportFieldList();
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            //importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);

            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
            importer.AddToImportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);

            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = "";

                if (Tipo == "N")
                {
                    PathListado = PathMoffis + @"\XML\NotaDebito\NuevaNotaDebito.xml";
                    importer.SetFilename(PathListado);
                }
                else
                {
                    PathListado = PathMoffis + @"\XML\NotaDebito\AnularNotaDebito.xml";
                    importer.SetFilename(PathListado);
                }

                importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                importer.Import();
                MessageBox.Show("Nota de Debito almacenada correctamente en Peachtree", "Guardar Nota Debito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception e)
            {
                if (e.Message.Substring(0, 7) == "WARNING")
                {
                    ControladorError = 1;
                    MessageBox.Show(e.Message);
                }
                else
                {
                    MessageBox.Show("Nota de Debito almacenada correctamente en Peachtree", "Guardar Nota Debito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void facturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Importfile_factura("N");
        }

        private void reciboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Importfile_Recibo();
        }

        private void notaCreditoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Importfile_NC("N");
        }

        private void notaDebitoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Importfile_ND("N");
        }



    }
}
