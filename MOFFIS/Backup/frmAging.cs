using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using CSSDK;

namespace CSSDK
{
	public class frmAging : System.Windows.Forms.Form
	{
		private Connect ptApp = new Connect();
		int[] Days;
		string[] Labels;
		decimal[] Totals;
		DateTime asOfDate = new DateTime();
		private string ButtonName;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.ListView listView1;

		private System.ComponentModel.Container components = null;

		public frmAging()
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.listView1 = new System.Windows.Forms.ListView();
			this.SuspendLayout();

			this.button1.Location = new System.Drawing.Point(328, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Get AR Aging";
			this.button1.Click += new System.EventHandler(this.button1_Click);

			this.button2.Location = new System.Drawing.Point(328, 40);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Get AP Aging";
			this.button2.Click += new System.EventHandler(this.button2_Click);

			this.radioButton1.Location = new System.Drawing.Point(8, 8);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(160, 16);
			this.radioButton1.TabIndex = 2;
			this.radioButton1.Text = "As of System Date";

			this.radioButton2.Location = new System.Drawing.Point(8, 30);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(160, 16);
			this.radioButton2.TabIndex = 3;
			this.radioButton2.Text = "As of End of Current Period";

			this.radioButton3.Location = new System.Drawing.Point(8, 52);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(160, 16);
			this.radioButton3.TabIndex = 4;
			this.radioButton3.Text = "As of Specific Date";

			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimePicker1.Location = new System.Drawing.Point(128, 48);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(88, 20);
			this.dateTimePicker1.TabIndex = 5;

			this.listView1.Location = new System.Drawing.Point(8, 80);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(408, 48);
			this.listView1.TabIndex = 6;

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 134);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.radioButton3);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "frmAging";
			this.Text = "Aging Information";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			switch(ButtonNumber())
			{
				case 0:
					MessageBox.Show("You must select a date before retrieving aging information","Error",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.RightAlign);
					break;
				case 1:
					asOfDate = ptApp.app.SystemDate;
					break;
				case 2:
					AccountingPeriods AcctPer = new AccountingPeriods();
					asOfDate = DateTime.Parse(AcctPer.GetLastDayOfCurrPer());
					AcctPer = null;
					break;
				case 3:
					asOfDate = this.dateTimePicker1.Value;
					break;
			}

			listView1.Items.Clear();
			listView1.Columns.Clear();
			listView1.View = View.Details;

			ptApp.app.GetARAgingByDate(asOfDate,out Days, out Labels, out Totals);
			listView1.Columns.Add("Module",-2,HorizontalAlignment.Left);

			for ( int i = 0; i < Labels.Length; i++ )
			{ 
				listView1.Columns.Add(Labels[i].ToString(), -2, HorizontalAlignment.Center);
			}
			
			listView1.Items.Add("Accounts Receivable");
			listView1.Items[0].SubItems.Add(double.Parse(Totals[0].ToString()).ToString("$#,##0.00"));
			listView1.Items[0].SubItems.Add(double.Parse(Totals[1].ToString()).ToString("$#,##0.00"));
			listView1.Items[0].SubItems.Add(double.Parse(Totals[2].ToString()).ToString("$#,##0.00"));
			listView1.Items[0].SubItems.Add(double.Parse(Totals[3].ToString()).ToString("$#,##0.00"));

			foreach(ColumnHeader col in listView1.Columns)
			{
				col.Width = -2;
			}
		}


		private int ButtonNumber ()
		{
			foreach(Control Button in this.Controls)
			{
				if(Button.GetType().ToString() == "System.Windows.Forms.RadioButton")
				{
					RadioButton newButton = (RadioButton)Button;
					if(newButton.Checked == true)
					{
						ButtonName = Button.Name;
					}
				}
			}
			switch(ButtonName)
			{
				case "radioButton1":
					return 1;
				case "radioButton2":
					return 2;
				case "radioButton3":
					return 3;
				default:
					return 0;
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			switch(ButtonNumber())
			{
				case 0:
					MessageBox.Show("You must select a date before retrieving aging information","Error",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.RightAlign);
					break;
				case 1:
					asOfDate = ptApp.app.SystemDate;
					break;
				case 2:
					AccountingPeriods AcctPer = new AccountingPeriods();
					asOfDate = DateTime.Parse(AcctPer.GetLastDayOfCurrPer());
					AcctPer = null;
					break;
				case 3:
					asOfDate = this.dateTimePicker1.Value;
					break;
			}

			listView1.Items.Clear();
			listView1.Columns.Clear();
			listView1.View = View.Details;
			ptApp.app.GetAPAgingByDate(asOfDate,out Days, out Labels, out Totals);
			listView1.Columns.Add("Module",-2,HorizontalAlignment.Left);

			for (int i = 0; i < Labels.Length; i++ )
			{ 
				listView1.Columns.Add(Labels[i].ToString(), -2, HorizontalAlignment.Center);
			}

			listView1.Items.Add("Accounts Payable");
			listView1.Items[0].SubItems.Add(double.Parse(Totals[0].ToString()).ToString("$#,##0.00"));
			listView1.Items[0].SubItems.Add(double.Parse(Totals[1].ToString()).ToString("$#,##0.00"));
			listView1.Items[0].SubItems.Add(double.Parse(Totals[2].ToString()).ToString("$#,##0.00"));
			listView1.Items[0].SubItems.Add(double.Parse(Totals[3].ToString()).ToString("$#,##0.00"));

			foreach(ColumnHeader col in listView1.Columns)
			{
				col.Width = -2;
			}
		}
	}
}
