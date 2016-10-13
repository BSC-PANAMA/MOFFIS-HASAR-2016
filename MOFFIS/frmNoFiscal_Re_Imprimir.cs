using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using TFHKADIR;
using Interop.PeachwServer;

namespace MOFFIS
{
    public partial class frmNoFiscal_Re_Imprimir : Form
    {
        private ConectarPT ptApp = new ConectarPT();
        private Interop.PeachwServer.Export exportador;

        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;

        private string customerID_FacturaSel = "";
        private string customerName_FacturaSel = "";
        private string number_FacturaSel;
        private string date_FacturaSel = "";
        private string tipoDocumento_FacturaSel = "";
        private string PathMoffis;

        public Tfhka Tf
        {
            get { return frmPrincipal.tf; }
            set { frmPrincipal.tf = value; }
        }

        public frmNoFiscal_Re_Imprimir()
        {
            InitializeComponent();
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

        private void ObtenerListadoFacturas()
        {
            DateTime fecha1 = this.dtp1.Value;
            DateTime fecha2 = this.dtp2.Value;

            exportador = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesJournal);

            exportador.ClearExportFieldList();
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DisplayedTerms);
            exportador.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
            exportador.SetSortField((short)PeachwIEObjSalesJournalSortBy.peachwIEObjSalesJournalSortBy_InvoiceNumber);

            exportador.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange, fecha1, fecha2);
            exportador.SetSortField((short)PeachwIEObjSalesJournalSortBy.peachwIEObjSalesJournalSortBy_InvoiceNumber);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\NoFiscales\ListadoFacturas.xml";
            string PathListado2 = PathMoffis + @"\XML\NoFiscales\ListadoFacturas2.xml";

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
            this.lvFacturas.Columns.Add("Nota", -2, HorizontalAlignment.Left);

            int contadorFacturas = 0;
            string customerID_Factura = "";
            string customerName_Factura = "";
            string number_Factura;
            string date_Factura = "";
            string coo_Factura = "";
            string esCOO = "";
            string isCreditMemo = "";
            int agregarFacturaConCOO;

            string tipoDocumento;

