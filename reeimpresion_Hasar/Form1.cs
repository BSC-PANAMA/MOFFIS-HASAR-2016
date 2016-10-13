using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using TFHKADIR;
using Interop.PeachwServer;

namespace reeimpresion_Hasar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.CrearDataTableUsuarios();
            this.ObtenerListadoUsuarios();
        }


       
        //private XmlImplementation imp;
        //private XmlDocument doc;
        //private XmlNodeList reader;
        private Array usersList;

        DataTable dtUsuarios;

        static public string PuertoImpresora;

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
            //MOFFIS.GLInformationsss accttype = new MOFFIS.GLInformationsss();

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

        private void btnRecargarListados_Click(object sender, EventArgs e)
        {
            this.lvFacturas.Clear();
            this.ObtenerListadoFacturas();
        }

        private void ClearForm()
        {
            this.lblNumeroFactura.Text = "";
            this.lblIdentificadorCliente.Text = "";
            this.lblNombreCliente.Text = "";
            this.lblFechaFactura.Text = "";
            this.lblTipoDocumento.Text = "";
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

        private void btnReimprimir_Click(object sender, EventArgs e)
        {
            int Q;
            int Q2;
            string numd;
            string tipo;
            int ND;
            int NC;
            numd = lblNumeroFactura.Text;
            Q = numd.IndexOf("-");
            ND = numd.IndexOf("ND");
            NC = numd.IndexOf("NC");

            if (ND > -1)
            {
                tipo = "d";
                Q = numd.IndexOf("-");
                numd = numd.Substring(Q + 1);
                Q2 = numd.IndexOf("-");
                numd = numd.Substring(Q2 + 1);

            }
            else if(NC > -1)
            {
                tipo = "c";
                Q = numd.IndexOf("-");
                numd = numd.Substring(Q + 1);
                Q2 = numd.IndexOf("-");
                numd = numd.Substring(Q2 + 1);

            }
            else
            {
                tipo = "f";
                Q = numd.IndexOf("-");
                numd = numd.Substring(Q + 1);
            }


            reimpresion re = new reimpresion();


            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();



            //string Pathre = PathMoffis.Substring(0, 3) + '"' + PathMoffis.Substring(3) + '"' + @"\\reimpresion\audcopyp -p 5 -t f -n 016-00000083,016-00000083";

            string Pathre = PathMoffis.Substring(0, 3) + '"' + PathMoffis.Substring(3) + '"' + @"\\reimpresion\audcopyp -p " + PuertoImpresora + " -t " + tipo + " -n " + numd + "," + numd + "";


            //string Pathre = "\""+ PathMoffis +"\"" + @"\\reimpresion\audcopyp -p 5 -t f -n 016-00000083,016-00000083";
            //string PathListado2 = PathMoffis + @"\XML\Factura\ListadoAccounts2.xml";
            re.ExecuteCommand(Pathre);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ObtenerListadoUsuarios()
        {
            string sIdUsuario;
            string sNombre;
            string sPassword;
            string sEstatus;
            string sPathCompania;
            string sUsuarioP;
            string sPasswordP;
            string sRol;
            string sPuerto;
            string sCompID;


            try
            {
                this.cbLoginUsuario.Items.Clear();
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                //PathMoffis = System.IO.Directory.GetParent(System.Windows.Forms.Application.ExecutablePath).Parent.Parent.FullName;
                string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\EmpresaUsuarios\Usuarios.xml";

                //MessageBox.Show(PathListado);

                if (this.ValidarExistenciaArchivo(PathListado))
                {
                    imp = new XmlImplementation();
                    doc = imp.CreateDocument();
                    doc.Load(PathListado);
                    reader = doc.GetElementsByTagName("PAW_Usuario");
                    usersList = Array.CreateInstance(typeof(string), 10, reader.Count);

                    for (int i = 0; i <= reader.Count - 1; i++)
                    {
                        sIdUsuario = "";
                        sNombre = "";
                        sPassword = "";
                        sEstatus = "";
                        sPathCompania = "";
                        sUsuarioP = "";
                        sPasswordP = "";
                        sRol = "";
                        sPuerto = "";
                        sCompID = "";

                        for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                        {
                            switch (reader[i].ChildNodes[a].Name)
                            {
                                case "UsuarioID":
                                    {
                                        sIdUsuario = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 0, i);
                                        break;
                                    }
                                case "Nombre":
                                    {
                                        sNombre = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 1, i);
                                        break;
                                    }
                                case "Password":
                                    {
                                        sPassword = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        //sPassword = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 2, i);
                                        break;
                                    }
                                case "Estatus":
                                    {
                                        sEstatus = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 3, i);
                                        break;
                                    }
                                case "PathCompania":
                                    {
                                        sPathCompania = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        //sPathCompania = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 4, i);
                                        break;
                                    }
                                case "Rol":
                                    {
                                        sRol = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 7, i);
                                        break;
                                    }
                                case "UsuarioP":
                                    {
                                        sUsuarioP = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 5, i);
                                        break;
                                    }
                                case "PasswordP":
                                    {
                                        //sPasswordP = reader[i].ChildNodes[a].InnerText; 
                                        sPasswordP = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 6, i);
                                        break;
                                    }
                                case "Puerto":
                                    {
                                        sPuerto = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 8, i);
                                        break;
                                    }
                                case "CompanyID":
                                    {
                                        sCompID = reader[i].ChildNodes[a].InnerText;
                                        usersList.SetValue(reader[i].ChildNodes[a].InnerText, 9, i);
                                        break;
                                    }
                            }
                        }

                        DataRow drDetalleUsuario = dtUsuarios.NewRow();
                        drDetalleUsuario["IdUsuario"] = sIdUsuario;
                        drDetalleUsuario["Nombre"] = sNombre;
                        drDetalleUsuario["Password"] = sPassword;
                        drDetalleUsuario["Estatus"] = sEstatus;
                        drDetalleUsuario["PathCompania"] = sPathCompania;
                        drDetalleUsuario["Rol"] = sRol;
                        drDetalleUsuario["UsuarioP"] = sUsuarioP;
                        drDetalleUsuario["PasswordP"] = sPasswordP;

                        drDetalleUsuario["Puerto"] = sPuerto;
                        drDetalleUsuario["IdCompania"] = sCompID;

                        dtUsuarios.Rows.Add(drDetalleUsuario);
                        dtUsuarios.AcceptChanges();
                    }

                    for (int i = 0; i <= usersList.GetUpperBound(1); i++)
                    {
                        this.cbLoginUsuario.Items.Add(usersList.GetValue(0, i));
                    }

                    imp = null;
                    doc = null;
                    reader = null;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No se puede confirmar la existencia del archivo de usuarios del sistema MOFFIS, consulte con el proveedor del sistema", "Error en archivo de Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void CrearDataTableUsuarios()
        {
            dtUsuarios = new DataTable();
            dtUsuarios.Columns.Add(new DataColumn("IdUsuario", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Nombre", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Password", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Estatus", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("PathCompania", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Rol", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("UsuarioP", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("PasswordP", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Puerto", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("IdCompania", System.Type.GetType("System.String")));

            dtUsuarios.AcceptChanges();
        }

        private bool ValidarExistenciaArchivo(string RutaArchivo)
        {
            return System.IO.File.Exists(RutaArchivo);
        }


        public void puerto()
        {
            //this.CambiarUsuario = "NO";
            int accesoPeachTree = 0;
            string sUsuario;
            string sUsuarioComp = "";
            string sPassword;
            string sPasswordComp = "";
            string logeado = "No";
            string sPathCompania = "";
            string sUsuarioP = "";
            string sPasswordP = "";
            string sRol = "";
            string sPuerto = "";
            string sIDCompania = "";

            sUsuarioComp = this.cbLoginUsuario.Text;

            foreach (DataRow dr in dtUsuarios.Rows)
            {
                sUsuario = dr.ItemArray.GetValue(0).ToString();
                sPassword = dr.ItemArray.GetValue(2).ToString();

                if (sUsuario == sUsuarioComp)
                {
                    logeado = "Si";
                    sPathCompania = dr.ItemArray.GetValue(4).ToString();
                    sRol = dr.ItemArray.GetValue(5).ToString();
                    sUsuarioP = dr.ItemArray.GetValue(6).ToString();
                    sPasswordP = dr.ItemArray.GetValue(7).ToString();

                    //nuevo multiempresa
                    sPuerto = dr.ItemArray.GetValue(8).ToString();
                    sIDCompania = dr.ItemArray.GetValue(9).ToString();


                    //Rol = sRol;
                    //PathCompania = sPathCompania;

                    //nuevo multiempresa

                    PuertoImpresora = sPuerto;
                    //CompaniaID = sIDCompania;

                    //MessageBox.Show(PuertoImpresora.ToString());

                }
            }
            
        }

        private void cbLoginUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            puerto();
        }
    }
}
