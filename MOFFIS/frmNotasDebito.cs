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
    public partial class frmNotasDebito : Form
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

        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;

        private Array custIDList;
        private Array salesRepList;
        private Array itemIDList;
        private Array glAcctIDList;
        private Array shippingMethodList;
        private Array ImpuestosList;
        private Array DescuentosList;

        //Seccion de Impuestos: variables asiganadas a impuestos [porcentaje, estado, tax type]
        private string Impuesto1Porcentaje;
        private string Impuesto2Porcentaje;
        private string Impuesto1Status;
        private string Impuesto2Status;
        private string Impuesto1TaxType;
        private string Impuesto2TaxType;

        private string sCustomerId;
        private string NumeroCuponCOO;
        private string cEstatus;
        private string CuentaAnulacion;
        private string CuentaDescuento;
        private string CuentaAR;
        private string CuentaEfectivo;
        private string CuentaCheque;
        private string CuentaTarjeta;
        private string ModificarPrecios;


        private string CambioEspecial;
        private string CodigoProducto;

        DataTable dtDetalleNotaDebito;

        private int ControladorError;
        int seleccion;
        double cBalance;
        double cCreditLimit;

        double sumadorMonto = 0;
        double totalFactura = 0;
        int handler;
        char FS = Convert.ToChar(28);
        char etx = Convert.ToChar(3);

        char FS2 = Convert.ToChar(128);
        int init;
        private string FechaImp;
        private string HoraImp;

        private static frmNotasDebito  m_FrmNotasDebito;

        string valorc = "";


        //nuevo multiempresa
        static public string IDcomp;

        private int NivelUsuario = 0;

        public string IDcompania
        {
            get { return frmNotasDebito.IDcomp; }
            set { frmNotasDebito.IDcomp = value; }
        }

        static public string PuertoImpresora;

        public string PuertoImp
        {
            get { return frmNotasDebito.PuertoImpresora; }
            set { frmNotasDebito.PuertoImpresora = value; }
        }

        public static frmNotasDebito DefInstance
        {
            get
            {
                if (m_FrmNotasDebito == null || m_FrmNotasDebito.IsDisposed)
                    m_FrmNotasDebito = new frmNotasDebito();
                return m_FrmNotasDebito;
            }
            set
            {
                m_FrmNotasDebito = value;
            }
        }

        //public Tfhka Tf
        //{
        //    get { return frmPrincipal.tf; }
        //    set { frmPrincipal.tf = value; }
        //}

        public frmNotasDebito()
        {
            InitializeComponent();
            this.CrearDataTable();
            this.ObtenerListadoClientes();
            this.ObtenerListadoShipVias();
            this.ObtenerListadoSalesRepresent();
            this.ObtenerListadoCuentasGL();
            this.ObtenerListadoItems();
            this.LeerImpuestos();
            this.ObtenerGUIDImpuestos();
            this.LeerDescuentos();
            this.LeerValoresDefault();
        }

        public void CrearDataTable()
        {
            dtDetalleNotaDebito = new DataTable();
            dtDetalleNotaDebito.Columns.Add(new DataColumn("Cantidad", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("Items", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("UnidadMedida", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("Descripcion", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("GLAccount", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("PrecioUnitario", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("Tax", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.Columns.Add(new DataColumn("Monto", System.Type.GetType("System.String")));
            dtDetalleNotaDebito.AcceptChanges();
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
            string PathListado = PathMoffis + @"\XML\NotaDebito\ListadoClientes.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoClientes2.xml";

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

        private void ObtenerListadoShipVias()
        {
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjShippingMethods);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjShippingMethodsField.peachwIEObjShippingMethodsField_GUID);
            exportador.AddToExportFieldList((short)PeachwIEObjShippingMethodsField.peachwIEObjShippingMethodsField_ShippingMethod);
            exportador.AddToExportFieldList((short)PeachwIEObjShippingMethodsField.peachwIEObjShippingMethodsField_ShippingMethodNumber);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaDebito\ListadoShippingMethods.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoShippingMethods2.xml";

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
            //CSSDK.GLInformation accttype = new CSSDK.GLInformation();

            reader = doc.GetElementsByTagName("PAW_Shipping_Method");
            shippingMethodList = Array.CreateInstance(typeof(string), 3, reader.Count + 1);
            shippingMethodList.SetValue("", 0, 0);
            shippingMethodList.SetValue("", 1, 0);
            shippingMethodList.SetValue("", 2, 0);

            this.cbShipVia.Items.Add("");

            for (int i = 0; i <= reader.Count - 1; i++)
            {
                foreach (XmlNode node in reader[i].ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Number":
                            {
                                shippingMethodList.SetValue(node.InnerText, 0, i + 1);
                                break;
                            }
                        case "ShippingMethod":
                            {
                                shippingMethodList.SetValue(node.InnerText, 1, i + 1);
                                cbShipVia.Items.Add(node.InnerText);
                                break;
                            }
                        case "GUID":
                            {
                                shippingMethodList.SetValue(node.InnerText, 2, i + 1);
                                break;
                            }
                    }
                }
            }
        }

        private void ObtenerListadoSalesRepresent()
        {
            string salesRep = "";
            string isSalesRep = "N";
            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjEmployeeList);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_EmployeeID);
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_EmployeeName);
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_SalesRep);
            exportador.AddToExportFieldList((short)PeachwIEObjEmployeeListField.peachwIEObjEmployeeListField_GUID);
            //exportador.SetFilterValue((short)PeachwIEObjEmployeeListFilter.peachwIEObjEmployeeListFilter_EmployeeOrSalesRep, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "TRUE", "TRUE");

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaDebito\ListadoSalesRepresent.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoSalesRepresent2.xml";

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
                    this.cbSalesRepresent.Items.Add(salesRep);
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
                string PathListado = PathMoffis + @"\XML\NotaDebito\ListadoAccounts.xml";
                string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoAccounts2.xml";

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

            //exportador.SetFilterValue((short)PeachwIEObjInventoryItemsListFilter.peachwIEObjInventoryItemsListFilter_ItemClassToExclude, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "ItemClassSerialized", "ItemClassSerialized");
            //exportador.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange, fecha1, fecha2);
            //exportador.SetFilterValue((short)PeachwIEObjSalesJournalFilter.peachwIEObjSalesJournalFilter_TransactionType, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "Quote", "Quote");
            //exportador.SetFilterValue((short)PeachwIEObjSalesJournalFilter.peachwIEObjSalesJournalFilter_InvoiceNumber, PeachwIEFilterOperation.peachwIEFilterOperationEqualTo, "000004", "000004");

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaDebito\ListadoItems.xml";
            string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoItems2.xml";

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
                    }
                    n = n + 1;
                }
                if (esInactivo == "FALSE")
                {
                    itemIDList.SetValue(itemID, 0, v + 1);
                    itemIDList.SetValue(descripcionCorta, 1, v + 1);
                    itemIDList.SetValue(descripcionLarga, 2, v + 1);
                    itemIDList.SetValue(precioUnitario, 3, v + 1);
                    taxReal = (Convert.ToInt32(taxType) + 1).ToString();
                    itemIDList.SetValue(taxReal, 4, v + 1);
                    itemIDList.SetValue(salesAccount, 5, v + 1);
                    itemIDList.SetValue(salesAccountGUID, 6, v + 1);
                    itemIDList.SetValue(unidadMedida, 7, v + 1);


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
            //string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoItems2.xml";

            imp = new XmlImplementation();
            doc = imp.CreateDocument();
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
            for (int i = 1; i <= glAcctIDList.GetUpperBound(1); i++)
            {
                if (glAcctIDList.GetValue(0, i).ToString() == impuestoAccount)
                {
                    impuestoITBMSTaxAccountIdGUID = glAcctIDList.GetValue(3, i).ToString();
                    break;
                    //lblpruebaaccountguid.Text = impuestoITBMSTaxAccountIdGUID;
                }
            }
        }

        private void LeerDescuentos()
        {
            imp = new XmlImplementation();
            doc = imp.CreateDocument();

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();



            string PathListado = "";

            if (IDcomp == "1")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos1.xml";

            }
            else if (IDcomp == "2")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos2.xml";

            }
            else if (IDcomp == "3")
            {
                PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos3.xml";

            }
            
            
            
            //string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\Descuentos\Descuentos.xml";

            doc.Load(PathListado);

            reader = doc.GetElementsByTagName("PAW_Descuento");
            DescuentosList = Array.CreateInstance(typeof(string), 7, reader.Count);
            this.cbDescuentos.Items.Add("");
            for (int i = 0; i <= reader.Count - 1; i++)
            {
                for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                {
                    switch (reader[i].ChildNodes[a].Name)
                    {
                        case "Monto":
                            {
                                cbDescuentos.Items.Add(reader[i].ChildNodes[a].InnerText);
                                break;
                            }
                    }
                }
            }

            imp = null;
            doc = null;
            reader = null;
        }

        private void frmNotasDebito_Load(object sender, EventArgs e)
        {
            //string Puerto = ConfigurationSettings.AppSettings["PuertoImpresora"].ToString();
            //Tf.CloseFpctrl();
            //bool port_status = Tf.OpenFpctrl(Puerto);

            this.notaDebitoDate2.Text = DateTime.Now.ToString("MM/dd/yyyy");
            this.notaDebitoDate.Text = this.ObtenerFechaHoraImpresora();
            this.dgvDetalleInvoice.DataSource = dtDetalleNotaDebito;
            this.LeerNumNotaDebito();
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
                    return "BSC123456";
                }
            }
            catch (Exception ex)
            {
                return "BSC123456";
            }
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

        private string ObtenerFechaHoraImpresora()
        {
            try
            {
                string hora, min, seg, tiempo, mensaje, SImp, SFis;
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
                        FechaImp = mesImpresora + "/" + diaImpresora + "/" + anioImpresora;
                    }

                    return FechaImp;
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


        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (ValidarItems())
            {
                string cantidadAP = "";

                if (this.txtCantidad.Text.Trim() == "")
                {
                    this.txtCantidad.Text = "1.00";
                    cantidadAP = "1.00";
                }
                else
                {
                    double prueba3 = Convert.ToDouble(this.txtCantidad.Text);
                    cantidadAP = string.Format("{0:#,#0.000}", prueba3);
                }

                double prueba2 = Convert.ToDouble(cantidadAP) * Convert.ToDouble(this.txtPrecioUnitario.Text);

                if (prueba2 >= 0.01)
                {
                    DataRow drDetalleNotaDebito = dtDetalleNotaDebito.NewRow();
                    string s = this.cbItems.Text;
                    string[] ss = s.Split('_');
                    string itemID = ss[0];

                    drDetalleNotaDebito["Cantidad"] = cantidadAP;
                    drDetalleNotaDebito["Items"] = itemID;
                    drDetalleNotaDebito["UnidadMedida"] = this.txtUnidadMedida.Text;
                    drDetalleNotaDebito["Descripcion"] = this.txtDescripcion.Text;


                    //ALEX
                    string sCuentaGLCompleta = this.cbGlacct.Text;
                    string[] sCuentaGLDesglozada = sCuentaGLCompleta.Split('_');
                    string sCuentaGLID = sCuentaGLDesglozada[0];
                    drDetalleNotaDebito["GLAccount"] = sCuentaGLID;
                    //drDetalleNotaDebito["GLAccount"] = this.cbGlacct.Text;
                    double prueba = Convert.ToDouble(this.txtPrecioUnitario.Text);
                    drDetalleNotaDebito["PrecioUnitario"] = string.Format("{0:#,#0.000}", prueba);

                    if (this.txtTax.Text == "")
                    {
                        this.txtTax.Text = "1";
                        drDetalleNotaDebito["Tax"] = "1";
                    }
                    else
                    {
                        drDetalleNotaDebito["Tax"] = this.txtTax.Text;
                    }


                    //Decimal d = Convert.ToDecimal(prueba2);
                    //string dec = d.ToString();
                    //string[] partes = dec.Split(',');
                    //string decimales = partes[1].Substring(0,(partes[1].Length>=2?2:partes[1].Length));
                    ////string decimales = partes[1].Substring(0,2);
                    //decimal resultado = Convert.ToDecimal(string.Format({0}{1}, partes[0], decimales));


                    drDetalleNotaDebito["Monto"] = string.Format("{0:#,#0.00}", prueba2); //Convert.ToInt32(this.txtCantidad.Text) * Convert.ToDouble(this.txtPrecioUnitario.Text);

                    dtDetalleNotaDebito.Rows.Add(drDetalleNotaDebito);
                    dtDetalleNotaDebito.AcceptChanges();

                    this.limpiarAddItem();
                    this.Sumar();
                }
                else
                {
                    MessageBox.Show("El valor del monto no puede ser menor a (0.01), revisar valores de cantidad y precio unitario)", "Valor Invalido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //DataRow drDetalleNotaDebito = dtDetalleNotaDebito.NewRow();

                //if (this.txtCantidad.Text == "")
                //{
                //    this.txtCantidad.Text = "1.00";
                //    drDetalleNotaDebito["Cantidad"] = "1.00";
                //}
                //else
                //{
                //    double prueba3 = Convert.ToDouble(this.txtCantidad.Text);
                //    drDetalleNotaDebito["Cantidad"] = string.Format("{0:#,#.00}", prueba3);
                //}

                //string s = this.cbItems.Text;
                //string[] ss = s.Split('_');
                //string itemID = ss[0];

                //drDetalleNotaDebito["Items"] = itemID;
                //drDetalleNotaDebito["UnidadMedida"] = this.txtUnidadMedida.Text;
                //drDetalleNotaDebito["Descripcion"] = this.txtDescripcion.Text;
                //drDetalleNotaDebito["GLAccount"] = this.cbGlacct.Text;
                //double prueba = Convert.ToDouble(this.txtPrecioUnitario.Text);
                //drDetalleNotaDebito["PrecioUnitario"] = string.Format("{0:#,#.00}", prueba);

                //if (this.txtTax.Text == "")
                //{
                //    this.txtTax.Text = "1";
                //    drDetalleNotaDebito["Tax"] = "1";
                //}
                //else
                //{
                //    drDetalleNotaDebito["Tax"] = this.txtTax.Text;
                //}
                //double prueba2 = Convert.ToDouble(this.txtCantidad.Text) * Convert.ToDouble(this.txtPrecioUnitario.Text); ;

                //drDetalleNotaDebito["Monto"] = string.Format("{0:#,#.00}", prueba2);

                //dtDetalleNotaDebito.Rows.Add(drDetalleNotaDebito);
                //dtDetalleNotaDebito.AcceptChanges();

                //this.limpiarAddItem();
                //this.Sumar();
            }
        }

        private bool ValidarItems()
        {
            if (this.txtCantidad.Text.Trim() == "")
            {

            }
            else
                if (Convert.ToDouble(this.txtCantidad.Text.Trim()) <= 0)
                {
                    MessageBox.Show("La cantidad no puede ser igual ni menor a cero (0)", "Valor Invalido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            if (this.txtDescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Debe introducir una descripcion", "Valor Invalido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.txtPrecioUnitario.Text.Trim() == "")
            {
                MessageBox.Show("Debe introducir un precio unitario", "Valor Invalido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //else
            //    if (Convert.ToDouble(this.txtPrecioUnitario.Text.Trim()) <= 0)
            //    {
            //        MessageBox.Show("El precio unitario no puede ser igual ni menor a cero (0)", "Valor Invalido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return false;
            //    }

            if (this.cbGlacct.Text.Trim() == "")
            {
                MessageBox.Show("Debe introducir una cuenta contable", "Valor Invalido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ARAccount_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CambiarItem();
        }

        private void CambiarItem()
        {
            string s = this.cbItems.Text;
            if (s.Trim() != "")
            {
                string[] ss = s.Split('_');
                string itemID = ss[0];
                for (int i = 0; i <= itemIDList.GetUpperBound(1); i++)
                {
                    if (itemIDList.GetValue(0, i).ToString() == itemID)
                    {
                        this.txtDescripcion.Text = itemIDList.GetValue(2, i).ToString();
                        
                        
                        
                        this.txtPrecioUnitario.Text = itemIDList.GetValue(3, i).ToString();
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




                            txtPrecioUnitario.SelectedIndex = NivelUsuario;

                        
                        
                        //this.cbGlacct.Text = itemIDList.GetValue(5, i).ToString();

                        this.cbGlacct.Text = this.fRetornarCuentaGLCompleta(itemIDList.GetValue(5, i).ToString());

                        this.txtTax.Text = itemIDList.GetValue(4, i).ToString();
                        //MessageBox.Show(this.cbItems.Text, "ID");
                        //this.AgregarProducto();
                        //this.cbItems.Text = "";
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
        }

        private bool ValidarCamposObligatoriosNotaDebito()
        {
            int validar = 0;
            string customerID = cbClientes.Text;
            if (customerID.Trim() == "")
            {
                validar = 1;
                MessageBox.Show("Debe seleccionar un cliente para continuar con el proceso");
            }

            string aRAccount = ARAccount.Text;
            if (aRAccount.Trim() == "")
            {
                validar = 1;
                MessageBox.Show("Debe seleccionar una cuenta por cobrar para continuar con el proceso");
            }

            if (dgvDetalleInvoice.Rows.Count > 1)
            {
            }
            else
            {
                validar = 1;
                MessageBox.Show("Debe haber al menos una linea de detalle para la nota de debito");
            }

            if (validar == 1)
            {
                return false;
            }
            else
            {
                return true;
            } 
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            cBalance = Convert.ToDouble(this.lblBalance.Text);
            this.Sumar();
            this.Pagar();
            double resultado = 0;
            double resultado2 = 0;
            if (this.ValidarCamposObligatoriosNotaDebito())
            {
                cBalance = cBalance + Convert.ToDouble(this.txtTotalFactura.Text);
                resultado2 = cCreditLimit - (cBalance + Convert.ToDouble(this.txtTotalFactura.Text));
                resultado = cCreditLimit - cBalance;

                DialogResult result;
                if (cEstatus == "0")
                {
                    this.Guardar();
                }
                else
                    if (cEstatus == "1")
                    {
                        if (resultado >= 0)
                        {
                            this.Guardar();
                        }
                        else
                        {
                            result = MessageBox.Show("Con esta transaccion supera el limite de crédito del cliente, Desea Continuar?", "Limite de Crédito Excedido", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                this.Guardar();
                            }
                        }
                    }
                    else
                        if (cEstatus == "2")
                        {
                            result = MessageBox.Show("Solicitó que siempre se informara de una transaccion para este cliente, Desea Continuar?", "Siempre notificar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                this.Guardar();
                            }
                        }
                        else
                            if (cEstatus == "3")
                            {
                                if (resultado2 >= 0)
                                {
                                    this.Guardar();
                                }
                                else
                                {
                                    result = MessageBox.Show("Con esta transaccion supera el limite de crédito del cliente, No puede guardar la factura", "Limite de Crédito Excedido", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                }
                            }
                            else
                                if (cEstatus == "4")
                                {
                                    if (cBalance >= 0)
                                    {
                                        result = MessageBox.Show("El cliente seleccionado no es sujeto de crédito, si desea facturarle modifique el estado de crédito en peachtree", "Cliente sin crédito", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                    }
                                    else
                                    {
                                        this.Guardar();
                                    }
                                }
            } 
        }

        private void Guardar()
        {
            //string Puerto = ConfigurationSettings.AppSettings["PuertoImpresora"].ToString();
            //Tf.CloseFpctrl();
            //bool port_status = Tf.OpenFpctrl(Puerto);

            if (ValidarImpresora())
            {
                if (this.ValidarReporteZ())
                {
                    if (this.ValidarCamposObligatoriosNotaDebito())
                    {
                        this.IndicadorError("F5");
                        ControladorError = 0;
                        NumeroCuponCOO = "";

                        if (this.ImprimirNotaDebito())
                        {
                            this.CreateXMLFile(0);
                            this.Importfile("N");
                            if (ControladorError == 1)
                            {
                                this.CreateXMLFile(1);
                                this.Importfile("N");
                            }
                            this.IndicadorError("F4");
                        }
                        else
                        {
                            this.ProcesoAnulacion();
                        }

                        this.ClearForm();
                        this.limpiarAddItem();
                        this.LeerNumNotaDebito();
                    }
                }
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
            this.IndicadorError("F4");
        }

        private void CreateXMLFile(int SegundaOpcion)
        {
            int lineaDesc = 0;
            int cantLineas = 0;
            int numberOfDistributions = 0;
            string cantidad;
            string itemID;
            string UM;
            string Descripcion;
            string GLAcc;
            string precioU;
            string Taxtype;
            string monto;

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaDebito\NuevaNotaDebito.xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_Invoices");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_Invoice");
            Writer.WriteAttributeString("xsi:type", "paw:invoice");


            string sCustomerCompleto = this.cbClientes.Text;
            string[] sCustomerDesglosado = sCustomerCompleto.Split('_');
            string sCustomerSeleccionadoID = sCustomerDesglosado[0];


            string customerID = sCustomerSeleccionadoID;


            //string customerID = this.cbClientes.Text;
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
            Writer.WriteElementString("Customer_Name", customerName);
            //Numero Factura
            string numeroFactura = "ND-" + this.lblNumInvoice.Text.Trim();
            if (SegundaOpcion.Equals(1))
            {
                numeroFactura = numeroFactura + "-2";
            }
            Writer.WriteElementString("Invoice_Number", numeroFactura);
            //Fecha Factura
            Writer.WriteStartElement("Date");
            Writer.WriteAttributeString("xsi:type", "paw:date");
            Writer.WriteString(this.notaDebitoDate.Text);
            Writer.WriteEndElement();
            //Es Cotizacion?
            Writer.WriteElementString("isQuote", "FALSE");
            //if (this.lblCotizacionOrSalesOrder.Text.Trim() != "")
            //{
            //    Writer.WriteElementString("Quote_Number", this.lblCotizacionOrSalesOrder.Text.Trim());
            //    Writer.WriteStartElement("Quote_Good_Thru_Date");
            //    Writer.WriteAttributeString("xsi:type", "paw:date");
            //    Writer.WriteString(this.notaDebitoDate.Text);
            //    Writer.WriteEndElement();
            //}

            //Tiene Drop Ship
            if (this.cbxDropShip.Checked)
            {
                Writer.WriteElementString("Drop_Ship", "TRUE");
            }
            else
            {
                Writer.WriteElementString("Drop_Ship", "FALSE");
            }
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
            if (this.cbShipVia.Text != "")
            {
                //Ship Via
                Writer.WriteElementString("Ship_Via", this.cbShipVia.Text);
                //Ship_Date
                //Writer.WriteStartElement("Ship_Date");
                //Writer.WriteAttributeString("xsi:type", "paw:date");
                //Writer.WriteString(this.dtpShipDate.Text.ToString("MM/dd/yyyy"));
                //Writer.WriteEndElement();
            }
            //Date Due
            //<Date_Due xsi:type="paw:date">3/31/11</Date_Due> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            //Discount Amount
            Writer.WriteElementString("Discount_Amount", "0.00");
            //Discount Date
            //<Discount_Date xsi:type="paw:date">3/15/11</Discount_Date> xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx 

            //Campo utilizado para indicar el tipo documento 
            Writer.WriteElementString("Displayed_Terms", "DOCND");

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


            string saRAccountIDCompleto = this.ARAccount.Text.ToString();
            string[] saRAccountIDSeleccionadoDesglosado = saRAccountIDCompleto.Split('_');
            string aRAccountID = saRAccountIDSeleccionadoDesglosado[0];
            //string aRAccountID = this.ARAccount.Text.ToString();
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
            Writer.WriteElementString("Accounts_Receivable_Amount", this.txtTotal.Text);

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

            //CreditMemoType
            Writer.WriteElementString("CreditMemoType", "FALSE");
            //ProgressBillingInvoice
            Writer.WriteElementString("ProgressBillingInvoice", "FALSE");

            cantLineas = (this.dgvDetalleInvoice.Rows.Count);
            numberOfDistributions = (this.dgvDetalleInvoice.Rows.Count - 1);

            if (impuestoITBMS7Habilitado == "Habilitado")
            {
                numberOfDistributions = numberOfDistributions + 1;
            }

            if (this.cbDescuentos.Text != "")
            {
                numberOfDistributions = numberOfDistributions + 1;
            }

            double impuesto10 = 0;
            impuesto10 = Convert.ToDouble(this.txtITBMS10.Text);

            //if (impuestoITBMS10Habilitado == "Habilitado")
            if (impuesto10 > 0)
            {
                numberOfDistributions = numberOfDistributions + 1;
            }

            //Number of Distributions
            Writer.WriteElementString("Number_of_Distributions", numberOfDistributions.ToString());


            //Statement Note
            Writer.WriteElementString("Statement_Note", txtStatementNote.Text);

            //Recur Number
            Writer.WriteElementString("Recur_Number", "0");
            //Recur Frequency
            Writer.WriteElementString("Recur_Frequency", "0");

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
                itbms = (Convert.ToDouble(this.txtITBMS7.Text) * -1).ToString();
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

            for (int lineas = 0; lineas < (cantLineas - 1); ++lineas)
            {
                lineaDesc = lineas + 1;
                cantidad = this.dgvDetalleInvoice.Rows[lineas].Cells[2].Value.ToString();
                itemID = this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                UM = this.dgvDetalleInvoice.Rows[lineas].Cells[4].Value.ToString();
                Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString();
                GLAcc = this.dgvDetalleInvoice.Rows[lineas].Cells[6].Value.ToString();
                precioU = this.dgvDetalleInvoice.Rows[lineas].Cells[7].Value.ToString();
                Taxtype = this.dgvDetalleInvoice.Rows[lineas].Cells[8].Value.ToString();

                if (Taxtype == "0")
                {
                    Taxtype = "1";
                }
                monto = this.dgvDetalleInvoice.Rows[lineas].Cells[9].Value.ToString();

                Writer.WriteStartElement("SalesLine");
                Writer.WriteElementString("Quantity", cantidad);

                Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                Writer.WriteElementString("InvoiceCMDistribution", (lineas + 1).ToString());

                Writer.WriteStartElement("Item_ID");
                Writer.WriteAttributeString("xsi:type", "paw:ID");
                Writer.WriteString(itemID);
                Writer.WriteEndElement();
                //<Item_GUID>{B0C207B9-90C9-4415-B784-B9096EDD1571}</Item_GUID> 
                Writer.WriteElementString("Description", Descripcion);

                Writer.WriteStartElement("GL_Account");
                Writer.WriteAttributeString("xsi:type", "paw:ID");
                Writer.WriteString(GLAcc);
                Writer.WriteEndElement();
                //<GL_Account_GUID>{1E045D3F-B36A-46FA-AFA0-0C9B01251457}</GL_Account_GUID> 

                Writer.WriteElementString("Unit_Price", (Convert.ToDouble(precioU) * -1).ToString());
                Writer.WriteElementString("Tax_Type", Taxtype);

                Writer.WriteElementString("Amount", (Convert.ToDouble(monto) * -1).ToString());
                //<GL_Inventory_Account xsi:type="paw:id">15100</GL_Inventory_Account> 
                //<INV_Account_GUID>{AE7A8DA8-409E-4B6D-8129-4078E2697F98}</INV_Account_GUID> 
                //<Cost_of_Sales_Account xsi:type="paw:id">50100</Cost_of_Sales_Account> 
                //<Cost_of_SalesAccount_GUID>{C0DDC9AF-73ED-484B-93A2-9A2E64791F4C}</Cost_of_SalesAccount_GUID> 
                //<Cost_of_Sales_Amount>690.00</Cost_of_Sales_Amount> 
                //<Retainage_Percent>0.00</Retainage_Percent> 

                if (UM.Trim() != "")
                {
                    Writer.WriteStartElement("UM_ID");
                    Writer.WriteAttributeString("xsi:type", "paw:id");
                    Writer.WriteString(UM);
                    Writer.WriteEndElement();
                }
                Writer.WriteElementString("UM_Stocking_Units", "1.00000");

                //<Stocking_Quantity>3.00000</Stocking_Quantity> 
                //<Stocking_Unit_Price>489.00000</Stocking_Unit_Price>

                Writer.WriteEndElement();//closes the sales line element
            }

            if (this.cbDescuentos.Text != "")
            {
                Writer.WriteStartElement("SalesLine");
                Writer.WriteElementString("Quantity", "1");

                Writer.WriteElementString("SalesOrderDistributionNumber", "0");
                Writer.WriteElementString("Apply_To_Sales_Order", "FALSE");
                Writer.WriteElementString("Apply_To_Proposal", "FALSE");
                Writer.WriteElementString("InvoiceCMDistribution", (lineaDesc + 1).ToString());

                //Writer.WriteStartElement("Item_ID");
                //Writer.WriteAttributeString("xsi:type", "paw:ID");
                //Writer.WriteString(itemID);
                //Writer.WriteEndElement();

                Writer.WriteElementString("Description", "Descuento");

                Writer.WriteStartElement("GL_Account");
                Writer.WriteAttributeString("xsi:type", "paw:ID");
                Writer.WriteString(CuentaDescuento);
                Writer.WriteEndElement();

                //Writer.WriteElementString("Unit_Price", (Convert.ToDouble(precioU) * -1).ToString());
                Writer.WriteElementString("Tax_Type", "20");

                Writer.WriteElementString("Amount", this.txtDescuento.Text);

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
                double s = Convert.ToDouble(txtITBMS10.Text) * -1;
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

        private bool ImprimirNotaDebito()
        {
            int cantLineas = 0;
            int lineaComentarios = 0;
            int ControladorError = 0;
            double valor;
            double valor2;
            string cantidad;
            string itemID;
            string Descripcion;
            string DescripcionSec;
            string precioU;
            string monto;
            string Tax;
            string codigoI;
            string cliente;
            string clienteRUC;
            string clienteNombreND;
            string clienteDireccion;
            string facturaRelacionada;
            string L_NComprobante = "123456";
            string L_fecha = "111129";
            string tiempo = "162530";
            string tipo = "B";
            string comando;
            string mensaje;
            string TPago;
            string T_Motivo = "Descuento";

            //string DescripcionSec;

            string sDescripcionTemp = "";
            string sDescripcion1 = "";
            string sDescripcion2 = "";
            string sDescripcion3 = "";
            int iCantLineas = 0;

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


                this.ObtenerNumRegistro();
                facturaRelacionada = "00000000";

                cliente = this.cbClientes.Text;
                clienteDireccion = this.Add1.Text.Trim();

                cliente = this.cbClientes.Text;
                if (this.CustVendName.Text.Trim() != "")
                {
                    clienteNombreND = this.CustVendName.Text.Trim();
                }
                else
                {
                    clienteNombreND = cliente;
                }
                if (clienteNombreND.Length > 42)
                {
                    clienteNombreND = clienteNombreND.Substring(0, 42);
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


                comando = "@" + FS + clienteNombreND + FS + clienteRUC + FS + facturaRelacionada + FS + RegisterMachineNumber + FS + L_fecha + FS + tiempo + FS + tipo;
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

                //comando = "A" + FS + "PreFactura de ePago: " + this.lblFacturaSistema.Text;
                //HASAR.LimpiarDoc();
                //mensaje = HASAR.MandaPaqueteFiscal(handler, comando).ToString();
                //if (Convert.ToInt32(mensaje) < 0)
                //{
                //    HASAR.Analisa_iRetorno(Convert.ToInt32(mensaje));
                //    return false;
                //}
                //else
                //    if (this.RevisarEstado() == false)
                //    {
                //        HASAR.Abort(3);
                //        this.Cortar();
                //        return false;
                //    }
                //totalFactura
                //montoPagar = String.Format("{0:##.00}", totalFactura);

                cantLineas = (this.dgvDetalleInvoice.Rows.Count - 1);
                for (int lineas = 0; lineas < cantLineas; ++lineas)
                {
                    try
                    {
                        cantidad = string.Format("{0:##0.000}", Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[2].Value.ToString()));
                        itemID = this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();
                        DescripcionSec = itemID;
                        if (itemID.Length > 20)
                        {
                            itemID = itemID.Substring(0, 20);
                        }

                        Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString();
                        
                        /*
                        if (Descripcion.Trim() == "")
                        {
                            Descripcion = DescripcionSec;
                        }
                        if (Descripcion.Length > 20)
                        {
                            Descripcion = Descripcion.Substring(0, 20);
                        }
                        */

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





                        precioU = string.Format("{0:##0.000}", (Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[7].Value)));
                        Tax = this.dgvDetalleInvoice.Rows[lineas].Cells[8].Value.ToString();
                        monto = this.dgvDetalleInvoice.Rows[lineas].Cells[9].Value.ToString();

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

                    if (CodigoProducto == "C")
                    {

                        if(precioU.Contains("-"))
                        {
                            double remplazo = Convert.ToDouble(precioU) * -1;
                            precioU = string.Format("{0:##0.000}", remplazo);
                            comando = "B" + FS + sDescripcion1 + FS + cantidad + FS + precioU + FS + codigoI + FS + "m" + FS + itemID;
                            //comando = "B" + FS + Descripcion + FS + cantidad + FS + precioU + FS + codigoI + FS + "M" + FS + itemID;
                        }
                        else
                        {
                            comando = "B" + FS + sDescripcion1 + FS + cantidad + FS + precioU + FS + codigoI + FS + "M" + FS + itemID;
                            //comando = "B" + FS + Descripcion + FS + cantidad + FS + precioU + FS + codigoI + FS + "M" + FS + itemID;
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
                            comando = "B" + FS + sDescripcion1 + FS + cantidad + FS + precioU + FS + codigoI + FS + "m" + FS + "*****";
                            //comando = "B" + FS + Descripcion + FS + cantidad + FS + precioU + FS + codigoI + FS + "M" + FS + itemID;
                        }
                        else
                        {
                            comando = "B" + FS + sDescripcion1 + FS + cantidad + FS + precioU + FS + codigoI + FS + "M" + FS + "*****";
                            //comando = "B" + FS + Descripcion + FS + cantidad + FS + precioU + FS + codigoI + FS + "M" + FS + itemID;
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

                if (this.cbDescuentos.Text != "")
                {
                    double sPorcDesc = Convert.ToDouble(this.cbDescuentos.Text);
                    comando = "D" + FS + T_Motivo + FS + String.Format("{0:#,#0.00}", sPorcDesc) + FS + "D" + FS + " ";
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

                int f;
                string Ceros;
                int cantidadC;
                string Comando;

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











            //int cantLineas = 0;
            //int lineaComentarios = 0;
            //double valor;
            //double valor2;
            //string cantidad;
            //string itemID;
            //string Descripcion;
            //string precioU;
            //string monto;
            //string Tax;
            //string codigoI;
            //string cliente;
            //string clienteNombreND;
            //string clienteDireccion;
            //string clienteRUC;
            //string facturaRelacionada;

            //this.ObtenerNumRegistro();
            ////if (this.lblNumeroFactura.Text.Trim() != "")
            ////{
            ////    facturaRelacionada = RegisterMachineNumber + "-" + this.lblNumeroFactura.Text.Trim();
            ////}
            ////else
            ////{
            //facturaRelacionada = RegisterMachineNumber + "-00000000";
            ////}

            //cliente = this.cbClientes.Text;
            //clienteDireccion = this.Add1.Text.Trim();

            //if (this.CustVendName.Text.Trim() != "")
            //{
            //    clienteNombreND = this.CustVendName.Text;
            //}
            //else
            //{
            //    clienteNombreND = cliente;
            //}
            //if (clienteNombreND.Length > 40)
            //{
            //    clienteNombreND = clienteNombreND.Substring(0, 40);
            //}

            //if (this.txtRUC.Text.Trim() != "")
            //{
            //    clienteRUC = this.txtRUC.Text.Trim();
            //}
            //else
            //{
            //    clienteRUC = cliente;
            //}
            //if (clienteRUC.Length > 20)
            //{
            //    clienteRUC = clienteRUC.Substring(0, 20);
            //}

            //if (Tf.SendCmd("jR" + clienteRUC) == true)
            //{
            //    if (Tf.SendCmd("jS" + clienteNombreND) == true)
            //    { }
            //    else
            //    {
            //        return this.DescribirError();
            //    }

            //    if (Tf.SendCmd("jF" + facturaRelacionada) == true)
            //    { }
            //    else
            //    {
            //        return this.DescribirError();
            //    }

            //    ////////////////// INFORMACION ADICIONAL ////////////////////////
            //    if (clienteDireccion != "")
            //    {
            //        lineaComentarios = lineaComentarios + 1;
            //        if (Tf.SendCmd("j" + lineaComentarios.ToString() + clienteDireccion) == true)
            //        { }
            //        else
            //        {
            //            return this.DescribirError();
            //        }
            //    }

            //    //if (clienteSaldoAnterior != "")
            //    //{
            //    //    lineaComentarios = lineaComentarios + 1;
            //    //    if (Tf.SendCmd("j" + lineaComentarios.ToString() + clienteSaldoAnterior + clienteAjuste) == true)
            //    //    { }
            //    //    else
            //    //    {
            //    //        return this.DescribirError();
            //    //    }
            //    //}
            //    ////////////////// INFORMACION ADICIONAL ////////////////////////

            //    cantLineas = (this.dgvDetalleInvoice.Rows.Count - 1);
            //    for (int lineas = 0; lineas < cantLineas; ++lineas)
            //    {
            //        try
            //        {
            //            cantidad = (Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[2].Value.ToString()) * 1000).ToString();
            //            itemID = this.dgvDetalleInvoice.Rows[lineas].Cells[3].Value.ToString();

            //            if (itemID.Trim() != "")
            //            {
            //                Descripcion = itemID + "-" + this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString(); ;
            //            }
            //            else
            //            {
            //                Descripcion = this.dgvDetalleInvoice.Rows[lineas].Cells[5].Value.ToString();
            //            } 

            //            if (Descripcion.Length > 46)
            //            {
            //                Descripcion = Descripcion.Substring(0, 46);
            //            }

            //            precioU = (Convert.ToDouble(this.dgvDetalleInvoice.Rows[lineas].Cells[7].Value) * 100).ToString();
            //            Tax = this.dgvDetalleInvoice.Rows[lineas].Cells[8].Value.ToString();
            //            monto = this.dgvDetalleInvoice.Rows[lineas].Cells[9].Value.ToString();


            //            valor = 0;
            //            valor2 = 0;
            //            valor = (Convert.ToDouble(precioU));
            //            valor2 = (Convert.ToDouble(cantidad));

            //            codigoI = " ";
            //            int controlador = 0;

            //            if (Tax == "1")
            //            {
            //                codigoI = "!";
            //                controlador = 1;
            //            }

            //            if (controlador == 0)
            //            {
            //                if (impuestoITBMS10Habilitado == "Habilitado")
            //                {
            //                    if (Tax == impuestoITBMS10TaxType)
            //                    {
            //                        codigoI = HexAsciiConvert("22");
            //                        controlador = 1;
            //                    }
            //                }
            //            }

            //            if (controlador == 0)
            //            {
            //                if (impuestoITBMS15Habilitado == "Habilitado")
            //                {
            //                    if (Tax == impuestoITBMS15TaxType)
            //                    {
            //                        codigoI = "#";
            //                        controlador = 1;
            //                    }
            //                }
            //            }

            //            if (controlador == 0)
            //            {
            //                codigoI = " ";
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            return false;
            //        }

            //        if (Tf.SendCmd("`" + codigoI + valor.ToString("0000000000") + valor2.ToString("00000000") + Descripcion) == true)
            //        { }
            //        else
            //        {
            //            return this.DescribirError();
            //        }
            //    }

            //    if (Tf.SendCmd("3"))
            //    { }
            //    else
            //    {
            //        return this.DescribirError();
            //    }

            //    if (this.cbDescuentos.Text != "")
            //    {
            //        double sPorcDesc = Convert.ToDouble(this.cbDescuentos.Text) * 100;
            //        if (Tf.SendCmd("p-" + sPorcDesc.ToString("0000")) == true)
            //        { }
            //        else
            //        {
            //            return this.DescribirError();
            //        }
            //    }

            //    int f;
            //    string Ceros;
            //    int cantidadC;
            //    string Comando;

            //    double montoCR = Convert.ToDouble(this.txtTotalFactura.Text) * 100;
            //    if (montoCR > 0)
            //    {
            //        Ceros = "0";
            //        Comando = "";
            //        cantidadC = 12 - montoCR.ToString().Length;
            //        if (cantidadC == 0)
            //        {
            //            Ceros = "";
            //        }
            //        else
            //        {
            //            cantidadC = cantidadC - 1;
            //            for (f = 0; f < cantidadC; ++f)
            //            {
            //                Ceros = Ceros + "0";
            //            }
            //        }

            //        Comando = Ceros + montoCR.ToString();
            //        if (Tf.SendCmd("213" + Comando) == true)
            //        { }
            //        else
            //        {
            //            return this.DescribirError();
            //        }
            //    }

            //    //double montoE = Convert.ToDouble(this.txtMontoEfectivo.Text) * 100;
            //    //if (montoE > 0)
            //    //{
            //    //    Ceros = "0";
            //    //    Comando = "";
            //    //    cantidadC = 12 - montoE.ToString().Length;
            //    //    if (cantidadC == 0)
            //    //    {
            //    //        Ceros = "";
            //    //    }
            //    //    else
            //    //    {
            //    //        cantidadC = cantidadC - 1;
            //    //        for (f = 0; f < cantidadC; ++f)
            //    //        {
            //    //            Ceros = Ceros + "0";
            //    //        }
            //    //    }

            //    //    Comando = Ceros + montoE.ToString();
            //    //    if (Tf.SendCmd("201" + Comando) == true)
            //    //    { }
            //    //    else
            //    //    {
            //    //        return this.DescribirError();
            //    //    }
            //    //}

            //    //double montoT = Convert.ToDouble(this.txtMontoTarjeta.Text) * 100;
            //    //if (montoT > 0)
            //    //{
            //    //    Ceros = "0";
            //    //    Comando = "";
            //    //    cantidadC = 12 - montoT.ToString().Length;
            //    //    if (cantidadC == 0)
            //    //    {
            //    //        Ceros = "";
            //    //    }
            //    //    else
            //    //    {
            //    //        cantidadC = cantidadC - 1;
            //    //        for (f = 0; f < cantidadC; ++f)
            //    //        {
            //    //            Ceros = Ceros + "0";
            //    //        }
            //    //    }

            //    //    Comando = Ceros + montoT.ToString();
            //    //    if (Tf.SendCmd("209" + Comando) == true)
            //    //    { }
            //    //    else
            //    //    {
            //    //        return this.DescribirError();
            //    //    }
            //    //}

            //    //double montoCH = Convert.ToDouble(this.txtMontoCheque.Text) * 100;
            //    //if (montoCH > 0)
            //    //{
            //    //    Ceros = "0";
            //    //    Comando = "";
            //    //    cantidadC = 12 - montoCH.ToString().Length;
            //    //    if (cantidadC == 0)
            //    //    {
            //    //        Ceros = "";
            //    //    }
            //    //    else
            //    //    {
            //    //        cantidadC = cantidadC - 1;
            //    //        for (f = 0; f < cantidadC; ++f)
            //    //        {
            //    //            Ceros = Ceros + "0";
            //    //        }
            //    //    }

            //    //    Comando = Ceros + montoCH.ToString();
            //    //    if (Tf.SendCmd("205" + Comando) == true)
            //    //    { }
            //    //    else
            //    //    {
            //    //        return this.DescribirError();
            //    //    }
            //    //}
            //}
            //else
            //{
            //    return this.DescribirError();
            //}
            //MessageBox.Show("Nota de Debito Impresa Correctamente");
            //return true;
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
            this.txtCustomerPO.Text = "";
            this.cbShipVia.Text = "";

            this.cbSalesRepresent.Text = "";
            //this.ARAccount.Text = "";
            //this.arAcctDesc.Text = "";
            this.cbItems.Text = "";
            this.txtUnidadMedida.Text = "";
            this.txtDescripcion.Text = "";
            this.cbGlacct.Text = "";
            this.txtPrecioUnitario.Text = "";
            this.txtTax.Text = "";

            this.txtCantidadItems.Text = "";
            this.cbDescuentos.Text = "";
            this.txtDescuento.Text = "";
            this.txtTotal.Text = "";
            this.txtITBMS7.Text = "";
            this.txtITBMS10.Text = "";
            this.txtTotalFactura.Text = "";

            this.txtCantidad.Text = "";

            this.txtMontoEfectivo.Text = "";
            this.cbTarjetas.Text = "";
            this.txtMontoTarjeta.Text = "";
            this.txtMontoCheque.Text = "";
            this.txtReferenciaCheque.Text = "";
            this.txtMontoCambio.Text = "";

            //foreach (Control ctrl in this.Controls)
            //{
            //    if (ctrl.GetType().ToString() == "System.Windows.Forms.TextBox"
            //        || ctrl.GetType().ToString() == "System.Windows.Forms.ComboBox")
            //    {
            //        ctrl.Text = "";
            //    }
            //}

            this.notaDebitoDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            this.dtDetalleNotaDebito.Clear();
            this.txtTotal.Text = "";
            this.txtTel.Text = "";

            this.txtCustomeNote.Text = "";
            this.txtStatementNote.Text = "";
            this.txtInternalNote.Text = "";
        }

        private void dgvDetalleInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //lblNumfila.Text = e.RowIndex.ToString();
            seleccion = e.RowIndex;
        }

        private void dgvDetalleInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string item = "";
            string CellValue;
            if (e.ColumnIndex > -1 && e.RowIndex > -1)// && TypeOf dataGridView1.CurrentCell Is DataGridViewLinkCell)
            {
                CellValue = (sender as DataGridView).CurrentCell.FormattedValue.ToString();
                if (CellValue == "Eliminar")
                {
                    DataRow Dr_delete;
                    Dr_delete = dtDetalleNotaDebito.Rows[seleccion];
                    Dr_delete.Delete();
                    dtDetalleNotaDebito.AcceptChanges();

                    this.Sumar();
                }
                if (CellValue == "Editar")
                {
                    this.txtCantidad.Text = this.dgvDetalleInvoice.Rows[seleccion].Cells[2].Value.ToString();
                    item = this.dgvDetalleInvoice.Rows[seleccion].Cells[3].Value.ToString();
                    this.BuscarItem(item);
                    this.txtUnidadMedida.Text = this.dgvDetalleInvoice.Rows[seleccion].Cells[4].Value.ToString();
                    this.txtDescripcion.Text = this.dgvDetalleInvoice.Rows[seleccion].Cells[5].Value.ToString();
                    this.cbGlacct.Text = this.dgvDetalleInvoice.Rows[seleccion].Cells[6].Value.ToString();
                    this.txtPrecioUnitario.Text = this.dgvDetalleInvoice.Rows[seleccion].Cells[7].Value.ToString();
                    this.txtTax.Text = this.dgvDetalleInvoice.Rows[seleccion].Cells[8].Value.ToString();

                    DataRow Dr_delete;
                    Dr_delete = dtDetalleNotaDebito.Rows[seleccion];
                    Dr_delete.Delete();
                    dtDetalleNotaDebito.AcceptChanges();

                    this.Sumar();
                }
            }
        }

        private void BuscarItem(string dgItemId)
        {
            try
            {
                string itemID = "";
                for (int i = 0; i <= itemIDList.GetUpperBound(1); i++)
                {
                    itemID = itemIDList.GetValue(0, i).ToString();
                    if (itemID == dgItemId)
                    {
                        this.cbItems.Text = itemIDList.GetValue(0, i).ToString() + "_" + itemIDList.GetValue(1, i).ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.cbItems.Text = "";
            }
        }

        private void btnSumarizar_Click(object sender, EventArgs e)
        {
            this.Sumar();
        }

        private void Sumar()
        {
            //VALIDAR Q CANTIDAD CDESC NO SEA MAYOR AL MAYOR DE PORCEN DESC
            double monto = 0;
            double sumador = 0;
            double sumadorProductos = 0;
            double ITBMS7 = 0;
            double ITBMS10 = 0;
            double ITBMS15 = 0;
            int tax = 0;
            foreach (DataRow dr in dtDetalleNotaDebito.Rows)
            {
                monto = Convert.ToDouble(dr[7].ToString());
                sumador = sumador + monto;
                sumadorProductos = sumadorProductos + Convert.ToDouble(dr[0].ToString());

                tax = Convert.ToInt32(dr[6].ToString());
                if (tax == 1)
                {
                    if (cbDescuentos.Text != "")
                    {

                        ITBMS7 = ITBMS7 + ((monto - (monto * (Convert.ToDouble(cbDescuentos.Text) / 100))) * 0.07);
                    }
                    else
                    {
                        ITBMS7 = ITBMS7 + (monto * 0.07);
                    }
                }

                if (impuestoITBMS10Habilitado == "Habilitado")
                {
                    if (tax == Convert.ToInt32(impuestoITBMS10TaxType))
                    {
                        if (this.cbDescuentos.Text != "")
                        {
                            ITBMS10 = ITBMS10 + ((monto - (monto * (Convert.ToDouble(this.cbDescuentos.Text) / 100))) * 0.1);
                        }
                        else
                        {
                            ITBMS10 = ITBMS10 + (monto * 0.1);
                        }
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
                        if (this.cbDescuentos.Text != "")
                        {
                            ITBMS15 = ITBMS15 + ((monto - (monto * (Convert.ToDouble(this.cbDescuentos.Text) / 100))) * 0.15);
                        }
                        else
                        {
                            ITBMS15 = ITBMS15 + (monto * 0.15);
                        }
                    }
                }
                else
                {
                    ITBMS15 = 0;
                }
            }

            if (this.cbDescuentos.Text != "")
            {
                double Porcdesc = Convert.ToDouble(cbDescuentos.Text);
                this.txtDescuento.Text = string.Format("{0:#,#0.00}", ((sumador * Porcdesc) / 100));
                sumador = sumador - ((sumador * Porcdesc) / 100);
            }
            else
                if (this.txtDescuento.Text != "")
                {
                    double Quantcdesc = Convert.ToDouble(txtDescuento.Text);
                    sumador = sumador - (Quantcdesc);
                }

            this.txtTotal.Text = string.Format("{0:#,#0.00}", sumador);
            this.txtCantidadItems.Text = string.Format("{0:#,#0.000}", sumadorProductos);
            this.txtITBMS7.Text = string.Format("{0:#,#0.00}", ITBMS7);

            string itbmsotros = string.Format("{0:#,#0.00}", ITBMS7);
            this.txtITBMS10.Text = string.Format("{0:#,#0.00}", Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS10)) + Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS15)));
            this.txtTotalFactura.Text = string.Format("{0:#,#0.00}", (sumador + (Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS7)) + Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS10)) + Convert.ToDouble(string.Format("{0:#,#0.00}", ITBMS15)))));
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
        
        private void LeerNumNotaDebito()
        {
            try
            {
                int serieLenght = 0;
                string serie;
                string NumeroSerie;
                string mensaje, SImp, SFis;
                string[] CadResp;
                string[] status;
                string respuesta;
                handler = frmPrincipal.handlerM;

                HASAR.LimpiarDoc();
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

                    int sec = Convert.ToInt32(status[9]) + 1;
                    this.lblNumInvoice.Text = serie + "-" + sec.ToString("00000000");
                }
                else
                {
                    this.lblNumInvoice.Text = "BSC-" + "00000000";
                    MessageBox.Show("Error Immpresora, revise que la impresora este correctamente encendida y conectada.", "Error en Impresora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                this.lblNumInvoice.Text = "BSC-" + "00000000";
            }
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            this.Pagar();
        }

        private void Pagar()
        {
            double montoEfec = 0;
            double montoTarj = 0;
            double montoCheq = 0;
            double pagoTotal = 0;
            double totalFactura = 0;

            if (txtMontoEfectivo.Text != "")
            {
                montoEfec = Convert.ToDouble(this.txtMontoEfectivo.Text);
                this.txtMontoEfectivo.Text = string.Format("{0:#,#0.00}", montoEfec); //montoEfec.ToString("#.##");
            }
            if (txtMontoTarjeta.Text != "")
            {
                montoTarj = Convert.ToDouble(this.txtMontoTarjeta.Text);
                this.txtMontoTarjeta.Text = string.Format("{0:#,#0.00}", montoTarj);
            }
            if (txtMontoCheque.Text != "")
            {
                montoCheq = Convert.ToDouble(this.txtMontoCheque.Text);
                this.txtMontoCheque.Text = string.Format("{0:#,#0.00}", montoCheq);
            }

            pagoTotal = montoEfec + montoTarj + montoCheq;
            totalFactura = Convert.ToDouble(txtTotalFactura.Text);

            double cambio;
            cambio = pagoTotal - totalFactura;
            this.txtMontoCambio.Text = string.Format("{0:#,#0.00}", cambio);
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

        private void cbDescuentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sumar();
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

        private void Sumarizar()
        {
            //VALIDAR Q CANTIDAD CDESC NO SEA MAYOR AL MAYOR DE PORCEN DESC
            double monto = 0;
            double sumador = 0;
            double sumadorProductos = 0;
            double ITBMS7 = 0;
            double ITBMS10 = 0;
            int tax = 0;
            foreach (DataRow dr in dtDetalleNotaDebito.Rows)
            {
                monto = Convert.ToDouble(dr[7].ToString());
                sumador = sumador + monto;
                sumadorProductos = sumadorProductos + Convert.ToDouble(dr[0].ToString());

                tax = Convert.ToInt32(dr[6].ToString());
                if (tax == 1)
                {
                    if (cbDescuentos.Text != "")
                    {

                        ITBMS7 = ITBMS7 + ((monto - (monto * (Convert.ToDouble(cbDescuentos.Text) / 100))) * 0.07);
                    }
                    else
                    {
                        ITBMS7 = ITBMS7 + (monto * 0.07);
                    }

                }

                if (Impuesto2Status == "Habilitado")
                {
                    if (tax == Convert.ToInt32(Impuesto2TaxType))
                    {
                        if (cbDescuentos.Text != "")
                        {
                            ITBMS10 = ITBMS10 + ((monto - (monto * (Convert.ToDouble(cbDescuentos.Text) / 100))) * 0.1);
                        }
                        else
                        {
                            ITBMS10 = ITBMS10 + (monto * 0.1);
                        }

                    }
                }
                else
                {
                    ITBMS10 = 0;
                }

            }

            if (cbDescuentos.Text != "")
            {
                double Porcdesc = Convert.ToDouble(cbDescuentos.Text);
                this.txtDescuento.Text = ((sumador * Porcdesc) / 100).ToString("#.##");
                sumador = sumador - ((sumador * Porcdesc) / 100);
            }
            else
                if (txtDescuento.Text != "")
                {
                    double Quantcdesc = Convert.ToDouble(txtDescuento.Text);
                    sumador = sumador - (Quantcdesc);
                }

            txtTotal.Text = sumador.ToString("#.##");
            txtCantidadItems.Text = sumadorProductos.ToString("#.###");
            txtITBMS7.Text = ITBMS7.ToString("#.##");
            txtITBMS10.Text = ITBMS10.ToString("#.##");
            txtTotalFactura.Text = (sumador + (ITBMS7 + ITBMS10)).ToString("#.##");
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

        private void txtMontoEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            bool isDec = false;
            int nroDec = 0;


            for (int i = 0; i < this.txtMontoEfectivo.Text.Length; i++)
            {
                if (this.txtMontoEfectivo.Text[i] == '.')
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

        private void txtMontoTarjeta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            bool isDec = false;
            int nroDec = 0;


            for (int i = 0; i < this.txtMontoTarjeta.Text.Length; i++)
            {
                if (this.txtMontoTarjeta.Text[i] == '.')
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

        private void txtMontoCheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            bool isDec = false;
            int nroDec = 0;


            for (int i = 0; i < this.txtMontoCheque.Text.Length; i++)
            {
                if (this.txtMontoCheque.Text[i] == '.')
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

        private void btnCancelarCotizacionOrSalesOrder_Click(object sender, EventArgs e)
        {
            this.ClearForm();
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
                                    break;
                                }
                            case "CuentaDescuento":
                                {
                                    CuentaDescuento = reader[i].ChildNodes[a].InnerText;
                                    break;
                                }
                            case "CuentaAR":
                                {
                                    //CuentaAR = reader[i].ChildNodes[a].InnerText;
                                    //this.ARAccount.Text = CuentaAR;
                                    //break;

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
                                    //this.ARAccount.Text = CuentaAR;
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

        private string HexAsciiConvert(string hex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        private void CreateXMLFileAnulado(int SegundaOpcion)
        {
            int lineaDesc = 0;
            int cantLineas = 0;
            int numberOfDistributions = 0;
            string cantidad;
            string itemID;
            string UM;
            string Descripcion;
            string GLAcc;
            string precioU;
            string Taxtype;
            string monto;

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NotaDebito\AnularNotaDebito.xml";

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
            Writer.WriteElementString("Customer_Name", customerName);
            //Numero Factura
            string numeroFactura = this.lblNumInvoice.Text.Trim() + "_Anulada";
            if (SegundaOpcion.Equals(1))
            {
                numeroFactura = numeroFactura + "-2";
            }
            Writer.WriteElementString("Invoice_Number", numeroFactura);
            //Fecha Factura
            Writer.WriteStartElement("Date");
            Writer.WriteAttributeString("xsi:type", "paw:date");
            Writer.WriteString(this.notaDebitoDate.Text);
            Writer.WriteEndElement();
            //Es Cotizacion?
            Writer.WriteElementString("isQuote", "FALSE");
            //if (this.lblCotizacionOrSalesOrder.Text.Trim() != "")
            //{
            //    Writer.WriteElementString("Quote_Number", this.lblCotizacionOrSalesOrder.Text.Trim());
            //    Writer.WriteStartElement("Quote_Good_Thru_Date");
            //    Writer.WriteAttributeString("xsi:type", "paw:date");
            //    Writer.WriteString(this.invoiceDate.Text);
            //    Writer.WriteEndElement();
            //}

            //Tiene Drop Ship
            if (this.cbxDropShip.Checked)
            {
                Writer.WriteElementString("Drop_Ship", "TRUE");
            }
            else
            {
                Writer.WriteElementString("Drop_Ship", "FALSE");
            }
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
            if (this.cbShipVia.Text != "")
            {
                Writer.WriteElementString("Ship_Via", this.cbShipVia.Text);
            }
            Writer.WriteElementString("Discount_Amount", "0.00");
            //Campo utilizado para indicar el tipo documento 
            Writer.WriteElementString("Displayed_Terms", "DOCND");

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
            Writer.WriteElementString("Accounts_Receivable_Amount", "0.00");
            //Note Prints After Line Items
            Writer.WriteElementString("Note_Prints_After_Line_Items", "FALSE");
            //Statement Note Prints Before Ref
            Writer.WriteElementString("Statement_Note_Prints_Before_Ref", "FALSE");
            //Beginning Balance Transaction
            Writer.WriteElementString("Beginning_Balance_Transaction", "FALSE");

            //CreditMemoType
            Writer.WriteElementString("CreditMemoType", "FALSE");
            //ProgressBillingInvoice
            Writer.WriteElementString("ProgressBillingInvoice", "FALSE");
            //Number of Distributions
            Writer.WriteElementString("Number_of_Distributions", "2");// numberOfDistributions.ToString());
            //Recur Number
            Writer.WriteElementString("Recur_Number", "0");
            //Recur Frequency
            Writer.WriteElementString("Recur_Frequency", "0");

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
            Writer.WriteElementString("Description", "NOTA DEBITO ANULADA");

            Writer.WriteStartElement("GL_Account");
            Writer.WriteAttributeString("xsi:type", "paw:ID");
            Writer.WriteString(CuentaAnulacion);//"40100");//vb
            Writer.WriteEndElement();
            //<GL_Account_GUID>{1E045D3F-B36A-46FA-AFA0-0C9B01251457}</GL_Account_GUID> 

            Writer.WriteElementString("Unit_Price", "0.00");
            Writer.WriteElementString("Tax_Type", "1");
            Writer.WriteElementString("Amount", "0.00");
            Writer.WriteElementString("Cost_of_Sales_Amount", "0.00");
            Writer.WriteElementString("UM_Stocking_Units", "1.00000");
            Writer.WriteEndElement();//closes the sales line element

            Writer.WriteEndElement();//Closes the Sales Lines element
            Writer.WriteEndElement();//Closes the paw_invoice element

            Writer.WriteEndElement();//closes the paw_invoices element and ends the document

            Writer.Close();
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

        private void cbGlacct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
