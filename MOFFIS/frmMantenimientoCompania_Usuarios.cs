using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Interop.PeachwServer;
using TFHKADIR;

namespace MOFFIS
{
    public partial class frmMantenimientoCompania_Usuarios : Form
    {
        private ConectarPT ptApp = new ConectarPT();
        private CompanyInfoList compList;
        private CompanyInfo compInfo;

        private DataTable dtUsuarios;
        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;
        private Array usersList;

        private bool withGUID;
        private string globalIdCompany = null;
        private string globalPathCompania = "";
        private string globalUsuarioAdmin = "";
        private string globalPasswordAdmin = "";
        private string globalUsuarioSF1 = "";
        private string globalPasswordSF1 = "";
        private string globalUsuarioSF2 = "";
        private string globalPasswordSF2 = "";
        private int IRetorno;

        string PathMoffis;

        public frmMantenimientoCompania_Usuarios()
        {
            InitializeComponent();
            cmbxCompany.SelectedIndex = 0;
            //Usuario de Peachtree
            this.ObtenerUsuariosPeachtree();
            //Usuarios de MOFFIS
            this.CrearDataTableUsuarios();
            this.ObtenerListadoUsuarios();
            this.ObtenerListadoValidacion();
        }

        //USUARIOS
        public void CrearDataTableUsuarios()
        {
            dtUsuarios = new DataTable();
            dtUsuarios.Columns.Add(new DataColumn("IdUsuario", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Nombre", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Password", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Estatus", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Company_id", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("PathCompania", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Rol", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("UsuarioP", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("PasswordP", System.Type.GetType("System.String")));
            dtUsuarios.Columns.Add(new DataColumn("Puerto", System.Type.GetType("System.String")));

            dtUsuarios.AcceptChanges();
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            string RolSel = "";
            string usuarioP = "";
            string passwordP = "";
            DataRow drDetalleUsuario = dtUsuarios.NewRow();
            drDetalleUsuario["IdUsuario"] = this.txtIDUsuario.Text;
            drDetalleUsuario["Nombre"] = this.txtNombreUsuario.Text;

            //drDetalleUsuario["Password"] = this.txtPasswordUsuario.Text;
            drDetalleUsuario["Password"] = Encriptador.RijndaelSimple.Encriptar(this.txtPasswordUsuario.Text);
            drDetalleUsuario["Estatus"] = "Activo";
            drDetalleUsuario["Company_id"] = (cbmx_CompanyId.SelectedIndex + 1).ToString();
            drDetalleUsuario["PathCompania"] = globalPathCompania;
            RolSel = this.cbRoles.Text;
            drDetalleUsuario["Rol"] = RolSel;

            if (RolSel == "ADMIN")
            {
                usuarioP = globalUsuarioAdmin;
                passwordP = globalPasswordAdmin;
            }
            else
                if (RolSel == "CAJERO_1")
                {
                    usuarioP = globalUsuarioSF1;
                    passwordP = globalPasswordSF1;
                }
                else
                    if (RolSel == "CAJERO_2")
                    {
                        usuarioP = globalUsuarioSF2;
                        passwordP = globalPasswordSF2;
                    }
                    else
                        if (RolSel == "REPORTES_XZ")
                        {
                            //usuarioP = Encriptador.RijndaelSimple.Encriptar("REPORTEXZ");
                            usuarioP = "REPORTEXZ";
                            passwordP = Encriptador.RijndaelSimple.Encriptar("REPORTEXZ");
                        }

            drDetalleUsuario["UsuarioP"] = usuarioP;
            drDetalleUsuario["PasswordP"] = passwordP;
            //drDetalleUsuario["Puerto"] = "COM" + txtPort.Text; //Concatenacion Para el puerto USB para la conexion de la Impresora.
            drDetalleUsuario["Puerto"] = txtPort.Text; //Concatenacion Para el puerto USB para la conexion de la Impresora.

            dtUsuarios.Rows.Add(drDetalleUsuario);
            dtUsuarios.AcceptChanges();
            this.LimpiarAddUsuarios();

            MessageBox.Show("Los datos del usuario fueron adicionados a la lista, porfavor dar click al boton Guardar Usuarios para almacenarlos permanentemente");
        }

        private void ObtenerUsuariosPeachtree()
        {
            string sPath = "";
            string sPathE = "";
            string scompañiaId = "";
            string sUsuarioAdmin = "";
            string sPasswordAdmin = "";
            string sPasswordAdminE = "";
            string sUsuarioSF1 = "";
            string sPasswordSF1 = "";
            string sPasswordSF1E = "";
            string sUsuarioSF2 = "";
            string sPasswordSF2 = "";
            string sPasswordSF2E = "";

            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\Sistema\BSC\UsuariosPeachtree" + globalIdCompany + ".xml";
                //string PathListado = PathMoffis + @"\XML\Sistema\BSC\UsuariosPeachtree.xml";

                if (System.IO.File.Exists(PathListado))
                {
                    imp = new XmlImplementation();
                    doc = imp.CreateDocument();
                    doc.Load(PathListado);
                    reader = doc.GetElementsByTagName("PAW_Usuarios");
                    usersList = Array.CreateInstance(typeof(string), 7, reader.Count);
                    for (int i = 0; i <= reader.Count - 1; i++)
                    {
                        for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                        {
                            switch (reader[i].ChildNodes[a].Name)
                            {
                                case "PathCompania":
                                    {
                                        sPath = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        sPathE = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                                case "UsuarioAdmin":
                                    {
                                        sUsuarioAdmin = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                                case "ContrasenaAdmin":
                                    {
                                        sPasswordAdmin = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        sPasswordAdminE = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                                case "UsuarioSF1":
                                    {
                                        sUsuarioSF1 = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                                case "ContrasenaSF1":
                                    {
                                        sPasswordSF1 = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        sPasswordSF1E = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                                case "UsuarioSF2":
                                    {
                                        sUsuarioSF2 = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                                case "ContrasenaSF2":
                                    {
                                        sPasswordSF2 = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                        sPasswordSF2E = reader[i].ChildNodes[a].InnerText;
                                        break;
                                    }
                            }
                        }
                    }

                    this.txtPathCompania.Text = sPath;
                    this.txtUsuarioAdmin.Text = sUsuarioAdmin;
                    this.txtPasswordAdmin.Text = sPasswordAdmin;
                    this.txtUsuarioSF1.Text = sUsuarioSF1;
                    this.txtPasswordSF1.Text = sPasswordSF1;
                    this.txtUsuarioSF2.Text = sUsuarioSF2;
                    this.txtPasswordSF2.Text = sPasswordSF2;

                    globalPathCompania = sPathE;

                    globalUsuarioAdmin = sUsuarioAdmin;
                    globalPasswordAdmin = sPasswordAdminE;
                    globalUsuarioSF1 = sUsuarioSF1;
                    globalPasswordSF1 = sPasswordSF1E;
                    globalUsuarioSF2 = sUsuarioSF2;
                    globalPasswordSF2 = sPasswordSF2E;

                    imp = null;
                    doc = null;
                    reader = null;
                }
                else
                {
                    MessageBox.Show("El Archivo de usuarios de Peachtree para esta compañia aún no ha sido creado", "Archivo No Existe", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ObtenerListadoUsuarios()
        {
            string sIdUsuario;
            string sNombre;
            string sCompanyId;
            string sPassword;
            string sEstatus;
            string sPathCompania;
            string sRol;
            string sUsuarioP;
            string sPasswordP;
            string sPort;

            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\EmpresaUsuarios\Usuarios.xml";


                if (System.IO.File.Exists(PathListado))
                {
                    imp = new XmlImplementation();
                    doc = imp.CreateDocument();
                    doc.Load(PathListado);
                    reader = doc.GetElementsByTagName("PAW_Usuario");

                    if (reader != null)
                    {
                        usersList = Array.CreateInstance(typeof(string), 8, reader.Count);
                        for (int i = 0; i <= reader.Count - 1; i++)
                        {
                            sIdUsuario = "";
                            sNombre = "";
                            sPassword = "";
                            sCompanyId = "";
                            sEstatus = "";
                            sPathCompania = "";
                            sRol = "";
                            sUsuarioP = "";
                            sPasswordP = "";
                            sPort = "";

                            for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                            {
                                switch (reader[i].ChildNodes[a].Name)
                                {
                                    case "UsuarioID":
                                        {
                                            sIdUsuario = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "Nombre":
                                        {
                                            sNombre = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "Password":
                                        {
                                            sPassword = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "Estatus":
                                        {
                                            sEstatus = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "CompanyID":
                                        {
                                            sCompanyId = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "PathCompania":
                                        {
                                            sPathCompania = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }

                                    case "Rol":
                                        {
                                            sRol = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "UsuarioP":
                                        {
                                            sUsuarioP = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "PasswordP":
                                        {
                                            sPasswordP = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "Puerto":
                                        {
                                            sPort = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                }
                            }

                            DataRow drDetalleUsuario = dtUsuarios.NewRow();
                            drDetalleUsuario["IdUsuario"] = sIdUsuario;
                            drDetalleUsuario["Nombre"] = sNombre;
                            drDetalleUsuario["Password"] = sPassword;
                            drDetalleUsuario["Estatus"] = sEstatus;
                            drDetalleUsuario["Company_id"] = sCompanyId;
                            drDetalleUsuario["PathCompania"] = sPathCompania;
                            drDetalleUsuario["Rol"] = sRol;
                            drDetalleUsuario["UsuarioP"] = sUsuarioP;
                            drDetalleUsuario["PasswordP"] = sPasswordP;
                            drDetalleUsuario["Puerto"] = sPort;

                            dtUsuarios.Rows.Add(drDetalleUsuario);

                            dtUsuarios.AcceptChanges();
                        }
                    }
                    imp = null;
                    doc = null;
                    reader = null;
                }
                else
                {
                    MessageBox.Show("El Archivo de usuarios del sistema MOFFIS no ha sido creado", "Archivo No Existe", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            { }
        }

        private void frmMantenimientoCompania_Usuarios_Load(object sender, EventArgs e)
        {
            //Compania
            this.dirListBoxCompanias.Path = ptApp.app.DataPath.ToString();
            this.driveListBoxCompanias.Drive = ptApp.app.DataPath.ToString();

            //Usuarios
            this.dgvUsuarios.DataSource = dtUsuarios;
        }

        private void btnCrearUsuariosPeachtree_Click(object sender, EventArgs e)
        {
            try
            {
                this.CrearXML_UsuariosPeachtree();
                MessageBox.Show("Los usuarios de acceso a Peachtree fueron creados correctamente");
                this.ObtenerUsuariosPeachtree();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el usuario de solo facturacion");
            }
        }

        private void CrearXML_UsuariosPeachtree()
        {
            //System.Configuration.ConfigurationSettings.AppSettings.Add


            //string sPath = this.txtPathCompania.Text;


            string sPath = Encriptador.RijndaelSimple.Encriptar(this.txtPathCompania.Text);

            string sUsuarioAdmin = this.txtUsuarioAdmin.Text;

            //string sPasswordAdmin = this.txtPasswordAdmin.Text; 
            string sPasswordAdmin = Encriptador.RijndaelSimple.Encriptar(this.txtPasswordAdmin.Text);

            string sUsuarioSF1 = this.txtUsuarioSF1.Text;

            //string sPasswordSF1 = this.txtPasswordSF1.Text;
            string sPasswordSF1 = Encriptador.RijndaelSimple.Encriptar(this.txtPasswordSF1.Text);

            string sUsuarioSF2 = this.txtUsuarioSF2.Text;

            //string sPasswordSF2 = this.txtPasswordSF2.Text;
            string sPasswordSF2 = Encriptador.RijndaelSimple.Encriptar(this.txtPasswordSF2.Text);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\BSC\UsuariosPeachtree" + globalIdCompany + ".xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_UsuariosPeachtree");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_Usuarios");
            Writer.WriteElementString("PathCompania", sPath);
            Writer.WriteElementString("UsuarioAdmin", sUsuarioAdmin);
            Writer.WriteElementString("ContrasenaAdmin", sPasswordAdmin);
            Writer.WriteElementString("UsuarioSF1", sUsuarioSF1);
            Writer.WriteElementString("ContrasenaSF1", sPasswordSF1);
            Writer.WriteElementString("UsuarioSF2", sUsuarioSF2);
            Writer.WriteElementString("ContrasenaSF2", sPasswordSF2);
            Writer.WriteEndElement();//("PAW_UsuarioSF")

            Writer.WriteEndElement();//("PAW_UsuariosSF")

            Writer.Close();

            globalPathCompania = sPath;
            globalUsuarioSF1 = sUsuarioSF1;
            globalPasswordSF1 = sPasswordSF1;
            globalUsuarioSF2 = sUsuarioSF2;
            globalPasswordSF2 = sPasswordSF2;
        }

        private void dirListBoxCompanias_DoubleClick(object sender, EventArgs e)
        {
            this.ListadoCompanias(withGUID);
        }

        public void ListadoCompanias(bool byGuid)
        {
            compList = (Interop.PeachwServer.CompanyInfoList)ptApp.app.GetCompanyInfoList(this.dirListBoxCompanias.Path.ToString());
            lvCompanias.Items.Clear();
            lvCompanias.Columns.Clear();
            if (byGuid == true)
            {
                this.lvCompanias.Columns.Add("GUID", -2, HorizontalAlignment.Left);
                this.lvCompanias.Columns.Add("Company Name", -2, HorizontalAlignment.Left);
                this.lvCompanias.Columns.Add("Path", -2, HorizontalAlignment.Left);
                for (int i = 0; i <= compList.Count - 1; i++)
                {
                    compInfo = (CompanyInfo)compList.Item(i);
                    this.lvCompanias.Items.Add(compInfo.GUID);
                    this.lvCompanias.Items[i].SubItems.Add(compInfo.Name);
                    this.lvCompanias.Items[i].SubItems.Add(compInfo.Path);
                }
            }
            else
            {
                this.lvCompanias.Columns.Add("Company Name", -2, HorizontalAlignment.Left);
                this.lvCompanias.Columns.Add("Path", -2, HorizontalAlignment.Left);
                for (int i = 0; i <= compList.Count - 1; i++)
                {
                    compInfo = (CompanyInfo)compList.Item(i);
                    this.lvCompanias.Items.Add(compInfo.Name);
                    this.lvCompanias.Items[i].SubItems.Add(compInfo.Path);
                }
            }
            this.lvCompanias.View = View.Details;
            foreach (ColumnHeader col in lvCompanias.Columns)
            {
                col.Width = -2;
            }
            compList = null;
            compInfo = null;
        }

        private void lvCompanias_DoubleClick(object sender, EventArgs e)
        {
            string sPath;
            string sPath2;
            if (withGUID == true)
            {
                sPath = this.lvCompanias.Items[lvCompanias.FocusedItem.Index].SubItems[2].Text;
                sPath2 = this.lvCompanias.Items[lvCompanias.FocusedItem.Index].SubItems[0].Text;
            }
            else
            {
                sPath = this.lvCompanias.Items[lvCompanias.FocusedItem.Index].SubItems[1].Text;
                sPath2 = this.lvCompanias.Items[lvCompanias.FocusedItem.Index].SubItems[0].Text;
            }

            this.txtPathCompania.Text = sPath;
            this.txtNcompania.Text = sPath2;
        }

        private void btnGuardarUsuarios_Click(object sender, EventArgs e)
        {
            try
            {
                this.CrearXML_Usuarios();
                MessageBox.Show("Los usuarios fueron almacenados correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al almacenar usuarios");
            }
        }

        private void CrearXML_Usuarios()
        {
            int cantLineas = 0;
            cantLineas = (dgvUsuarios.Rows.Count - 1);
            string sIdUsuario = "";
            string sNombre = "";
            string sPassword = "";
            string sEstatus = "";
            string sIdCompañia = "";
            string sPathCompania = "";
            string sUsuarioP = "";
            string sPasswordP = "";
            string sRol = "";
            string sPuerto = "";

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\EmpresaUsuarios\Usuarios.xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_Usuarios");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            for (int lineas = 0; lineas < cantLineas; ++lineas)
            {
                sIdUsuario = dgvUsuarios.Rows[lineas].Cells[1].Value.ToString();
                sNombre = dgvUsuarios.Rows[lineas].Cells[2].Value.ToString();
                sPassword = dgvUsuarios.Rows[lineas].Cells[3].Value.ToString();
                //sPassword = Encriptador.RijndaelSimple.Encriptar(dgvUsuarios.Rows[lineas].Cells[3].Value.ToString()); 
                sEstatus = dgvUsuarios.Rows[lineas].Cells[4].Value.ToString();
                sIdCompañia = dgvUsuarios.Rows[lineas].Cells[5].Value.ToString();
                sPathCompania = dgvUsuarios.Rows[lineas].Cells[6].Value.ToString();
                sRol = dgvUsuarios.Rows[lineas].Cells[7].Value.ToString();

                //sUsuarioP = Encriptador.RijndaelSimple.Encriptar(dgvUsuarios.Rows[lineas].Cells[7].Value.ToString()); 
                sUsuarioP = dgvUsuarios.Rows[lineas].Cells[8].Value.ToString();
                //sPasswordP = Encriptador.RijndaelSimple.Encriptar(dgvUsuarios.Rows[lineas].Cells[8].Value.ToString()); 
                sPasswordP = dgvUsuarios.Rows[lineas].Cells[9].Value.ToString();
                sPuerto = dgvUsuarios.Rows[lineas].Cells[10].Value.ToString();

                Writer.WriteStartElement("PAW_Usuario");
                Writer.WriteElementString("UsuarioID", sIdUsuario);
                Writer.WriteElementString("Nombre", sNombre);
                Writer.WriteElementString("Password", sPassword);
                Writer.WriteElementString("Estatus", sEstatus);
                Writer.WriteElementString("CompanyID", sIdCompañia);
                Writer.WriteElementString("PathCompania", sPathCompania);
                Writer.WriteElementString("Rol", sRol);
                Writer.WriteElementString("UsuarioP", sUsuarioP);
                Writer.WriteElementString("PasswordP", sPasswordP);
                Writer.WriteElementString("Puerto", sPuerto);

                Writer.WriteEndElement();//("PAW_Usuario")
            }

            Writer.WriteEndElement();//("PAW_Usuarios")

            Writer.Close();
        }

        private void LimpiarAddUsuarios()
        {
            this.txtIDUsuario.Text = "";
            this.txtNombreUsuario.Text = "";
            this.txtPasswordUsuario.Text = "";
            this.cbRoles.Text = "";
        }

        private void driveListBoxCompanias_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dirListBoxCompanias.Path = driveListBoxCompanias.Drive.ToString();
            //CompanyList(withGUID);
        }

        private void btnInicializarIndicadorError_Click(object sender, EventArgs e)
        {
            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                string PathListado = PathMoffis + @"\XML\Sistema\BSC\IndicadorError.xml";
                //string PathListado2 = PathMoffis + @"\XML\NotaDebito\ListadoItems2.xml";

                XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

                Writer.WriteStartElement("Indicador_Error");
                Writer.WriteStartElement("IndicadorError");
                Writer.WriteString("F0");
                Writer.WriteEndElement();
                Writer.WriteEndElement();

                Writer.Close();
                MessageBox.Show("El archivo inicializador de errores fue creado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear archivo inicializador");
            }
        }

        private void cmbxCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            globalIdCompany = (cmbxCompany.SelectedIndex + 1).ToString();
            Clean_InfoCompany();
            ObtenerUsuariosPeachtree();
        }


        private void Clean_InfoCompany()
        {
            foreach (Control x in panel7.Controls)
            {
                if (x is TextBox)
                {
                    ((TextBox)x).Text = String.Empty;
                }
            }

        }

        private void cbmx_CompanyId_SelectedIndexChanged(object sender, EventArgs e)
        {
            globalIdCompany = (cbmx_CompanyId.SelectedIndex + 1).ToString();
            Clean_InfoCompany();
            ObtenerUsuariosPeachtree();
        }

        //nuevo 06-11-13

        private void CrearXML_ValidarMoffis()
        {
            //System.Configuration.ConfigurationSettings.AppSettings.Add


            //string sPath = this.txtPathCompania.Text;


            string sCompania = Encriptador.RijndaelSimple.Encriptar(this.txtNcompania.Text);
            string sConsultor = Encriptador.RijndaelSimple.Encriptar(this.txtContraconsultor.Text);
            string sDireccion = this.txtDirvali.Text;
            string sTelefono = this.txtTelvali.Text;

            ////string sPasswordSF1 = this.txtPasswordSF1.Text;
            //string sPasswordSF1 = Encriptador.RijndaelSimple.Encriptar(this.txtPasswordSF1.Text);

            //string sUsuarioSF2 = this.txtUsuarioSF2.Text;

            ////string sPasswordSF2 = this.txtPasswordSF2.Text;
            //string sPasswordSF2 = Encriptador.RijndaelSimple.Encriptar(this.txtPasswordSF2.Text);

            PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
            string PathListado = PathMoffis + @"\XML\Sistema\BSC\ValidacionMoffis.xml";
            //string PathListado = PathMoffis + @"\XML\Sistema\BSC\ValidacionMoffis" + globalIdCompany + ".xml";

            XmlTextWriter Writer = new XmlTextWriter(PathListado, System.Text.Encoding.UTF8);

            Writer.WriteStartElement("PAW_ValidarMoffisPeachtree");

            Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
            Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

            Writer.WriteStartElement("PAW_ValidarMoffis");
            Writer.WriteElementString("Compania", sCompania);
            Writer.WriteElementString("Consultor", sConsultor);
            Writer.WriteElementString("Direccion", sDireccion);
            Writer.WriteElementString("Telefono", sTelefono);
            Writer.WriteEndElement();//("PAW_UsuarioSF")
            Writer.WriteEndElement();//("PAW_UsuariosSF")

            Writer.Close();

            //globalPathCompania = sPath;
            //globalUsuarioSF1 = sUsuarioSF1;
            //globalPasswordSF1 = sPasswordSF1;
            //globalUsuarioSF2 = sUsuarioSF2;
            //globalPasswordSF2 = sPasswordSF2;
        }

        private void btnValidarInstalacion_Click(object sender, EventArgs e)
        {

        }

        //nuevo 06-11-13

        private void ObtenerListadoValidacion()
        {
            string sCompania;
            string sConsultor;
            string sDireccion;
            string sTelefono;


            try
            {
                PathMoffis = System.Windows.Forms.Application.StartupPath.ToString();
                //string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\EmpresaUsuarios\Usuarios.xml";
                string PathListado = PathMoffis + @"\XML\Sistema\BSC\ValidacionMoffis.xml";


                sCompania = "";
                sConsultor = "";
                sDireccion = "";
                sTelefono = "";


                if (System.IO.File.Exists(PathListado))
                {
                    imp = new XmlImplementation();
                    doc = imp.CreateDocument();
                    doc.Load(PathListado);
                    reader = doc.GetElementsByTagName("PAW_ValidarMoffis");

                    if (reader != null)
                    {
                        usersList = Array.CreateInstance(typeof(string), 8, reader.Count);
                        for (int i = 0; i <= reader.Count - 1; i++)
                        {


                            for (int a = 0; a <= reader[i].ChildNodes.Count - 1; a++)
                            {
                                switch (reader[i].ChildNodes[a].Name)
                                {
                                    case "Compania":
                                        {
                                            sCompania = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                            break;
                                        }
                                    case "Consultor":
                                        {
                                            sConsultor = Encriptador.RijndaelSimple.Desencriptar(reader[i].ChildNodes[a].InnerText);
                                            break;
                                        }
                                    case "Direccion":
                                        {
                                            sDireccion = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }
                                    case "Telefono":
                                        {
                                            sTelefono = reader[i].ChildNodes[a].InnerText;
                                            break;
                                        }

                                }
                            }


                        }

                        txtNcompania.Text = sCompania;
                        txtContraconsultor.Text = sConsultor;
                        txtDirvali.Text = sDireccion;
                        txtTelvali.Text = sTelefono;


                    }

                    btnGuardarUsuarios.Enabled = true;
                    txtContraconsultor.ReadOnly = true;
                    txtDirvali.ReadOnly = true;
                    txtTelvali.ReadOnly = true;
                    btnValidarInstalacion.Enabled = false;



                    imp = null;
                    doc = null;
                    reader = null;
                }
                else
                {
                    MessageBox.Show("El Archivo de validacion del sistema MOFFIS no ha sido creado", "Archivo No Existe", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            { }
        }

        private void btnValidarInstalacion_Click_1(object sender, EventArgs e)
        {
            correo Correo = new correo();
            if (txtContraconsultor.Text == "")
            {
                MessageBox.Show("Por favor introdusca la contraseña de consultor");
            }

            if (txtDirvali.Text == "")
            {
                MessageBox.Show("Por favor introdusca la direccion");
            }

            if (txtTelvali.Text == "")
            {
                MessageBox.Show("Por favor introdusca el telefono");
            }

            if (txtNcompania.Text == "")
            {
                MessageBox.Show("Por favor introdusca nombre de la compania");
            }

            if (txtContraconsultor.Text != "" && txtDirvali.Text != "" && txtTelvali.Text != "" && txtNcompania.Text != "")
            {
                CLAVES validarc = new CLAVES();

                if (validarc.varificarclave(txtContraconsultor.Text))
                {
                    DialogResult result;


                    result = MessageBox.Show("Desea validar su version de moffis con los datos introducidos", "Validar Moffis", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {

                        if (Correo.SendEmail_Act_Proc(txtNcompania.Text, txtTelvali.Text, txtDirvali.Text, txtContraconsultor.Text))
                        {
                            CrearXML_ValidarMoffis();
                            btnGuardarUsuarios.Enabled = true;
                            txtContraconsultor.ReadOnly = true;
                            txtDirvali.ReadOnly = true;
                            txtTelvali.ReadOnly = true;
                            btnValidarInstalacion.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Error al enviar correo de validacion");
                        }
                    }

                }
                else
                {
                    MessageBox.Show("contraseña de consultor no valida");
                }




            }
        }
    }
}
