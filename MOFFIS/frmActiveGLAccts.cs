using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MOFFIS
{
	public class frmActiveGLAccts : System.Windows.Forms.Form
	{
        private ConectarPT ptApp = new ConectarPT();
		private GLInformationsss accountType = new GLInformationsss();
		private string[] acctID;
		private int[] acctType;
		private string[] acctDesc;
		private System.Windows.Forms.ListView listView1;
		private System.ComponentModel.Container components = null;

		public frmActiveGLAccts()
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.SuspendLayout();

			this.listView1.Location = new System.Drawing.Point(8, 8);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(432, 168);
			this.listView1.TabIndex = 0;

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 182);
			this.Controls.Add(this.listView1);
			this.Name = "frmActiveGLAccts";
			this.Text = "frmActiveGLAccts";
			this.Load += new System.EventHandler(this.frmActiveGLAccts_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmActiveGLAccts_Load(object sender, System.EventArgs e)
		{
			ptApp.app.GetActiveAccounts(out acctID, out acctType, out acctDesc);
			listView1.View = View.Details;
			listView1.Columns.Add("Account ID",-2,HorizontalAlignment.Left);
			listView1.Columns.Add("Account Type", -2, HorizontalAlignment.Left);
			listView1.Columns.Add("Account Description", -2, HorizontalAlignment.Left);
			for(int i = 0;i <= acctID[0].Length -1;i++)
			{
				listView1.Items.Add(acctID[i]);
				listView1.Items[i].SubItems.Add(accountType.getAcctTypeWords(acctType[i]));
				listView1.Items[i].SubItems.Add(acctDesc[i]);
			}
			accountType = null;
			foreach(ColumnHeader col in listView1.Columns)
			{
				col.Width = -2;
			}
			ptApp = null;
		}
	}
	public class frmActiveGLAcctsWithGuid : System.Windows.Forms.Form
	{
        private ConectarPT ptApp = new ConectarPT();
		private GLInformationsss accountType = new GLInformationsss();
		private string[] acctID;
		private int[] acctType;
		private string[] acctDesc;
		private string[] acctGuid;
		private System.Windows.Forms.ListView listView1;
		private System.ComponentModel.Container components = null;

		public frmActiveGLAcctsWithGuid()
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.SuspendLayout();

			this.listView1.Location = new System.Drawing.Point(8, 8);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(682, 168);
			this.listView1.TabIndex = 0;

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(698, 182);
			this.Controls.Add(this.listView1);
			this.Name = "frmActiveGLAccts";
			this.Text = "frmActiveGLAccts";
			this.Load += new System.EventHandler(this.frmActiveGLAcctsWithGuid_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmActiveGLAcctsWithGuid_Load(object sender, System.EventArgs e)
		{
			ptApp.app.GetActiveAccountsWithGuid(out acctID, out acctType, out acctDesc, out acctGuid);
			listView1.View = View.Details;
			
			listView1.Columns.Add("Account ID",-2,HorizontalAlignment.Left);
			listView1.Columns.Add("Account Type", -2, HorizontalAlignment.Left);
			listView1.Columns.Add("Account Description", -2, HorizontalAlignment.Left);
			listView1.Columns.Add("GUID", -2, HorizontalAlignment.Left);
			
			for(int i = 0;i <= acctID[0].Length -1;i++)
			{
				listView1.Items.Add(acctID[i]);
				listView1.Items[i].SubItems.Add(accountType.getAcctTypeWords(acctType[i]));
				listView1.Items[i].SubItems.Add(acctDesc[i]);
				listView1.Items[i].SubItems.Add(acctGuid[i]);
			}
			
			accountType = null;
			
			foreach(ColumnHeader col in listView1.Columns)
			{
				col.Width = -2;
			}
			
			ptApp = null;
		}
	}
}
