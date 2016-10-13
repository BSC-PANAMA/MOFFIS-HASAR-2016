using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CSSDK
{

	public class frmAccountingPeriods : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView listView1;

		private System.ComponentModel.Container components = null;

		public frmAccountingPeriods()
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

			this.listView1.Location = new System.Drawing.Point(8, 8);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(624, 208);
			this.listView1.TabIndex = 0;

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 222);
			this.Controls.Add(this.listView1);
			this.Name = "frmAccountingPeriods";
			this.Text = "frmAccountingPeriods";
			this.Load += new System.EventHandler(this.frmAccountingPeriods_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmAccountingPeriods_Load(object sender, System.EventArgs e)
		{
			AccountingPeriods acctpers = new AccountingPeriods();
			listView1.Items.Clear();
			listView1.Columns.Clear();
			listView1.View = View.Details;
			listView1.GridLines = true;

			listView1.Columns.Add("Period",-2,HorizontalAlignment.Center);
			listView1.Columns.Add("Begin Date",-2,HorizontalAlignment.Right);
			listView1.Columns.Add("to",-2,HorizontalAlignment.Center);
			listView1.Columns.Add("End Date",-2,HorizontalAlignment.Left);
			listView1.Columns.Add("Period",-2,HorizontalAlignment.Center);
			listView1.Columns.Add("Begin Date",-2,HorizontalAlignment.Right);
			listView1.Columns.Add("to",-2,HorizontalAlignment.Center);
			listView1.Columns.Add("End Date",-2,HorizontalAlignment.Left);
			listView1.Columns.Add("Period",-2,HorizontalAlignment.Center);
			listView1.Columns.Add("Begin Date",-2,HorizontalAlignment.Right);
			listView1.Columns.Add("to",-2,HorizontalAlignment.Center);
			listView1.Columns.Add("End Date",-2,HorizontalAlignment.Left);

			int pernum;

			DateTime perdate = new DateTime();

			for(int i = 1;i < acctpers.PeriodsPerYear; i++ )
			{
				listView1.Items.Add("0");
				perdate = DateTime.Parse(acctpers.StartDate[i].ToString());
				listView1.Items[i-1].SubItems.Add(perdate.ToString("MM/dd/yyyy"));
				listView1.Items[i-1].SubItems.Add("to");
				perdate = DateTime.Parse(acctpers.EndDate[i].ToString());
				listView1.Items[i-1].SubItems.Add(perdate.ToString("MM/dd/yyyy"));
				pernum = i;
				listView1.Items[i-1].SubItems.Add(pernum.ToString());
                perdate = DateTime.Parse(acctpers.StartDate[i + 14].ToString());
				listView1.Items[i-1].SubItems.Add(perdate.ToString("MM/dd/yyyy"));
				listView1.Items[i-1].SubItems.Add("to");
                perdate = DateTime.Parse(acctpers.EndDate[i + 14].ToString());
				listView1.Items[i-1].SubItems.Add(perdate.ToString("MM/dd/yyyy"));
				pernum = i + acctpers.PeriodsPerYear;
				listView1.Items[i-1].SubItems.Add(pernum.ToString());
 
			}

			foreach(ColumnHeader col in listView1.Columns)
			{
				col.Width = -2;
			}
			
			acctpers = null;
		}
	}
}