            for (int i = 0; i <= reader.Count - 1; i++)
            {
                customerID_Factura = "";
                customerName_Factura = "";
                number_Factura = "";
                date_Factura = "";
                coo_Factura = "";
                isCreditMemo = "";
                esCOO = "";
                agregarFacturaConCOO = 0;

                    foreach (XmlNode node in reader[i].ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "Customer_ID":
                                {
                                    customerID_Factura = node.InnerText;
                                    break;
                                }
                            case "Customer_Name":
                                {
                                    customerName_Factura = node.InnerText;
                                    break;
                                }
                            case "Invoice_Number":
                                {
                                    number_Factura = node.InnerText;
                                    break;
                                }
                            case "Date":
                                {
                                    date_Factura = node.InnerText;
                                    break;
                                }
                            case "Displayed_Terms":
                                {
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

                    if (coo_Factura.Length > 3)
                    {
                        esCOO = coo_Factura.Substring(0, 3);
                        if (esCOO == "DOC")
                        {
                            agregarFacturaConCOO = 1;
                        }
                    }

                    if (agregarFacturaConCOO == 1)
                    {
                        if (number_Factura.Trim() != "")
                        {
                            if (number_Factura.Length > 11)
                            {
                                tipoDocumento = number_Factura.Substring(0, 2);

                                this.lvFacturas.Items.Add(number_Factura);
                                this.lvFacturas.Items[contadorFacturas].SubItems.Add(customerID_Factura);
                                this.lvFacturas.Items[contadorFacturas].SubItems.Add(customerName_Factura);
                                this.lvFacturas.Items[contadorFacturas].SubItems.Add(date_Factura);

                                if (tipoDocumento == "NC")
                                {
                                    if (isCreditMemo == "TRUE")
                                    {
                                        this.lvFacturas.Items[contadorFacturas].SubItems.Add("Nota Credito");
                                    }
                                    else
                                    {
                                        this.lvFacturas.Items[contadorFacturas].SubItems.Add("");
                                    }
                                }             
                                    
                                if (tipoDocumento == "ND")                                    
                                {
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add("Nota Debito");                                    
                                }

                                if ((tipoDocumento != "ND") && (tipoDocumento != "NC"))                                    
                                {
                                    this.lvFacturas.Items[contadorFacturas].SubItems.Add("Factura");                                  
                                }

                                contadorFacturas = contadorFacturas + 1;
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

        private void btnReimprimir_Click(object sender, EventArgs e)
        {
            string numf;
            numf = "016-00000083";
            reimpresion re = new reimpresion();
            //PathMoffis = "\"";
            PathMoffis =  System.Windows.Forms.Application.StartupPath.ToString();
            //PathMoffis = PathMoffis + "\"";
            //string Pathre = PathMoffis + @"\reimpresion\audcopyp -p 5 -t f -n 016-00000083,016-00000083";
            int Q;
            int w;

            Q = numf.Length;
            w = numf.IndexOf("-");

            string Pathre = PathMoffis.Substring(0, 3) + '"' + PathMoffis.Substring(3) + '"' + @"\\reimpresion\audcopyp -p 5 -t f -n " + numf.Substring(w+1) + "," + numf.Substring(w +1) + "";

           // MessageBox.Show(Pathre);
            //string Pathre = PathMoffis.Substring(3);

            //string Pathre = '"' + PathMoffis + '"' + @"\reimpresion\audcopyp -p 5 -t f -n 016-00000083,016-00000083";
            //string PathListado2 = PathMoffis + @"\XML\Factura\ListadoAccounts2.xml";
            re.ExecuteCommand(Pathre);
            //re.ExecuteCommand("C:\\\"BS&C\"\\\"VERSIONES FINALES\"\\\"VERSION FINAL MOFFIS HASAR\"\\VERSION_2014_v3_64\\MOFFIS\\MOFFIS\\bin\\Debug\\reimpresion\\audcopyp -p 5 -t f -n 016-00000083,016-00000083");


            //string tipo;
            //string cooReimprimir = "";
            //try
            //{
            //    tipo = number_FacturaSel.Substring(0, 2);
                
            //    if (Tf.CheckFprinter() == true)
            //    {
            //        if (ValidarImpresora())
            //        {
            //            if ((tipo != "ND") && (tipo != "NC"))  
            //            {
            //                cooReimprimir = number_FacturaSel.Substring(4, 8);
            //                if (Tf.SendCmd("RF" + cooReimprimir + cooReimprimir) == true)
            //                {
            //                    MessageBox.Show("Documento impreso corretamente");
            //                }
            //                else
            //                {
            //                    MessageBox.Show("No se pudo reimprimir documento");
            //                }
            //            }
            //            else
            //                if (tipo == "NC")
            //                {
            //                    cooReimprimir = number_FacturaSel.Substring(7, 8);
            //                    if (Tf.SendCmd("RC" + cooReimprimir + cooReimprimir) == true)
            //                    {
            //                        MessageBox.Show("Documento impreso corretamente");
            //                    }
            //                    else
            //                    {
            //                        MessageBox.Show("No se pudo reimprimir documento");
            //                    }
            //                }
            //                else
            //                    if (tipo == "ND")
            //                    {
            //                        cooReimprimir = number_FacturaSel.Substring(7, 8);
            //                        if (Tf.SendCmd("RD" + cooReimprimir + cooReimprimir) == true)
            //                        {
            //                            MessageBox.Show("Documento impreso corretamente");
            //                        }
            //                        else
            //                        {
            //                            MessageBox.Show("No se pudo reimprimir documento");
            //                        }
            //                    }
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Al parecer ocurre un problema con la impresora", "Error con Impresora", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("No se pudo Re-Imprimir el documento", "error", MessageBoxButtons.OK);
            //}
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
        }

        private void lvFacturas_DoubleClick(object sender, EventArgs e)
        {
            this.ClearForm();
            number_FacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].Text;
            customerID_FacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[1].Text;
            customerName_FacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[2].Text;
            date_FacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[3].Text;
            tipoDocumento_FacturaSel = this.lvFacturas.Items[lvFacturas.FocusedItem.Index].SubItems[4].Text;

            this.lblNumeroFactura.Text = number_FacturaSel;
            this.lblIdentificadorCliente.Text = customerID_FacturaSel;
            this.lblNombreCliente.Text = customerName_FacturaSel;
            this.lblFechaFactura.Text = date_FacturaSel;
            this.lblTipoDocumento.Text = tipoDocumento_FacturaSel;
        }

        private void ClearForm()
        {
            this.lblNumeroFactura.Text = "";
            this.lblIdentificadorCliente.Text = "";
            this.lblNombreCliente.Text = "";
            this.lblFechaFactura.Text = "";
            this.lblTipoDocumento.Text = "";
        }

        private void RecargarCotizaciones_SalesOrders()
        {
            this.ClearForm();
            this.lvFacturas.Clear();
            this.ObtenerListadoFacturas();
        }

        private void frmNoFiscal_Re_Imprimir_Load(object sender, EventArgs e)
        {

        }

        private void btnRecargarListados_Click(object sender, EventArgs e)
        {
            this.lvFacturas.Clear();
            this.ObtenerListadoFacturas();
        }
        
    }
}
