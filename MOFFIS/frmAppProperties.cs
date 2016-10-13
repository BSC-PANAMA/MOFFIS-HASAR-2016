using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MOFFIS;

namespace MOFFIS
{

	public class frmAppProperties : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView proplist;
		private System.ComponentModel.Container components = null;

		public frmAppProperties()
		{
			InitializeComponent();
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.proplist = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // proplist
            // 
            this.proplist.FullRowSelect = true;
            this.proplist.Location = new System.Drawing.Point(8, 8);
            this.proplist.MultiSelect = false;
            this.proplist.Name = "proplist";
            this.proplist.Size = new System.Drawing.Size(664, 208);
            this.proplist.TabIndex = 0;
            this.proplist.UseCompatibleStateImageBehavior = false;
            this.proplist.View = System.Windows.Forms.View.Details;
            // 
            // frmAppProperties
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(680, 222);
            this.Controls.Add(this.proplist);
            this.Name = "frmAppProperties";
            this.Text = "Propiedades de la Aplicacion";
            this.Load += new System.EventHandler(this.form_load);
            this.ResumeLayout(false);

		}
		#endregion
		private void form_load(object sender, EventArgs e)
		{
            ConectarPT ptApp = new ConectarPT();
			this.SuspendLayout();

			proplist.Columns.Add("Propiedad", -2, HorizontalAlignment.Left);
			proplist.Columns.Add("Valor de la Propiedad", -2, HorizontalAlignment.Left);

            //proplist.Items.Add("Application Path");
            proplist.Items.Add("Ruta de la Aplicacion");
			proplist.Items[0].SubItems.Add(ptApp.app.ApplicationPath);
			
			proplist.Items.Add("Are There Unnumbered Invoices For Web Billing?");
			proplist.Items[1].SubItems.Add(ptApp.app.AreThereUnnumberedInvoicesForWebBilling().ToString());

			proplist.Items.Add("Company Is Open?");
			proplist.Items[2].SubItems.Add(ptApp.app.CompanyIsOpen.ToString());

			proplist.Items.Add("Company Path");
			proplist.Items[3].SubItems.Add(ptApp.app.CompanyPath);

			proplist.Items.Add("Company Type Code");
			proplist.Items[4].SubItems.Add(ptApp.app.CompanyTypeCode.ToString());

			proplist.Items.Add("Company Uses Passwords?");
			proplist.Items[5].SubItems.Add(ptApp.app.CompanyUsesPasswords.ToString());

			proplist.Items.Add("Company Uses Peachtree Payroll Services (PPS)");
			proplist.Items[6].SubItems.Add(ptApp.app.CompanyUsesPPS.ToString());

			proplist.Items.Add("Current Company GUID");
			proplist.Items[7].SubItems.Add(ptApp.app.CurrentCompanyGUID.ToString());

			proplist.Items.Add("Current Company Name");
			proplist.Items[8].SubItems.Add(ptApp.app.CurrentCompanyName.ToString());

			proplist.Items.Add("Current Multiple Flavor Name");
			proplist.Items[9].SubItems.Add(ptApp.app.CurrentMultipleFlavorName.ToString());

			proplist.Items.Add("Current Tax Year");
			proplist.Items[10].SubItems.Add(ptApp.app.CurrentTaxYear.ToString());

			proplist.Items.Add("Current User GUID");
			proplist.Items[11].SubItems.Add(ptApp.app.CurrentUserGUID.ToString());

			proplist.Items.Add("Current User Has Full Access?");
            proplist.Items[12].SubItems.Add(ptApp.app.CurrentUserHasFullAccess(Interop.PeachwServer.PeachwPermissionSummaryType.peachwPermSummarySystem).ToString());

			proplist.Items.Add("Current User ID");
			proplist.Items[13].SubItems.Add(ptApp.app.CurrentUserID.ToString());

			proplist.Items.Add("Customer Number");
			proplist.Items[14].SubItems.Add(ptApp.app.CustomerNumber.ToString());
            
			proplist.Items.Add("Data Path");
			proplist.Items[15].SubItems.Add(ptApp.app.DataPath.ToString());

			proplist.Items.Add("Help Path");
			proplist.Items[16].SubItems.Add(ptApp.app.HelpPath.ToString());

			proplist.Items.Add("HWND");
			proplist.Items[17].SubItems.Add(ptApp.app.HWND.ToString());

			proplist.Items.Add("INI File Name");
			proplist.Items[18].SubItems.Add(ptApp.app.IniFileName.ToString());

			proplist.Items.Add("Product ID");
			proplist.Items[19].SubItems.Add(ptApp.app.ProductID.ToString());

			proplist.Items.Add("Product Name");
			proplist.Items[20].SubItems.Add(ptApp.app.ProductName.ToString());

			proplist.Items.Add("Product Number");
			proplist.Items[21].SubItems.Add(ptApp.app.ProductNumber.ToString());

			proplist.Items.Add("Product Sub Code");
			proplist.Items[22].SubItems.Add(ptApp.app.ProductSubCode.ToString());

			proplist.Items.Add("Product Type Code");
			proplist.Items[23].SubItems.Add(ptApp.app.ProductTypeCode.ToString());

			proplist.Items.Add("Product Type Name");
			proplist.Items[24].SubItems.Add(ptApp.app.ProductTypeName.ToString());

			proplist.Items.Add("Product Version");
			proplist.Items[25].SubItems.Add(ptApp.app.ProductVersion.ToString());

			proplist.Items.Add("Registry Sub Key");
			proplist.Items[26].SubItems.Add(ptApp.app.RegistrySubKey.ToString());

			proplist.Items.Add("Serial Number");
			proplist.Items[27].SubItems.Add(ptApp.app.SerialNumber.ToString());

			proplist.Items.Add("Session GUID");
			proplist.Items[28].SubItems.Add(ptApp.app.SessionGUID.ToString());

			proplist.Items.Add("System Date");
			proplist.Items[29].SubItems.Add(ptApp.app.SystemDate.ToString());

			proplist.Items.Add("Tax Registration Number");
			proplist.Items[30].SubItems.Add(ptApp.app.SystemDate.ToString());

			proplist.Items.Add("Tax Table Version");
			proplist.Items[31].SubItems.Add(ptApp.app.TaxTableVersion.ToString());

			proplist.Show();

			foreach(ColumnHeader col in proplist.Columns)
			{
				col.Width = -2;
			}
		}
	}
}
