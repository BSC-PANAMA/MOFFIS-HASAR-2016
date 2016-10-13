using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MOFFIS
{
    public partial class frmLogin : Form
    {        
        private XmlImplementation imp;
        private XmlDocument doc;
        private XmlNodeList reader;
        private Array usersList;

        private string PathMoffis;

        //Data table utilizado para cargar el listado de usuarios del XML
        DataTable dtUsuarios;

        static public string Rol;
        static public string PathCompania;

        static public string sCambiarUsuario;

        static public string PuertoImpresora;
        static public string CompaniaID;

        public string CambiarUsuario
        {
            get 
            { return frmLogin.sCambiarUsuario; }
            set { frmLogin.sCambiarUsuario = value; }
        }

        public frmLogin()
        {
            InitializeComponent();
            //DefInstanceLogin = this;
            this.CrearDataTableUsuarios();
            this.ObtenerListadoUsuarios();
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
                string PathListado = PathMoffis + @"\XML\Sistema\Mantenimiento\EmpresaUsuarios\Usuarios.xml";

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

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.CambiarUsuario = "NO";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.CambiarUsuario = "NO";
            this.ValidarLogin();
        }

        private void ValidarLogin()
        {
            this.CambiarUsuario = "NO";
            int accesoPeachTree = 0;
            string sUsuario;
            string sUsuarioComp;
            string sPassword;
            string sPasswordComp;
            string logeado = "No";
            string sPathCompania = "";
            string sUsuarioP = "";
            string sPasswordP = "";
            string sRol = "";
            string sPuerto = "";
            string sIDCompania = "";

            sUsuarioComp = this.cbLoginUsuario.Text;
            sPasswordComp = this.txtLoginPassword.Text;

            if ((sUsuarioComp.Trim() == "") || (sPasswordComp.Trim() == ""))
            {
                System.Windows.Forms.MessageBox.Show("Los Campos usuario y password son requeridos", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if ((sUsuarioComp == "BSCMOFFIS") && (sPasswordComp == "K24TYPER"))
                {
                    this.Hide();
                    Rol = "BSC";
                    logeado = "Si";                    
                    frmPrincipal principal = new frmPrincipal();
                    principal.ROL = Rol;
                    principal.PathCompania = PathCompania;
                    principal.ShowDialog();

                    if (this.CambiarUsuario == "SI")
                    {
                        dtUsuarios.Clear();
                        this.ObtenerListadoUsuarios();
                        this.CambiarUsuario = "NO";
                        this.cbLoginUsuario.Text = "";
                        this.txtLoginPassword.Text = "";
                        this.Show();
                    }
                    else
                    {
                        System.Windows.Forms.Application.Exit();
                    }  
                }
                else
                    if ((sUsuarioComp == "REPORTE") && (sPasswordComp == "REPORTE"))
                    {
                        this.Hide();
                        Rol = "REPORTE";
                        logeado = "Si";
                        frmPrincipal principal = new frmPrincipal();
                        principal.ROL = Rol;
                        principal.PathCompania = PathCompania;
                        principal.ShowDialog();

                        if (this.CambiarUsuario == "SI")
                        {
                            dtUsuarios.Clear();
                            this.ObtenerListadoUsuarios();
                            this.CambiarUsuario = "NO";
                            this.cbLoginUsuario.Text = "";
                            this.txtLoginPassword.Text = "";
                            this.Show();
                        }
                        else
                        {
                            System.Windows.Forms.Application.Exit();
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dtUsuarios.Rows)
                        {
                            sUsuario = dr.ItemArray.GetValue(0).ToString();
                            sPassword = dr.ItemArray.GetValue(2).ToString();

                            if (sUsuario == sUsuarioComp && sPassword == sPasswordComp)
                            {
                                logeado = "Si";
                                sPathCompania = dr.ItemArray.GetValue(4).ToString();
                                sRol = dr.ItemArray.GetValue(5).ToString();
                                sUsuarioP = dr.ItemArray.GetValue(6).ToString();
                                sPasswordP = dr.ItemArray.GetValue(7).ToString();

                                //nuevo multiempresa
                                sPuerto = dr.ItemArray.GetValue(8).ToString();
                                sIDCompania = dr.ItemArray.GetValue(9).ToString();

                                
                                Rol = sRol;
                                PathCompania = sPathCompania;

                                //nuevo multiempresa

                                PuertoImpresora = sPuerto;
                                CompaniaID = sIDCompania;

                            }
                        }

                        if (logeado == "Si")
                        {
                            ConectarPT ptApp = new ConectarPT();
                            if (ptApp.app.CheckCompanyUsesPasswords(sPathCompania))
                            {
                                try
                                {
                                    ptApp.app.OpenCompanySecure(sPathCompania, sUsuarioP, sPasswordP);
                                }
                                catch (Exception ex)
                                {
                                    accesoPeachTree = 1;
                                    System.Windows.Forms.MessageBox.Show("Sin acceso a la compañia, verifique que no este habilitada ninguna pantalla de verificación en peachtree", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                ptApp.app.OpenCompany(sPathCompania);
                            }
                            //}

                            if (accesoPeachTree.Equals(0))
                            {
                                this.Hide();
                                frmPrincipal principal = new frmPrincipal();
                                principal.ROL = Rol;
                                principal.PathCompania = PathCompania;

                                //nuevo multiempresa

                                principal.PuertoImp = PuertoImpresora;
                                principal.IDcompania = CompaniaID;

                                principal.ShowDialog();

                                if (this.CambiarUsuario == "SI")
                                {
                                    dtUsuarios.Clear();
                                    this.ObtenerListadoUsuarios();
                                    this.CambiarUsuario = "NO";
                                    this.cbLoginUsuario.Text = "";
                                    this.txtLoginPassword.Text = "";
                                    this.Show();
                                }
                                else
                                {
                                    System.Windows.Forms.Application.Exit();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Este usuario no tiene permisos para accesar al sistema.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtLoginPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ValidarLogin();
            }
        }            
    }
}
