using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Interop.PeachwServer;
using TFHKADIR;

namespace MOFFIS
{
    public partial class frmMantenimiento : Form
    {
        private Interop.PeachwServer.Export exportador;
        private ConectarPT ptApp = new ConectarPT();

        DataTable dtUsuarios; // used as datasource of DataGridView
        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;

        private Array usersList;
        private Array taxesList;
        private Array taxesAList;
        private Array impuestosList;
        private Array glAcctIDList;

        private int IRetorno;

        private string PathMoffis;
        private string CuentaAnulacion;
        private string CuentaDescuento;

        private string CuentaAR;
        private string CuentaEfectivo;
        private string CuentaCheque;
        private string CuentaTarjeta;
        private string ModificarPrecios;
        private string Decimales;


        private string CodigoProducto = "";
        private string CambiosEspeciales = "";

        public string Id_compañia = null;
        public string Puerto = null;

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

        public frmMantenimiento()
        {
            InitializeComponent();
            this.ObtenerListadoCuentasGL();
           // this.LeerValoresDefault();         
        }

        public static Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;
        }

        private void ObtenerListadoCuentasGL()
        {
            try
            {
                exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjChartOfAccounts);

                exportador.ClearExportFieldList();
                exportador.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerId);
                exportador.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerDescription);
                exportador.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Inactive);
                exportador.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Type);
                exportador.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GUID);

                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ListadoAccounts.xml";
                string PathListado2 = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ListadoAccounts2.xml";

                exportador.SetFilename(PathListado);
                exportador.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
                exportador.Export();

                string fic = PathListado;
                string texto;

                Encoding enc = GetFileEncoding(fic);
                System.IO.StreamReader sr = new System.IO.StreamReader(fic, enc);
                texto = sr.ReadToEnd();

                System.IO.StreamWriter sw = new System.IO.StreamWriter(PathListado2);
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                sw.Write(texto);
                sw.Close();
                sr.Close();

                imp = new XmlImplementation();
                doc = imp.CreateDocument();
                doc.Load(PathListado2);
                MOFFIS.GLInformationsss accttype = new MOFFIS.GLInformationsss();

                reader = doc.GetElementsByTagName("PAW_Account");

                glAcctIDList = Array.CreateInstance(typeof(string), 4, reader.Count);

                int v = 0;
                string accountID;
                string accountDescription;
                string accountType;
                string accountGUID;
                string accountTW;
                string esInactivo;

                for (int i = 0; i <= reader.Count - 1; i++)
                {
                    accountID = "";
                    accountDescription = "";
                    accountType = "";
                    accountGUID = "";
                    accountTW = "";
                    esInactivo = "";

                    foreach (XmlNode node in reader[i].ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "ID":
                                {
                                    accountID = node.InnerText;
                                    break;
                                }
                            case "Description":
                                {
                                    accountDescription = node.InnerText;
                                    break;
                                }
                            case "Type":
                                {
                                    accountType = node.InnerText;
                                    break;
                                }
                            case "isInactive":
                                {
                                    esInactivo = node.InnerText;
                                    break;
                                }
                            case "GUID":
                                {
                                    accountGUID = node.InnerText;
                                    break;
                                }
                        }
                    }

                    if (esInactivo == "FALSE")
                    {
                        glAcctIDList.SetValue(accountID, 0, v);
                        glAcctIDList.SetValue(accountDescription, 1, v);
                        glAcctIDList.SetValue(accountType, 2, v);
                        if (accttype.getAcctTypeWords(Convert.ToInt32(accountType)) == "Accounts Receivable")
                        {
                            this.ARAccount.Items.Add(accountID + "_" + accountDescription + "_" + accountType);
                            //this.ARAccount.Items.Add(accountID);
                        }
                        glAcctIDList.SetValue(accountGUID, 3, v);

                        this.cbGlacctAnulacion.Items.Add(accountID + "_" + accountDescription + "_" + accountType);
                        this.cbGlacctDescuento.Items.Add(accountID + "_" + accountDescription + "_" + accountType);
                        this.cbGlacctEfectivo.Items.Add(accountID + "_" + accountDescription + "_" + accountType);
                        this.cbGlacctCheque.Items.Add(accountID + "_" + accountDescription + "_" + accountType);
                        this.cbGlacctTarjeta.Items.Add(accountID + "_" + accountDescription + "_" + accountType);

                        //this.cbGlacct.Items.Add(accountID);
                        v = v + 1;
                    }
                }

                exportador = null;
                imp = null;
                doc = null;
                reader = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmMantenimiento_Load(object sender, EventArgs e)
        {
            this.LeerValoresDefault();
            this.ObtenerListadoSalesTaxes();
            this.LeerImpuestos();
            this.LeerDescuentos();
        }  

        private void btnGenerarReporteX_Click(object sender, EventArgs e)
        {
            handler = frmPrincipal.handlerM;
            string respuesta;
            string[] CadResp;
            string[] status;
            string mensaje, mensaje1, mensaje2, SImp, SFis;
            string comando = "9" + FS + "X" + FS + "X";

            HASAR.LimpiarDoc();
            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
            //mensaje = HASAR.MandaPaqueteFiscal(handler, "9∟X∟X").ToString();
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
            string comando = "9" +FS+ "Z" +FS+ "S";

            HASAR.LimpiarDoc();
            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
            //mensaje = HASAR.MandaPaqueteFiscal(handler, "9∟Z∟S").ToString();

            /*
            comando = "E" + FS + "T";
            HASAR.LimpiarDoc();
            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
            */

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
                    MessageBox.Show("Reducción Z impresa correctamente", "Reducción Z", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
            else
            {
                MessageBox.Show("Error en generación de reducción Z", "Error en reducción Z", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Mantenimiento Impuestos
        private void ObtenerListadoSalesTaxes()
        {
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesTaxCodes);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjSalesTaxCodesField.peachwIEObjSalesTaxCodesField_SalesTaxCodeId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesTaxCodesField.peachwIEObjSalesTaxCodesField_SalesTaxCodeDescription);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();

            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ListadoSalesTaxes.xml";

            exportador.SetFilename(PathListado);
            exportador.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
            exportador.Export();

            imp = new XmlImplementation();
            doc = imp.CreateDocument();
            doc.Load(PathListado);
            reader = doc.GetElementsByTagName("PAW_Sales_Tax_Code");
            taxesList = Array.CreateInstance(typeof(string), 2, reader.Count);
            for (int i = 0; i <= reader.Count - 1; i++)
            {
                foreach (XmlNode node in reader[i].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "ID":
                            {
                                taxesList.SetValue(node.InnerText, 0, i);
                                break;
                            }
                        case "Description":
                            {
                                taxesList.SetValue(node.InnerText, 1, i);
                                break;
                            }
                    }
                }
            }

            for (int i = 0; i <= taxesList.GetUpperBound(1); i++)
            {
                this.cbSalesTaxes1.Items.Add(taxesList.GetValue(0, i));// + "_" + taxesList.GetValue(1, i));   
            }
            exportador = null;
            imp = null;
            doc = null;
            reader = null;
        }

        private void ObtenerListadoSalesTaxesA(string SaleTaxID)
        {
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesTaxCodes);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjSalesTaxCodesField.peachwIEObjSalesTaxCodesField_AuthId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesTaxCodesField.peachwIEObjSalesTaxCodesField_AuthDescription);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesTaxCodesField.peachwIEObjSalesTaxCodesField_TaxRate);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesTaxCodesField.peachwIEObjSalesTaxCodesField_AccountId);

            exportador.SetFilterValue((short)PeachwIEObjSalesTaxCodesFilter.peachwIEObjSalesTaxCodesFilter_SalesTaxCodeId, PeachwIEFilterOperation.peachwIEFilterOperationRange, SaleTaxID, SaleTaxID);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ListadoSalesTaxesA.xml";

            exportador.SetFilename(PathListado);
            exportador.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
            exportador.Export();

            imp = new XmlImplementation();
            doc = imp.CreateDocument();
            doc.Load(PathListado);
            reader = doc.GetElementsByTagName("Authority");
            taxesAList = Array.CreateInstance(typeof(string), 4, reader.Count);
            for (int i = 0; i <= reader.Count - 1; i++)
            {
                foreach (XmlNode node in reader[i].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "ID":
                            {
                                taxesAList.SetValue(node.InnerText, 0, i);
                                break;
                            }
                        case "Description":
                            {
                                taxesAList.SetValue(node.InnerText, 1, i);
                                break;
                            }
                        case "Rate":
                            {
                                taxesAList.SetValue(node.InnerText, 2, i);
                                break;
                            }
                        case "AccountID":
                            {
                                taxesAList.SetValue(node.InnerText, 3, i);
                                break;
                            }
                    }
                }
            }

            for (int i = 0; i <= taxesAList.GetUpperBound(1); i++)
            {
                    this.cbSalesTaxesA1.Items.Add(taxesAList.GetValue(0, i));// + "_" + taxesList.GetValue(1, i));             
            }
            exportador = null;
            imp = null;
            doc = null;
            reader = null;
        }

        private void cbSalesTaxes1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LimpiarDatosImpuesto(1);
            string salesTaxID = this.cbSalesTaxes1.Text;
            this.ObtenerListadoSalesTaxesA(salesTaxID);
        }

        private void cbSalesTaxesA1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LimpiarDatosImpuesto(2);
            string salesTaxA = this.cbSalesTaxesA1.Text;
            this.CompletarDatosImpuesto(salesTaxA);
        }

        private void CompletarDatosImpuesto(string salesTaxA)
        {
            this.LimpiarDatosImpuesto(2);
            for (int i = 0; i <= taxesAList.GetUpperBound(0) - 1; i++)
            {
                if (taxesAList.GetValue(0, i).ToString() == salesTaxA)
                {
                    if (taxesAList.GetValue(1, i) != null)
                        this.txtSaleTaxName1.Text = taxesAList.GetValue(1, i).ToString();
                    if (taxesAList.GetValue(2, i) != null)
                        this.txtPorcImpuesto1.Text = taxesAList.GetValue(2, i).ToString();
                    if (taxesAList.GetValue(3, i) != null)
                        this.txtAccountSTA1.Text = taxesAList.GetValue(3, i).ToString();

                    break;
                }
            }
        }

        private void LimpiarDatosImpuesto(int Impuesto)
        {
            if (Impuesto == 1)
            {
                this.cbSalesTaxesA1.Items.Clear();
                this.cbSalesTaxesA1.Text = "";
                this.txtSaleTaxName1.Text = "";
                this.txtPorcImpuesto1.Text = "";
                this.txtAccountSTA1.Text = "";
                this.cbTaxType1.Text = "";
            }
            if (Impuesto == 2)
            {
                this.txtSaleTaxName1.Text = "";                    
                this.txtPorcImpuesto1.Text="";
                this.txtAccountSTA1.Text = "";
                this.cbTaxType1.Text = "";
            }
        }

        private void btnCrearImpuestos_Click(object sender, EventArgs e)
        {
            try
            {
                this.CrearXML_Impuestos();
                MessageBox.Show("Los impuestos fueron creados correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear los impuestos");
            }
        }

        private void CrearXML_Impuestos()
        {
            string salesTax = this.cbSalesTaxes1.Text;

            string salesTaxA = this.cbSalesTaxesA1.Text;

            string salesTaxName1 = this.txtSaleTaxName1.Text;
            string salesTaxName2 = "Freight Amount";

            string porcentaje1 = this.txtPorcImpuesto1.Text;
            string porcentaje2 = this.txtPorcImpuesto2.Text;
            string porcentaje3 = this.txtPorcImpuesto3.Text;

            string accountIdST1 = this.txtAccountSTA1.Text;

            string taxType1 = this.cbTaxType1.Text;
            string taxType2 = this.cbTaxType2.Text;
            string taxType3 = this.cbTaxType3.Text;

            string Habilitado1;
            string Habilitado2;
            string Habilitado3;

            if (this.cbxUsaImpuesto1.Checked)
            {
                Habilitado1 = "Habilitado";
            }
            else
            {
                Habilitado1 = "Deshabilitado";
            }

            if (this.cbxUsaImpuesto2.Checked)
            {
                Habilitado2 = "Habilitado";
            }
            else
            {
                Habilitado2 = "Deshabilitado";
            }

            if (this.cbxUsaImpuesto3.Checked)
            {
                Habilitado3 = "Habilitado";
            }
            else
            {
                Habilitado3 = "Deshabilitado";
            }

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales" + Id_compañia + ".xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_Impuestos");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_Impuesto");
            Writer.WriteElementString("SalesTax", salesTax);
            Writer.WriteElementString("SalesTaxA", salesTaxA);
            Writer.WriteElementString("SalesTaxName1", salesTaxName1);
            Writer.WriteElementString("Porcentaje1", porcentaje1);
            Writer.WriteElementString("Account1", accountIdST1);
            Writer.WriteElementString("Item_Tax_Type1", taxType1);
            Writer.WriteElementString("Habilitado1", Habilitado1);

            Writer.WriteElementString("SalesTaxName2", salesTaxName2);
            Writer.WriteElementString("Porcentaje2", porcentaje2);
            Writer.WriteElementString("Item_Tax_Type2", taxType2);
            Writer.WriteElementString("Habilitado2", Habilitado2);

            Writer.WriteElementString("SalesTaxName3", salesTaxName2);
            Writer.WriteElementString("Porcentaje3", porcentaje3);
            Writer.WriteElementString("Item_Tax_Type3", taxType3);
            Writer.WriteElementString("Habilitado3", Habilitado3);

            Writer.WriteEndElement();//("PAW_Impuesto")

            Writer.WriteEndElement();//("PAW_Impuestos")

            Writer.Close();
        }

        private void LeerImpuestos()
        {
            string SalesTax = "";
            string SalesTaxA = "";
            string SalesTaxName1 = "";
            string Porcentaje1 = "";
            string Account1 = "";
            string Item_Tax_Type1 = "";
            string Habilitado1 = "";
            string SalesTaxName2 = "";
            string Porcentaje2 = "";
            string Item_Tax_Type2 = "";
            string Habilitado2 = "";
            string SalesTaxName3 = "";
            string Porcentaje3 = "";
            string Item_Tax_Type3 = "";
            string Habilitado3 = "";

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales" + Id_compañia + ".xml";
            //string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales.xml";
            if (System.IO.File.Exists(PathListado))
            {
                int impuestoSel = 0;
                imp = new XmlImplementation();
                doc = imp.CreateDocument();
                doc.Load(PathListado);

                reader = doc.GetElementsByTagName("PAW_Impuesto");
                impuestosList = Array.CreateInstance(typeof(string), 7, reader.Count);

                for (int i = 0; i <= reader.Count - 1; i++)
                {
                    foreach (XmlNode node in reader[i].ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "SalesTax":
                                {
                                    SalesTax = node.InnerText;
                                    break;
                                }
                            case "SalesTaxA":
                                {
                                    SalesTaxA = node.InnerText;
                                    break;
                                }
                            case "SalesTaxName1":
                                {
                                    SalesTaxName1 = node.InnerText;
                                    break;
                                }
                            case "Porcentaje1":
                                {
                                    Porcentaje1 = node.InnerText;
                                    break;
                                }
                            case "Account1":
                                {
                                    Account1 = node.InnerText;
                                    break;
                                }
                            case "Item_Tax_Type1":
                                {
                                    Item_Tax_Type1 = node.InnerText;
                                    break;
                                }
                            case "Habilitado1":
                                {
                                    Habilitado1 = node.InnerText;
                                    break;
                                }
                            case "SalesTaxName2":
                                {
                                    SalesTaxName2 = node.InnerText;
                                    break;
                                }
                            case "Porcentaje2":
                                {
                                    Porcentaje2 = node.InnerText;
                                    break;
                                }
                            case "Item_Tax_Type2":
                                {
                                    Item_Tax_Type2 = node.InnerText;
                                    break;
                                }
                            case "Habilitado2":
                                {
                                    Habilitado2 = node.InnerText;
                                    break;
                                }
                            case "SalesTaxName3":
                                {
                                    SalesTaxName3 = node.InnerText;
                                    break;
                                }
                            case "Porcentaje3":
                                {
                                    Porcentaje3 = node.InnerText;
                                    break;
                                }
                            case "Item_Tax_Type3":
                                {
                                    Item_Tax_Type3 = node.InnerText;
                                    break;
                                }
                            case "Habilitado3":
                                {
                                    Habilitado3 = node.InnerText;
                                    break;
                                }
                        }
                    }
                }

                if (reader.Count > 0)
                {
                    this.cbSalesTaxes1.Text = SalesTax;
                    this.LimpiarDatosImpuesto(1);
                    string salesTaxID = this.cbSalesTaxes1.Text;
                    this.ObtenerListadoSalesTaxesA(salesTaxID);

                    this.cbSalesTaxesA1.Text = SalesTaxA;
                    this.txtSaleTaxName1.Text = SalesTaxName1;
                    this.txtPorcImpuesto1.Text = Porcentaje1;
                    this.txtAccountSTA1.Text = Account1;
                    this.cbTaxType1.Text = Item_Tax_Type1;
                    if (Habilitado1 == "Habilitado")
                    {
                        this.cbxUsaImpuesto1.Checked = true;
                    }
                    else
                    {
                        this.cbxUsaImpuesto1.Checked = false;
                    }

                    if (Habilitado2 == "Habilitado")
                    {
                        this.cbxUsaImpuesto2.Checked = true;
                    }
                    else
                    {
                        this.cbxUsaImpuesto2.Checked = false;
                    }
                    this.cbTaxType2.Text = Item_Tax_Type2;

                    if (Habilitado3 == "Habilitado")
                    {
                        this.cbxUsaImpuesto3.Checked = true;
                    }
                    else
                    {
                        this.cbxUsaImpuesto3.Checked = false;
                    }
                    this.cbTaxType3.Text = Item_Tax_Type3;
                }

                imp = null;
                doc = null;
                reader = null;
            }

        }
        #endregion

        #region Mantenimiento Descuentos
        private void btnCrearDescuentos_Click(object sender, EventArgs e)
        {
            try
            {
                this.CrearXML_Descuentos();
                MessageBox.Show("Los descuentos fueron creados correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear los descuentos");
            }
        }

        private void CrearXML_Descuentos()
        {
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos" + Id_compañia + ".xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_Descuentos");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            if (cbx1.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento1");
                Writer.WriteElementString("Monto", "1");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx2.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento2");
                Writer.WriteElementString("Monto", "2");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx3.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento3");
                Writer.WriteElementString("Monto", "3");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx4.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento4");
                Writer.WriteElementString("Monto", "4");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx5.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento5");
                Writer.WriteElementString("Monto", "5");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx6.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento6");
                Writer.WriteElementString("Monto", "6");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx7.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento7");
                Writer.WriteElementString("Monto", "7");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx8.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento8");
                Writer.WriteElementString("Monto", "8");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx9.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento9");
                Writer.WriteElementString("Monto", "9");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx10.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento10");
                Writer.WriteElementString("Monto", "10");
                Writer.WriteEndElement();//("PAW_Descuento")
            }

            if (cbx11.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento11");
                Writer.WriteElementString("Monto", "11");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx12.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento12");
                Writer.WriteElementString("Monto", "12");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx13.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento13");
                Writer.WriteElementString("Monto", "13");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx14.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento14");
                Writer.WriteElementString("Monto", "14");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx15.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento15");
                Writer.WriteElementString("Monto", "15");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx16.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento16");
                Writer.WriteElementString("Monto", "16");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx17.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento17");
                Writer.WriteElementString("Monto", "17");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx18.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento18");
                Writer.WriteElementString("Monto", "18");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx19.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento19");
                Writer.WriteElementString("Monto", "19");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx20.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento20");
                Writer.WriteElementString("Monto", "20");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx21.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento21");
                Writer.WriteElementString("Monto", "21");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx22.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento22");
                Writer.WriteElementString("Monto", "22");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx23.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento23");
                Writer.WriteElementString("Monto", "23");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx24.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento24");
                Writer.WriteElementString("Monto", "24");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx25.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento25");
                Writer.WriteElementString("Monto", "25");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx26.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento26");
                Writer.WriteElementString("Monto", "26");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx27.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento27");
                Writer.WriteElementString("Monto", "27");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx28.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento28");
                Writer.WriteElementString("Monto", "28");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx29.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento29");
                Writer.WriteElementString("Monto", "29");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx30.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento30");
                Writer.WriteElementString("Monto", "30");
                Writer.WriteEndElement();//("PAW_Descuento")
            }

            if (cbx31.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento31");
                Writer.WriteElementString("Monto", "31");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx32.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento32");
                Writer.WriteElementString("Monto", "32");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx33.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento33");
                Writer.WriteElementString("Monto", "33");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx34.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento34");
                Writer.WriteElementString("Monto", "34");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx35.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento35");
                Writer.WriteElementString("Monto", "35");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx36.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento36");
                Writer.WriteElementString("Monto", "36");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx37.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento37");
                Writer.WriteElementString("Monto", "37");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx38.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento38");
                Writer.WriteElementString("Monto", "38");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx39.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento39");
                Writer.WriteElementString("Monto", "39");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx40.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento40");
                Writer.WriteElementString("Monto", "40");
                Writer.WriteEndElement();//("PAW_Descuento")
            }

            if (cbx41.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento41");
                Writer.WriteElementString("Monto", "41");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx42.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento42");
                Writer.WriteElementString("Monto", "42");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx43.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento43");
                Writer.WriteElementString("Monto", "43");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx44.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento44");
                Writer.WriteElementString("Monto", "44");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx45.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento45");
                Writer.WriteElementString("Monto", "45");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx46.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento46");
                Writer.WriteElementString("Monto", "46");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx47.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento47");
                Writer.WriteElementString("Monto", "47");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx48.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento48");
                Writer.WriteElementString("Monto", "48");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx49.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento49");
                Writer.WriteElementString("Monto", "49");
                Writer.WriteEndElement();//("PAW_Descuento")
            }
            if (cbx50.Checked)
            {
                Writer.WriteStartElement("PAW_Descuento");
                Writer.WriteElementString("DescuentoID", "Descuento50");
                Writer.WriteElementString("Monto", "50");
                Writer.WriteEndElement();//("PAW_Descuento")
            }

            Writer.WriteEndElement();//("PAW_Descuentos")

            Writer.Close();
        }

        private void LeerDescuentos()
        {
            try
            {
                string DescuentoID = "";
                imp = new XmlImplementation();
                doc = imp.CreateDocument();

                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos" + Id_compañia + ".xml";
                //string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos.xml";

                if (System.IO.File.Exists(PathListado))
                {

                    doc.Load(PathListado);

                    reader = doc.GetElementsByTagName("PAW_Descuento");

                    for (int i = 0; i <= reader.Count - 1; i++)
                    {
                        foreach (XmlNode node in reader[i].ChildNodes)
                        {
                            switch (node.Name)
                            {
                                case "DescuentoID":
                                    {
                                        DescuentoID = node.InnerText;
                                        if (DescuentoID == "Descuento1")
                                            this.cbx1.Checked = true;
                                        if (DescuentoID == "Descuento2")
                                            this.cbx2.Checked = true;
                                        if (DescuentoID == "Descuento3")
                                            this.cbx3.Checked = true;
                                        if (DescuentoID == "Descuento4")
                                            this.cbx4.Checked = true;
                                        if (DescuentoID == "Descuento5")
                                            this.cbx5.Checked = true;
                                        if (DescuentoID == "Descuento6")
                                            this.cbx6.Checked = true;
                                        if (DescuentoID == "Descuento7")
                                            this.cbx7.Checked = true;
                                        if (DescuentoID == "Descuento8")
                                            this.cbx8.Checked = true;
                                        if (DescuentoID == "Descuento9")
                                            this.cbx9.Checked = true;
                                        if (DescuentoID == "Descuento10")
                                            this.cbx10.Checked = true;
                                        if (DescuentoID == "Descuento11")
                                            this.cbx10.Checked = true;
                                        if (DescuentoID == "Descuento12")
                                            this.cbx12.Checked = true;
                                        if (DescuentoID == "Descuento13")
                                            this.cbx13.Checked = true;
                                        if (DescuentoID == "Descuento14")
                                            this.cbx14.Checked = true;
                                        if (DescuentoID == "Descuento15")
                                            this.cbx15.Checked = true;
                                        if (DescuentoID == "Descuento16")
                                            this.cbx16.Checked = true;
                                        if (DescuentoID == "Descuento17")
                                            this.cbx17.Checked = true;
                                        if (DescuentoID == "Descuento18")
                                            this.cbx18.Checked = true;
                                        if (DescuentoID == "Descuento19")
                                            this.cbx19.Checked = true;
                                        if (DescuentoID == "Descuento20")
                                            this.cbx20.Checked = true;
                                        if (DescuentoID == "Descuento21")
                                            this.cbx21.Checked = true;
                                        if (DescuentoID == "Descuento22")
                                            this.cbx22.Checked = true;
                                        if (DescuentoID == "Descuento23")
                                            this.cbx23.Checked = true;
                                        if (DescuentoID == "Descuento24")
                                            this.cbx24.Checked = true;
                                        if (DescuentoID == "Descuento25")
                                            this.cbx25.Checked = true;
                                        if (DescuentoID == "Descuento26")
                                            this.cbx26.Checked = true;
                                        if (DescuentoID == "Descuento27")
                                            this.cbx27.Checked = true;
                                        if (DescuentoID == "Descuento28")
                                            this.cbx28.Checked = true;
                                        if (DescuentoID == "Descuento29")
                                            this.cbx29.Checked = true;
                                        if (DescuentoID == "Descuento30")
                                            this.cbx30.Checked = true;
                                        if (DescuentoID == "Descuento31")
                                            this.cbx31.Checked = true;
                                        if (DescuentoID == "Descuento32")
                                            this.cbx32.Checked = true;
                                        if (DescuentoID == "Descuento33")
                                            this.cbx33.Checked = true;
                                        if (DescuentoID == "Descuento34")
                                            this.cbx34.Checked = true;
                                        if (DescuentoID == "Descuento35")
                                            this.cbx35.Checked = true;
                                        if (DescuentoID == "Descuento36")
                                            this.cbx36.Checked = true;
                                        if (DescuentoID == "Descuento37")
                                            this.cbx37.Checked = true;
                                        if (DescuentoID == "Descuento38")
                                            this.cbx38.Checked = true;
                                        if (DescuentoID == "Descuento39")
                                            this.cbx39.Checked = true;
                                        if (DescuentoID == "Descuento40")
                                            this.cbx40.Checked = true;
                                        if (DescuentoID == "Descuento41")
                                            this.cbx41.Checked = true;
                                        if (DescuentoID == "Descuento42")
                                            this.cbx42.Checked = true;
                                        if (DescuentoID == "Descuento43")
                                            this.cbx43.Checked = true;
                                        if (DescuentoID == "Descuento44")
                                            this.cbx44.Checked = true;
                                        if (DescuentoID == "Descuento45")
                                            this.cbx45.Checked = true;
                                        if (DescuentoID == "Descuento46")
                                            this.cbx46.Checked = true;
                                        if (DescuentoID == "Descuento47")
                                            this.cbx47.Checked = true;
                                        if (DescuentoID == "Descuento48")
                                            this.cbx48.Checked = true;
                                        if (DescuentoID == "Descuento49")
                                            this.cbx49.Checked = true;
                                        if (DescuentoID == "Descuento50")
                                            this.cbx50.Checked = true;



                                        break;
                                    }
                            }
                        }
                    }

                    imp = null;
                    doc = null;
                    reader = null;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        private void btnValoresDefault_Click(object sender, EventArgs e)
        {
            try
            {
                this.CrearXML_ValoresDefault();
                MessageBox.Show("Los valores default fueron creados exitosamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear los valores default");
            }
        }

        private void CrearXML_ValoresDefault()
        {
            string temp = "";
            string[] ss;

            string cuentaAnulacion;
            string cuentaDescuento;
            string cuentaAR;
            string cuentaEfectivo;
            string cuentaCheque;
            string cuentaTarjeta;
            string modificarPrecios;
            string decimales = "";
            string CodigoProducto = "";
            string CambiosEspeciales = "";

            temp = this.cbGlacctAnulacion.Text;
            ss = temp.Split('_');
            cuentaAnulacion = ss[0];

            temp = this.cbGlacctDescuento.Text;
            ss = temp.Split('_');
            cuentaDescuento = ss[0];

            temp = this.ARAccount.Text;
            ss = temp.Split('_');
            cuentaAR = ss[0];

            temp = this.cbGlacctEfectivo.Text;
            ss = temp.Split('_');
            cuentaEfectivo = ss[0];

            temp = this.cbGlacctCheque.Text;
            ss = temp.Split('_');
            cuentaCheque = ss[0];

            temp = this.cbGlacctTarjeta.Text;
            ss = temp.Split('_');
            cuentaTarjeta = ss[0];

            if (this.cbxModificarPrecios.Checked)
            {
                modificarPrecios = "SI";
            }
            else
            {
                modificarPrecios = "NO";
            }


            if (this.rb2decimales.Checked)
            {
                decimales = "2";
            }
            else if (this.rb4decimales.Checked)
            {
                decimales = "4";
            }


            if (this.ckbEspecial.Checked)
            {
                CambiosEspeciales = "SI";
            }
            else
            {
                CambiosEspeciales = "NO";
            }


            if (this.rbCodigoC .Checked)
            {
                CodigoProducto = "C";
            }
            else if (this.rbCodigoD .Checked)
            {
                CodigoProducto = "D";
            }




            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ValoresDefault" + Id_compañia + ".xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_ValoresDefault");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_ValorDeafult");
            Writer.WriteElementString("CuentaAnulacion", cuentaAnulacion);
            Writer.WriteElementString("CuentaDescuento", cuentaDescuento);
            Writer.WriteElementString("CuentaAR", cuentaAR);
            Writer.WriteElementString("CuentaEfectivo", cuentaEfectivo);
            Writer.WriteElementString("CuentaCheque", cuentaCheque);
            Writer.WriteElementString("CuentaTarjeta", cuentaTarjeta);
            Writer.WriteElementString("ModificarPrecios", modificarPrecios);
            Writer.WriteElementString("Decimales", decimales);
            Writer.WriteElementString("Pie", txtPie.Text);
            Writer.WriteElementString("CodigoProducto", CodigoProducto);
            Writer.WriteElementString("CambiosEspeciales", CambiosEspeciales);

            Writer.WriteEndElement();//("PAW_ValorDeafult")
            Writer.WriteEndElement();//("PAW_ValoresDefault")
            Writer.Close();
        }

        private void LeerValoresDefault()
        {
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ValoresDefault" + Id_compañia + ".xml";

            if (System.IO.File.Exists(PathListado))
            {
                imp = new XmlImplementation();
                doc = imp.CreateDocument();
                doc.Load(PathListado);

                reader = doc.GetElementsByTagName("PAW_ValorDeafult");
                for (int i = 0; i <= reader.Count - 1; i++)
                {
                    for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                    {
                        switch (reader[i].ChildNodes[a].Name)
                        {
                            case "CuentaAnulacion":
                                {
                                    CuentaAnulacion = reader[i].ChildNodes[a].InnerText;
                                    this.BuscarCuenta("A");
                                    break;
                                }
                            case "CuentaDescuento":
                                {
                                    CuentaDescuento = reader[i].ChildNodes[a].InnerText;
                                    this.BuscarCuenta("D");
                                    break;
                                }
                            case "CuentaAR":
                                {
                                    CuentaAR = reader[i].ChildNodes[a].InnerText;
                                    this.BuscarCuenta("AR");
                                    break;
                                }
                            case "CuentaEfectivo":
                                {
                                    CuentaEfectivo = reader[i].ChildNodes[a].InnerText;
                                    this.BuscarCuenta("E");
                                    break;
                                }
                            case "CuentaCheque":
                                {
                                    CuentaCheque = reader[i].ChildNodes[a].InnerText;
                                    this.BuscarCuenta("C");
                                    break;
                                }
                            case "CuentaTarjeta":
                                {
                                    CuentaTarjeta = reader[i].ChildNodes[a].InnerText;
                                    this.BuscarCuenta("T");
                                    break;
                                }
                            case "ModificarPrecios":
                                {
                                    ModificarPrecios = reader[i].ChildNodes[a].InnerText;
                                    if (ModificarPrecios == "SI")
                                    {
                                        this.cbxModificarPrecios.Checked = true;
                                    }
                                    else
                                    {
                                        this.cbxModificarPrecios.Checked = false;
                                    }
                                    break;
                                }
                            case "Decimales":
                                {
                                    Decimales = reader[i].ChildNodes[a].InnerText;
                                    if (Decimales == "2")
                                    {
                                        this.rb4decimales.Checked = false;
                                        this.rb2decimales.Checked = true;

                                    }
                                    else
                                    {
                                        this.rb2decimales.Checked = false;
                                        this.rb4decimales.Checked = true;
                                    }
                                    break;
                                }

                            case "CambiosEspeciales":
                                {
                                    CambiosEspeciales = reader[i].ChildNodes[a].InnerText;
                                    if (CambiosEspeciales == "SI")
                                    {
                                        this.ckbEspecial.Checked = true;
                                    }
                                    else
                                    {
                                        this.ckbEspecial.Checked = false;
                                    }
                                    break;
                                }
                            case "CodigoProducto":
                                {
                                    
                                    CodigoProducto = reader[i].ChildNodes[a].InnerText;
                                    if (CodigoProducto == "C")
                                    {
                                        this.rbCodigoC.Checked = true;
                                        this.rbCodigoD.Checked = false;

                                    }
                                    else
                                    {
                                        this.rbCodigoC.Checked = false;
                                        this.rbCodigoD.Checked = true;
                                    }
                                    break;
                                }

                            case "Pie":
                                {
                                    this.txtPie.Text = reader[i].ChildNodes[a].InnerText;
                                    //this.BuscarCuenta("T");
                                    break;
                                }
                        }
                    }
                }
                imp = null;
                doc = null;
                reader = null;
            }
        }

        private void BuscarCuenta(string tipo)
        {
            int i;
            string temp = "";
            string[] ss;
            string CuentaTemp;

            if (tipo == "A")
            {
                for (i = 0; i <= this.cbGlacctAnulacion.Items.Count - 1; ++i)
                {
                    temp = this.cbGlacctAnulacion.Items[i].ToString();
                    ss = temp.Split('_');
                    CuentaTemp = ss[0];
                    if (CuentaAnulacion == CuentaTemp)
                    {
                        this.cbGlacctAnulacion.Text = temp;
                        break;
                    }
                }
            }
            else
                if (tipo == "D")
                {
                    for (i = 0; i <= this.cbGlacctDescuento.Items.Count - 1; ++i)
                    {
                        temp = this.cbGlacctDescuento.Items[i].ToString();
                        ss = temp.Split('_');
                        CuentaTemp = ss[0];
                        if (CuentaDescuento == CuentaTemp)
                        {
                            this.cbGlacctDescuento.Text = temp;
                            break;
                        }
                    }
                }
                else
                    if (tipo == "AR")
                    {
                        for (i = 0; i <= this.ARAccount.Items.Count - 1; ++i)
                        {
                            temp = this.ARAccount.Items[i].ToString();
                            ss = temp.Split('_');
                            CuentaTemp = ss[0];
                            if (CuentaAR == CuentaTemp)
                            {
                                this.ARAccount.Text = temp;
                                break;
                            }
                        }
                    }
                    else
                        if (tipo == "E")
                        {
                            for (i = 0; i <= this.cbGlacctEfectivo.Items.Count - 1; ++i)
                            {
                                temp = this.cbGlacctEfectivo.Items[i].ToString();
                                ss = temp.Split('_');
                                CuentaTemp = ss[0];
                                if (CuentaEfectivo == CuentaTemp)
                                {
                                    this.cbGlacctEfectivo.Text = temp;
                                    break;
                                }
                            }
                        }
                        else
                            if (tipo == "C")
                            {
                                for (i = 0; i <= this.cbGlacctCheque.Items.Count - 1; ++i)
                                {
                                    temp = this.cbGlacctCheque.Items[i].ToString();
                                    ss = temp.Split('_');
                                    CuentaTemp = ss[0];
                                    if (CuentaCheque == CuentaTemp)
                                    {
                                        this.cbGlacctCheque.Text = temp;
                                        break;
                                    }
                                }
                            }
                            else
                                if (tipo == "T")
                                {
                                    for (i = 0; i <= this.cbGlacctTarjeta.Items.Count - 1; ++i)
                                    {
                                        temp = this.cbGlacctTarjeta.Items[i].ToString();
                                        ss = temp.Split('_');
                                        CuentaTemp = ss[0];
                                        if (CuentaTarjeta == CuentaTemp)
                                        {
                                            this.cbGlacctTarjeta.Text = temp;
                                            break;
                                        }
                                    }
                                } 
        }
    }
}
