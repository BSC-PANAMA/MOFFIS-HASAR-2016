using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Interop.PeachwServer;
using System.Text;
using TFHKADIR;
using System.Configuration;

namespace MOFFIS
{
    public partial class frmNotasCredito : Form
    {
        private Interop.PeachwServer.Export exportador;
        private Interop.PeachwServer.Import importer;
        private ConectarPT ptApp = new ConectarPT();

        //Variables de Impuestos
        private string impuestoITBMSTaxID;
        private string impuestoITBMS7TaxID;
        private string impuestoITBMS7TaxDescription;
        private string impuestoITBMS10TaxDescription;
        private string impuestoITBMS15TaxDescription;
        private string impuestoITBMS7TaxAccountId;
        private string impuestoITBMS10TaxAccountId;
        private string impuestoITBMS15TaxAccountId;
        private string impuestoITBMSTaxAccountIdGUID;
        private string impuestoITBMS7TaxRate;
        private string impuestoITBMS10TaxRate;
        private string impuestoITBMS15TaxRate;
        private string impuestoITBMS7TaxType;
        private string impuestoITBMS10TaxType;
        private string impuestoITBMS15TaxType;
        private string impuestoITBMS7Habilitado;
        private string impuestoITBMS10Habilitado;
        private string impuestoITBMS15Habilitado;
        private string PathMoffis;
        private string RegisterMachineNumber;
        private string PieDocumento;
        
        private Array custIDList;
        private Array salesRepList;
        private Array glAcctIDList;
        private Array itemIDList;        

        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;

        private string sCustomerIdFacturaSel;
        private string sNumeroFacturaSel;
        private string sFechaFacturaSel;
        private string sIdentificadorCOOSel;

        DataTable dtDetalleNotaCredito;

        string NumNCPrincipal;

        private int ControladorError;

        private string cEstatus;
        double cBalance;
        double cCreditLimit;

        private string CuentaAnulacion;
        private string CuentaAR;
        private string CuentaEfectivo;
        private string CuentaCheque;
        private string CuentaTarjeta;
        private string ModificarPrecios;


        private string CambioEspecial;
        private string CodigoProducto;

        double sumadorMonto = 0;
        double totalFactura = 0;
        int handler;
        char FS = Convert.ToChar(28);
        char etx = Convert.ToChar(3);
        char FS2 = Convert.ToChar(128);
        int init;
        private string FechaImp;

        private string HoraImp;

        string valorc = "";

        private static frmNotasCredito m_FrmNotasCredito;


        //nuevo multiempresa
        static public string IDcomp;


        private int NivelUsuario = 0;

        public string IDcompania
        {
            get { return frmNotasCredito.IDcomp; }
            set { frmNotasCredito.IDcomp = value; }
        }

        static public string PuertoImpresora;

        public string PuertoImp
        {
            get { return frmNotasCredito.PuertoImpresora; }
            set { frmNotasCredito.PuertoImpresora = value; }
        }

        public static frmNotasCredito DefInstance
        {
            get
            {
                if (m_FrmNotasCredito == null || m_FrmNotasCredito.IsDisposed)
                    m_FrmNotasCredito = new frmNotasCredito();
                return m_FrmNotasCredito;
            }
            set
            {
                m_FrmNotasCredito = value;
            }
        }

        public frmNotasCredito()
        {
            InitializeComponent();
            this.CrearDataTableNotaCredito();
            this.ObtenerListadoClientes();
            this.ObtenerListadoSalesRepresent();
            this.ObtenerListadoCuentasGL();
            this.ObtenerListadoItems();
            this.LeerImpuestos();
            this.ObtenerGUIDImpuestos();
            this.LeerValoresDefault();
            //this.ObtenerListadoFacturas();
        }

