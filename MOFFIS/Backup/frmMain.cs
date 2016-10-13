using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace CSSDK
{
	public class frmMain : System.Windows.Forms.Form
    {
        public static string sName = "Name";
        public static string sPassword = "Password";
		private Connect ptApp = new Connect();

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem menuItem28;
        private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItem38;
		private System.Windows.Forms.MenuItem menuItem39;
		private System.Windows.Forms.MenuItem menuItem41;
		private System.Windows.Forms.MenuItem menuItem42;
		private System.Windows.Forms.MenuItem menuItem44;
		private System.Windows.Forms.MenuItem menuItem45;
		private System.Windows.Forms.MenuItem menuItem46;
        private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.MenuItem menuItem49;
		private System.Windows.Forms.MenuItem menuItem50;
		private System.Windows.Forms.MenuItem menuItem51;
		private System.Windows.Forms.MenuItem menuItem52;
        private System.Windows.Forms.MenuItem menuItem53;
        private MenuItem menuItem30;
        private IContainer components;

		public frmMain()
		{

			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.menuItem38 = new System.Windows.Forms.MenuItem();
            this.menuItem39 = new System.Windows.Forms.MenuItem();
            this.menuItem46 = new System.Windows.Forms.MenuItem();
            this.menuItem41 = new System.Windows.Forms.MenuItem();
            this.menuItem30 = new System.Windows.Forms.MenuItem();
            this.menuItem42 = new System.Windows.Forms.MenuItem();
            this.menuItem45 = new System.Windows.Forms.MenuItem();
            this.menuItem44 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItem52 = new System.Windows.Forms.MenuItem();
            this.menuItem53 = new System.Windows.Forms.MenuItem();
            this.menuItem49 = new System.Windows.Forms.MenuItem();
            this.menuItem50 = new System.Windows.Forms.MenuItem();
            this.menuItem51 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem10,
            this.menuItem12,
            this.menuItem13});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5,
            this.menuItem6,
            this.menuItem7,
            this.menuItem8,
            this.menuItem9});
            this.menuItem1.Text = "Archivo";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Conectar a Peachtree";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Cerrar Peachtree";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem14,
            this.menuItem15});
            this.menuItem5.Text = "Abrir Compania";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 0;
            this.menuItem14.Text = "Por GUID";
            this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 1;
            this.menuItem15.Text = "Por Nombre";
            this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "Abrir Compania Previa";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 5;
            this.menuItem7.Text = "Cerrar Compania";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 6;
            this.menuItem8.Text = "-";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 7;
            this.menuItem9.Text = "Salir";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem11,
            this.menuItem19,
            this.menuItem16,
            this.menuItem20});
            this.menuItem10.Text = "Metodos de Interface";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 0;
            this.menuItem11.Text = "Propiedades de la Aplicacion";
            this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 1;
            this.menuItem19.Text = "-";
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 2;
            this.menuItem16.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem17,
            this.menuItem18});
            this.menuItem16.Text = "General Ledger";
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 0;
            this.menuItem17.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem23,
            this.menuItem24});
            this.menuItem17.Text = "Cuentas Activas GL";
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 0;
            this.menuItem23.Text = "Sin GUID";
            this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 1;
            this.menuItem24.Text = "Con GUID";
            this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 1;
            this.menuItem18.Text = "Obtener Periodos Contables";
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 3;
            this.menuItem20.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem21,
            this.menuItem22});
            this.menuItem20.Text = "Aging";
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 0;
            this.menuItem21.Text = "Accounts Receivable";
            this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 1;
            this.menuItem22.Text = "Accounts Payable";
            this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem25,
            this.menuItem38});
            this.menuItem12.Text = "Metodos de Exportacion";
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 0;
            this.menuItem25.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem26});
            this.menuItem25.Text = "Mantenimiento de Registros";
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 0;
            this.menuItem26.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem28,
            this.menuItem29});
            this.menuItem26.Text = "Cuentas";
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 0;
            this.menuItem28.Text = "Ver";
            this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 1;
            this.menuItem29.Text = "Eliminar";
            this.menuItem29.Click += new System.EventHandler(this.menuItem29_Click);
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 1;
            this.menuItem38.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem39,
            this.menuItem42});
            this.menuItem38.Text = "Registros Transaccionales";
            // 
            // menuItem39
            // 
            this.menuItem39.Index = 0;
            this.menuItem39.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem46,
            this.menuItem41,
            this.menuItem30});
            this.menuItem39.Text = "Accounts Receivable";
            // 
            // menuItem46
            // 
            this.menuItem46.Index = 0;
            this.menuItem46.Text = "Ver Sales Invoice";
            this.menuItem46.Click += new System.EventHandler(this.menuItem46_Click);
            // 
            // menuItem41
            // 
            this.menuItem41.Index = 1;
            this.menuItem41.Text = "Borrar Sales Invoice";
            this.menuItem41.Click += new System.EventHandler(this.menuItem41_Click);
            // 
            // menuItem30
            // 
            this.menuItem30.Index = 2;
            this.menuItem30.Text = "Ver Sales Orders";
            this.menuItem30.Click += new System.EventHandler(this.menuItem30_Click);
            // 
            // menuItem42
            // 
            this.menuItem42.Index = 1;
            this.menuItem42.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem45,
            this.menuItem44});
            this.menuItem42.Text = "Accounts Payable";
            // 
            // menuItem45
            // 
            this.menuItem45.Index = 0;
            this.menuItem45.Text = "Ver Purchase Invoice";
            this.menuItem45.Click += new System.EventHandler(this.menuItem45_Click);
            // 
            // menuItem44
            // 
            this.menuItem44.Index = 1;
            this.menuItem44.Text = "Borrar Purchase Invoice";
            this.menuItem44.Click += new System.EventHandler(this.menuItem44_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 3;
            this.menuItem13.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem27,
            this.menuItem49});
            this.menuItem13.Text = "Metodos de Importacion";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 0;
            this.menuItem27.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem52});
            this.menuItem27.Text = "Mantenimiento de Registros";
            // 
            // menuItem52
            // 
            this.menuItem52.Index = 0;
            this.menuItem52.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem53});
            this.menuItem52.Text = "Cuentas";
            // 
            // menuItem53
            // 
            this.menuItem53.Index = 0;
            this.menuItem53.Text = "Nuevo";
            this.menuItem53.Click += new System.EventHandler(this.menuItem53_Click);
            // 
            // menuItem49
            // 
            this.menuItem49.Index = 1;
            this.menuItem49.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem50,
            this.menuItem51});
            this.menuItem49.Text = "Registros Transaccionales";
            // 
            // menuItem50
            // 
            this.menuItem50.Index = 0;
            this.menuItem50.Text = "Nuevo Sales Invoice";
            this.menuItem50.Click += new System.EventHandler(this.menuItem50_Click);
            // 
            // menuItem51
            // 
            this.menuItem51.Index = 1;
            this.menuItem51.Text = "Nuevo Purchase Invoice";
            this.menuItem51.Click += new System.EventHandler(this.menuItem51_Click);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(800, 457);
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "frmMain";
            this.Text = "Peachtree Accounting SDK Sample Code";
            this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
            Login pFrmLogin = new Login();

            if (pFrmLogin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                frmMain.sName = pFrmLogin.NameEditBox.Text;
                frmMain.sPassword = pFrmLogin.PasswordEditBox.Text;

            }

			Application.Run(new frmMain());
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
            Login pFrmLogin = new Login();

            if (pFrmLogin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                frmMain.sName = pFrmLogin.NameEditBox.Text;
                frmMain.sPassword = pFrmLogin.PasswordEditBox.Text;

            }

			Connect ptApp = new Connect();
			MessageBox.Show("Peachtree is now running.");
		}

		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			frmAppProperties appProps = new frmAppProperties();
			appProps.MdiParent = this;
			appProps.Show();
		}

		private void menuItem21_Click(object sender, System.EventArgs e)
		{
			frmAging agingForm = new frmAging();
			agingForm.MdiParent = this;
			agingForm.Show();
		}

		private void menuItem22_Click(object sender, System.EventArgs e)
		{
			frmAging agingForm = new frmAging();
			agingForm.MdiParent = this;
			agingForm.Show();		
		}

		private void menuItem18_Click(object sender, System.EventArgs e)
		{
			frmAccountingPeriods AcctPers = new frmAccountingPeriods();
			AcctPers.MdiParent = this;
			AcctPers.Show();
		}

		private void menuItem23_Click(object sender, System.EventArgs e)
		{
			frmActiveGLAccts ActAccts = new frmActiveGLAccts();
			ActAccts.MdiParent = this;
			ActAccts.Show();
		}

		private void menuItem24_Click(object sender, System.EventArgs e)
		{
			frmActiveGLAcctsWithGuid ActAccts = new frmActiveGLAcctsWithGuid();
			ActAccts.MdiParent = this;
			ActAccts.Show();
		}

		private void menuItem28_Click(object sender, System.EventArgs e)
		{
			frmMaintGLAccts maintcoa = new frmMaintGLAccts();
			maintcoa.MdiParent = this;

			maintcoa.button1.Visible = false;
			maintcoa.button2.Visible = false;
			maintcoa.button3.Visible = false;
			maintcoa.Show();
		}

		private void menuItem29_Click(object sender, System.EventArgs e)
		{
			frmMaintGLAccts maintcoa = new frmMaintGLAccts();
			maintcoa.MdiParent = this;
			maintcoa.button1.Visible = false;
			maintcoa.button2.Visible = false;
			maintcoa.Show();
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
            Interop.PeachwServer.Application app;
            Interop.PeachwServer.Login login = new Interop.PeachwServer.LoginClass();
            app = (Interop.PeachwServer.Application)login.GetApplication(frmMain.sName, frmMain.sPassword);
			app.CloseCompany();
		}

		private void menuItem14_Click(object sender, System.EventArgs e)
		{
			frmOpenCompany openComp = new frmOpenCompany();
			openComp.MdiParent = this;
			openComp.Show();
			openComp.withGUID = true;
			openComp.CompanyList(openComp.withGUID);
		}

		private void menuItem15_Click(object sender, System.EventArgs e)
		{
			frmOpenCompany openComp = new frmOpenCompany();
			openComp.MdiParent = this;
			openComp.Show();
			openComp.withGUID = false;
			openComp.CompanyList(openComp.withGUID);
		}


		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			Connect myApp = new Connect();
			myApp.app.OpenPreviousCompany();
			myApp = null;
			
		}

		private void menuItem53_Click(object sender, System.EventArgs e)
		{
			frmMaintGLAccts viewCOA = new frmMaintGLAccts();
			viewCOA.MdiParent = this;
			viewCOA.comboBox1.DropDownStyle = ComboBoxStyle.Simple;
			viewCOA.Show();
			viewCOA.comboBox1.Items.Clear();
			viewCOA.button1.Visible = true;
			viewCOA.button2.Visible = false;
			viewCOA.button3.Visible = false;
		}

		private void menuItem46_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
			frmInvoices salesinv = new frmInvoices();
			salesinv.MdiParent = this;
			salesinv.Show();
            this.Cursor = Cursors.Default;
		}

		private void menuItem41_Click(object sender, System.EventArgs e)
		{
			frmInvoices salesdel = new frmInvoices();
			salesdel.MdiParent = this;
			salesdel.Show();
		}

		private void menuItem45_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
			frmPurchases purchinv = new frmPurchases();
			purchinv.MdiParent = this;
			purchinv.Show();
            this.Cursor = Cursors.Default;
		}

		private void menuItem44_Click(object sender, System.EventArgs e)
		{
			frmPurchases purchdel = new frmPurchases();
			purchdel.MdiParent = this;
			purchdel.Show();
		}

		private void menuItem50_Click(object sender, System.EventArgs e)
		{
			frmNewCustInvoices newcustinv = new frmNewCustInvoices();
			newcustinv.MdiParent = this;
			newcustinv.Show();
		}

		private void menuItem51_Click(object sender, System.EventArgs e)
		{
			frmNewVendInvoices newvendinv = new frmNewVendInvoices();
			newvendinv.MdiParent = this;
			newvendinv.Show();
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			ptApp.app.ExecuteCommand("File|Exit",null);
		}

        private void menuItem9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem30_Click(object sender, EventArgs e)
        {
            //SalesOrderDetail frm = new SalesOrderDetail();
            //frm.MdiParent = this;
            //frm.Show();
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {

        }




	}
}