        public void CrearDataTableNotaCredito()
        {
            dtDetalleNotaCredito = new DataTable();
            dtDetalleNotaCredito.Columns.Add(new DataColumn("Items", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("Cantidad", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("UnidadMedida", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("Retorno", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("Descripcion", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("GLAccount", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("PrecioUnitario", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("Tax", System.Type.GetType("System.Int32")));
            dtDetalleNotaCredito.Columns.Add(new DataColumn("Monto", System.Type.GetType("System.String")));
            dtDetalleNotaCredito.AcceptChanges();
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

        private void ObtenerListadoClientes()
        {
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjCustomerList);
            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerId);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToAddressLine1);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToAddressLine2);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToCity);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToState);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToZip);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerName);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerField1);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerCreditLimit);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerCreditStatus);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBalance);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerInactive);


            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerPriceLevel);
            exportador.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerPriceLevelText);

            //exportador.SetFilterValue((short)PeachwIEObjCustomerListFilter.peachwIEObjCustomerListFilter_ActiveFlag, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "1", "1");
            //exportador.SetFilterValue((short)PeachwIEObjEmployeeListFilter.peachwIEObjEmployeeListFilter_EmployeeOrSalesRep, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "TRUE", "TRUE");

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\ListadoClientes.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaCredito\ListadoClientes2.xml";

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
            reader = doc.GetElementsByTagName("PAW_Customer");
            custIDList = Array.CreateInstance(typeof(string), 13, reader.Count + 1);

            //Agrega el registro blanco
            custIDList.SetValue("", 0, 0);
            custIDList.SetValue("", 1, 0);
            custIDList.SetValue("", 2, 0);
            custIDList.SetValue("", 3, 0);
            custIDList.SetValue("", 4, 0);
            custIDList.SetValue("", 5, 0);
            custIDList.SetValue("", 6, 0);
            custIDList.SetValue("", 7, 0);
            custIDList.SetValue("", 8, 0);
            custIDList.SetValue("", 9, 0);
            custIDList.SetValue("", 10, 0);
            custIDList.SetValue("", 11, 0);
            custIDList.SetValue("", 12, 0);

            this.cbClientes.Items.Add("");

            int v = 0;
            string customerID;
            string customerName;
            string customerLine1;
            string customerLine2;
            string customerCity;
            string customerState;
            string customerZip;
            string customerCreditLimit;
            string customerRUC;
            string customerBalance;
            string customerGUID;
            string customerCreditStatus;
            string esInactivo;
            string Pricing_Level;

            for (int i = 0; i <= reader.Count - 1; i++)
            {
                customerID = "";
                customerName = "";
                customerLine1 = "";
                customerLine2 = "";
                customerCity = "";
                customerState = "";
                customerZip = "";
                customerCreditLimit = "";
                customerRUC = "";
                customerBalance = "";
                customerGUID = "";
                customerCreditStatus = "";
                esInactivo = "";
                Pricing_Level = "";

                for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                {
                    switch (reader[i].ChildNodes[a].Name)
                    {
                        case "ID":
                            {
                                customerID = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "Name":
                            {
                                customerName = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "isInactive":
                            {
                                esInactivo = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "BillToAddress":
                            {
                                for (int b = 0; b <= reader[i].ChildNodes[a].ChildNodes.Count - 1; b++)
                                {
                                    switch (reader[i].ChildNodes[a].ChildNodes[b].Name)
                                    {
                                        case "Line1":
                                            {
                                                customerLine1 = reader[i].ChildNodes[a].ChildNodes[b].InnerText;
                                                break;
                                            }
                                        case "Line2":
                                            {
                                                customerLine2 = reader[i].ChildNodes[a].ChildNodes[b].InnerText;
                                                break;
                                            }
                                        case "City":
                                            {
                                                customerCity = reader[i].ChildNodes[a].ChildNodes[b].InnerText;
                                                break;
                                            }
                                        case "State":
                                            {
                                                customerState = reader[i].ChildNodes[a].ChildNodes[b].InnerText;
                                                break;
                                            }
                                        case "Zip":
                                            {
                                                customerZip = reader[i].ChildNodes[a].ChildNodes[b].InnerText;
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                        case "Credit_Limit":
                            {
                                customerCreditLimit = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "CustomFields":
                            {
                                int evaluador = 0;
                                for (int b = 0; b <= reader[i].ChildNodes[a].ChildNodes[0].ChildNodes.Count - 1; b++)
                                {
                                    switch (reader[i].ChildNodes[a].ChildNodes[0].ChildNodes[b].Name)
                                    {
                                        case "Value":
                                            {
                                                customerRUC = reader[i].ChildNodes[a].ChildNodes[0].ChildNodes[b].InnerText;
                                                evaluador = 1;
                                                break;
                                            }
                                    }
                                }
                                if (evaluador.Equals(0))
                                {
                                    customerRUC = "Sin RUC Registrado";
                                }
                                break;
                            }
                        case "Customer_Balance":
                            {
                                customerBalance = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "GUID":
                            {
                                customerGUID = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "Credit_Status":
                            {
                                customerCreditStatus = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "Pricing_Level":
                            {
                                Pricing_Level = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                    }
                }

                if (esInactivo == "FALSE")
                {
                    custIDList.SetValue(customerID, 0, v + 1);
                    custIDList.SetValue(customerName, 1, v + 1);
                    custIDList.SetValue(customerLine1, 2, v + 1);
                    custIDList.SetValue(customerLine2, 3, v + 1);
                    custIDList.SetValue(customerCity, 4, v + 1);
                    custIDList.SetValue(customerState, 5, v + 1);
                    custIDList.SetValue(customerZip, 6, v + 1);
                    custIDList.SetValue(customerCreditLimit, 7, v + 1);
                    custIDList.SetValue(customerRUC, 8, v + 1);
                    custIDList.SetValue(customerBalance, 9, v + 1);
                    custIDList.SetValue(customerGUID, 10, v + 1);
                    custIDList.SetValue(customerCreditStatus, 11, v + 1);
                    custIDList.SetValue(Pricing_Level, 12, v + 1);

                    //this.cbClientes.Items.Add(customerID);
                    this.cbClientes.Items.Add(customerID + "_" + customerName);//****ALEX
                    v = v + 1;
                }
            }
            exportador = null;
            imp = null;
            doc = null;
            reader = null;
        }

        private void ObtenerListadoSalesRepresent()
        {
            string salesRep = "";
            string isSalesRep = "N";
            string salerename = "";
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjEmployeeList);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_EmployeeID);
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_EmployeeName);
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_SalesRep);
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_GUID);

            //exportador.SetFilterValue((short)PeachwIEObjEmployeeListFilter.peachwIEObjEmployeeListFilter_EmployeeOrSalesRep, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "TRUE", "TRUE");

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\ListadoSalesRepresent.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaCredito\ListadoSalesRepresent2.xml";

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
            reader = doc.GetElementsByTagName("PAW_Employee");
            salesRepList = Array.CreateInstance(typeof(string), 4, reader.Count + 1);
            salesRepList.SetValue("", 0, 0);
            salesRepList.SetValue("", 1, 0);
            salesRepList.SetValue("", 2, 0);
            salesRepList.SetValue("", 3, 0);

            this.cbSalesRepresent.Items.Add(""); 

            for (int i = 0; i <= reader.Count - 1; i++)
            {
                foreach (XmlNode node in reader[i].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "ID":
                            {
                                salesRepList.SetValue(node.InnerText, 0, i + 1);
                                salesRep = node.InnerText;
                                break;
                            }
                        case "Name":
                            {
                                salesRepList.SetValue(node.InnerText, 1, i + 1);
                                salerename = node.InnerText; 
                                break;
                            }
                        case "isSalesRep":
                            {
                                salesRepList.SetValue(node.InnerText, 2, i + 1);
                                isSalesRep = node.InnerText;
                                break;
                            }
                        case "EmployeeGUID":
                            {
                                salesRepList.SetValue(node.InnerText, 3, i + 1);
                                break;
                            }
                    }
                }
                if (isSalesRep == "TRUE")
                {
                    //this.cbSalesRepresent.Items.Add(salesRep);
                    this.cbSalesRepresent.Items.Add(salesRep + "_" + salerename); 
                }
            }

            exportador = null;
            imp = null;
            doc = null;
            reader = null;
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
                string PathListado = PathMoffis + @"\XML\NotaCredito\ListadoAccounts.xml";
                string PathListado2 = PathMoffis + @"\XML\NotaCredito\ListadoAccounts2.xml";

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
                                    //glAcctIDList.SetValue(node.InnerText, 0, i);
                                    //this.cbGlacct.Items.Add(node.InnerText);
                                    //if (accttype.getAcctTypeWords(Convert.ToInt32(reader[i].ChildNodes[2].InnerText)) == "Accounts Receivable")
                                    //    this.ARAccount.Items.Add(node.InnerText);
                                    break;
                                }
                            case "Description":
                                {
                                    accountDescription = node.InnerText;
                                    //glAcctIDList.SetValue(node.InnerText, 1, i);
                                    break;
                                }
                            case "Type":
                                {
                                    accountType = node.InnerText;
                                    //glAcctIDList.SetValue(node.InnerText, 2, i);
                                    break;
                                }
                            case "isInactive":
                                {
                                    esInactivo = node.InnerText;
                                    //glAcctIDList.SetValue(node.InnerText, 2, i);
                                    break;
                                }
                            case "GUID":
                                {
                                    accountGUID = node.InnerText;
                                    //glAcctIDList.SetValue(node.InnerText, 3, i);
                                    break;
                                }
                        }
                    }

                    if (esInactivo == "FALSE")
                    {
                        //glAcctIDList.SetValue(accountID, 0, v);
                        //glAcctIDList.SetValue(accountDescription, 1, v);
                        //glAcctIDList.SetValue(accountType, 2, v);
                        //if (accttype.getAcctTypeWords(Convert.ToInt32(accountType)) == "Accounts Receivable")
                        //{
                        //    this.ARAccount.Items.Add(accountID);
                        //}
                        //glAcctIDList.SetValue(accountGUID, 3, v);
                        //this.cbGlacct.Items.Add(accountID);
                        //v = v + 1;

                        glAcctIDList.SetValue(accountID, 0, v);
                        glAcctIDList.SetValue(accountDescription, 1, v);
                        glAcctIDList.SetValue(accountType, 2, v);
                        glAcctIDList.SetValue(accountGUID, 3, v);
                        this.ARAccount.Items.Add(accountID + "_" + accountDescription);
                        this.cbGlacct.Items.Add(accountID + "_" + accountDescription);
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

        private void ObtenerListadoItems()
        {
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjInventoryItemsList);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_ItemId);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_GUID);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_Inactive);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_ItemDescription);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_SalesAccountId);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_SalesAccountGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_SalesDesc);//**
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice1);

            /* OTROS UNIT PRICE */

            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice2);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice3);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice4);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice5);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice6);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice7);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice8);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice9);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice10);



            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_IsTaxable);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_SalesTaxType);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_TaxTypeName);
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitOfMeasure);//**

            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_InventoryAccountId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_InventoryAccountGUID);//**
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_InvChgAccountId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_InvChgAccountGUID);//**

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\ListadoItems.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaCredito\ListadoItems2.xml";

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

            string taxReal;
            string prueba;
            int v = 0;

            imp = new XmlImplementation();
            doc = imp.CreateDocument();
            doc.Load(PathListado2);
            reader = doc.GetElementsByTagName("PAW_Item");

            string itemID;
            string descripcionCorta;
            string descripcionLarga;
            string precioUnitario;
            string precioUnitario2;
            string precioUnitario3;
            string precioUnitario4;
            string precioUnitario5;
            string precioUnitario6;
            string precioUnitario7;
            string precioUnitario8;
            string precioUnitario9;
            string precioUnitario10;
            string taxType;
            string salesAccount;
            string salesAccountGUID;
            string unidadMedida;
            string esInactivo;

            int n;

            itemIDList = Array.CreateInstance(typeof(string), 17, reader.Count + 1);
            itemIDList.SetValue("", 0, 0);
            itemIDList.SetValue("", 1, 0);
            itemIDList.SetValue("", 2, 0);
            itemIDList.SetValue("", 3, 0);
            itemIDList.SetValue("", 4, 0);
            itemIDList.SetValue("", 5, 0);
            itemIDList.SetValue("", 6, 0);
            itemIDList.SetValue("", 7, 0);
            itemIDList.SetValue("", 8, 0);
            itemIDList.SetValue("", 9, 0);
            itemIDList.SetValue("", 10, 0);
            itemIDList.SetValue("", 11, 0);
            itemIDList.SetValue("", 12, 0);
            itemIDList.SetValue("", 13, 0);
            itemIDList.SetValue("", 14, 0);
            itemIDList.SetValue("", 15, 0);
            itemIDList.SetValue("", 16, 0);

            this.cbItems.Items.Add("");

            for (int i = 0; i <= reader.Count - 1; i++)
            {
                itemID = "";
                descripcionCorta = "";
                descripcionLarga = "";
                precioUnitario = "";
                precioUnitario2 = "";
                precioUnitario3 = "";
                precioUnitario4 = "";
                precioUnitario5 = "";
                precioUnitario6 = "";
                precioUnitario7 = "";
                precioUnitario8 = "";
                precioUnitario9 = "";
                precioUnitario10 = "";
                taxType = "";
                salesAccount = "";
                salesAccountGUID = "";
                unidadMedida = "";
                esInactivo = "";

                n = 0;
                foreach (XmlNode node in reader[i].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "ID":
                            {
                                itemID = node.InnerText;
                                break;
                            }
                        case "Description":
                            {
                                descripcionCorta = node.InnerText;
                                break;
                            }
                        case "isInactive":
                            {
                                esInactivo = node.InnerText;
                                break;
                            }
                        case "Description_for_Sales":
                            {
                                descripcionLarga = node.InnerText;
                                break;
                            }
                        case "Sales_Prices":
                            {
                                for (int b = 0; b <= reader[i].ChildNodes[n].ChildNodes.Count - 1; b++)
                                {
                                    if (b == 0)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 1)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario2 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 2)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario3 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 3)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario4 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 4)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario5 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 5)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario6 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 6)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario7 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 7)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario8 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 8)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario9 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                    else if (b == 9)
                                    {
                                        switch (reader[i].ChildNodes[n].ChildNodes[b].ChildNodes[0].Name)
                                        {
                                            case "Sales_Price":
                                                {

                                                    precioUnitario10 = reader[i].ChildNodes[n].ChildNodes[b].InnerText;
                                                    break;

                                                }
                                        }
                                    }
                                }
                                break;
                            }
                        case "Tax_Type":
                            {
                                taxType = node.InnerText;
                                break;
                            }
                        case "GL_Sales_Account":
                            {
                                salesAccount = node.InnerText;
                                break;
                            }
                        case "GL_Sales_Account_GUID":
                            {
                                salesAccountGUID = node.InnerText;
                                break;
                            }
                        case "Stocking_UM":
                            {
                                unidadMedida = node.InnerText;
                                break;
                            }
                        //case "Porcentaje2":
                        //    {
                        //        Porcentaje2 = node.InnerText;
                        //        break;
                        //    }
                    }
                    n = n + 1;
                }
                if (esInactivo == "FALSE")
                {
                    itemIDList.SetValue(itemID, 0, v + 1);//itemID
                    itemIDList.SetValue(descripcionCorta, 1, v + 1);//descripcion corta
                    itemIDList.SetValue(descripcionLarga, 2, v + 1);//descripcion larga
                    itemIDList.SetValue(precioUnitario, 3, v + 1);//precio unitario
                    taxReal = (Convert.ToInt32(taxType) + 1).ToString();//tax type
                    itemIDList.SetValue(taxReal, 4, v + 1);
                    itemIDList.SetValue(salesAccount, 5, v + 1);//sales account
                    itemIDList.SetValue(salesAccountGUID, 6, v + 1);//sales account GUID                    
                    itemIDList.SetValue(unidadMedida, 7, v + 1);//unidad medida


                    itemIDList.SetValue(precioUnitario2, 8, v + 1);
                    itemIDList.SetValue(precioUnitario3, 9, v + 1);
                    itemIDList.SetValue(precioUnitario4, 10, v + 1);
                    itemIDList.SetValue(precioUnitario5, 11, v + 1);
                    itemIDList.SetValue(precioUnitario6, 12, v + 1);
                    itemIDList.SetValue(precioUnitario7, 13, v + 1);
                    itemIDList.SetValue(precioUnitario8, 14, v + 1);
                    itemIDList.SetValue(precioUnitario9, 15, v + 1);
                    itemIDList.SetValue(precioUnitario10, 16, v + 1);


                    prueba = itemID + "_" + descripcionCorta;
                    //prueba = itemID;// +"_" + descripcionCorta;  MEDI OPTIC
                    this.cbItems.Items.Add(prueba);
                    v = v + 1;
                }
            }

            exportador = null;
            imp = null;
            doc = null;
            reader = null;
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

            imp = new XmlImplementation();
            doc = imp.CreateDocument();

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();

            string PathListado = "";

            //nuevo multicompania
            if (IDcomp == "1")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales1.xml";

            }
            else if (IDcomp == "2")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales2.xml";
            }
            else if (IDcomp == "3")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales3.xml";
            }


            
            //string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Impuestos\ImpuestosFinales.xml";
            //string PathListado2 = PathMoffis + @"\XML\NotaCredito\ListadoItems2.xml";

            doc.Load(PathListado);

            reader = doc.GetElementsByTagName("PAW_Impuesto");

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
                impuestoITBMSTaxID = SalesTax;
                impuestoITBMS7TaxID = SalesTaxA;
                impuestoITBMS7TaxDescription = SalesTaxName1;
                impuestoITBMS10TaxDescription = "Freight Amount";
                impuestoITBMS15TaxDescription = "Freight Amount";

                impuestoITBMS7TaxAccountId = Account1;
                impuestoITBMS10TaxAccountId = Account1;
                impuestoITBMS15TaxAccountId = Account1;

                impuestoITBMS7TaxRate = Porcentaje1;
                impuestoITBMS10TaxRate = Porcentaje2;
                impuestoITBMS15TaxRate = Porcentaje3;

                impuestoITBMS7TaxType = Item_Tax_Type1;
                impuestoITBMS10TaxType = Item_Tax_Type2;
                impuestoITBMS15TaxType = Item_Tax_Type3;

                impuestoITBMS7Habilitado = Habilitado1;
                impuestoITBMS10Habilitado = Habilitado2;
                impuestoITBMS15Habilitado = Habilitado3;

                this.lblPorcentaje1.Text = Porcentaje1;
                this.lblPorcentaje2.Text = Porcentaje2;
                this.lblPorcentaje3.Text = Porcentaje3;

                this.lblEstadoImp1.Text = Habilitado1;
                this.lblEstadoImp2.Text = Habilitado2;
                this.lblEstadoImp3.Text = Habilitado3;

                this.lblTaxTypeImp1.Text = Item_Tax_Type1;
                this.lblTaxTypeImp2.Text = Item_Tax_Type2;
                this.lblTaxTypeImp3.Text = Item_Tax_Type3;
            }

            imp = null;
            doc = null;
            reader = null;
        }

        private void ObtenerGUIDImpuestos()
        {
            string impuestoAccount = impuestoITBMS7TaxAccountId;
            for (int i = 0; i <= glAcctIDList.GetUpperBound(1); i++)
            {
                if (glAcctIDList.GetValue(0, i).ToString() == impuestoAccount)
                {
                    impuestoITBMSTaxAccountIdGUID = glAcctIDList.GetValue(3, i).ToString();
                    break;
                }
            }
        }

        private void ObtenerListadoFacturas()
        {
            DateTime fecha1 = this.dtp1.Value;//DateTime.Now.AddDays(-7);
            DateTime fecha2 = this.dtp2.Value;//DateTime.Now;
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesJournal);

            exportador.ClearExportFieldList();            
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DisplayedTerms);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);

            //exportador.SetDateFilterValue(peachwIEDateFilterOperationRange, getFirstOpenDay(), getLastOpenDay())
            //exportador.SetSortField((short)peachwIEObjSalesJournalSortBy_Date);          
            exportador.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange, fecha1, fecha2);
            exportador.SetSortField((short)PeachwIEObjSalesJournalSortBy.peachwIEObjSalesJournalSortBy_InvoiceNumber);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\ListadoFacturas.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaCredito\ListadoFacturas2.xml";

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

            reader = doc.GetElementsByTagName("PAW_Invoice");

            this.lvFacturas.Columns.Add("Factura", -2, HorizontalAlignment.Left);
            this.lvFacturas.Columns.Add("ID Cliente", -2, HorizontalAlignment.Left);
            this.lvFacturas.Columns.Add("Nombre Cliente", -2, HorizontalAlignment.Left);
            this.lvFacturas.Columns.Add("Fecha", -2, HorizontalAlignment.Left);
            this.lvFacturas.Columns.Add("Monto", -2, HorizontalAlignment.Left);
            this.lvFacturas.Columns.Add("Identificador", -2, HorizontalAlignment.Left);

            int contadorFacturas = 0;
            string customerID_Factura = "";
            string customerName_Factura = "";
            string number_Factura = "";
            string date_Factura = "";
            string monto_Factura = "";
            string coo_Factura = "";
            string esCOO = "";
            string esNotaDebito = "";
            string isCreditMemo = "";
            string identificadorImpresora = "";
            int agregarFacturaConCOO;
            int agregarNotaDebito;

            for (int i = 0; i <= reader.Count - 1; i++)
            {
                customerID_Factura = "";
                customerName_Factura = "";
                number_Factura = "";
                date_Factura = "";
                monto_Factura = "";
                coo_Factura = "";                
                isCreditMemo = "";

                esCOO = "";
                esNotaDebito = "";
                identificadorImpresora = "";
                agregarFacturaConCOO = 0;
                agregarNotaDebito = 0;

                foreach (XmlNode node in reader[i].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Customer_ID":
                            {
                                //this.lvFacturas.Items.Add(node.InnerText);
                                customerID_Factura = node.InnerText;
                                break;
                            }
                        case "Customer_Name":
                            {
                                //this.lvFacturas.Items[i].SubItems.Add(node.InnerText);
                                customerName_Factura = node.InnerText;
                                break;
                            }
                        case "Invoice_Number":
                            {
                                //this.lvFacturas.Items[i].SubItems.Add(node.InnerText);
                                number_Factura = node.InnerText;
                                break;
                            }
                        case "Date":
                            {
                                //this.lvFacturas.Items[i].SubItems.Add(node.InnerText);
                                date_Factura = node.InnerText;
                                break;
                            }
                        case "Displayed_Terms":
                            {
                                //this.lvFacturas.Items[i].SubItems.Add(node.InnerText);
                                coo_Factura = node.InnerText;
                                break;
                            }                            
                        case "CreditMemoType":                                
                            {
                                isCreditMemo = node.InnerText;
                                break;
                            }
                        case "SalesLines":
                            {
                                //    for (int b = 0; b <= reader[i].ChildNodes[0].ChildNodes.Count - 1; b++)
                                //    {
                                //        switch (reader[i].ChildNodes[0].ChildNodes[b].Name)
                                //            {
                                //                case "Amount":                                                    
                                //                    {
                                //                        monto_Factura = reader[i].ChildNodes[0].ChildNodes[b].InnerText;
                                //                        break;
                                //                    }
                                //            }
                                //    }
                                break;
                            }
                    }
                }

                if (number_Factura.Trim() != "")
                {
                    if (number_Factura.Length > 4)
                    {
                        esNotaDebito=number_Factura.Substring(0, 2);
                        if (esNotaDebito == "ND")
                        {
                            agregarNotaDebito = 1;
                        }
                    }

                    if(agregarNotaDebito.Equals(0))
                    {
                        if (isCreditMemo == "FALSE")
                        {
                            if (number_Factura.Length == 12)
                            {
                                //esCOO = coo_Factura.Substring(0, 3);
                                //if (esCOO == "COO")
                                //{
                                    agregarFacturaConCOO = 1;
                                    identificadorImpresora = number_Factura.Substring(4, 8);
                                //}
                            }

                            if (agregarFacturaConCOO == 1)
                            {
                                if (number_Factura.Trim() != "")
                                {
                                    this.lvFacturas.Items.Add(number_Factura);
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add(customerID_Factura);
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add(customerName_Factura);
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add(date_Factura);
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add(monto_Factura);
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add(coo_Factura);

                                    contadorFacturas = contadorFacturas + 1;
                                }
                            }
                        }
                    }

                }
            }

            exportador = null;
            imp = null;
            doc = null;
            reader = null;

            this.lvFacturas.View = View.Details;
            foreach (ColumnHeader col in lvFacturas.Columns)
            {
                col.Width = -2;
            }
        }

        private void frmNotasCredito_Load(object sender, EventArgs e)
        {
            this.txtNotaCreditoDate2.Text = DateTime.Now.ToString("MM/dd/yyyy");
            this.txtNotaCreditoDate.Text = this.ObtenerFechaHoraImpresora();
            this.dgvDetalleInvoice.DataSource = dtDetalleNotaCredito;
            this.LeerNumNotaCredito();
        }

        private void ObtenerNumRegistro()
        {
            try
            {
                RegisterMachineNumber = getRegisteredMachineNumber();
            }
            catch (Exception ex)
            {
                RegisterMachineNumber = "?????????????";
            }
        }

        private string getRegisteredMachineNumber()
        {
            try
            {
                string mensaje, SImp, SFis;
                string[] CadResp;
                string[] status;
                string respuesta;
                HASAR.LimpiarDoc();
                handler = frmPrincipal.handlerM;
                mensaje = HASAR.MandaPaqueteFiscal(handler, "s").ToString();
                if (mensaje == "0")
                {
                    respuesta = HASAR.LeerDoc();
                    CadResp = respuesta.Split(etx);
                    status = CadResp[0].Split(FS);
                    SImp = status[1];
                    SFis = status[2];

                    mensaje = HASAR.error_SF(SImp, 1);
                    if (mensaje != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje);
                    }

                    mensaje = HASAR.error_SF(SFis, 2);
                    if (mensaje != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje);
                    }

                    return status[8];
                }
                else
                {
                    return "BSC";
                }
            }
            catch (Exception ex)
            {
                return "BSC";
            }
        }

        private string ObtenerFechaHoraImpresora()
        {
            try
            {
                string FechaImpRetornar="", mensaje, SImp, SFis;
                string[] CadResp;
                string[] status;
                string respuesta;
                HASAR.LimpiarDoc();
                handler = frmPrincipal.handlerM;
                mensaje = HASAR.MandaPaqueteFiscal(handler, "Y").ToString();
                if (mensaje == "0")
                {
                    respuesta = HASAR.LeerDoc();
                    CadResp = respuesta.Split(etx);
                    status = CadResp[0].Split(FS);
                    SImp = status[1];
                    SFis = status[2];
                    FechaImp = status[3];
                    HoraImp = status[4];
                    mensaje = HASAR.error_SF(SImp, 1);
                    if (mensaje != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje);
                    }

                    mensaje = HASAR.error_SF(SFis, 2);
                    if (mensaje != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje);
                    }

                    string mesImpresora;
                    string diaImpresora;
                    string anioImpresora;
                    if (FechaImp.Length == 6)
                    {
                        mesImpresora = FechaImp.Substring(2, 2);
                        diaImpresora = FechaImp.Substring(4, 2);
                        anioImpresora = FechaImp.Substring(0, 2);
                        FechaImpRetornar = mesImpresora + "/" + diaImpresora + "/" + anioImpresora;
                    }

                    return FechaImpRetornar;
                }
                else
                {
                    return "Sin Fecha";
                }
            }
            catch (Exception ex)
            {
                return "Sin Fecha";
            }
        }

        private void lvFacturas_DoubleClick(object sender, EventArgs e)
        {
            this.ClearForm();
            this.panelProductos.Enabled = false;
            sNumeroFacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].Text;     
            sCustomerIdFacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[1].Text;
            sFechaFacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[3].Text;
            sIdentificadorCOOSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[5].Text;

            this.lblNumeroFactura.Text = sNumeroFacturaSel;
            this.lblIdentificadorCOO.Text = sIdentificadorCOOSel;
            this.ObtenerFactura();
            tcNC.SelectedIndex = 0;
        }

        private void ObtenerFactura()
        {
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesJournal);
            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToInvoiceDistNum);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToInvoiceNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ApplyToSalesOrder);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_BeginningBalanceTransaction);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_COSTAccountGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CostOfSalesAccountId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CostOfSalesAmount);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerPurchaseOrder);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DateClearedInAccountRec);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DateDue);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DiscountAmount);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DiscountDate);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DisplayedTerms);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DropShip);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enApplyToProposal);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enCOSAcntDateClearedInBankRec);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enGL_DateClearedInBankRec);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enInvAcntDateClearedInBankRec);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enProgressBillingInvoice);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enRetainagePercent);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enSerialNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enStockingQuantity);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMID);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnitPrice);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnits);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enVoidedBy);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_enUMStockingUnits);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_INVAccountGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InventoryAccountId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceDistNum);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNote);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNote2);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_JobGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_JobId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NotePrintsAfterLineItems);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
            //exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumFields);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quote);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_QuoteGoodThruDate);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_QuoteNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ReceiptNum);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ReturnAuthorization);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderDistNum);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepId);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesRepresentativeGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxAuthority);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxCode);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipByDate);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipDate);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCountry);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToName);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipVia);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_StatementNote);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_StatementNotePrintsBeforeInvoiceRef);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionGUID);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TransactionPeriod);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);//**
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UPCSKU);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Weight);

            //exportador.SetDateFilterValue((short)PeachwIEObjSalesJournalFilter. PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange, sFecha, sFecha);
            //exportador.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange, "03/01/07","03/31/07");

            exportador.SetFilterValue((short)PeachwIEObjSalesJournalFilter.peachwIEObjSalesJournalFilter_CustomerId, PeachwIEFilterOperation.peachwIEFilterOperationRange, sCustomerIdFacturaSel, sCustomerIdFacturaSel);
            exportador.SetFilterValue((short)PeachwIEObjSalesJournalFilter.peachwIEObjSalesJournalFilter_InvoiceNumber, PeachwIEFilterOperation.peachwIEFilterOperationRange, sNumeroFacturaSel, sNumeroFacturaSel);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\DetalleFactura.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaCredito\DetalleFactura2.xml";

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
            //MOFFIS.GLInformationsss accttype = new MOFFIS.GLInformationsss();

            reader = doc.GetElementsByTagName("PAW_Invoice");

            string sItemId = "";
            string sItemCantidad = "";
            string sItemUnidadM = "";
            string sItemDescripcion = "";
            string sItemGLAccount = "";
            string sItemPrecioU = "";
            string sItemTaxType = "";
            string sNumDist;
            string sAmountISC = "0";

            string SalesRepID = "";
            string ARAccountID = "";

                        
            for (int i = 0; i <= reader.Count - 1; i++)
            {
                for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                {
                    switch (reader[i].ChildNodes[a].Name)
                    {
                        case "Customer_ID":
                            {
                                //this.cbClientes.Text = reader[i].ChildNodes[a].InnerText;
                                //this.cbClientesChange();
                                //break;
                                string sClienteTemp = reader[i].ChildNodes[a].InnerText.Trim();
                                for (int iContItems = 0; iContItems < this.cbClientes.Items.Count; ++iContItems)
                                {
                                    string sCustomerCompleto = this.cbClientes.Items[iContItems].ToString();
                                    string[] sCustomerDesglosado = sCustomerCompleto.Split('_');
                                    string sCustomerID = sCustomerDesglosado[0];
                                    if (sClienteTemp == sCustomerID)
                                    {
                                        this.cbClientes.SelectedIndex = iContItems;
                                        this.cbClientesChange();
                                        break;
                                    }
                                }
                                //this.cbClientes.Text = reader[i].ChildNodes[a].InnerText;
                                //this.cbClientesChange();
                                break;
                            }
                        case "Customer_Name":
                            {
                                break;
                            }                            
                        case "Date":
                            {
                                //this.lblFecha.Text = Convert.ToDateTime(reader[i].ChildNodes[a].InnerText).ToShortDateString();
                                break;
                            }

                        case "Invoice_Number":
                            {
                                this.lblNumeroFactura.Text = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "Customer_PO":
                            {
                                this.txtCustomerPO.Text = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "Ship_Via":
                            {
                                //this.lblFecha.Text = Convert.ToDateTime(reader[i].ChildNodes[a].InnerText).ToShortDateString();
                                break;
                            }

                        case "Ship_Date":
                            {
                                //this.lblNumeroFactura.Text = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                        case "Displayed_Terms":
                            {
                                break;
                            }
                        case "Accounts_Receivable_Account":
                            {
                                //this.ARAccount.Text = reader[i].ChildNodes[a].InnerText;
                                //this.CambioARAccount();
                                //break;

                                ARAccountID = reader[i].ChildNodes[a].InnerText;


                                for (int iContItems3 = 0; iContItems3 < this.ARAccount.Items.Count; ++iContItems3)
                                {
                                    string sARAccountCompleto = this.ARAccount.Items[iContItems3].ToString();
                                    string[] sARAccountDesglosado = sARAccountCompleto.Split('_');
                                    string sARAccountID = sARAccountDesglosado[0];
                                    if (ARAccountID == sARAccountID)
                                    {
                                        this.ARAccount.SelectedIndex = iContItems3;
                                        //this.cbClientesChange();
                                        break;
                                    }
                                }
                                break;

                            }
                        
                        case "Sales_Representative_ID":
                            {
                                //this.cbSalesRepresent.Text = reader[i].ChildNodes[a].InnerText;
                                //break;

                                SalesRepID = reader[i].ChildNodes[a].InnerText;
                                //MODIFICACION
                                for (int iContItems2 = 0; iContItems2 < this.cbSalesRepresent.Items.Count; ++iContItems2)
                                {
                                    string sSalesRepCompleto = this.cbSalesRepresent.Items[iContItems2].ToString();
                                    string[] sSalesRepDesglosado = sSalesRepCompleto.Split('_');
                                    string sSalesRepID = sSalesRepDesglosado[0];
                                    if (SalesRepID == sSalesRepID)
                                    {
                                        this.cbSalesRepresent.SelectedIndex = iContItems2;
                                        //this.cbClientesChange();
                                        break;
                                    }
                                }

                                break;


                            }
                        case "SalesLines":
                            {
                                for (int b = 0; b <= reader[i].ChildNodes[a].ChildNodes.Count - 1; b++)
                                {
                                    sItemId = "";
                                    sItemCantidad = "";
                                    sItemUnidadM = "";
                                    sItemDescripcion = "";
                                    sItemGLAccount = "";
                                    sItemPrecioU = "";
                                    sItemTaxType = "";
                                    sNumDist = "0";

                                    for (int c = 0; c <= reader[i].ChildNodes[a].ChildNodes[b].ChildNodes.Count - 1; c++)
                                    {
                                        switch (reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].Name)
                                        {
                                            case "Item_ID":
                                                {
                                                    sItemId = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }
                                            case "Quantity":
                                                {
                                                    sItemCantidad = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }
                                            case "UM_ID":
                                                {
                                                    sItemUnidadM = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }                                                
                                            case "Description":
                                                {
                                                    sItemDescripcion = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }
                                            case "GL_Account":
                                                {
                                                    sItemGLAccount = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }   
                                            case "Unit_Price":
                                                {
                                                    sItemPrecioU = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }                                                 
                                            case "Tax_Type":
                                                {
                                                    sItemTaxType = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }
                                            case "InvoiceCMDistribution":
                                                {
                                                    sNumDist = reader[i].ChildNodes[a].ChildNodes[b].ChildNodes[c].InnerText;
                                                    break;
                                                }
                                        }
                                    }

                                    string cantidadTemporal = "";

                                    if (sNumDist != "0")
                                    {
                                        DataRow drDetalleNotaCredito = dtDetalleNotaCredito.NewRow();
                                        drDetalleNotaCredito["Items"] = sItemId;
                                        if (sItemCantidad.Trim() == "")
                                        {
                                            drDetalleNotaCredito["Cantidad"] = "1.00";
                                            cantidadTemporal = "1.00";
                                        }
                                        else
                                        {
                                            double prueba3 = Convert.ToDouble(sItemCantidad);
                                            drDetalleNotaCredito["Cantidad"] = string.Format("{0:#,#0.000}", prueba3);
                                            cantidadTemporal = string.Format("{0:#,#0.000}", prueba3);
                                        }
                                        //drDetalleNotaCredito["Cantidad"] = sItemCantidad;
                                        drDetalleNotaCredito["UnidadMedida"] = sItemUnidadM;
                                        drDetalleNotaCredito["Retorno"] = "0";
                                        drDetalleNotaCredito["Descripcion"] = sItemDescripcion;
                                        drDetalleNotaCredito["GLAccount"] = sItemGLAccount;

                                        double precioUnitarioDouble = Convert.ToDouble(sItemPrecioU);
                                        drDetalleNotaCredito["PrecioUnitario"] = string.Format("{0:#,#0.000}", precioUnitarioDouble);
                                        if (sItemTaxType.Trim() == "")
                                        {
                                            drDetalleNotaCredito["Tax"] = "1";
                                        }
                                        else
                                        {
                                            drDetalleNotaCredito["Tax"] = sItemTaxType;
                                        }
                                        drDetalleNotaCredito["Monto"] = "";

                                        dtDetalleNotaCredito.Rows.Add(drDetalleNotaCredito);
                                        dtDetalleNotaCredito.AcceptChanges();
                                    }
                                }
                                break;
                            }

                        case "Note":
                            {
                                this.txtCustomeNote.Text = reader[i].ChildNodes[a].InnerText;
                                break;
                            }

                        case "Statement_Note":
                            {
                                this.txtStatementNote.Text = reader[i].ChildNodes[a].InnerText;
                                break;
                            }

                        case "Internal_Note":
                            {
                                this.txtInternalNote.Text = reader[i].ChildNodes[a].InnerText;
                                break;
                            }
                    }
                }                        
            }

            exportador = null;
            imp = null;
            doc = null;
            reader = null;
        }

        private void ARAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CambioARAccount();
        }

        private void CambioARAccount()
        {
            for (int i = 0; i <= glAcctIDList.GetUpperBound(1); i++)
            {
                if (glAcctIDList.GetValue(0, i).ToString() == this.ARAccount.Text)
                {
                    this.arAcctDesc.Text = glAcctIDList.GetValue(1, i).ToString();
                    break;
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarNumericos())
            {
                this.Sumar();
                if (Convert.ToDouble(this.txtTotalFactura.Text) > 0)
                {
                    if (this.ValidarStatusError(1))
                    {
                        if (this.ValidarCamposObligatoriosNotaCredito())
                        {
                            if (this.ValidarImpresora())
                            {
                                if (this.ValidarReporteZ())
                                {
                                    if (this.ValidarPeachtree())
                                    {
                                        this.IndicadorError("F3");
                                        ControladorError = 0;

                                        if (this.ImprimirNotaCredito())
                                        {
                                            //this.CreateXMLFile(0);
                                            this.CreateXMLFile3(0);
                                            this.Importfile("N");

                                            if (ControladorError == 1)
                                            {
                                                //this.CreateXMLFile(1);
                                                this.CreateXMLFile3(1);
                                                this.Importfile("N");
                                            }
                                            this.IndicadorError("F2");
                                        }
                                        else
                                        {
                                            this.ProcesoAnulacion();
                                        }

                                        this.LimpiarCampos();
                                        this.limpiarAddItem();
                                        //this.RecargarFacturas();
                                        this.LeerNumNotaCredito();
                                    }
                                }                                
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se puede realizar nota de crédito sin haber seleccionado cantidades a devolver", "No se puede realizar nota de crédito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Existe un valor devuelto el cual no es numerico y no se puede realizar el calculo", "Valor no numerico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcesoAnulacion()
        {
            this.CreateXMLFileAnulado(0);
            this.Importfile("A");
            MessageBox.Show("Ocurio un error en el proceso de impresion, se procedio a Anular el Documento");
            if (ControladorError == 1)
            {
                this.CreateXMLFileAnulado(1);
                this.Importfile("A");
            }
            this.IndicadorError("F2");
        }

        private bool RevisarEstado()
        {
            string mensaje, SImp, SFis;
            string[] CadResp;
            string[] status;
            string respuesta;

            respuesta = HASAR.LeerDoc();
            CadResp = respuesta.Split(etx);
            status = CadResp[0].Split(FS);
            SImp = status[1];
            SFis = status[2];

            mensaje = HASAR.error_SF(SImp, 1);
            if (mensaje != "0")
            {
                MessageBox.Show("Errores: " + mensaje);
                return false;
            }

            mensaje = HASAR.error_SF(SFis, 2);
            if (mensaje != "0")
            {
                MessageBox.Show("Errores: " + mensaje);
                return false;
            }

            return true;
        }

        private void Cortar()
        {
            string comando;
            string mensaje;
            comando = "E" + FS + "T";
            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
        }

        private bool ImprimirNotaCredito()
        {
            int cantLineas = 0;
            int lineaComentarios = 0;
            double valor;
            double valor2;
            string cantidad;
            string cantidad2;
            string itemID;
            string Descripcion;
            string precioU;
            string monto;
            string Tax;
            string Dia = "";
            string Mes = "";
            string Anio = "";
            string Anio2 = "";
            string Hora = "";
            string Minuto = "";
            string Segundo = "";
            string cliente;
            string clienteRUC;
            string clienteNombreNC;
            string clienteDireccion;
            string facturaRelacionada;
            string codigoI;
            string tipo = "D";
            string comando;
            string L_fecha = FechaImp;
            string tiempo = HoraImp;
            string mensaje;
            string TPago;
            string DescripcionSec;

            string sDescripcionTemp = "";
            string sDescripcion1 = "";
            string sDescripcion2 = "";
            string sDescripcion3 = "";
            int iCantLineas = 0;

            //string L_NComprobante = "123456";
        
            try
            {

                if (CambioEspecial == "SI")
                {
                    comando = "]" + FS + "1" + FS + "";
                    HASAR.LimpiarDoc();
                    mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                    if (Convert.ToInt32(mensaje) < 0)
                    {
                        HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                        return false;
                    }
                    else
                        if (this.RevisarEstado() == false)
                        {
                            HASAR.Abort(3);
                            this.Cortar();
                            return false;
                        }

                    if (txtStatementNote.Text != "")
                    {
                        comando = "]" + FS + "2" + FS + "NUMERO DE ORDEN: " + txtStatementNote.Text;
                        HASAR.LimpiarDoc();
                        mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                        if (Convert.ToInt32(mensaje) < 0)
                        {
                            HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                            return false;
                        }
                        else
                            if (this.RevisarEstado() == false)
                            {
                                HASAR.Abort(3);
                                this.Cortar();
                                return false;
                            }


                    }


                    if (txtCustomerPO.Text != "")
                    {
                        comando = "]" + FS + "3" + FS + "REFERENCIA: " + txtCustomerPO.Text;
                        HASAR.LimpiarDoc();
                        mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                        if (Convert.ToInt32(mensaje) < 0)
                        {
                            HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                            return false;
                        }
                        else
                            if (this.RevisarEstado() == false)
                            {
                                HASAR.Abort(3);
                                this.Cortar();
                                return false;
                            }
                    }
                }
                else
                {
                    if (txtCustomerPO.Text != "")
                    {
                        comando = "]" + FS + "1" + FS + "PO:" + txtCustomerPO.Text;
                    }
                    else
                    {
                        comando = "]" + FS + "1" + "";
                    }
                    HASAR.LimpiarDoc();
                    mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                    if (Convert.ToInt32(mensaje) < 0)
                    {
                        HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                        return false;
                    }
                    else
                        if (this.RevisarEstado() == false)
                        {
                            HASAR.Abort(3);
                            this.Cortar();
                            return false;
                        }


                    if (cbSalesRepresent.Text != "")
                    {
                        comando = "]" + FS + "2" + FS + "Representante de Ventas:" + cbSalesRepresent.Text;
                    }
                    else
                    {
                        comando = "]" + FS + "2" + "";
                    }
                    HASAR.LimpiarDoc();
                    mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                    if (Convert.ToInt32(mensaje) < 0)
                    {
                        HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                        return false;
                    }
                    else
                        if (this.RevisarEstado() == false)
                        {
                            HASAR.Abort(3);
                            this.Cortar();
                            return false;
                        }


                    if (txtTel.Text != "")
                    {
                        comando = "]" + FS + "3" + FS + "Dir:" + Add1.Text + " " + Add2.Text + " / Tel:" + txtTel.Text;
                    }
                    else
                    {
                        comando = "]" + FS + "3" + FS + "Dir:" + Add1.Text + " " + Add2.Text;
                    }
                    HASAR.LimpiarDoc();
                    mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                    if (Convert.ToInt32(mensaje) < 0)
                    {
                        HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                        return false;
                    }
                    else
                        if (this.RevisarEstado() == false)
                        {
                            HASAR.Abort(3);
                            this.Cortar();
                            return false;
                        }
                }

                DateTime x = DateTime.Now;
                Dia = x.Day.ToString("00");
                Mes = x.Month.ToString("00");
                Anio = x.Year.ToString("00");
                Anio2 = Anio.Substring(2, 2);
                Hora = x.Hour.ToString("00");
                Minuto = x.Minute.ToString("00");
                Segundo = x.Second.ToString("00");

                this.ObtenerNumRegistro();
                if (this.lblNumeroFactura.Text.Trim() != "")
                {
                    facturaRelacionada = this.lblNumeroFactura.Text.Trim().Substring(4, 8);
                }
                else
                {
                    facturaRelacionada = "00000000";
                }

                cliente = this.cbClientes.Text;
                clienteDireccion = this.Add1.Text.Trim();

                if (this.CustVendName.Text.Trim() != "")
                {
                    clienteNombreNC = this.CustVendName.Text;
                }
                else
                {
                    clienteNombreNC = cliente;
                }
                if (clienteNombreNC.Length > 42)
                {
                    clienteNombreNC = clienteNombreNC.Substring(0, 42);
                }

                if (this.txtRUC.Text.Trim() != "")
                {
                    clienteRUC = this.txtRUC.Text.Trim();
                }
                else
                {
                    clienteRUC = cliente;
                }
                if (clienteRUC.Length > 30)
                {
                    clienteRUC = clienteRUC.Substring(0, 30);
                }

                comando = "@" + FS + clienteNombreNC + FS + clienteRUC + FS + facturaRelacionada + FS + RegisterMachineNumber + FS + L_fecha + FS + tiempo + FS + tipo;
                HASAR.LimpiarDoc();
                mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();

                if (Convert.ToInt32(mensaje) < 0)
                {
                    HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                    return false;
                }
                else
                    if (this.RevisarEstado() == false)
                    {
                        HASAR.Abort(3);
                        this.Cortar();
                        return false;
                    }

                                
                cantLineas = (dgvDetalleInvoice.Rows.Count - 1);
                for (int lineas = 0; lineas < cantLineas; ++lineas)
                {
                    cantidad = dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                    if (cantidad != "")
                    {
                        try
                        {
                            //cantidad2 = (Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString()) * 1000).ToString();
                            cantidad2 = string.Format("{0:##.000}", Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString()));
                            //itemID = dgvDetalleInvoice.Rows[lineas].Cells[0].Value.ToString();
                            itemID = this.dgvDetalleInvoice.Rows[lineas].Cells[0].Value.ToString();
                            DescripcionSec = itemID;
      
                            if (itemID.Length > 20)
                            {
                                itemID = itemID.Substring(0, 20);
                            }



                            Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[4].Value.ToString();
                            //Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString();
                            if (Descripcion.Trim() == "")
                            {
                                sDescripcion1 = DescripcionSec;
                            }
                            else
                                if (Descripcion.Length > 100)
                                {
                                    sDescripcion3 = Descripcion.Substring(0, 50);
                                    sDescripcionTemp = Descripcion.Substring(50, Descripcion.Length - 50);
                                    sDescripcion2 = sDescripcionTemp.Substring(0, 50);
                                    sDescripcion1 = sDescripcionTemp.Substring(50, sDescripcionTemp.Length - 50);
                                    iCantLineas = 2;
                                }
                                else
                                    if (Descripcion.Length > 50)
                                    {
                                        sDescripcion2 = Descripcion.Substring(0, 50);
                                        sDescripcion1 = Descripcion.Substring(50, Descripcion.Length - 50);
                                        iCantLineas = 1;
                                    }
                                    else
                                    {
                                        sDescripcion1 = Descripcion;
                                        iCantLineas = 0;
                                    }






                            //precioU = (Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[6].Value) * 100).ToString();
                            precioU = string.Format("{0:##0.000}", (Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[6].Value)));
                            Tax = this.dgvDetalleInvoice.Rows[lineas].Cells[7].Value.ToString();
                            monto = this.dgvDetalleInvoice.Rows[lineas].Cells[8].Value.ToString();

                            valor = 0;
                            valor2 = 0;
                            valor = (Convert.ToDouble(precioU));
                            valor2 = (Convert.ToDouble(cantidad2));

                            codigoI = "0000";
                            int controlador = 0;
                            if (Tax == "1")
                            {
                                codigoI = "0700";
                                controlador = 1;
                            }
                            if (controlador == 0)
                            {
                                if (impuestoITBMS10Habilitado == "Habilitado")
                                {
                                    if (Tax == impuestoITBMS10TaxType)
                                    {
                                        codigoI = "1000";
                                        controlador = 1;
                                    }
                                }
                            }

                            if (controlador == 0)
                            {
                                if (impuestoITBMS15Habilitado == "Habilitado")
                                {
                                    if (Tax == impuestoITBMS15TaxType)
                                    {
                                        codigoI = "1500";
                                        controlador = 1;
                                    }
                                }
                            }

                            if (controlador == 0)
                            {
                                codigoI = "0000";
                            }
                        }

                        catch (Exception ex)
                        {
                            return false;
                        }


                        //cod descripcion
                        if (CodigoProducto != "C")
                        {
                            comando = "A" + FS + "Cod:" + itemID;
                            HASAR.LimpiarDoc();
                            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                            if (Convert.ToInt32(mensaje) < 0)
                            {
                                HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                                return false;
                            }
                            else
                                if (this.RevisarEstado() == false)
                                {
                                    HASAR.Abort(3);
                                    this.Cortar();
                                    return false;
                                }
                        }


                        if (iCantLineas == 2)
                        {
                            comando = "A" + FS + sDescripcion3;
                            HASAR.LimpiarDoc();
                            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                            if (Convert.ToInt32(mensaje) < 0)
                            {
                                HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                                return false;
                            }
                            else
                                if (this.RevisarEstado() == false)
                                {
                                    HASAR.Abort(3);
                                    this.Cortar();
                                    return false;
                                }
                            iCantLineas = 1;
                        }

                        if (iCantLineas == 1)
                        {
                            comando = "A" + FS + sDescripcion2;
                            HASAR.LimpiarDoc();
                            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                            if (Convert.ToInt32(mensaje) < 0)
                            {
                                HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                                return false;
                            }
                            else
                                if (this.RevisarEstado() == false)
                                {
                                    HASAR.Abort(3);
                                    this.Cortar();
                                    return false;
                                }
                        }

                        //comando = "B" + FS + Descripcion + FS + cantidad2 + FS + precioU + FS + codigoI + FS + "M" + FS + "12345";
                        if (CodigoProducto == "C")
                        {


                            if(precioU.Contains("-"))
                            {
                                double remplazo = Convert.ToDouble(precioU) * -1;
                                precioU = string.Format("{0:##0.000}", remplazo);
                                comando = "B" + FS + sDescripcion1 + FS + cantidad2 + FS + precioU + FS + codigoI + FS + "m" + FS + itemID;
                            }
                            else
                            {
                                comando = "B" + FS + sDescripcion1 + FS + cantidad2 + FS + precioU + FS + codigoI + FS + "M" + FS + itemID;
                            }

                            




                            HASAR.LimpiarDoc();
                            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();

                            if (Convert.ToInt32(mensaje) < 0)
                            {
                                HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                                return false;
                            }
                            else
                                if (this.RevisarEstado() == false)
                                {
                                    HASAR.Abort(3);
                                    this.Cortar();
                                    return false;
                                }

                        }
                        else
                        {


                            if (precioU.Contains("-"))
                            {
                                double remplazo = Convert.ToDouble(precioU) * -1;
                                precioU = string.Format("{0:##0.000}", remplazo);
                                comando = "B" + FS + sDescripcion1 + FS + cantidad2 + FS + precioU + FS + codigoI + FS + "m" + FS + "*****";
                            }
                            else
                            {
                                comando = "B" + FS + sDescripcion1 + FS + cantidad2 + FS + precioU + FS + codigoI + FS + "M" + FS + "*****";
                            }

                                



                            HASAR.LimpiarDoc();
                            mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();

                            if (Convert.ToInt32(mensaje) < 0)
                            {
                                HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                                return false;
                            }
                            else
                                if (this.RevisarEstado() == false)
                                {
                                    HASAR.Abort(3);
                                    this.Cortar();
                                    return false;
                                }
                        }
                    }
                }
                                
                comando = "C";
                HASAR.LimpiarDoc();
                mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                if (Convert.ToInt32(mensaje) < 0)
                {
                    HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                    return false;
                }
                else
                    if (this.RevisarEstado() == false)
                    {
                        HASAR.Abort(3);
                        this.Cortar();
                        return false;
                    }


                //double montoCR = Convert.ToDouble(this.txtTotalFactura.Text);
                //if (montoCR > 0)
                //{
                //    valorc = this.txtTotalFactura.Text;
                //    valorc = valorc.Replace(".", ",");
                //    montoCR = Convert.ToDouble(valorc);

                //    TPago = "5";
                //    comando = "D" + FS + "Credito" + FS + montoCR + FS + "T" + FS + TPago;
                //    HASAR.LimpiarDoc();
                //    mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                //    if (Convert.ToInt32(mensaje) < 0)
                //    {
                //        HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                //        return false;
                //    }
                //    else
                //        if (this.RevisarEstado() == false)
                //        {
                //            HASAR.Abort(3);
                //            this.Cortar();
                //            return false;
                //        }
                //}

                comando = "E" + FS + "T";
                HASAR.LimpiarDoc();
                mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                if (Convert.ToInt32(mensaje) < 0)
                {
                    HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                    return false;
                }
                else
                    if (this.RevisarEstado() == false)
                    {
                        HASAR.Abort(3);
                        this.Cortar();
                        return false;
                    }





                comando = "^" + FS + "2" + FS + PieDocumento;
                HASAR.LimpiarDoc();
                mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                if (Convert.ToInt32(mensaje) < 0)
                {
                    HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                    return false;
                }
                else
                    if (this.RevisarEstado() == false)
                    {
                        HASAR.Abort(3);
                        this.Cortar();
                        return false;
                    }


                return true;
            }                        
            catch (Exception ex)                        
            {                            
                return false;                        
            }  
        }

        private void LeerNumNotaCredito()
        {
            try
            {
                int serieLenght = 0;
                string serie;
                string NumeroSerie;
                string hora, min, seg, tiempo, mensaje, SImp, SFis;
                string[] CadResp;
                string[] status;
                string respuesta;
                HASAR.LimpiarDoc();
                handler = frmPrincipal.handlerM;
                mensaje = HASAR.MandaPaqueteFiscal(handler, "*").ToString();
                if (mensaje == "0")
                {
                    respuesta = HASAR.LeerDoc();
                    CadResp = respuesta.Split(etx);
                    status = CadResp[0].Split(FS);
                    SImp = status[1];
                    SFis = status[2];
                    mensaje = HASAR.error_SF(SImp, 1);
                    if (mensaje != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje);
                    }

                    mensaje = HASAR.error_SF(SFis, 2);
                    if (mensaje != "0")
                    {
                        MessageBox.Show("Errores: " + mensaje);
                    }

                    NumeroSerie = getRegisteredMachineNumber();
                    serieLenght = NumeroSerie.Trim().Length;
                    serie = NumeroSerie.Trim().Substring(serieLenght - 3, 3);

                    int sec = Convert.ToInt32(status[10]) + 1;
                    this.lblNumeroNC.Text = serie + "-" + sec.ToString("00000000");
                }
                else
                {
                    this.lblNumeroNC.Text = "BSC-" + "00000000";
                    if (Convert.ToInt32(mensaje) < 0)
                    {
                        HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblNumeroNC.Text = "BSC-" + "00000000";
            }
        }

        private void CreateXMLFile(int SegundaOpcion)
        {
            int lineaDesc = 0;
            int cantLineas = 0;
            int cantLineas2 = 0;
            int numberOfDistributions = 0;
            //int numdist = 0;
            string cantidad;

            string itemID;
            string UM;
            string Descripcion;
            string GLAcc;
            string precioU;
            string Taxtype;
            string monto;
            string valorRetorno;

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\NewNotaCredito.xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_Invoices");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_Invoice");
            Writer.WriteAttributeString("xsi:type", "paw:invoice");

            string customerID = this.cbClientes.Text;
            string customerGUID = "";
            string customerName = "";
            //Cliente ID
            Writer.WriteStartElement("Customer_ID");
            Writer.WriteAttributeString("xsi:type", "paw:id");
            Writer.WriteString(customerID);
            Writer.WriteEndElement();
            //Cliente GUID
            customerGUID = this.ObtenerCustomerGUID(customerID);
            if (customerGUID != "")
            {
                Writer.WriteElementString("Customer_GUID", customerGUID);
            }
            //Cliente Nombre
            customerName = this.CustVendName.Text;
            Writer.WriteElementString("Customer_Name", this.CustVendName.Text);
            //Numero NotaCredito
            string numeroNotaCredito ="NC-" + this.lblNumeroNC.Text.Trim();
            if (SegundaOpcion.Equals(1))
            {
                numeroNotaCredito = numeroNotaCredito + "-2";
            }
            Writer.WriteElementString("Invoice_Number", numeroNotaCredito);
            //Fecha NotaCredito
            Writer.WriteStartElement("Date");
            Writer.WriteAttributeString("xsi:type", "paw:date");
            Writer.WriteString(this.txtNotaCreditoDate.Text);
            Writer.WriteEndElement();
            //Es Cotizacion?
            Writer.WriteElementString("isQuote", "FALSE");
            //Tiene Drop Ship                
            Writer.WriteElementString("Drop_Ship", "FALSE");

            //ShipToAddress
            Writer.WriteStartElement("ShipToAddress");
            Writer.WriteElementString("Name", this.CustVendName.Text);
            Writer.WriteElementString("Line1", this.Add1.Text);
            Writer.WriteElementString("Line2", this.Add2.Text);
            Writer.WriteElementString("City", this.City.Text);
            Writer.WriteElementString("State", this.State.Text);
            Writer.WriteElementString("Zip", this.State.Text);
            Writer.WriteStartElement("Sales_Tax_Code");
            Writer.WriteAttributeString("xsi:type", "paw:id");
            Writer.WriteString(impuestoITBMSTaxID);
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            //Customer PO
            if (this.txtCustomerPO.Text.Trim() != "")
            {
                Writer.WriteElementString("Customer_PO", this.txtCustomerPO.Text);
            }
            //Date Due
            ////<Date_Due xsi:type="paw:date">3/31/11</Date_Due> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  
            //Discount Amount
            Writer.WriteElementString("Discount_Amount", "0.00");
            //Discount Date
            ////<Discount_Date xsi:type="paw:date">3/15/11</Discount_Date> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  

            //Campo utilizado para indicar el tipo documento 
            Writer.WriteElementString("Displayed_Terms", "DOCNC");

            string salesRepID = this.cbSalesRepresent.Text;
            string salesRepGUID = "";
            if (salesRepID != "")
            {
                //Sales Rep ID
                Writer.WriteStartElement("Sales_Representative_ID");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(salesRepID);
                Writer.WriteEndElement();
                //Sales Rep GUID
                salesRepGUID = this.ObtenerSalesRepGUID(salesRepID);
                if (salesRepGUID != "")
                {
                    Writer.WriteElementString("Sales_Rep_GUID", salesRepGUID);
                }
            }

            string aRAccountID = this.ARAccount.Text.ToString();
            string aRAccountGUID = "";
            if (aRAccountID != "")
            {
                //ARAccount ID            
                Writer.WriteStartElement("Accounts_Receivable_Account");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(aRAccountID);
                Writer.WriteEndElement();
                //ARAccount GUID
                aRAccountGUID = this.ObtenerARAccountGUID(aRAccountID);
                if (aRAccountGUID != "")
                {
                    Writer.WriteElementString("AR_Account_GUID", aRAccountGUID);
                }
            }
            //Accounts Receivable Monto
            double accRAmount = (Convert.ToDouble(this.txtTotal.Text) * -1);
            Writer.WriteElementString("Accounts_Receivable_Amount", accRAmount.ToString());
            //Note Prints After Line Items
            Writer.WriteElementString("Note_Prints_After_Line_Items", "FALSE");
            //Statement Note Prints Before Ref
            Writer.WriteElementString("Statement_Note_Prints_Before_Ref", "FALSE");
            //Beginning Balance Transaction
            Writer.WriteElementString("Beginning_Balance_Transaction", "FALSE");

            //<Transaction_Period>21</Transaction_Period> 
            //<Transaction_Number>31</Transaction_Number> 
            //<GUID>{758EB4D4-BD8E-4EEC-A3E1-EF7561AE2AE5}</GUID> 

            ////Aplica a factura
            //if (this.lblNumeroFactura.Text != "")
            //{
            //    Writer.WriteElementString("ApplyToInvoiceNumber", this.lblNumeroFactura.Text);
            //}
            //Es Nota Credito
            Writer.WriteElementString("CreditMemoType", "TRUE");
            //ProgressBillingInvoice
            Writer.WriteElementString("ProgressBillingInvoice", "FALSE");

            cantLineas = (this.dgvDetalleInvoice.Rows.Count);
            //numberOfDistributions = (this.dgvDetalleInvoice.Rows.Count - 1);
            cantLineas2 = 0;         
            for (int lineas = 0; lineas < (cantLineas - 1); ++lineas)
            {
                valorRetorno = this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                if(valorRetorno != "")
                {
                    cantLineas2 = cantLineas2 + 1;
                }
            }

            numberOfDistributions = cantLineas2;

            if (impuestoITBMS7Habilitado == "Habilitado")
            {
                numberOfDistributions = numberOfDistributions + 1;
            }

            double impuesto10 = 0;
            impuesto10 = Convert.ToDouble(this.txtITBMS10.Text);

            if (impuesto10 > 0)
            {
                numberOfDistributions = numberOfDistributions + 1;
            } 

            Writer.WriteElementString("Number_of_Distributions", numberOfDistributions.ToString());
            Writer.WriteStartElement("SalesLines");

            //ITBMS
            Writer.WriteStartElement("SalesLine");
            Writer.WriteElementString("Quantity", "0.00000");
            Writer.WriteElementString("SalesOrderDistributionNumber", "0");
            Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
            Writer.WriteElementString("Apply_To_Proposal", "FALSE");
            Writer.WriteElementString("InvoiceCMDistribution", "0");
            Writer.WriteElementString("Description", impuestoITBMS7TaxDescription);
            Writer.WriteStartElement("GL_Account");
            Writer.WriteAttributeString("xsi:type", "paw:id");
            Writer.WriteString(impuestoITBMS7TaxAccountId);
            Writer.WriteEndElement();
            Writer.WriteElementString("GL_Account_GUID", impuestoITBMSTaxAccountIdGUID);
            Writer.WriteElementString("Unit_Price", "0.00000");
            Writer.WriteElementString("Tax_Type", "0");
            Writer.WriteElementString("Weight", "0.00000");

            string itbms = "0";
            if (impuestoITBMS7Habilitado == "Habilitado")
            {
                itbms = (Convert.ToDouble(this.txtITBMS7.Text) * 1).ToString();
            }
            else
            {
                itbms = "0";
            }
            Writer.WriteElementString("Amount", itbms);//*****************************                
            Writer.WriteElementString("Cost_of_Sales_Amount", "0.00");
            Writer.WriteElementString("Retainage_Percent", "0.00");
            Writer.WriteElementString("UM_Stocking_Units", "0.00000");
            Writer.WriteElementString("Stocking_Quantity", "0.00000");
            Writer.WriteElementString("Stocking_Unit_Price", "0.00000");
            Writer.WriteElementString("Sales_Tax_Authority", impuestoITBMS7TaxID);

            Writer.WriteEndElement();//closes the sales line element

            int x = 0;
            for (int lineas = 0; lineas < (cantLineas - 1); ++lineas)
            {                                
                valorRetorno = dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                if(valorRetorno != "")
                {
                    lineaDesc = lineas + 1;                                        
                    itemID = this.dgvDetalleInvoice.Rows[lineas].Cells[0].Value.ToString();
                    cantidad = this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                    UM = this.dgvDetalleInvoice.Rows[lineas].Cells[2].Value.ToString();
                    Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[4].Value.ToString();
                    GLAcc = this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString();
                    precioU = this.dgvDetalleInvoice.Rows[lineas].Cells[6].Value.ToString();
                    Taxtype = this.dgvDetalleInvoice.Rows[lineas].Cells[7].Value.ToString();

                    if (Taxtype == "0")
                    {
                        Taxtype = "1";
                    }
                    monto = dgvDetalleInvoice.Rows[lineas].Cells[8].Value.ToString();

                    Writer.WriteStartElement("SalesLine");
                    if (cantidad.Trim() == "")
                    {
                        Writer.WriteElementString("Quantity", (Convert.ToDouble("0") * -1).ToString());
                    }
                    else 
                    {
                        Writer.WriteElementString("Quantity", (Convert.ToDouble(cantidad) * -1).ToString());
                    }
                    

                    Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                    Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                    Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                    Writer.WriteElementString("InvoiceCMDistribution", (x + 1).ToString());
                    x=x + 1;

                    Writer.WriteStartElement("Item_ID");
                    Writer.WriteAttributeString("xsi:type", "paw:ID");
                    Writer.WriteString(itemID);
                    Writer.WriteEndElement();

                    Writer.WriteElementString("Description", Descripcion);

                    Writer.WriteStartElement("GL_Account");
                    Writer.WriteAttributeString("xsi:type", "paw:ID");
                    Writer.WriteString(GLAcc);
                    Writer.WriteEndElement();

                    Writer.WriteElementString("Unit_Price", (Convert.ToDouble(precioU)).ToString());
                    Writer.WriteElementString("Tax_Type", Taxtype);

                    Writer.WriteElementString("Amount", (Convert.ToDouble(monto)).ToString());

                    //Writer.WriteStartElement("UM_ID");
                    //Writer.WriteAttributeString("xsi:type", "paw:id");
                    //Writer.WriteString(UM);
                    //Writer.WriteEndElement();

                    Writer.WriteEndElement();//closes the sales line element
                }
            }

            if (impuesto10 > 0)
            {
                //FREIGHT
                Writer.WriteStartElement("SalesLine");
                Writer.WriteElementString("Quantity", "0.00000");
                Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                Writer.WriteElementString("InvoiceCMDistribution", "0");
                Writer.WriteElementString("Description", "Freight Amount");

                Writer.WriteStartElement("GL_Account");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(impuestoITBMS7TaxAccountId);
                Writer.WriteEndElement();
                Writer.WriteElementString("GL_Account_GUID", impuestoITBMSTaxAccountIdGUID);

                Writer.WriteElementString("Unit_Price", "0.00000");
                Writer.WriteElementString("Tax_Type", "26");
                Writer.WriteElementString("Weight", "0.00000");

                string amountFreight;
                double s = Convert.ToDouble(txtITBMS10.Text) * 1;
                amountFreight = s.ToString();

                Writer.WriteElementString("Amount", amountFreight);//*****************************                
                Writer.WriteElementString("Cost_of_Sales_Amount", "0.00");
                Writer.WriteElementString("Retainage_Percent", "0.00");
                Writer.WriteElementString("UM_Stocking_Units", "1.00000");
                Writer.WriteElementString("Stocking_Quantity", "0.00000");
                Writer.WriteElementString("Stocking_Unit_Price", "0.00000");
                Writer.WriteEndElement();//closes the sales line element
            }

            Writer.WriteEndElement();//Closes the Sales Lines element

            Writer.WriteEndElement();//Closes the paw_invoice element

            Writer.WriteEndElement();//closes the paw_invoices element and ends the document

            Writer.Close();
        }

        private void CreateXMLFile3(int SegundaOpcion)
        {
            int lineaDesc = 0;
            int cantLineas = 0;
            int cantLineas2 = 0;
            int numberOfDistributions = 0;
            //int numdist = 0;
            string cantidad;

            string itemID;
            string UM;
            string Descripcion;
            string GLAcc;
            string precioU;
            string Taxtype;
            string monto;
            string valorRetorno;
            double impuesto10 = 0;
            int restaitbms = 0;
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\NewNotaCredito.xml";
            int contadorn = 0;
            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);
            int x = 0;

            Writer.WriteStartElement("PAW_Invoices");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            cantLineas = (this.dgvDetalleInvoice.Rows.Count);
            for (int lineas = 0; lineas <= (cantLineas - 1); ++lineas)
            //for (int lineas = 0; lineas < (cantLineas); ++lineas)
            {

                Writer.WriteStartElement("PAW_Invoice");
                Writer.WriteAttributeString("xsi:type", "paw:invoice");


                string sClienteSeleccionadoCompleto = this.cbClientes.Text;
                string[] sClienteSeleccionadoDesglosado = sClienteSeleccionadoCompleto.Split('_');
                string sClienteSeleccionadoID = sClienteSeleccionadoDesglosado[0];

                string customerID = sClienteSeleccionadoID;
                string customerGUID = "";
                string customerName = "";
                //Cliente ID
                Writer.WriteStartElement("Customer_ID");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(customerID);
                Writer.WriteEndElement();
                //Cliente GUID
                customerGUID = this.ObtenerCustomerGUID(customerID);
                if (customerGUID != "")
                {
                    Writer.WriteElementString("Customer_GUID", customerGUID);
                }
                //Cliente Nombre
                customerName = this.CustVendName.Text;
                Writer.WriteElementString("Customer_Name", this.CustVendName.Text);
                //Numero NotaCredito
                string numeroNotaCredito = "NC-" + this.lblNumeroNC.Text.Trim();
                if (SegundaOpcion.Equals(1))
                {
                    numeroNotaCredito = numeroNotaCredito + "-2";
                }
                Writer.WriteElementString("Invoice_Number", numeroNotaCredito);

                //Fecha NotaCredito
                Writer.WriteStartElement("Date");
                Writer.WriteAttributeString("xsi:type", "paw:date");
                Writer.WriteString(this.txtNotaCreditoDate.Text);
                Writer.WriteEndElement();
                //Es Cotizacion?
                Writer.WriteElementString("isQuote", "FALSE");
                //Tiene Drop Ship                
                Writer.WriteElementString("Drop_Ship", "FALSE");

                //ShipToAddress
                Writer.WriteStartElement("ShipToAddress");
                Writer.WriteElementString("Name", this.CustVendName.Text);
                Writer.WriteElementString("Line1", this.Add1.Text);
                Writer.WriteElementString("Line2", this.Add2.Text);
                Writer.WriteElementString("City", this.City.Text);
                Writer.WriteElementString("State", this.State.Text);
                Writer.WriteElementString("Zip", this.State.Text);
                Writer.WriteStartElement("Sales_Tax_Code");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(impuestoITBMSTaxID);
                Writer.WriteEndElement();
                Writer.WriteEndElement();
                //Customer PO
                if (this.txtCustomerPO.Text.Trim() != "")
                {
                    Writer.WriteElementString("Customer_PO", this.txtCustomerPO.Text);
                }
                //Date Due
                ////<Date_Due xsi:type="paw:date">3/31/11</Date_Due> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  
                //Discount Amount
                Writer.WriteElementString("Discount_Amount", "0.00");
                //Discount Date
                ////<Discount_Date xsi:type="paw:date">3/15/11</Discount_Date> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  

                //Campo utilizado para indicar el tipo documento 
                Writer.WriteElementString("Displayed_Terms", "DOCNC");


                string salesRepIDSeleccionadoCompleto = this.cbSalesRepresent.Text;
                string[] salesRepIDSeleccionadoDesglosado = salesRepIDSeleccionadoCompleto.Split('_');
                string salesRepID = salesRepIDSeleccionadoDesglosado[0];


                //string salesRepID = this.cbSalesRepresent.Text;
                string salesRepGUID = "";
                if (salesRepID != "")
                {
                    //Sales Rep ID
                    Writer.WriteStartElement("Sales_Representative_ID");
                    Writer.WriteAttributeString("xsi:type", "paw:id");
                    Writer.WriteString(salesRepID);
                    Writer.WriteEndElement();
                    //Sales Rep GUID
                    salesRepGUID = this.ObtenerSalesRepGUID(salesRepID);
                    if (salesRepGUID != "")
                    {
                        Writer.WriteElementString("Sales_Rep_GUID", salesRepGUID);
                    }
                }

                //string aRAccountID = this.ARAccount.Text.ToString();



                string sARAccountSeleccionadoCompleto = this.ARAccount.Text;
                string[] sARAccountSeleccionadoDesglosado = sARAccountSeleccionadoCompleto.Split('_');
                string aRAccountID = sARAccountSeleccionadoDesglosado[0];

                string aRAccountGUID = "";
                if (aRAccountID != "")
                {
                    //ARAccount ID            
                    Writer.WriteStartElement("Accounts_Receivable_Account");
                    Writer.WriteAttributeString("xsi:type", "paw:id");
                    Writer.WriteString(aRAccountID);
                    Writer.WriteEndElement();
                    //ARAccount GUID
                    aRAccountGUID = this.ObtenerARAccountGUID(aRAccountID);
                    if (aRAccountGUID != "")
                    {
                        Writer.WriteElementString("AR_Account_GUID", aRAccountGUID);
                    }
                }
                //Accounts Receivable Monto
                double accRAmount = (Convert.ToDouble(this.txtTotal.Text) * -1);
                Writer.WriteElementString("Accounts_Receivable_Amount", accRAmount.ToString());


                //Notes
                Writer.WriteElementString("Note", txtCustomeNote.Text);
                //Internal Note
                Writer.WriteElementString("Internal_Note", txtInternalNote.Text);

                //Note Prints After Line Items
                Writer.WriteElementString("Note_Prints_After_Line_Items", "FALSE");
                //Statement Note Prints Before Ref
                Writer.WriteElementString("Statement_Note_Prints_Before_Ref", "FALSE");
                //Beginning Balance Transaction
                Writer.WriteElementString("Beginning_Balance_Transaction", "FALSE");

                //<Transaction_Period>21</Transaction_Period> 
                //<Transaction_Number>31</Transaction_Number> 
                //<GUID>{758EB4D4-BD8E-4EEC-A3E1-EF7561AE2AE5}</GUID> 

                //Aplica a factura
                if (this.lblNumeroFactura.Text != "")
                {
                    Writer.WriteElementString("ApplyToInvoiceNumber", this.lblNumeroFactura.Text);
                    if (contadorn == (cantLineas - 1))
                    {
                        Writer.WriteElementString("ApplyToInvoiceDistNumber", "65535");

                    }
                    else
                    {
                        Writer.WriteElementString("ApplyToInvoiceDistNumber", (lineas + 1).ToString());
                    }
                }

                //Es Nota Credito
                Writer.WriteElementString("CreditMemoType", "TRUE");
                //ProgressBillingInvoice
                Writer.WriteElementString("ProgressBillingInvoice", "FALSE");

                cantLineas = (this.dgvDetalleInvoice.Rows.Count);
                //numberOfDistributions = (this.dgvDetalleInvoice.Rows.Count - 1);
                cantLineas2 = 0;
                for (int lineas2 = 0; lineas2 < (cantLineas - 1); ++lineas2)
                {
                    valorRetorno = this.dgvDetalleInvoice.Rows[lineas2].Cells[3].Value.ToString();
                    if (valorRetorno != "")
                    {
                        cantLineas2 = cantLineas2 + 1;
                    }
                }

                numberOfDistributions = cantLineas2;

                if (impuestoITBMS7Habilitado == "Habilitado")
                {
                    numberOfDistributions = numberOfDistributions + 1;
                }


                impuesto10 = Convert.ToDouble(this.txtITBMS10.Text);

                if (impuesto10 > 0)
                {
                    numberOfDistributions = numberOfDistributions + 1;
                }

                Writer.WriteElementString("Number_of_Distributions", numberOfDistributions.ToString());

                //Statement Note
                Writer.WriteElementString("Statement_Note", txtStatementNote.Text);



                Writer.WriteStartElement("SalesLines");



                if (contadorn == (cantLineas - 1))
                {

                    //ITBMS
                    Writer.WriteStartElement("SalesLine");
                    Writer.WriteElementString("Quantity", "0.00000");
                    Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                    Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                    Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                    Writer.WriteElementString("InvoiceCMDistribution", "0");
                    Writer.WriteElementString("Description", impuestoITBMS7TaxDescription);
                    Writer.WriteStartElement("GL_Account");
                    Writer.WriteAttributeString("xsi:type", "paw:id");
                    Writer.WriteString(impuestoITBMS7TaxAccountId);
                    Writer.WriteEndElement();
                    Writer.WriteElementString("GL_Account_GUID", impuestoITBMSTaxAccountIdGUID);
                    Writer.WriteElementString("Unit_Price", "0.00000");
                    Writer.WriteElementString("Tax_Type", "0");
                    Writer.WriteElementString("Weight", "0.00000");

                    string itbms = "0";
                    if (impuestoITBMS7Habilitado == "Habilitado")
                    {
                        itbms = (Convert.ToDouble(this.txtITBMS7.Text) * 1).ToString();
                    }
                    else
                    {
                        itbms = "0";
                    }
                    Writer.WriteElementString("Amount", itbms);//*****************************                
                    Writer.WriteElementString("Cost_of_Sales_Amount", "0.00");
                    Writer.WriteElementString("Retainage_Percent", "0.00");
                    Writer.WriteElementString("UM_Stocking_Units", "0.00000");
                    Writer.WriteElementString("Stocking_Quantity", "0.00000");
                    Writer.WriteElementString("Stocking_Unit_Price", "0.00000");
                    Writer.WriteElementString("Sales_Tax_Authority", impuestoITBMS7TaxID);

                    Writer.WriteEndElement();//closes the sales line element

                }
                else
                {

                    valorRetorno = dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                    if (valorRetorno != "")
                    {
                        lineaDesc = lineas + 1;
                        itemID = this.dgvDetalleInvoice.Rows[lineas].Cells[0].Value.ToString();
                        cantidad = this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                        UM = this.dgvDetalleInvoice.Rows[lineas].Cells[2].Value.ToString();
                        Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[4].Value.ToString();
                        GLAcc = this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString();
                        precioU = this.dgvDetalleInvoice.Rows[lineas].Cells[6].Value.ToString();
                        Taxtype = this.dgvDetalleInvoice.Rows[lineas].Cells[7].Value.ToString();

                        if (Taxtype == "0")
                        {
                            Taxtype = "1";
                        }
                        monto = dgvDetalleInvoice.Rows[lineas].Cells[8].Value.ToString();

                        Writer.WriteStartElement("SalesLine");
                        if (cantidad.Trim() == "")
                        {
                            Writer.WriteElementString("Quantity", (Convert.ToDouble("0") * -1).ToString());
                        }
                        else
                        {
                            Writer.WriteElementString("Quantity", (Convert.ToDouble(cantidad) * -1).ToString());
                        }


                        Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                        Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                        Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                        Writer.WriteElementString("InvoiceCMDistribution", (x + 1).ToString());
                        x = x + 1;

                        Writer.WriteStartElement("Item_ID");
                        Writer.WriteAttributeString("xsi:type", "paw:ID");
                        Writer.WriteString(itemID);
                        Writer.WriteEndElement();

                        Writer.WriteElementString("Description", Descripcion);

                        Writer.WriteStartElement("GL_Account");
                        Writer.WriteAttributeString("xsi:type", "paw:ID");
                        Writer.WriteString(GLAcc);
                        Writer.WriteEndElement();

                        Writer.WriteElementString("Unit_Price", (Convert.ToDouble(precioU)).ToString());
                        Writer.WriteElementString("Tax_Type", Taxtype);

                        Writer.WriteElementString("Amount", (Convert.ToDouble(monto)).ToString());

                        //Writer.WriteStartElement("UM_ID");
                        //Writer.WriteAttributeString("xsi:type", "paw:id");
                        //Writer.WriteString(UM);
                        //Writer.WriteEndElement();

                        Writer.WriteEndElement();//closes the sales line element
                    }


                    if (impuesto10 > 0)
                    {

                        //FREIGHT
                        Writer.WriteStartElement("SalesLine");
                        Writer.WriteElementString("Quantity", "0.00000");
                        Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                        Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                        Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                        Writer.WriteElementString("InvoiceCMDistribution", "0");
                        Writer.WriteElementString("Description", "Freight Amount");

                        Writer.WriteStartElement("GL_Account");
                        Writer.WriteAttributeString("xsi:type", "paw:id");
                        Writer.WriteString(impuestoITBMS7TaxAccountId);
                        Writer.WriteEndElement();
                        Writer.WriteElementString("GL_Account_GUID", impuestoITBMSTaxAccountIdGUID);

                        Writer.WriteElementString("Unit_Price", "0.00000");
                        Writer.WriteElementString("Tax_Type", "26");
                        Writer.WriteElementString("Weight", "0.00000");

                        string amountFreight;
                        double s = Convert.ToDouble(txtITBMS10.Text) * 1;
                        amountFreight = s.ToString();

                        Writer.WriteElementString("Amount", amountFreight);//*****************************                
                        Writer.WriteElementString("Cost_of_Sales_Amount", "0.00");
                        Writer.WriteElementString("Retainage_Percent", "0.00");
                        Writer.WriteElementString("UM_Stocking_Units", "1.00000");
                        Writer.WriteElementString("Stocking_Quantity", "0.00000");
                        Writer.WriteElementString("Stocking_Unit_Price", "0.00000");
                        Writer.WriteEndElement();//closes the sales line element

                    }

                    Writer.WriteEndElement();//Closes the Sales Lines element

                    Writer.WriteEndElement();//Closes the paw_invoice element
                }
                contadorn = contadorn + 1;
            }
            Writer.WriteEndElement();//closes the paw_invoices element and ends the document

            Writer.Close();
        }


        private string ObtenerCustomerGUID(string CustomerID)
        {
            string GUID = "";
            string customerGUID = "";
            try
            {
                for (int i = 0; i <= custIDList.GetUpperBound(0) - 1; i++)
                {
                    if (custIDList.GetValue(0, i).ToString() == CustomerID)
                    {
                        try
                        {
                            for (int y = 0; y <= 11; y++)
                            {
                                if (custIDList.GetValue(y, i).ToString().Length > 0)
                                {
                                    GUID = custIDList.GetValue(y, i).ToString().Substring(0, 1);
                                    if (GUID == "{")
                                    {
                                        customerGUID = custIDList.GetValue(y, i).ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            customerGUID = "";
                        }
                        //if (custIDList.GetValue(8, i) != null)
                        //{
                        //    customerGUID = custIDList.GetValue(8, i).ToString();                            
                        //}
                        break;
                    }
                }
                return customerGUID;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string ObtenerSalesRepGUID(string SalesRepID)
        {
            string GUID = "";
            string salesRepGUID = "";
            try
            {
                for (int i = 0; i <= salesRepList.GetUpperBound(0) - 1; i++)
                {
                    if (salesRepList.GetValue(0, i).ToString() == SalesRepID)
                    {
                        try
                        {
                            for (int y = 0; y <= 3; y++)
                            {
                                if (salesRepList.GetValue(y, i).ToString().Length > 0)
                                {
                                    GUID = salesRepList.GetValue(y, i).ToString().Substring(0, 1);
                                    if (GUID == "{")
                                    {
                                        salesRepGUID = salesRepList.GetValue(y, i).ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            salesRepGUID = "";
                        }

                        break;
                        //if (salesRepList.GetValue(3, i) != null)
                        //{
                        //    salesRepGUID = salesRepList.GetValue(3, i).ToString();                            
                        //}
                        //break;  
                    }
                }
                return salesRepGUID;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string ObtenerARAccountGUID(string ARAccountID)
        {
            string GUID = "";
            string aRAccountGUID = "";
            try
            {
                for (int i = 0; i <= glAcctIDList.GetUpperBound(0) - 1; i++)
                {
                    if (glAcctIDList.GetValue(0, i).ToString() == ARAccountID)
                    {
                        try
                        {
                            for (int y = 0; y <= 3; y++)
                            {
                                if (glAcctIDList.GetValue(y, i).ToString().Length > 0)
                                {
                                    GUID = glAcctIDList.GetValue(y, i).ToString().Substring(0, 1);
                                    if (GUID == "{")
                                    {
                                        aRAccountGUID = glAcctIDList.GetValue(y, i).ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            aRAccountGUID = "";
                        }
                        //if (glAcctIDList.GetValue(3, i) != null)
                        //{
                        //    aRAccountGUID = glAcctIDList.GetValue(3, i).ToString();                            
                        //}
                        break;
                    }
                }
                return aRAccountGUID;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void Importfile(string Tipo)
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

        private void ClearForm()
        {
            this.cbClientes.Text = "";
            this.CustVendName.Text = "";
            this.Add1.Text = "";
            this.Add2.Text = "";
            this.City.Text = "";
            this.State.Text = "";
            this.ZIP.Text = "";

            this.txtRUC.Text = "";

            this.lblFecha.Text = "";
            this.lblNumeroFactura.Text = "";
            this.txtCustomerPO.Text = "";
            //this.txtTerminos.Text = "";

            this.cbSalesRepresent.Text = "";
            this.txtAutRetorno.Text = "";
            //this.ARAccount.Text = "";
            //this.arAcctDesc.Text = "";

            this.txtCantidad.Text = "";
            this.cbItems.Text = "";
            this.txtUnidadMedida.Text = "";
            this.txtDescripcion.Text = "";
            this.cbGlacct.Text = "";
            this.txtPrecioUnitario.Text = "";
            this.txtTax.Text = "";

            this.txtCantidadItems.Text = "";
            this.txtTotal.Text = "";
            this.txtITBMS7.Text = "";
            this.txtITBMS10.Text = "";
            this.txtTotalFactura.Text = "";


            this.txtNotaCreditoDate2.Text = DateTime.Now.ToString("MM/dd/yyyy");
            this.txtNotaCreditoDate.Text = this.ObtenerFechaHoraImpresora();
            this.dtDetalleNotaCredito.Clear();
            this.txtTotal.Text = "";


            this.lblBalance.Text = "0.00";
            this.lblCreditLimit.Text = "0.00";
            this.lblCreditStatus.Text = "";
            this.txtTel.Text = "";

            this.txtCustomeNote.Text = "";
            this.txtStatementNote.Text = "";
            this.txtInternalNote.Text = "";

        }

        private void cbClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbClientesChange();
        }

        private void cbClientesChange()
        {
            try
            {
                //****ALEX
                string sClienteSeleccionadoCompleto = this.cbClientes.Text;


                if (sClienteSeleccionadoCompleto.Trim() != "")
                {

                    string[] sClienteSeleccionadoDesglosado = sClienteSeleccionadoCompleto.Split('_');
                    string sClienteSeleccionadoID = sClienteSeleccionadoDesglosado[0];


                    string CreditStatus = "";
                    string Message = "";

                    this.LimpiarDatosCliente();
                    for (int i = 0; i <= custIDList.GetUpperBound(1); i++)
                    {
                        if (custIDList.GetValue(0, i).ToString() == sClienteSeleccionadoID)
                        {
                            //if (custIDList.GetValue(1, i) != null)
                            //    this.CustVendName.Text = custIDList.GetValue(1, i).ToString();
                            //if (custIDList.GetValue(2, i) != null)
                            //    this.Add1.Text = custIDList.GetValue(2, i).ToString();
                            //if (custIDList.GetValue(3, i) != null)
                            //    this.Add2.Text = custIDList.GetValue(3, i).ToString();
                            //if (custIDList.GetValue(4, i) != null)
                            //    this.City.Text = custIDList.GetValue(4, i).ToString();
                            //if (custIDList.GetValue(5, i) != null)
                            //    this.State.Text = custIDList.GetValue(5, i).ToString();
                            //if (custIDList.GetValue(6, i) != null)
                            //    this.ZIP.Text = custIDList.GetValue(6, i).ToString();
                            //if (custIDList.GetValue(7, i) != null)
                            //    this.txtRUC.Text = custIDList.GetValue(7, i).ToString();

                            this.CustVendName.Text = custIDList.GetValue(1, i).ToString();
                            this.Add1.Text = custIDList.GetValue(2, i).ToString();
                            this.Add2.Text = custIDList.GetValue(3, i).ToString();
                            this.City.Text = custIDList.GetValue(4, i).ToString();
                            this.State.Text = custIDList.GetValue(5, i).ToString();
                            this.ZIP.Text = custIDList.GetValue(6, i).ToString();
                            this.txtRUC.Text = custIDList.GetValue(8, i).ToString();
                            cBalance = Convert.ToDouble(custIDList.GetValue(9, i).ToString());
                            this.lblBalance.Text = custIDList.GetValue(9, i).ToString();
                            cCreditLimit = Convert.ToDouble(custIDList.GetValue(7, i).ToString());
                            this.lblCreditLimit.Text = custIDList.GetValue(7, i).ToString();
                            CreditStatus = custIDList.GetValue(11, i).ToString();

                            if (CreditStatus == "0")
                            {
                                cEstatus = "0";
                                Message = "No credit limit";
                            }
                            else
                                if (CreditStatus == "1")
                                {
                                    cEstatus = "1";
                                    Message = "Notify over limit";
                                }
                                else
                                    if (CreditStatus == "2")
                                    {
                                        cEstatus = "2";
                                        Message = "Always Notify";
                                    }
                                    else
                                        if (CreditStatus == "3")
                                        {
                                            cEstatus = "3";
                                            Message = "Hold over limit";
                                        }
                                        else
                                            if (CreditStatus == "4")
                                            {
                                                cEstatus = "4";
                                                Message = "Always hold";
                                            }
                            this.lblCreditStatus.Text = Message;

                            NivelUsuario = Convert.ToInt32(custIDList.GetValue(12, i).ToString());

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void LimpiarDatosCliente()
        {
            this.CustVendName.Text = "";
            this.Add1.Text = "";
            this.Add2.Text = "";
            this.City.Text = "";
            this.State.Text = "";
            this.ZIP.Text = "";
            this.txtTel.Text = "";
        }

        private void dgvDetalleInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string HexAsciiConvert(string hex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
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
                mensaje = HASAR.MandaPaqueteFiscal(handler, "*").ToString();
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

        private void btnCancelarFactura_Click(object sender, EventArgs e)
        {
            this.LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            this.ClearForm();
            this.panelProductos.Enabled = true;
            this.lblNumeroFactura.Text = "";
            this.lblIdentificadorCOO.Text = "";
        }

        private void btnRecargarListados_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            this.panelProductos.Enabled = true;
            this.RecargarFacturas();
        }

        private void RecargarFacturas()
        {
            //this.lvListadoCotizacionesU.Clear();
            //this.dtCotizacionesUtilizadas.Clear();
            //this.ObtenerListadoCotizacionesUtilizados();
            this.lvFacturas.Clear();
            this.ObtenerListadoFacturas();
        }

        private void btnCalcularMontos_Click(object sender, EventArgs e)
        {
            this.Sumar();
        }

        private bool ValidarNumericos()
        {
            bool isNum;
            double retNum;

            int isNumeric = 0;
            double cantRetorno;
            foreach (DataRow dr in dtDetalleNotaCredito.Rows)
            {
                if (dr[3].ToString().Trim() == "")
                {
                    cantRetorno = 0;
                }
                else
                {
                    isNum = Double.TryParse(dr[3].ToString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

                    if(isNum == false)
                    {
                        isNumeric = 1;
                    }
                }
            }

            if (isNumeric == 1)
                return false;
            else
                return true;
        }

        private void Sumar()
        {

            int rowNC = 0;
            double cantRetorno;
            double monto = 0;
            double sumador = 0;
            double sumadorProductos = 0;
            double ITBMS7 = 0;
            double ITBMS10 = 0;
            double ITBMS15 = 0;
            double PrecioU = 0;
            int tax = 0;
            if (ValidarNumericos())
            {
                foreach (DataRow dr in dtDetalleNotaCredito.Rows)
                {
                    monto = 0;
                    if (dr[3].ToString().Trim() == "")
                    {
                        cantRetorno = 0;
                    }
                    else
                    {
                        cantRetorno = Convert.ToDouble(dr[3].ToString());
                    }
                    PrecioU = Convert.ToDouble(dr[6].ToString());
                    monto = PrecioU * cantRetorno;
                    dtDetalleNotaCredito.Rows[rowNC][8] = string.Format("{0:#,#0.00}", monto);

                    rowNC = rowNC + 1;
                    sumador = sumador + monto;
                    sumadorProductos = sumadorProductos + cantRetorno;

                    tax = Convert.ToInt32(dr[7].ToString());
                    if (tax == 1)
                    {
                        ITBMS7 = ITBMS7 + (monto * 0.07);
                    }

                    if (impuestoITBMS10Habilitado == "Habilitado")
                    {
                        if (tax == Convert.ToInt32(impuestoITBMS10TaxType))
                        {
                            ITBMS10 = ITBMS10 + (monto * 0.1);
                        }
                    }
                    else
                    {
                        ITBMS10 = 0;
                    }

                    if (impuestoITBMS15Habilitado == "Habilitado")
                    {
                        if (tax == Convert.ToInt32(impuestoITBMS15TaxType))
                        {
                            ITBMS15 = ITBMS15 + (monto * 0.15);
                        }
                    }
                    else
                    {
                        ITBMS15 = 0;
                    }
                }

                //txtTotal.Text = string.Format("{0:#,#.00}", sumador);        
                //txtCantidadItems.Text = string.Format("{0:#,#.00}", sumadorProductos);        
                //txtITBMS7.Text = string.Format("{0:#,#.00}", ITBMS7);        
                //txtITBMS10.Text = string.Format("{0:#,#.00}", ITBMS10);            

                //txtTotalFactura.Text = string.Format("{0:#,#.00}", sumador + (ITBMS7 + ITBMS10));

                this.txtTotal.Text = string.Format("{0:#,#0.00}", sumador);
                this.txtCantidadItems.Text = string.Format("{0:#,#0.000}", sumadorProductos);
                this.txtITBMS7.Text = string.Format("{0:#,#0.00}", ITBMS7);

                string itbmsotros = string.Format("{0:#,#0.00}", ITBMS7);
                this.txtITBMS10.Text = string.Format("{0:#,#0.00}", Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS10)) + Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS15)));
                this.txtTotalFactura.Text = string.Format("{0:#,#0.00}", (sumador + (Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS7)) + Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS10)) + Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS15)))));
            }
            else
            {
                MessageBox.Show("Existe un valor devuelto el cual no es numerico y no se puede realizar el calculo","Valor no numerico",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void cbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CambiarItem();
        }

        private void CambiarItem()
        {
            string sDescripcionProducto = "";
            string s = this.cbItems.Text;
            if (s.Trim() != "")
            {
                string[] ss = s.Split('_');
                string itemID = ss[0];
                for (int i = 0; i <= itemIDList.GetUpperBound(1); i++)
                {
                    if (itemIDList.GetValue(0, i).ToString() == itemID)
                    {
                       // this.txtDescripcion.Text = itemIDList.GetValue(2, i).ToString();
                        sDescripcionProducto = itemIDList.GetValue(1, i).ToString();
                        
                        
                        
                        if (itemIDList.GetValue(2, i).ToString().Trim() != "")
                        {
                            this.txtDescripcion.Text = itemIDList.GetValue(2, i).ToString();
                            //this.txtDescripcion.Text = sDescripcionProducto;
                        }
                        else
                        {
                            this.txtDescripcion.Text = sDescripcionProducto;
                        }

                        //this.txtUnidadMedida.Text = itemIDList.GetValue(7, i).ToString();

                        //this.txtDescripcion.Text = itemIDList.GetValue(2, i).ToString();
                        //this.txtPrecioUnitario.Text = itemIDList.GetValue(3, i).ToString();
                        this.txtUnidadMedida.Text = itemIDList.GetValue(7, i).ToString();


                        this.txtPrecioUnitario.Items.Clear();
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(3, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(8, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(9, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(10, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(11, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(12, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(13, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(14, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(15, i).ToString());
                        this.txtPrecioUnitario.Items.Add(itemIDList.GetValue(16, i).ToString());
                        this.txtPrecioUnitario.Text = itemIDList.GetValue(3, i).ToString();


                            txtPrecioUnitario.SelectedIndex = NivelUsuario;
                        

                        this.cbGlacct.Text = this.fRetornarCuentaGLCompleta(itemIDList.GetValue(5, i).ToString());

                        this.txtTax.Text = itemIDList.GetValue(4, i).ToString();
                        break;
                    }
                }
            }
        }

        private string fRetornarCuentaGLCompleta(string sCuentaGLID)
        {
            string sCuentaGLCompleta = "";
            for (int i = 0; i <= glAcctIDList.GetUpperBound(1); i++)
            {
                if (glAcctIDList.GetValue(0, i).ToString() == sCuentaGLID)
                {
                    sCuentaGLCompleta = glAcctIDList.GetValue(0, i).ToString() + "_" + glAcctIDList.GetValue(1, i).ToString();
                    break;
                }
            }
            return sCuentaGLCompleta;
        }



        private void ARAccount_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //for (int i = 0; i <= glAcctIDList.GetUpperBound(1); i++)
            //{
            //    if (glAcctIDList.GetValue(0, i).ToString() == this.ARAccount.Text)
            //    {
            //        this.arAcctDesc.Text = glAcctIDList.GetValue(1, i).ToString();
            //        break;
            //    }
            //}
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (ValidarItems())
            {
                DataRow drDetalleInvoice = dtDetalleNotaCredito.NewRow();

                if (this.txtCantidad.Text == "")
                {
                    this.txtCantidad.Text = "1.00";
                    drDetalleInvoice["Cantidad"] = "1.00";
                    drDetalleInvoice["Retorno"] = "1.00";
                }
                else
                {
                    double prueba3 = Convert.ToDouble(this.txtCantidad.Text);
                    drDetalleInvoice["Cantidad"] = string.Format("{0:#,#0.000}", prueba3);
                    drDetalleInvoice["Retorno"] = string.Format("{0:#,#0.00}", prueba3);
                    
                }

                string s = this.cbItems.Text;
                string[] ss = s.Split('_');
                string itemID = ss[0];

                drDetalleInvoice["Items"] = itemID;
                drDetalleInvoice["UnidadMedida"] = this.txtUnidadMedida.Text;
                drDetalleInvoice["Descripcion"] = this.txtDescripcion.Text;
                //drDetalleInvoice["GLAccount"] = this.cbGlacct.Text;


                string sGLAccountSeleccionadoCompleto = this.cbGlacct.Text;
                string[] sGLAccountSeleccionadoDesglosado = sGLAccountSeleccionadoCompleto.Split('_');
                string sGLAccountSeleccionadoID = sGLAccountSeleccionadoDesglosado[0];


                drDetalleInvoice["GLAccount"] = sGLAccountSeleccionadoID;
                
                double prueba = Convert.ToDouble(this.txtPrecioUnitario.Text);
                drDetalleInvoice["PrecioUnitario"] = string.Format("{0:#,#0.000}", prueba);

                if (this.txtTax.Text == "")
                {
                    this.txtTax.Text = "1";
                    drDetalleInvoice["Tax"] = "1";
                }
                else
                {
                    drDetalleInvoice["Tax"] = this.txtTax.Text;
                }
                double prueba2 = Convert.ToDouble(this.txtCantidad.Text) * Convert.ToDouble(this.txtPrecioUnitario.Text); ;

                drDetalleInvoice["Monto"] = string.Format("{0:#,#0.00}", prueba2); //Convert.ToInt32(this.txtCantidad.Text) * Convert.ToDouble(this.txtPrecioUnitario.Text);

                dtDetalleNotaCredito.Rows.Add(drDetalleInvoice);
                dtDetalleNotaCredito.AcceptChanges();

                this.limpiarAddItem();
                this.Sumar();
            }
        }

        private bool ValidarItems()
        {
            if (this.txtDescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Debe introducir una descripcion");
                return false;
            }

            if (this.txtPrecioUnitario.Text.Trim() == "")
            {
                MessageBox.Show("Debe introducir un precio unitario");
                return false;
            }

            if (this.cbGlacct.Text.Trim() == "")
            {
                MessageBox.Show("Debe introducir una cuenta contable");
                return false;
            }
            return true;
        }

        private void limpiarAddItem()
        {
            this.txtCantidad.Text = "";
            this.cbItems.Text = "";
            this.txtDescripcion.Text = "";
            this.txtUnidadMedida.Text = "";
            this.cbGlacct.Text = "";
            this.txtPrecioUnitario.Text = "";
            this.txtTax.Text = "";
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            bool isDec = false;
            int nroDec = 0;

            for (int i = 0; i < this.txtCantidad.Text.Length; i++)
            {
                if (this.txtCantidad.Text[i] == '.')
                {
                    isDec = true;
                }
                if (isDec && nroDec++ >= 3)
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (isDec) ? true : false;
            else
                e.Handled = true;        
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            bool isDec = false;
            int nroDec = 0;

            for (int i = 0; i < this.txtPrecioUnitario.Text.Length; i++)
            {
                if (this.txtPrecioUnitario.Text[i] == '.')
                {
                    isDec = true;
                }
                if (isDec && nroDec++ >= 3)
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (isDec) ? true : false;
            else
                e.Handled = true;
        }

        private bool ValidarCamposObligatoriosNotaCredito()
        {
            string Errores = "";
            int validar = 0;
            string customerID = this.cbClientes.Text;
            if (customerID.Trim() == "")
            {
                validar = 1;
                Errores += "Debe seleccionar un cliente del listado." + '\x0D';
            }

            string aRAccount = this.ARAccount.Text;
            if (aRAccount.Trim() == "")
            {
                validar = 1;
                Errores += "Debe seleccionar una cuenta por cobrar." + '\x0D';
            }

            int contadorDetalle = this.dgvDetalleInvoice.Rows.Count;
            if (contadorDetalle.Equals(1))
            {
                validar = 1;
                Errores += "Debe tener al menos una linea de detalle de factura." + '\x0D';
            }

            if (validar == 1)
            {
                System.Windows.Forms.MessageBox.Show(Errores, "Validar campos obligatorios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidarPeachtree()
        {
            try
            {
                string pathCompaniaUsuario = MOFFIS.frmPrincipal.pathCompaniaUsuario;
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

        private void IndicadorError(string Estado)
        {
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\BSC\IndicadorError.xml";
            //string PathListado2 = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("Indicador_Error");
            Writer.WriteStartElement("IndicadorError");
            Writer.WriteString(Estado);
            Writer.WriteEndElement();
            Writer.WriteEndElement();

            Writer.Close();
        }

        private bool ValidarStatusError(int mostrarMessage)
        {
            // validar q archivo de status de error exista
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\BSC\IndicadorError.xml";
            //string PathListado2 = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";

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
                }
                else
                {
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


        private void CreateXMLFileAnulado(int SegundaOpcion)
        {
            int lineaDesc = 0;
            int cantLineas = 0;
            int cantLineas2 = 0;
            int numberOfDistributions = 0;
            //int numdist = 0;
            string cantidad;

            string itemID;
            string UM;
            string Descripcion;
            string GLAcc;
            string precioU;
            string Taxtype;
            string monto;
            string valorRetorno;

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";
            //string PathListado2 = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_Invoices");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_Invoice");
            Writer.WriteAttributeString("xsi:type", "paw:invoice");

            string customerID = this.cbClientes.Text;
            string customerGUID = "";
            string customerName = "";
            //Cliente ID
            Writer.WriteStartElement("Customer_ID");
            Writer.WriteAttributeString("xsi:type", "paw:id");
            Writer.WriteString(customerID);
            Writer.WriteEndElement();
            //Cliente GUID
            customerGUID = this.ObtenerCustomerGUID(customerID);
            if (customerGUID != "")
            {
                Writer.WriteElementString("Customer_GUID", customerGUID);
            }
            //Cliente Nombre
            customerName = this.CustVendName.Text;
            Writer.WriteElementString("Customer_Name", this.CustVendName.Text);
            //Numero NotaCredito
            string numeroNotaCredito = "NC-" + this.lblNumeroNC.Text.Trim() + "_Anulada";
            if (SegundaOpcion.Equals(1))
            {
                numeroNotaCredito = numeroNotaCredito + "-2";
            }
            Writer.WriteElementString("Invoice_Number", numeroNotaCredito);
            //Fecha NotaCredito
            Writer.WriteStartElement("Date");
            Writer.WriteAttributeString("xsi:type", "paw:date");
            Writer.WriteString(this.txtNotaCreditoDate.Text);
            Writer.WriteEndElement();
            //Es Cotizacion?
            Writer.WriteElementString("isQuote", "FALSE");
            //Tiene Drop Ship                
            Writer.WriteElementString("Drop_Ship", "FALSE");

            //ShipToAddress
            Writer.WriteStartElement("ShipToAddress");
            Writer.WriteElementString("Name", this.CustVendName.Text);
            Writer.WriteElementString("Line1", this.Add1.Text);
            Writer.WriteElementString("Line2", this.Add2.Text);
            Writer.WriteElementString("City", this.City.Text);
            Writer.WriteElementString("State", this.State.Text);
            Writer.WriteElementString("Zip", this.State.Text);
            Writer.WriteStartElement("Sales_Tax_Code");
            Writer.WriteAttributeString("xsi:type", "paw:id");
            Writer.WriteString(impuestoITBMSTaxID);
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            //Customer PO
            if (this.txtCustomerPO.Text.Trim() != "")
            {
                Writer.WriteElementString("Customer_PO", this.txtCustomerPO.Text);
            }
            //Date Due
            ////<Date_Due xsi:type="paw:date">3/31/11</Date_Due> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  
            //Discount Amount
            Writer.WriteElementString("Discount_Amount", "0.00");
            //Discount Date
            ////<Discount_Date xsi:type="paw:date">3/15/11</Discount_Date> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  

            //Campo utilizado para indicar el tipo documento 
            Writer.WriteElementString("Displayed_Terms", "DOCNC");

            string salesRepID = this.cbSalesRepresent.Text;
            string salesRepGUID = "";
            if (salesRepID != "")
            {
                //Sales Rep ID
                Writer.WriteStartElement("Sales_Representative_ID");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(salesRepID);
                Writer.WriteEndElement();
                //Sales Rep GUID
                salesRepGUID = this.ObtenerSalesRepGUID(salesRepID);
                if (salesRepGUID != "")
                {
                    Writer.WriteElementString("Sales_Rep_GUID", salesRepGUID);
                }
            }

            string aRAccountID = this.ARAccount.Text.ToString();
            string aRAccountGUID = "";
            if (aRAccountID != "")
            {
                //ARAccount ID            
                Writer.WriteStartElement("Accounts_Receivable_Account");
                Writer.WriteAttributeString("xsi:type", "paw:id");
                Writer.WriteString(aRAccountID);
                Writer.WriteEndElement();
                //ARAccount GUID
                aRAccountGUID = this.ObtenerARAccountGUID(aRAccountID);
                if (aRAccountGUID != "")
                {
                    Writer.WriteElementString("AR_Account_GUID", aRAccountGUID);
                }
            }
            //Accounts Receivable Monto
            double accRAmount = 0;
            Writer.WriteElementString("Accounts_Receivable_Amount", "0.00");
            //Note Prints After Line Items
            Writer.WriteElementString("Note_Prints_After_Line_Items", "FALSE");
            //Statement Note Prints Before Ref
            Writer.WriteElementString("Statement_Note_Prints_Before_Ref", "FALSE");
            //Beginning Balance Transaction
            Writer.WriteElementString("Beginning_Balance_Transaction", "FALSE");

            //<Transaction_Period>21</Transaction_Period> 
            //<Transaction_Number>31</Transaction_Number> 
            //<GUID>{758EB4D4-BD8E-4EEC-A3E1-EF7561AE2AE5}</GUID> 

            ////Aplica a factura
            //if (this.lblNumeroFactura.Text != "")
            //{
            //    Writer.WriteElementString("ApplyToInvoiceNumber", this.lblNumeroFactura.Text);
            //}
            //Es Nota Credito
            Writer.WriteElementString("CreditMemoType", "TRUE");
            //ProgressBillingInvoice
            Writer.WriteElementString("ProgressBillingInvoice", "FALSE");



            Writer.WriteElementString("Number_of_Distributions", "2");
            Writer.WriteStartElement("SalesLines");

            //ITBMS
            Writer.WriteStartElement("SalesLine");
            Writer.WriteElementString("Quantity", "0.00000");
            Writer.WriteElementString("SalesOrderDistributionNumber", "0");
            Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
            Writer.WriteElementString("Apply_To_Proposal", "FALSE");
            Writer.WriteElementString("InvoiceCMDistribution", "0");
            Writer.WriteElementString("Description", impuestoITBMS7TaxDescription);
            Writer.WriteStartElement("GL_Account");
            Writer.WriteAttributeString("xsi:type", "paw:id");
            Writer.WriteString(impuestoITBMS7TaxAccountId);
            Writer.WriteEndElement();
            Writer.WriteElementString("GL_Account_GUID", impuestoITBMSTaxAccountIdGUID);
            Writer.WriteElementString("Unit_Price", "0.00000");
            Writer.WriteElementString("Tax_Type", "0");
            Writer.WriteElementString("Weight", "0.00000");
            string itbms = "0.00";
            Writer.WriteElementString("Amount", itbms);//*****************************                
            Writer.WriteElementString("Cost_of_Sales_Amount", "0.00");
            Writer.WriteElementString("Retainage_Percent", "0.00");
            Writer.WriteElementString("UM_Stocking_Units", "0.00000");
            Writer.WriteElementString("Stocking_Quantity", "0.00000");
            Writer.WriteElementString("Stocking_Unit_Price", "0.00000");
            Writer.WriteElementString("Sales_Tax_Authority", impuestoITBMS7TaxID);
            Writer.WriteEndElement();//closes the sales line element
                  
            Writer.WriteStartElement("SalesLine");                    
            Writer.WriteElementString("Quantity", "0.00");                    
            Writer.WriteElementString("SalesOrderDistributionNumber", "0");                   
            Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");                    
            Writer.WriteElementString("Apply_To_Proposal", "FALSE");                    
            Writer.WriteElementString("InvoiceCMDistribution", "1");                    
            Writer.WriteElementString("Description", "NOTA CREDITO ANULADA");                    
            Writer.WriteStartElement("GL_Account");                    
            Writer.WriteAttributeString("xsi:type", "paw:ID");                    
            Writer.WriteString("40100");                    
            Writer.WriteEndElement();                    
            Writer.WriteElementString("Unit_Price", "0.00");
            Writer.WriteElementString("Tax_Type", "1");
            Writer.WriteElementString("Amount", "0.00");                    
            Writer.WriteEndElement();//closes the sales line element


            Writer.WriteEndElement();//Closes the Sales Lines element

            Writer.WriteEndElement();//Closes the paw_invoice element

            Writer.WriteEndElement();//closes the paw_invoices element and ends the document

            Writer.Close();
        }

        private void LeerValoresDefault()
        {
            // validar q archivo de status de error exista
            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();

            string PathListado = "";

            if (IDcomp == "1")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ValoresDefault1.xml";

            }
            else if (IDcomp == "2")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ValoresDefault2.xml";

            }
            else if (IDcomp == "3")
            {

                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ValoresDefault3.xml";
            }
            
            
            //string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Default\ValoresDefault.xml";
            //string PathListado2 = PathMoffis + @"\XML\NotaCredito\AnularNotaCredito.xml";

            if (System.IO.File.Exists(PathListado))
            {
                imp = new XmlImplementation();
                doc = imp.CreateDocument();
                doc.Load(PathListado);

                reader = doc.GetElementsByTagName("PAW_ValorDeafult");
                //DescuentosList = Array.CreateInstance(typeof(string), 7, reader.Count);
                //this.cbDescuentos.Items.Add("");
                for (int i = 0; i <= reader.Count - 1; i++)
                {
                    for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                    {
                        switch (reader[i].ChildNodes[a].Name)
                        {
                            case "CuentaAnulacion":
                                {
                                    CuentaAnulacion = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "CuentaAR":
                                {
                                    //CuentaAR = reader[i].ChildNodes[a].InnerText;
                                    //this.ARAccount.Text = CuentaAR;

                                    CuentaAR = reader[i].ChildNodes[a].InnerText;
                                    //CuentaARD = reader[i].ChildNodes[a].InnerText;


                                    for (int iContItems = 0; iContItems < this.ARAccount.Items.Count; ++iContItems)
                                    {

                                        string sItemCompleto = this.ARAccount.Items[iContItems].ToString();
                                        string[] sItemDesglosado = sItemCompleto.Split('_');
                                        string aRAccountID = sItemDesglosado[0];
                                        if (aRAccountID == CuentaAR)
                                        {
                                            this.ARAccount.SelectedIndex = iContItems;
                                            break;
                                        }
                                    }
                                    
                                    break;
                                }
                            case "CuentaEfectivo":
                                {
                                    CuentaEfectivo = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "CuentaCheque":
                                {
                                    CuentaCheque = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "CuentaTarjeta":
                                {
                                    CuentaTarjeta = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "ModificarPrecios":
                                {
                                    ModificarPrecios = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "Pie":
                                {
                                    PieDocumento = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "CambiosEspeciales":
                                {
                                    CambioEspecial = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "CodigoProducto":
                                {
                                    CodigoProducto = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                        }
                    }
                }
                imp = null;
                doc = null;
                reader = null;
            }
            else
            {
                MessageBox.Show("Debe configurar los valores default del sistema, porfavor notifique al administrador del sistema", "Valores default in crear", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            bool isDec = false;
            int nroDec = 0;

            for (int i = 0; i < this.txtTax.Text.Length; i++)
            {
                if (this.txtTax.Text[i] == '.')
                {
                    isDec = true;
                }
                if (isDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (isDec) ? true : false;
            else
                e.Handled = true;
        }

    }
}
