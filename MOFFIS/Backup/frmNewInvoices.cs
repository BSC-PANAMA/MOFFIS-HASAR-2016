using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Interop.PeachwServer;

namespace CSSDK
{
	/// <summary>
	/// Summary description for frmNewInvoices.
	/// </summary>
	public class frmNewCustInvoices : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox invoiceDate;
		private System.Windows.Forms.TextBox ZIP;
		private System.Windows.Forms.TextBox State;
		private System.Windows.Forms.TextBox City;
		private System.Windows.Forms.TextBox Add2;
		private System.Windows.Forms.TextBox Add1;
		private System.Windows.Forms.TextBox CustVendName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.Label lblCustVendName;
		public System.Windows.Forms.Label lblCustVendID;
		private System.Windows.Forms.TextBox qty1;
		private System.Windows.Forms.ComboBox itemID1;
		private System.Windows.Forms.TextBox Desc1;
		private System.Windows.Forms.TextBox unitprice1;
		private System.Windows.Forms.TextBox amount1;
		private System.Windows.Forms.TextBox amount2;
		private System.Windows.Forms.TextBox unitprice2;
		private System.Windows.Forms.TextBox Desc2;
		private System.Windows.Forms.ComboBox itemID2;
		private System.Windows.Forms.TextBox qty2;
		private System.Windows.Forms.TextBox amount3;
		private System.Windows.Forms.TextBox unitprice3;
		private System.Windows.Forms.TextBox Desc3;
		private System.Windows.Forms.ComboBox itemID3;
		private System.Windows.Forms.TextBox qty3;
		private System.Windows.Forms.TextBox amount4;
		private System.Windows.Forms.TextBox unitprice4;
		private System.Windows.Forms.TextBox Desc4;
		private System.Windows.Forms.ComboBox itemID4;
		private System.Windows.Forms.TextBox qty4;
		private System.Windows.Forms.TextBox amount5;
		private System.Windows.Forms.TextBox unitprice5;
		private System.Windows.Forms.TextBox Desc5;
		private System.Windows.Forms.ComboBox itemID5;
		private System.Windows.Forms.TextBox qty5;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox ARAccount;
		private System.Windows.Forms.Label label12;
		public System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox custVendID;
        private Interop.PeachwServer.Export exporter;
        private Interop.PeachwServer.Import importer;
		private Connect ptApp = new Connect();
		private System.ComponentModel.Container components = null;
		private XmlImplementation imp;
		private XmlDocument doc;
		private XmlNodeList reader;
		private Array custIDList;
		private Array itemIDList;
		private System.Windows.Forms.ComboBox glacct5;
		private System.Windows.Forms.ComboBox glacct4;
		private System.Windows.Forms.ComboBox glacct3;
		private System.Windows.Forms.ComboBox glacct2;
		private System.Windows.Forms.ComboBox glacct1;
		private System.Windows.Forms.TextBox arAcctDesc;
		private System.Windows.Forms.TextBox invtotal;
		private System.Windows.Forms.TextBox invNum;
		private Array glAcctIDList;

		public frmNewCustInvoices()
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label7 = new System.Windows.Forms.Label();
            this.invoiceDate = new System.Windows.Forms.TextBox();
            this.ZIP = new System.Windows.Forms.TextBox();
            this.State = new System.Windows.Forms.TextBox();
            this.City = new System.Windows.Forms.TextBox();
            this.Add2 = new System.Windows.Forms.TextBox();
            this.Add1 = new System.Windows.Forms.TextBox();
            this.CustVendName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCustVendName = new System.Windows.Forms.Label();
            this.lblCustVendID = new System.Windows.Forms.Label();
            this.qty1 = new System.Windows.Forms.TextBox();
            this.itemID1 = new System.Windows.Forms.ComboBox();
            this.Desc1 = new System.Windows.Forms.TextBox();
            this.unitprice1 = new System.Windows.Forms.TextBox();
            this.amount1 = new System.Windows.Forms.TextBox();
            this.amount2 = new System.Windows.Forms.TextBox();
            this.unitprice2 = new System.Windows.Forms.TextBox();
            this.Desc2 = new System.Windows.Forms.TextBox();
            this.itemID2 = new System.Windows.Forms.ComboBox();
            this.qty2 = new System.Windows.Forms.TextBox();
            this.amount3 = new System.Windows.Forms.TextBox();
            this.unitprice3 = new System.Windows.Forms.TextBox();
            this.Desc3 = new System.Windows.Forms.TextBox();
            this.itemID3 = new System.Windows.Forms.ComboBox();
            this.qty3 = new System.Windows.Forms.TextBox();
            this.amount4 = new System.Windows.Forms.TextBox();
            this.unitprice4 = new System.Windows.Forms.TextBox();
            this.Desc4 = new System.Windows.Forms.TextBox();
            this.itemID4 = new System.Windows.Forms.ComboBox();
            this.qty4 = new System.Windows.Forms.TextBox();
            this.amount5 = new System.Windows.Forms.TextBox();
            this.unitprice5 = new System.Windows.Forms.TextBox();
            this.Desc5 = new System.Windows.Forms.TextBox();
            this.itemID5 = new System.Windows.Forms.ComboBox();
            this.qty5 = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.invtotal = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.glacct5 = new System.Windows.Forms.ComboBox();
            this.glacct4 = new System.Windows.Forms.ComboBox();
            this.glacct3 = new System.Windows.Forms.ComboBox();
            this.glacct2 = new System.Windows.Forms.ComboBox();
            this.glacct1 = new System.Windows.Forms.ComboBox();
            this.ARAccount = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.custVendID = new System.Windows.Forms.ComboBox();
            this.invNum = new System.Windows.Forms.TextBox();
            this.arAcctDesc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(295, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 20);
            this.label7.TabIndex = 33;
            this.label7.Text = "Ciudad, Estado, ZIP";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // invoiceDate
            // 
            this.invoiceDate.Location = new System.Drawing.Point(116, 32);
            this.invoiceDate.Name = "invoiceDate";
            this.invoiceDate.Size = new System.Drawing.Size(208, 20);
            this.invoiceDate.TabIndex = 2;
            // 
            // ZIP
            // 
            this.ZIP.Enabled = false;
            this.ZIP.Location = new System.Drawing.Point(672, 76);
            this.ZIP.Name = "ZIP";
            this.ZIP.Size = new System.Drawing.Size(40, 20);
            this.ZIP.TabIndex = 31;
            // 
            // State
            // 
            this.State.Enabled = false;
            this.State.Location = new System.Drawing.Point(648, 76);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(24, 20);
            this.State.TabIndex = 29;
            // 
            // City
            // 
            this.City.Enabled = false;
            this.City.Location = new System.Drawing.Point(424, 76);
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(223, 20);
            this.City.TabIndex = 28;
            // 
            // Add2
            // 
            this.Add2.Enabled = false;
            this.Add2.Location = new System.Drawing.Point(424, 52);
            this.Add2.Name = "Add2";
            this.Add2.Size = new System.Drawing.Size(292, 20);
            this.Add2.TabIndex = 27;
            // 
            // Add1
            // 
            this.Add1.Enabled = false;
            this.Add1.Location = new System.Drawing.Point(424, 28);
            this.Add1.Name = "Add1";
            this.Add1.Size = new System.Drawing.Size(292, 20);
            this.Add1.TabIndex = 26;
            // 
            // CustVendName
            // 
            this.CustVendName.Location = new System.Drawing.Point(424, 4);
            this.CustVendName.Name = "CustVendName";
            this.CustVendName.Size = new System.Drawing.Size(292, 20);
            this.CustVendName.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 21);
            this.label6.TabIndex = 23;
            this.label6.Text = "Fecha de Invoice";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 21);
            this.label5.TabIndex = 22;
            this.label5.Text = "Numero de Invoice";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(336, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "Direccion";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(336, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Direccion";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCustVendName
            // 
            this.lblCustVendName.Location = new System.Drawing.Point(336, 12);
            this.lblCustVendName.Name = "lblCustVendName";
            this.lblCustVendName.Size = new System.Drawing.Size(80, 20);
            this.lblCustVendName.TabIndex = 19;
            this.lblCustVendName.Text = "Nombre";
            this.lblCustVendName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCustVendID
            // 
            this.lblCustVendID.Location = new System.Drawing.Point(8, 56);
            this.lblCustVendID.Name = "lblCustVendID";
            this.lblCustVendID.Size = new System.Drawing.Size(104, 21);
            this.lblCustVendID.TabIndex = 18;
            this.lblCustVendID.Text = "ID Cliente";
            this.lblCustVendID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // qty1
            // 
            this.qty1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.qty1.Location = new System.Drawing.Point(8, 164);
            this.qty1.Name = "qty1";
            this.qty1.Size = new System.Drawing.Size(72, 20);
            this.qty1.TabIndex = 5;
            this.qty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty1.Leave += new System.EventHandler(this.qty1_leave);
            // 
            // itemID1
            // 
            this.itemID1.Location = new System.Drawing.Point(80, 164);
            this.itemID1.Name = "itemID1";
            this.itemID1.Size = new System.Drawing.Size(88, 21);
            this.itemID1.TabIndex = 6;
            this.itemID1.SelectedIndexChanged += new System.EventHandler(this.itemID1_SelectedIndexChanged);
            this.itemID1.Leave += new System.EventHandler(this.itemID1_leave);
            // 
            // Desc1
            // 
            this.Desc1.Location = new System.Drawing.Point(168, 164);
            this.Desc1.Name = "Desc1";
            this.Desc1.Size = new System.Drawing.Size(264, 20);
            this.Desc1.TabIndex = 7;
            // 
            // unitprice1
            // 
            this.unitprice1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.unitprice1.Location = new System.Drawing.Point(520, 164);
            this.unitprice1.Name = "unitprice1";
            this.unitprice1.Size = new System.Drawing.Size(80, 20);
            this.unitprice1.TabIndex = 9;
            this.unitprice1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.unitprice1.Leave += new System.EventHandler(this.unitprice1_leave);
            // 
            // amount1
            // 
            this.amount1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.amount1.Location = new System.Drawing.Point(600, 164);
            this.amount1.Name = "amount1";
            this.amount1.Size = new System.Drawing.Size(120, 20);
            this.amount1.TabIndex = 10;
            this.amount1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amount1.Leave += new System.EventHandler(this.amount1_leave);
            // 
            // amount2
            // 
            this.amount2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.amount2.Location = new System.Drawing.Point(600, 188);
            this.amount2.Name = "amount2";
            this.amount2.Size = new System.Drawing.Size(120, 20);
            this.amount2.TabIndex = 16;
            this.amount2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amount2.Leave += new System.EventHandler(this.amount2_leave);
            // 
            // unitprice2
            // 
            this.unitprice2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.unitprice2.Location = new System.Drawing.Point(520, 188);
            this.unitprice2.Name = "unitprice2";
            this.unitprice2.Size = new System.Drawing.Size(80, 20);
            this.unitprice2.TabIndex = 15;
            this.unitprice2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Desc2
            // 
            this.Desc2.Location = new System.Drawing.Point(168, 188);
            this.Desc2.Name = "Desc2";
            this.Desc2.Size = new System.Drawing.Size(264, 20);
            this.Desc2.TabIndex = 13;
            // 
            // itemID2
            // 
            this.itemID2.Location = new System.Drawing.Point(80, 188);
            this.itemID2.Name = "itemID2";
            this.itemID2.Size = new System.Drawing.Size(88, 21);
            this.itemID2.TabIndex = 12;
            this.itemID2.SelectedIndexChanged += new System.EventHandler(this.itemID2_SelectedIndexChanged);
            this.itemID2.Leave += new System.EventHandler(this.itemID2_leave);
            // 
            // qty2
            // 
            this.qty2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.qty2.Location = new System.Drawing.Point(8, 188);
            this.qty2.Name = "qty2";
            this.qty2.Size = new System.Drawing.Size(72, 20);
            this.qty2.TabIndex = 11;
            this.qty2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty2.Leave += new System.EventHandler(this.qty2_leave);
            // 
            // amount3
            // 
            this.amount3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.amount3.Location = new System.Drawing.Point(600, 212);
            this.amount3.Name = "amount3";
            this.amount3.Size = new System.Drawing.Size(120, 20);
            this.amount3.TabIndex = 22;
            this.amount3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amount3.Leave += new System.EventHandler(this.amount3_leave);
            // 
            // unitprice3
            // 
            this.unitprice3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.unitprice3.Location = new System.Drawing.Point(520, 212);
            this.unitprice3.Name = "unitprice3";
            this.unitprice3.Size = new System.Drawing.Size(80, 20);
            this.unitprice3.TabIndex = 21;
            this.unitprice3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.unitprice3.Leave += new System.EventHandler(this.unitprice3_leave);
            // 
            // Desc3
            // 
            this.Desc3.Location = new System.Drawing.Point(168, 212);
            this.Desc3.Name = "Desc3";
            this.Desc3.Size = new System.Drawing.Size(264, 20);
            this.Desc3.TabIndex = 19;
            // 
            // itemID3
            // 
            this.itemID3.Location = new System.Drawing.Point(80, 212);
            this.itemID3.Name = "itemID3";
            this.itemID3.Size = new System.Drawing.Size(88, 21);
            this.itemID3.TabIndex = 18;
            this.itemID3.SelectedIndexChanged += new System.EventHandler(this.itemID3_SelectedIndexChanged);
            this.itemID3.Leave += new System.EventHandler(this.itemID3_leave);
            // 
            // qty3
            // 
            this.qty3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.qty3.Location = new System.Drawing.Point(8, 212);
            this.qty3.Name = "qty3";
            this.qty3.Size = new System.Drawing.Size(72, 20);
            this.qty3.TabIndex = 17;
            this.qty3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty3.Leave += new System.EventHandler(this.qty3_leave);
            // 
            // amount4
            // 
            this.amount4.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.amount4.Location = new System.Drawing.Point(600, 236);
            this.amount4.Name = "amount4";
            this.amount4.Size = new System.Drawing.Size(120, 20);
            this.amount4.TabIndex = 28;
            this.amount4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amount4.Leave += new System.EventHandler(this.amount4_leave);
            // 
            // unitprice4
            // 
            this.unitprice4.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.unitprice4.Location = new System.Drawing.Point(520, 236);
            this.unitprice4.Name = "unitprice4";
            this.unitprice4.Size = new System.Drawing.Size(80, 20);
            this.unitprice4.TabIndex = 27;
            this.unitprice4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.unitprice4.Leave += new System.EventHandler(this.unitprice4_leave);
            // 
            // Desc4
            // 
            this.Desc4.Location = new System.Drawing.Point(168, 236);
            this.Desc4.Name = "Desc4";
            this.Desc4.Size = new System.Drawing.Size(264, 20);
            this.Desc4.TabIndex = 25;
            // 
            // itemID4
            // 
            this.itemID4.Location = new System.Drawing.Point(80, 236);
            this.itemID4.Name = "itemID4";
            this.itemID4.Size = new System.Drawing.Size(88, 21);
            this.itemID4.TabIndex = 24;
            this.itemID4.SelectedIndexChanged += new System.EventHandler(this.itemID4_SelectedIndexChanged);
            this.itemID4.Leave += new System.EventHandler(this.itemID4_leave);
            // 
            // qty4
            // 
            this.qty4.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.qty4.Location = new System.Drawing.Point(8, 236);
            this.qty4.Name = "qty4";
            this.qty4.Size = new System.Drawing.Size(72, 20);
            this.qty4.TabIndex = 23;
            this.qty4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty4.Leave += new System.EventHandler(this.qty4_leave);
            // 
            // amount5
            // 
            this.amount5.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.amount5.Location = new System.Drawing.Point(600, 260);
            this.amount5.Name = "amount5";
            this.amount5.Size = new System.Drawing.Size(120, 20);
            this.amount5.TabIndex = 34;
            this.amount5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amount5.Leave += new System.EventHandler(this.amount5_leave);
            // 
            // unitprice5
            // 
            this.unitprice5.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.unitprice5.Location = new System.Drawing.Point(520, 260);
            this.unitprice5.Name = "unitprice5";
            this.unitprice5.Size = new System.Drawing.Size(80, 20);
            this.unitprice5.TabIndex = 33;
            this.unitprice5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.unitprice5.Leave += new System.EventHandler(this.unitprice5_leave);
            // 
            // Desc5
            // 
            this.Desc5.Location = new System.Drawing.Point(168, 260);
            this.Desc5.Name = "Desc5";
            this.Desc5.Size = new System.Drawing.Size(264, 20);
            this.Desc5.TabIndex = 31;
            // 
            // itemID5
            // 
            this.itemID5.Location = new System.Drawing.Point(80, 260);
            this.itemID5.Name = "itemID5";
            this.itemID5.Size = new System.Drawing.Size(88, 21);
            this.itemID5.TabIndex = 30;
            this.itemID5.SelectedIndexChanged += new System.EventHandler(this.itemID5_SelectedIndexChanged);
            this.itemID5.Leave += new System.EventHandler(this.itemID5_leave);
            // 
            // qty5
            // 
            this.qty5.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.qty5.Location = new System.Drawing.Point(8, 260);
            this.qty5.Name = "qty5";
            this.qty5.Size = new System.Drawing.Size(72, 20);
            this.qty5.TabIndex = 29;
            this.qty5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty5.Leave += new System.EventHandler(this.qty5_leave);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(336, 292);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 35;
            this.SaveButton.Text = "Guardar";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 61;
            this.label2.Text = "Item";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(168, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(272, 20);
            this.label8.TabIndex = 62;
            this.label8.Text = "Descripcion";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(520, 140);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 20);
            this.label9.TabIndex = 63;
            this.label9.Text = "Precio Unitario";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(600, 140);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 20);
            this.label10.TabIndex = 64;
            this.label10.Text = "Cantidad";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 65;
            this.label1.Text = "Cantidad";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // invtotal
            // 
            this.invtotal.Enabled = false;
            this.invtotal.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.invtotal.Location = new System.Drawing.Point(600, 284);
            this.invtotal.Name = "invtotal";
            this.invtotal.Size = new System.Drawing.Size(120, 20);
            this.invtotal.TabIndex = 66;
            this.invtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(432, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 20);
            this.label11.TabIndex = 72;
            this.label11.Text = "Cuenta GL";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // glacct5
            // 
            this.glacct5.Location = new System.Drawing.Point(432, 260);
            this.glacct5.Name = "glacct5";
            this.glacct5.Size = new System.Drawing.Size(88, 21);
            this.glacct5.TabIndex = 32;
            // 
            // glacct4
            // 
            this.glacct4.Location = new System.Drawing.Point(432, 236);
            this.glacct4.Name = "glacct4";
            this.glacct4.Size = new System.Drawing.Size(88, 21);
            this.glacct4.TabIndex = 26;
            // 
            // glacct3
            // 
            this.glacct3.Location = new System.Drawing.Point(432, 212);
            this.glacct3.Name = "glacct3";
            this.glacct3.Size = new System.Drawing.Size(88, 21);
            this.glacct3.TabIndex = 20;
            // 
            // glacct2
            // 
            this.glacct2.Location = new System.Drawing.Point(432, 188);
            this.glacct2.Name = "glacct2";
            this.glacct2.Size = new System.Drawing.Size(88, 21);
            this.glacct2.TabIndex = 14;
            // 
            // glacct1
            // 
            this.glacct1.Location = new System.Drawing.Point(432, 164);
            this.glacct1.Name = "glacct1";
            this.glacct1.Size = new System.Drawing.Size(88, 21);
            this.glacct1.TabIndex = 8;
            // 
            // ARAccount
            // 
            this.ARAccount.Location = new System.Drawing.Point(424, 104);
            this.ARAccount.Name = "ARAccount";
            this.ARAccount.Size = new System.Drawing.Size(88, 21);
            this.ARAccount.TabIndex = 4;
            this.ARAccount.SelectedIndexChanged += new System.EventHandler(this.ARAccount_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(256, 104);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(168, 21);
            this.label12.TabIndex = 74;
            this.label12.Text = "Accounts Recievable Account";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(499, 284);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 21);
            this.label13.TabIndex = 75;
            this.label13.Text = "Total de Invoice";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // custVendID
            // 
            this.custVendID.Location = new System.Drawing.Point(116, 56);
            this.custVendID.Name = "custVendID";
            this.custVendID.Size = new System.Drawing.Size(208, 21);
            this.custVendID.TabIndex = 3;
            this.custVendID.SelectedIndexChanged += new System.EventHandler(this.custVendID_SelectedIndexChanged);
            // 
            // invNum
            // 
            this.invNum.Location = new System.Drawing.Point(116, 8);
            this.invNum.Name = "invNum";
            this.invNum.Size = new System.Drawing.Size(208, 20);
            this.invNum.TabIndex = 1;
            // 
            // arAcctDesc
            // 
            this.arAcctDesc.Enabled = false;
            this.arAcctDesc.Location = new System.Drawing.Point(512, 104);
            this.arAcctDesc.Name = "arAcctDesc";
            this.arAcctDesc.Size = new System.Drawing.Size(200, 20);
            this.arAcctDesc.TabIndex = 78;
            // 
            // frmNewCustInvoices
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(728, 318);
            this.Controls.Add(this.arAcctDesc);
            this.Controls.Add(this.invNum);
            this.Controls.Add(this.custVendID);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ARAccount);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.glacct5);
            this.Controls.Add(this.glacct4);
            this.Controls.Add(this.glacct3);
            this.Controls.Add(this.glacct2);
            this.Controls.Add(this.glacct1);
            this.Controls.Add(this.invtotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.amount5);
            this.Controls.Add(this.unitprice5);
            this.Controls.Add(this.Desc5);
            this.Controls.Add(this.itemID5);
            this.Controls.Add(this.qty5);
            this.Controls.Add(this.amount4);
            this.Controls.Add(this.unitprice4);
            this.Controls.Add(this.Desc4);
            this.Controls.Add(this.itemID4);
            this.Controls.Add(this.qty4);
            this.Controls.Add(this.amount3);
            this.Controls.Add(this.unitprice3);
            this.Controls.Add(this.Desc3);
            this.Controls.Add(this.itemID3);
            this.Controls.Add(this.qty3);
            this.Controls.Add(this.amount2);
            this.Controls.Add(this.unitprice2);
            this.Controls.Add(this.Desc2);
            this.Controls.Add(this.itemID2);
            this.Controls.Add(this.qty2);
            this.Controls.Add(this.amount1);
            this.Controls.Add(this.unitprice1);
            this.Controls.Add(this.Desc1);
            this.Controls.Add(this.itemID1);
            this.Controls.Add(this.qty1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.invoiceDate);
            this.Controls.Add(this.ZIP);
            this.Controls.Add(this.State);
            this.Controls.Add(this.City);
            this.Controls.Add(this.Add2);
            this.Controls.Add(this.Add1);
            this.Controls.Add(this.CustVendName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCustVendName);
            this.Controls.Add(this.lblCustVendID);
            this.Name = "frmNewCustInvoices";
            this.Text = "Nuevo Invoice";
            this.Load += new System.EventHandler(this.frmNewInvoices_Load);
            this.Leave += new System.EventHandler(this.unitprice2_leave);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void GetCustIDList()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjCustomerList);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerId);
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToAddressLine1);
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToAddressLine2);
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToCity);
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToState);
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerBillToZip);
			exporter.AddToExportFieldList((short)PeachwIEObjCustomerListField.peachwIEObjCustomerListField_CustomerName);
			exporter.SetFilename(@"c:\XML\customers.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
            doc.Load(@"c:\XML\customers.xml");
			reader = doc.GetElementsByTagName("PAW_Customer");
			custIDList = Array.CreateInstance(typeof(string),7,reader.Count);
			for(int i = 0;i <= reader.Count -1;i++)
			{
				for(int a = 0;a <= reader[i].ChildNodes.Count - 1;a++)
				{
					switch(reader[i].ChildNodes[a].Name)
					{
						case "ID":
						{
							custIDList.SetValue(reader[i].ChildNodes[a].InnerText,0,i);
							break;
						}
						case "Name":
						{
							custIDList.SetValue(reader[i].ChildNodes[a].InnerText,1,i);
							break;
						}
						case "BillToAddress":
						{
							for(int b = 0;b <= reader[i].ChildNodes[a].ChildNodes.Count -1;b++)
							{
								switch(reader[i].ChildNodes[a].ChildNodes[b].Name)
								{
									case "Line1":
									{
										custIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,2,i);
										break;
									}
									case "Line2":
									{
										custIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,3,i);
										break;
									}
									case "City":
									{
										custIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,4,i);
										break;
									}
									case "State":
									{
										custIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,5,i);
										break;
									}
									case "Zip":
									{
										custIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,6,i);
										break;
									}
								}
							}
							break;
						}
					}
				}
			}
			for(int i = 0;i <= custIDList.GetUpperBound(1);i++)
			{
				this.custVendID.Items.Add(custIDList.GetValue(0,i));
			}
			exporter = null;
			imp = null;
			doc = null;
			reader = null;
		}
		
		private void GetItemIDList()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjInventoryItemsList);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_ItemId);
			exporter.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_ItemDescription);
			exporter.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice1);
			exporter.SetFilename(@"c:\XML\items.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\items.xml");
			reader = doc.GetElementsByTagName("PAW_Item");
            			
			itemIDList = Array.CreateInstance(typeof(string),3,reader.Count);

			for(int i = 0;i <= reader.Count -1;i++)
			{
				itemIDList.SetValue(reader[i].ChildNodes[0].InnerText,0,i);
				itemIDList.SetValue(reader[i].ChildNodes[1].InnerText,1,i);
				itemIDList.SetValue(reader[i].ChildNodes[2].InnerText,2,i);
				this.itemID1.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID2.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID3.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID4.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID5.Items.Add(reader[i].ChildNodes[0].InnerText);
			}
			exporter = null;
			imp = null;
			doc = null;
			reader = null;

		}
		private void GetGLAccts()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjChartOfAccounts);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerId);
			exporter.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerDescription);
			exporter.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Type);
			exporter.SetFilename(@"c:\XML\accounts.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\accounts.xml");
			GLInformation accttype = new GLInformation();

			reader = doc.GetElementsByTagName("PAW_Account");

			glAcctIDList = Array.CreateInstance(typeof(string),3,reader.Count);

			for(int i = 0;i<=reader.Count-1;i++)
			{
				foreach(XmlNode node in reader[i].ChildNodes)
				{
					switch(node.Name)
					{
						case "ID":
						{
							glAcctIDList.SetValue(node.InnerText,0,i);
							this.glacct1.Items.Add(node.InnerText);
							this.glacct2.Items.Add(node.InnerText);
							this.glacct3.Items.Add(node.InnerText);
							this.glacct4.Items.Add(node.InnerText);
							this.glacct5.Items.Add(node.InnerText);
							if(accttype.getAcctTypeWords(Convert.ToInt32(reader[i].ChildNodes[2].InnerText))=="Accounts Receivable")
								this.ARAccount.Items.Add(node.InnerText);
							break;
						}
						case "Description":
						{
							glAcctIDList.SetValue(node.InnerText,1,i);
							break;
						}
						case "Type":
						{
							glAcctIDList.SetValue(node.InnerText,2,i);
							break;
						}
					}
				}
			}
		}

		private void frmNewInvoices_Load(object sender, System.EventArgs e)
		{
			this.invoiceDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
			GetCustIDList();
			GetItemIDList();
			GetGLAccts();
		}

		private void custVendID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0;i<=custIDList.GetUpperBound(0)-1;i++)
			{
				if(custIDList.GetValue(0,i).ToString() == this.custVendID.Text)
				{
					if(custIDList.GetValue(1,i) != null)
						this.CustVendName.Text = custIDList.GetValue(1,i).ToString();
					if(custIDList.GetValue(2,i) != null)
						this.Add1.Text = custIDList.GetValue(2,i).ToString();
					if(custIDList.GetValue(3,i) != null)
						this.Add2.Text = custIDList.GetValue(3,i).ToString();
					if(custIDList.GetValue(4,i) != null)
						this.City.Text = custIDList.GetValue(4,i).ToString();
					if(custIDList.GetValue(5,i) != null)
						this.State.Text = custIDList.GetValue(5,i).ToString();
					if(custIDList.GetValue(6,i) != null)
						this.ZIP.Text = custIDList.GetValue(6,i).ToString();
				}
			}
		}

		private void itemID1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID1.Text)
				{
					this.Desc1.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice1.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void ARAccount_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= glAcctIDList.GetUpperBound(1); i++)
			{
				if(glAcctIDList.GetValue(0,i).ToString() == this.ARAccount.Text)
				{
					this.arAcctDesc.Text = glAcctIDList.GetValue(1,i).ToString();
				}
			}
		}

		private void itemID2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID2.Text)
				{
					this.Desc2.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice2.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID3.Text)
				{
					this.Desc3.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice3.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID4.Text)
				{
					this.Desc4.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice4.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID5_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID5.Text)
				{
					this.Desc5.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice5.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void SaveButton_Click(object sender, System.EventArgs e)
		{
			CreateXMLFile();
			Importfile();
			ClearForm();
		}
		private void ClearForm()
		{
			foreach(Control ctrl in this.Controls)
			{
				if(ctrl.GetType().ToString() == "System.Windows.Forms.TextBox"
					|| ctrl.GetType().ToString() == "System.Windows.Forms.ComboBox")
				{
					ctrl.Text = "";
				}
			}
		}
		private void Importfile()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjCashReceiptsJournal);
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.SetFilename(@"c:\XML\test.xml");
			exporter.Export();

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


			importer.SetFilename(@"c:\XML\sales.xml");
			importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			try
			{
				importer.Import();
			}
			catch(System.Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
		private void CreateXMLFile()
		{
			XmlTextWriter Writer = new XmlTextWriter(@"c:\XML\sales.xml",System.Text.Encoding.UTF8);
			
			Writer.WriteStartElement("PAW_Invoices");
			Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
			Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
			Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

			Writer.WriteStartElement("PAW_Invoice");
			Writer.WriteAttributeString("xsi:type", "paw:receipt");

			Writer.WriteStartElement("Customer_ID");
			Writer.WriteAttributeString("xsi:type", "paw:ID");
			Writer.WriteString(this.custVendID.Text);
			Writer.WriteEndElement();
			Writer.WriteElementString("Customer_Name",this.CustVendName.Text);
			Writer.WriteStartElement("Date");
			Writer.WriteAttributeString("xsi:type","paw:date");
			Writer.WriteString(this.invoiceDate.Text);
			Writer.WriteEndElement();	
			Writer.WriteElementString("Invoice_Number",this.invNum.Text);
			Writer.WriteElementString("Line1",this.Add1.Text);
			Writer.WriteElementString("Line2",this.Add2.Text);
			Writer.WriteElementString("City",this.City.Text);
			Writer.WriteElementString("State",this.State.Text);
			Writer.WriteElementString("Zip",this.State.Text);
			Writer.WriteStartElement("Accounts_Receivable_Account");
			Writer.WriteAttributeString("xsi:type","paw:ID");
			Writer.WriteString(this.ARAccount.Text.ToString());
			Writer.WriteEndElement();
			Writer.WriteElementString("Accounts_Receivable_Amount",this.invtotal.Text);
			Writer.WriteElementString("CreditMemoType","FALSE");
			int numdist = 0;

			if(this.qty1.Text != "" || this.itemID1.Text != "" || this.Desc1.Text != "" ||
				this.glacct1.Text != "" || this.unitprice1.Text != "" || this.amount1.Text != "")
			{
				numdist++;
			}
			if(this.qty2.Text != "" || this.itemID2.Text != "" || this.Desc2.Text != "" ||
				this.glacct2.Text != "" || this.unitprice2.Text != "" || this.amount2.Text != "")
			{
				numdist++;
			}
			if(this.qty3.Text != "" || this.itemID3.Text != "" || this.Desc3.Text != "" ||
				this.glacct3.Text != "" || this.unitprice3.Text != "" || this.amount3.Text != "")
			{
				numdist++;
			}
			if(this.qty4.Text != "" || this.itemID4.Text != "" || this.Desc4.Text != "" ||
				this.glacct4.Text != "" || this.unitprice4.Text != "" || this.amount4.Text != "")
			{
				numdist++;
			}
			if(this.qty5.Text != ""  || this.itemID5.Text != "" || this.Desc5.Text != "" ||
				this.glacct5.Text != "" || this.unitprice5.Text != "" || this.amount5.Text != "")
			{
				numdist++;
			}
			Writer.WriteElementString("Number_of_Distributions",numdist.ToString());
			Writer.WriteStartElement("SalesLines");

			if(this.qty1.Text != "" && this.itemID1.Text != "" && this.Desc1.Text != "" &&
				this.glacct1.Text != "" && this.unitprice1.Text != "" && this.amount1.Text != "")
			{

				Writer.WriteStartElement("SalesLine");
				if(this.qty1.Text != "")
					Writer.WriteElementString("Quantity",this.qty1.Text);
				if(this.itemID1.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID1.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc1.Text != "")
					Writer.WriteElementString("Description",this.Desc1.Text);
				if(this.glacct1.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct1.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice1.Text != "")
					Writer.WriteElementString("Unit_Price",(Convert.ToDouble(this.unitprice1.Text)*-1).ToString());
				Writer.WriteElementString("Tax_Type","2");
				if(this.amount1.Text != "")
					Writer.WriteElementString("Amount",(Convert.ToDouble(this.amount1.Text) * -1).ToString());

				Writer.WriteEndElement();//closes the sales line element
			}

			if(this.qty2.Text != "" && this.itemID2.Text != "" && this.Desc2.Text != "" &&
				this.glacct2.Text != "" && this.unitprice2.Text != "" && this.amount2.Text != "")
			{

				Writer.WriteStartElement("SalesLine");
				if(this.qty2.Text != "")
					Writer.WriteElementString("Quantity",this.qty2.Text);
				if(this.itemID2.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID2.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc2.Text != "")
					Writer.WriteElementString("Description",this.Desc1.Text);
				if(this.glacct2.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct2.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice2.Text != "")
					Writer.WriteElementString("Unit_Price",(Convert.ToDouble(this.unitprice2.Text)*-1).ToString());
				Writer.WriteElementString("Tax_Type","2");

				if(this.amount2.Text != "")
					Writer.WriteElementString("Amount",(Convert.ToDouble(this.amount2.Text) * -1).ToString());

				Writer.WriteEndElement();//closes the sales line element
			}
			if(this.qty3.Text != "" && this.itemID3.Text != "" && this.Desc3.Text != "" &&
				this.glacct3.Text != "" && this.unitprice3.Text != "" && this.amount3.Text != "")
			{

				Writer.WriteStartElement("SalesLine");
				if(this.qty3.Text != "")
					Writer.WriteElementString("Quantity",this.qty3.Text);
				if(this.itemID3.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID3.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc3.Text != "")
					Writer.WriteElementString("Description",this.Desc3.Text);
				if(this.glacct3.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct3.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice3.Text != "")
					Writer.WriteElementString("Unit_Price",(Convert.ToDouble(this.unitprice1.Text)*-1).ToString());
				
				Writer.WriteElementString("Tax_Type","2");

				if(this.amount3.Text != "")
					Writer.WriteElementString("Amount",((Convert.ToDouble(this.amount3.Text)*-1).ToString()));

				Writer.WriteEndElement();//closes the sales line element
			}
			if(this.qty4.Text != "" && this.itemID4.Text != "" && this.Desc4.Text != "" &&
				this.glacct4.Text != "" && this.unitprice4.Text != "" && this.amount4.Text != "")
			{

				Writer.WriteStartElement("SalesLine");
				if(this.qty4.Text != "")
					Writer.WriteElementString("Quantity",this.qty4.Text);
				if(this.itemID4.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID4.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc4.Text != "")
					Writer.WriteElementString("Description",this.Desc4.Text);
				if(this.glacct4.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct4.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice4.Text != "")
					Writer.WriteElementString("Unit_Price",(Convert.ToDouble(this.unitprice4.Text)*-1).ToString());
				Writer.WriteElementString("Tax_Type","2");

				if(this.amount4.Text != "")
					Writer.WriteElementString("Amount",(Convert.ToDouble(this.amount4.Text) * -1).ToString());

				Writer.WriteEndElement();//closes the sales line element
			}
			if(this.qty5.Text != "" && this.itemID5.Text != "" && this.Desc5.Text != "" &&
				this.glacct5.Text != "" && this.unitprice5.Text != "" && this.amount5.Text != "")
			{

				Writer.WriteStartElement("SalesLine");
				if(this.qty5.Text != "")
					Writer.WriteElementString("Quantity",this.qty5.Text);
				if(this.itemID5.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID5.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc5.Text != "")
					Writer.WriteElementString("Description",this.Desc5.Text);
				if(this.glacct5.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct5.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice5.Text != "")
					Writer.WriteElementString("Unit_Price",(Convert.ToDouble(this.unitprice5.Text)*-1).ToString());
				Writer.WriteElementString("Tax_Type","2");

				if(this.amount5.Text != "")
					Writer.WriteElementString("Amount",(Convert.ToDouble(this.amount5.Text) * -1).ToString());

				Writer.WriteEndElement();//closes the sales line element
			}
			Writer.WriteEndElement();//Closes the Sales Lines element

			Writer.WriteEndElement();//Closes the paw_invoice element
			
			Writer.WriteEndElement();//closes the paw_invoices element and ends the document
			
			Writer.Close();
		}
		private void CalcInvoice()
		{
			double total = 0;
			foreach(Control ctrl in this.Controls)
			{
				if(ctrl.Name.StartsWith("amount"))
				{
					if (ctrl.Text != "")
					{
						total += Convert.ToDouble(ctrl.Text);
					}
				}
			}
			invtotal.Text = total.ToString("#,##0.00");
		}

		private void qty1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}
		private void itemID1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}
		private void unitprice1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}
		private void amount1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}

		private void qty2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void itemID2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void unitprice2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void amount2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void qty3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void itemID3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void unitprice3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void amount3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void qty4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void itemID4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void unitprice4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void amount4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void qty5_leave(object sender, EventArgs e)
		{
			calcLine(amount5,qty5,unitprice5);
			CalcInvoice();
		}
		private void itemID5_leave(object sender, EventArgs e)
		{
			calcLine(amount5,qty5,unitprice5);
			CalcInvoice();
		}
		private void unitprice5_leave(object sender, EventArgs e)
		{
			if(Convert.ToDouble(qty5.Text) != 0)
			{
				calcLine(amount5,qty5,unitprice5);
				CalcInvoice();
			}
		}
		private void amount5_leave(object sender, EventArgs e)
		{
			calcLine(amount5,qty5,unitprice5);
			CalcInvoice();
		}
		private void calcLine(TextBox amount,TextBox qty,TextBox unitPrice)
		{
			double amt = 0;
			double units = 0;
			double uprice = 0;
        
			if(amount.Text != "")
				amt = Convert.ToDouble(amount.Text);

			if (qty.Text != "")
				units = Convert.ToDouble(qty.Text);
        
			if (unitPrice.Text != "")
				uprice = Convert.ToDouble(unitPrice.Text);
        
			if (amt == 0 && uprice != 0 && units != 0)
				amt = units * uprice;
			else if (amt != 0 && uprice != 0 && units == 0)
				units = amt / uprice;
			else if (amt != 0 && units != 0 && uprice == 0)
				uprice = amt / units;
			else if (amt != 0 && units != 0 && uprice != 0 && 
				amt != units * uprice)
				amt = units * uprice;
        
			if (amt != 0)
				amount.Text = amt.ToString("#,##0.00");

			if (units != 0)
				qty.Text = units.ToString("#,##0.00000");
        
			if (uprice != 0)
				unitPrice.Text = uprice.ToString("#,##0.00000");
        
			CalcInvoice();

		}
	}
	public class frmNewVendInvoices : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox invoiceDate;
		private System.Windows.Forms.TextBox ZIP;
		private System.Windows.Forms.TextBox State;
		private System.Windows.Forms.TextBox City;
		private System.Windows.Forms.TextBox Add2;
		private System.Windows.Forms.TextBox Add1;
		private System.Windows.Forms.TextBox CustVendName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.Label lblCustVendName;
		public System.Windows.Forms.Label lblCustVendID;
		private System.Windows.Forms.TextBox qty1;
		private System.Windows.Forms.ComboBox itemID1;
		private System.Windows.Forms.TextBox Desc1;
		private System.Windows.Forms.TextBox unitprice1;
		private System.Windows.Forms.TextBox amount1;
		private System.Windows.Forms.TextBox amount2;
		private System.Windows.Forms.TextBox unitprice2;
		private System.Windows.Forms.TextBox Desc2;
		private System.Windows.Forms.ComboBox itemID2;
		private System.Windows.Forms.TextBox qty2;
		private System.Windows.Forms.TextBox amount3;
		private System.Windows.Forms.TextBox unitprice3;
		private System.Windows.Forms.TextBox Desc3;
		private System.Windows.Forms.ComboBox itemID3;
		private System.Windows.Forms.TextBox qty3;
		private System.Windows.Forms.TextBox amount4;
		private System.Windows.Forms.TextBox unitprice4;
		private System.Windows.Forms.TextBox Desc4;
		private System.Windows.Forms.ComboBox itemID4;
		private System.Windows.Forms.TextBox qty4;
		private System.Windows.Forms.TextBox amount5;
		private System.Windows.Forms.TextBox unitprice5;
		private System.Windows.Forms.TextBox Desc5;
		private System.Windows.Forms.ComboBox itemID5;
		private System.Windows.Forms.TextBox qty5;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox ARAccount;
		private System.Windows.Forms.Label label12;
		public System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox custVendID;
        private Interop.PeachwServer.Export exporter;
        private Interop.PeachwServer.Import importer;
		private Connect ptApp = new Connect();
		private System.ComponentModel.Container components = null;
		private XmlImplementation imp;
		private XmlDocument doc;
		private XmlNodeList reader;
		private Array vendIDList;
		private Array itemIDList;
		private System.Windows.Forms.ComboBox glacct5;
		private System.Windows.Forms.ComboBox glacct4;
		private System.Windows.Forms.ComboBox glacct3;
		private System.Windows.Forms.ComboBox glacct2;
		private System.Windows.Forms.ComboBox glacct1;
		private System.Windows.Forms.TextBox arAcctDesc;
		private System.Windows.Forms.TextBox invtotal;
		private System.Windows.Forms.TextBox invNum;
		private Array glAcctIDList;

		public frmNewVendInvoices()
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
			this.label7 = new System.Windows.Forms.Label();
			this.invoiceDate = new System.Windows.Forms.TextBox();
			this.ZIP = new System.Windows.Forms.TextBox();
			this.State = new System.Windows.Forms.TextBox();
			this.City = new System.Windows.Forms.TextBox();
			this.Add2 = new System.Windows.Forms.TextBox();
			this.Add1 = new System.Windows.Forms.TextBox();
			this.CustVendName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblCustVendName = new System.Windows.Forms.Label();
			this.lblCustVendID = new System.Windows.Forms.Label();
			this.qty1 = new System.Windows.Forms.TextBox();
			this.itemID1 = new System.Windows.Forms.ComboBox();
			this.Desc1 = new System.Windows.Forms.TextBox();
			this.unitprice1 = new System.Windows.Forms.TextBox();
			this.amount1 = new System.Windows.Forms.TextBox();
			this.amount2 = new System.Windows.Forms.TextBox();
			this.unitprice2 = new System.Windows.Forms.TextBox();
			this.Desc2 = new System.Windows.Forms.TextBox();
			this.itemID2 = new System.Windows.Forms.ComboBox();
			this.qty2 = new System.Windows.Forms.TextBox();
			this.amount3 = new System.Windows.Forms.TextBox();
			this.unitprice3 = new System.Windows.Forms.TextBox();
			this.Desc3 = new System.Windows.Forms.TextBox();
			this.itemID3 = new System.Windows.Forms.ComboBox();
			this.qty3 = new System.Windows.Forms.TextBox();
			this.amount4 = new System.Windows.Forms.TextBox();
			this.unitprice4 = new System.Windows.Forms.TextBox();
			this.Desc4 = new System.Windows.Forms.TextBox();
			this.itemID4 = new System.Windows.Forms.ComboBox();
			this.qty4 = new System.Windows.Forms.TextBox();
			this.amount5 = new System.Windows.Forms.TextBox();
			this.unitprice5 = new System.Windows.Forms.TextBox();
			this.Desc5 = new System.Windows.Forms.TextBox();
			this.itemID5 = new System.Windows.Forms.ComboBox();
			this.qty5 = new System.Windows.Forms.TextBox();
			this.SaveButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.invtotal = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.glacct5 = new System.Windows.Forms.ComboBox();
			this.glacct4 = new System.Windows.Forms.ComboBox();
			this.glacct3 = new System.Windows.Forms.ComboBox();
			this.glacct2 = new System.Windows.Forms.ComboBox();
			this.glacct1 = new System.Windows.Forms.ComboBox();
			this.ARAccount = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.custVendID = new System.Windows.Forms.ComboBox();
			this.invNum = new System.Windows.Forms.TextBox();
			this.arAcctDesc = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(336, 76);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 20);
			this.label7.TabIndex = 33;
			this.label7.Text = "Ciudad, Estado, ZIP";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// invoiceDate
			// 
			this.invoiceDate.Location = new System.Drawing.Point(88, 32);
			this.invoiceDate.Name = "invoiceDate";
			this.invoiceDate.Size = new System.Drawing.Size(208, 20);
			this.invoiceDate.TabIndex = 2;
			this.invoiceDate.Text = "";
			// 
			// ZIP
			// 
			this.ZIP.Enabled = false;
			this.ZIP.Location = new System.Drawing.Point(672, 76);
			this.ZIP.Name = "ZIP";
			this.ZIP.Size = new System.Drawing.Size(40, 20);
			this.ZIP.TabIndex = 31;
			this.ZIP.Text = "";
			// 
			// State
			// 
			this.State.Enabled = false;
			this.State.Location = new System.Drawing.Point(648, 76);
			this.State.Name = "State";
			this.State.Size = new System.Drawing.Size(24, 20);
			this.State.TabIndex = 29;
			this.State.Text = "";
			// 
			// City
			// 
			this.City.Enabled = false;
			this.City.Location = new System.Drawing.Point(424, 76);
			this.City.Name = "City";
			this.City.Size = new System.Drawing.Size(223, 20);
			this.City.TabIndex = 28;
			this.City.Text = "";
			// 
			// Add2
			// 
			this.Add2.Enabled = false;
			this.Add2.Location = new System.Drawing.Point(424, 52);
			this.Add2.Name = "Add2";
			this.Add2.Size = new System.Drawing.Size(292, 20);
			this.Add2.TabIndex = 27;
			this.Add2.Text = "";
			// 
			// Add1
			// 
			this.Add1.Enabled = false;
			this.Add1.Location = new System.Drawing.Point(424, 28);
			this.Add1.Name = "Add1";
			this.Add1.Size = new System.Drawing.Size(292, 20);
			this.Add1.TabIndex = 26;
			this.Add1.Text = "";
			// 
			// CustVendName
			// 
			this.CustVendName.Enabled = false;
			this.CustVendName.Location = new System.Drawing.Point(424, 4);
			this.CustVendName.Name = "CustVendName";
			this.CustVendName.Size = new System.Drawing.Size(292, 20);
			this.CustVendName.TabIndex = 25;
			this.CustVendName.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(0, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(83, 21);
			this.label6.TabIndex = 23;
			this.label6.Text = "Fecha Invoice";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(0, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 21);
			this.label5.TabIndex = 22;
			this.label5.Text = "Numero de Invoice";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(336, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 20);
			this.label4.TabIndex = 21;
			this.label4.Text = "Direccion";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(336, 28);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 20);
			this.label3.TabIndex = 20;
			this.label3.Text = "Direccion";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustVendName
			// 
			this.lblCustVendName.Location = new System.Drawing.Point(336, 12);
			this.lblCustVendName.Name = "lblCustVendName";
			this.lblCustVendName.Size = new System.Drawing.Size(80, 20);
			this.lblCustVendName.TabIndex = 19;
			this.lblCustVendName.Text = "Nombre";
			this.lblCustVendName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustVendID
			// 
			this.lblCustVendID.Location = new System.Drawing.Point(0, 56);
			this.lblCustVendID.Name = "lblCustVendID";
			this.lblCustVendID.Size = new System.Drawing.Size(83, 21);
			this.lblCustVendID.TabIndex = 18;
			this.lblCustVendID.Text = "ID";
			this.lblCustVendID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// qty1
			// 
			this.qty1.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.qty1.Location = new System.Drawing.Point(8, 164);
			this.qty1.Name = "qty1";
			this.qty1.Size = new System.Drawing.Size(72, 20);
			this.qty1.TabIndex = 5;
			this.qty1.Text = "";
			this.qty1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.qty1.Leave += new EventHandler(this.qty1_leave);
			// 
			// itemID1
			// 
			this.itemID1.Location = new System.Drawing.Point(80, 164);
			this.itemID1.Name = "itemID1";
			this.itemID1.Size = new System.Drawing.Size(88, 21);
			this.itemID1.TabIndex = 6;
			this.itemID1.SelectedIndexChanged += new System.EventHandler(this.itemID1_SelectedIndexChanged);
			this.itemID1.Leave += new EventHandler(this.itemID1_leave);
			// 
			// Desc1
			// 
			this.Desc1.Location = new System.Drawing.Point(168, 164);
			this.Desc1.Name = "Desc1";
			this.Desc1.Size = new System.Drawing.Size(264, 20);
			this.Desc1.TabIndex = 7;
			this.Desc1.Text = "";
			// 
			// unitprice1
			// 
			this.unitprice1.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.unitprice1.Location = new System.Drawing.Point(520, 164);
			this.unitprice1.Name = "unitprice1";
			this.unitprice1.Size = new System.Drawing.Size(80, 20);
			this.unitprice1.TabIndex = 9;
			this.unitprice1.Text = "";
			this.unitprice1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.unitprice1.Leave += new EventHandler(this.unitprice1_leave);
			// 
			// amount1
			// 
			this.amount1.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.amount1.Location = new System.Drawing.Point(600, 164);
			this.amount1.Name = "amount1";
			this.amount1.Size = new System.Drawing.Size(120, 20);
			this.amount1.TabIndex = 10;
			this.amount1.Text = "";
			this.amount1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.amount1.Leave += new EventHandler(this.amount1_leave);
			// 
			// amount2
			// 
			this.amount2.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.amount2.Location = new System.Drawing.Point(600, 188);
			this.amount2.Name = "amount2";
			this.amount2.Size = new System.Drawing.Size(120, 20);
			this.amount2.TabIndex = 16;
			this.amount2.Text = "";
			this.amount2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.amount2.Leave += new EventHandler(this.amount2_leave);
			// 
			// unitprice2
			// 
			this.unitprice2.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.unitprice2.Location = new System.Drawing.Point(520, 188);
			this.unitprice2.Name = "unitprice2";
			this.unitprice2.Size = new System.Drawing.Size(80, 20);
			this.unitprice2.TabIndex = 15;
			this.unitprice2.Text = "";
			this.unitprice2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.unitprice2.Leave += new EventHandler(this.unitprice2_leave);
			// 
			// Desc2
			// 
			this.Desc2.Location = new System.Drawing.Point(168, 188);
			this.Desc2.Name = "Desc2";
			this.Desc2.Size = new System.Drawing.Size(264, 20);
			this.Desc2.TabIndex = 13;
			this.Desc2.Text = "";
			// 
			// itemID2
			// 
			this.itemID2.Location = new System.Drawing.Point(80, 188);
			this.itemID2.Name = "itemID2";
			this.itemID2.Size = new System.Drawing.Size(88, 21);
			this.itemID2.TabIndex = 12;
			this.itemID2.SelectedIndexChanged += new System.EventHandler(this.itemID2_SelectedIndexChanged);
			this.itemID2.Leave += new EventHandler(this.itemID2_leave);
			// 
			// qty2
			// 
			this.qty2.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.qty2.Location = new System.Drawing.Point(8, 188);
			this.qty2.Name = "qty2";
			this.qty2.Size = new System.Drawing.Size(72, 20);
			this.qty2.TabIndex = 11;
			this.qty2.Text = "";
			this.qty2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.qty2.Leave += new EventHandler(this.qty2_leave);
			// 
			// amount3
			// 
			this.amount3.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.amount3.Location = new System.Drawing.Point(600, 212);
			this.amount3.Name = "amount3";
			this.amount3.Size = new System.Drawing.Size(120, 20);
			this.amount3.TabIndex = 22;
			this.amount3.Text = "";
			this.amount3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.amount3.Leave += new EventHandler(this.amount3_leave);
			// 
			// unitprice3
			// 
			this.unitprice3.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.unitprice3.Location = new System.Drawing.Point(520, 212);
			this.unitprice3.Name = "unitprice3";
			this.unitprice3.Size = new System.Drawing.Size(80, 20);
			this.unitprice3.TabIndex = 21;
			this.unitprice3.Text = "";
			this.unitprice3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.unitprice3.Leave += new EventHandler(this.unitprice3_leave);
			// 
			// Desc3
			// 
			this.Desc3.Location = new System.Drawing.Point(168, 212);
			this.Desc3.Name = "Desc3";
			this.Desc3.Size = new System.Drawing.Size(264, 20);
			this.Desc3.TabIndex = 19;
			this.Desc3.Text = "";
			// 
			// itemID3
			// 
			this.itemID3.Location = new System.Drawing.Point(80, 212);
			this.itemID3.Name = "itemID3";
			this.itemID3.Size = new System.Drawing.Size(88, 21);
			this.itemID3.TabIndex = 18;
			this.itemID3.SelectedIndexChanged += new System.EventHandler(this.itemID3_SelectedIndexChanged);
			this.itemID3.Leave += new EventHandler(this.itemID3_leave);
			// 
			// qty3
			// 
			this.qty3.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.qty3.Location = new System.Drawing.Point(8, 212);
			this.qty3.Name = "qty3";
			this.qty3.Size = new System.Drawing.Size(72, 20);
			this.qty3.TabIndex = 17;
			this.qty3.Text = "";
			this.qty3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.qty3.Leave += new EventHandler(this.qty3_leave);
			// 
			// amount4
			// 
			this.amount4.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.amount4.Location = new System.Drawing.Point(600, 236);
			this.amount4.Name = "amount4";
			this.amount4.Size = new System.Drawing.Size(120, 20);
			this.amount4.TabIndex = 28;
			this.amount4.Text = "";
			this.amount4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.amount4.Leave += new EventHandler(this.amount4_leave);
			// 
			// unitprice4
			// 
			this.unitprice4.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.unitprice4.Location = new System.Drawing.Point(520, 236);
			this.unitprice4.Name = "unitprice4";
			this.unitprice4.Size = new System.Drawing.Size(80, 20);
			this.unitprice4.TabIndex = 27;
			this.unitprice4.Text = "";
			this.unitprice4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.unitprice4.Leave += new EventHandler(this.unitprice4_leave);
			// 
			// Desc4
			// 
			this.Desc4.Location = new System.Drawing.Point(168, 236);
			this.Desc4.Name = "Desc4";
			this.Desc4.Size = new System.Drawing.Size(264, 20);
			this.Desc4.TabIndex = 25;
			this.Desc4.Text = "";
			// 
			// itemID4
			// 
			this.itemID4.Location = new System.Drawing.Point(80, 236);
			this.itemID4.Name = "itemID4";
			this.itemID4.Size = new System.Drawing.Size(88, 21);
			this.itemID4.TabIndex = 24;
			this.itemID4.SelectedIndexChanged += new System.EventHandler(this.itemID4_SelectedIndexChanged);
			this.itemID4.Leave += new EventHandler(this.itemID4_leave);
			// 
			// qty4
			// 
			this.qty4.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.qty4.Location = new System.Drawing.Point(8, 236);
			this.qty4.Name = "qty4";
			this.qty4.Size = new System.Drawing.Size(72, 20);
			this.qty4.TabIndex = 23;
			this.qty4.Text = "";
			this.qty4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.qty4.Leave += new EventHandler(this.qty4_leave);
			// 
			// amount5
			// 
			this.amount5.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.amount5.Location = new System.Drawing.Point(600, 260);
			this.amount5.Name = "amount5";
			this.amount5.Size = new System.Drawing.Size(120, 20);
			this.amount5.TabIndex = 34;
			this.amount5.Text = "";
			this.amount5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.amount5.Leave += new EventHandler(this.amount5_leave);
			// 
			// unitprice5
			// 
			this.unitprice5.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.unitprice5.Location = new System.Drawing.Point(520, 260);
			this.unitprice5.Name = "unitprice5";
			this.unitprice5.Size = new System.Drawing.Size(80, 20);
			this.unitprice5.TabIndex = 33;
			this.unitprice5.Text = "";
			this.unitprice5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.unitprice5.Leave += new EventHandler(this.unitprice5_leave);
			// 
			// Desc5
			// 
			this.Desc5.Location = new System.Drawing.Point(168, 260);
			this.Desc5.Name = "Desc5";
			this.Desc5.Size = new System.Drawing.Size(264, 20);
			this.Desc5.TabIndex = 31;
			this.Desc5.Text = "";
			// 
			// itemID5
			// 
			this.itemID5.Location = new System.Drawing.Point(80, 260);
			this.itemID5.Name = "itemID5";
			this.itemID5.Size = new System.Drawing.Size(88, 21);
			this.itemID5.TabIndex = 30;
			this.itemID5.SelectedIndexChanged += new System.EventHandler(this.itemID5_SelectedIndexChanged);
			this.itemID5.Leave += new EventHandler(this.itemID5_leave);
			// 
			// qty5
			// 
			this.qty5.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.qty5.Location = new System.Drawing.Point(8, 260);
			this.qty5.Name = "qty5";
			this.qty5.Size = new System.Drawing.Size(72, 20);
			this.qty5.TabIndex = 29;
			this.qty5.Text = "";
			this.qty5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.qty5.Leave += new EventHandler(this.qty5_leave);
			// 
			// SaveButton
			// 
			this.SaveButton.Location = new System.Drawing.Point(336, 292);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.TabIndex = 35;
			this.SaveButton.Text = "Guardar";
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(80, 140);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 20);
			this.label2.TabIndex = 61;
			this.label2.Text = "Item";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(168, 140);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(272, 20);
			this.label8.TabIndex = 62;
			this.label8.Text = "Descripcion";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(520, 140);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 20);
			this.label9.TabIndex = 63;
			this.label9.Text = "Precio Unitario";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(600, 140);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 20);
			this.label10.TabIndex = 64;
			this.label10.Text = "Monto";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 140);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 20);
			this.label1.TabIndex = 65;
			this.label1.Text = "Cantidad";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// invtotal
			// 
			this.invtotal.Enabled = false;
			this.invtotal.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.invtotal.Location = new System.Drawing.Point(600, 284);
			this.invtotal.Name = "invtotal";
			this.invtotal.Size = new System.Drawing.Size(120, 20);
			this.invtotal.TabIndex = 66;
			this.invtotal.Text = "";
			this.invtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(432, 140);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(88, 20);
			this.label11.TabIndex = 72;
			this.label11.Text = "Cuenta GL";
			this.label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// glacct5
			// 
			this.glacct5.Location = new System.Drawing.Point(432, 260);
			this.glacct5.Name = "glacct5";
			this.glacct5.Size = new System.Drawing.Size(88, 21);
			this.glacct5.TabIndex = 32;
			// 
			// glacct4
			// 
			this.glacct4.Location = new System.Drawing.Point(432, 236);
			this.glacct4.Name = "glacct4";
			this.glacct4.Size = new System.Drawing.Size(88, 21);
			this.glacct4.TabIndex = 26;
			// 
			// glacct3
			// 
			this.glacct3.Location = new System.Drawing.Point(432, 212);
			this.glacct3.Name = "glacct3";
			this.glacct3.Size = new System.Drawing.Size(88, 21);
			this.glacct3.TabIndex = 20;
			// 
			// glacct2
			// 
			this.glacct2.Location = new System.Drawing.Point(432, 188);
			this.glacct2.Name = "glacct2";
			this.glacct2.Size = new System.Drawing.Size(88, 21);
			this.glacct2.TabIndex = 14;
			// 
			// glacct1
			// 
			this.glacct1.Location = new System.Drawing.Point(432, 164);
			this.glacct1.Name = "glacct1";
			this.glacct1.Size = new System.Drawing.Size(88, 21);
			this.glacct1.TabIndex = 8;
			// 
			// ARAccount
			// 
			this.ARAccount.Location = new System.Drawing.Point(424, 104);
			this.ARAccount.Name = "ARAccount";
			this.ARAccount.Size = new System.Drawing.Size(88, 21);
			this.ARAccount.TabIndex = 4;
			this.ARAccount.SelectedIndexChanged += new System.EventHandler(this.ARAccount_SelectedIndexChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(256, 104);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(168, 21);
			this.label12.TabIndex = 74;
			this.label12.Text = "Accounts Recievable Account";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(520, 284);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(80, 21);
			this.label13.TabIndex = 75;
			this.label13.Text = "Total del Invoice";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// custVendID
			// 
			this.custVendID.Location = new System.Drawing.Point(88, 56);
			this.custVendID.Name = "custVendID";
			this.custVendID.Size = new System.Drawing.Size(208, 21);
			this.custVendID.TabIndex = 3;
			this.custVendID.SelectedIndexChanged += new System.EventHandler(this.custVendID_SelectedIndexChanged);
			// 
			// invNum
			// 
			this.invNum.Location = new System.Drawing.Point(88, 8);
			this.invNum.Name = "invNum";
			this.invNum.Size = new System.Drawing.Size(208, 20);
			this.invNum.TabIndex = 1;
			this.invNum.Text = "";
			// 
			// arAcctDesc
			// 
			this.arAcctDesc.Enabled = false;
			this.arAcctDesc.Location = new System.Drawing.Point(512, 104);
			this.arAcctDesc.Name = "arAcctDesc";
			this.arAcctDesc.Size = new System.Drawing.Size(200, 20);
			this.arAcctDesc.TabIndex = 78;
			this.arAcctDesc.Text = "";
			// 
			// frmNewCustInvoices
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(728, 318);
			this.Controls.Add(this.arAcctDesc);
			this.Controls.Add(this.invNum);
			this.Controls.Add(this.custVendID);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.ARAccount);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.glacct5);
			this.Controls.Add(this.glacct4);
			this.Controls.Add(this.glacct3);
			this.Controls.Add(this.glacct2);
			this.Controls.Add(this.glacct1);
			this.Controls.Add(this.invtotal);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.amount5);
			this.Controls.Add(this.unitprice5);
			this.Controls.Add(this.Desc5);
			this.Controls.Add(this.itemID5);
			this.Controls.Add(this.qty5);
			this.Controls.Add(this.amount4);
			this.Controls.Add(this.unitprice4);
			this.Controls.Add(this.Desc4);
			this.Controls.Add(this.itemID4);
			this.Controls.Add(this.qty4);
			this.Controls.Add(this.amount3);
			this.Controls.Add(this.unitprice3);
			this.Controls.Add(this.Desc3);
			this.Controls.Add(this.itemID3);
			this.Controls.Add(this.qty3);
			this.Controls.Add(this.amount2);
			this.Controls.Add(this.unitprice2);
			this.Controls.Add(this.Desc2);
			this.Controls.Add(this.itemID2);
			this.Controls.Add(this.qty2);
			this.Controls.Add(this.amount1);
			this.Controls.Add(this.unitprice1);
			this.Controls.Add(this.Desc1);
			this.Controls.Add(this.itemID1);
			this.Controls.Add(this.qty1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.invoiceDate);
			this.Controls.Add(this.ZIP);
			this.Controls.Add(this.State);
			this.Controls.Add(this.City);
			this.Controls.Add(this.Add2);
			this.Controls.Add(this.Add1);
			this.Controls.Add(this.CustVendName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblCustVendName);
			this.Controls.Add(this.lblCustVendID);
			this.Name = "frmNewVendInvoices";
			this.Text = "Nuevo Invoice de Vendor";
			this.Load += new System.EventHandler(this.frmNewInvoices_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void GetVendIDList()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjVendorList);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorId);
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorAddressLine1);
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorAddressLine2);
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorCity);
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorState);
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorZip);
			exporter.AddToExportFieldList((short)PeachwIEObjVendorListField.peachwIEObjVendorListField_VendorName);
			exporter.SetFilename(@"c:\XML\vendors.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\vendors.xml");
			reader = doc.GetElementsByTagName("PAW_Vendor");
			vendIDList = Array.CreateInstance(typeof(string),7,reader.Count);
			for(int i = 0;i <= reader.Count -1;i++)
			{
				for(int a = 0;a <= reader[i].ChildNodes.Count - 1;a++)
				{
					switch(reader[i].ChildNodes[a].Name)
					{
						case "ID":
						{
							vendIDList.SetValue(reader[i].ChildNodes[a].InnerText,0,i);
							break;
						}
						case "Name":
						{
							vendIDList.SetValue(reader[i].ChildNodes[a].InnerText,1,i);
							break;
						}
						case "RemitToAddress":
						{
							for(int b = 0;b <= reader[i].ChildNodes[a].ChildNodes.Count -1;b++)
							{
								switch(reader[i].ChildNodes[a].ChildNodes[b].Name)
								{
									case "Line1":
									{
										vendIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,2,i);
										break;
									}
									case "Line2":
									{
										vendIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,3,i);
										break;
									}
									case "City":
									{
										vendIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,4,i);
										break;
									}
									case "State":
									{
										vendIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,5,i);
										break;
									}
									case "Zip":
									{
										vendIDList.SetValue(reader[i].ChildNodes[a].ChildNodes[b].InnerText,6,i);
										break;
									}
								}
							}
							break;
						}
					}
				}
			}
			for(int i = 0;i <= vendIDList.GetUpperBound(1);i++)
			{
				this.custVendID.Items.Add(vendIDList.GetValue(0,i));
			}
			exporter = null;
			imp = null;
			doc = null;
			reader = null;
		}
		
		private void GetItemIDList()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjInventoryItemsList);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_ItemId);
			exporter.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_ItemDescription);
			exporter.AddToExportFieldList((short)PeachwIEObjInventoryItemsListField.peachwIEObjInventoryItemsListField_UnitPrice1);
			exporter.SetFilename(@"c:\XML\items.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\items.xml");
			reader = doc.GetElementsByTagName("PAW_Item");
            			
			itemIDList = Array.CreateInstance(typeof(string),3,reader.Count);

			for(int i = 0;i <= reader.Count -1;i++)
			{
				itemIDList.SetValue(reader[i].ChildNodes[0].InnerText,0,i);
				itemIDList.SetValue(reader[i].ChildNodes[1].InnerText,1,i);
				itemIDList.SetValue(reader[i].ChildNodes[2].InnerText,2,i);
				this.itemID1.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID2.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID3.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID4.Items.Add(reader[i].ChildNodes[0].InnerText);
				this.itemID5.Items.Add(reader[i].ChildNodes[0].InnerText);
			}
			exporter = null;
			imp = null;
			doc = null;
			reader = null;

		}
		private void GetGLAccts()
		{
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjChartOfAccounts);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerId);
			exporter.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerDescription);
			exporter.AddToExportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Type);
			exporter.SetFilename(@"c:\XML\accounts.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\accounts.xml");
			GLInformation accttype = new GLInformation();

			reader = doc.GetElementsByTagName("PAW_Account");

			glAcctIDList = Array.CreateInstance(typeof(string),3,reader.Count);

			for(int i = 0;i<=reader.Count-1;i++)
			{
				foreach(XmlNode node in reader[i].ChildNodes)
				{
					switch(node.Name)
					{
						case "ID":
						{
							glAcctIDList.SetValue(node.InnerText,0,i);
							this.glacct1.Items.Add(node.InnerText);
							this.glacct2.Items.Add(node.InnerText);
							this.glacct3.Items.Add(node.InnerText);
							this.glacct4.Items.Add(node.InnerText);
							this.glacct5.Items.Add(node.InnerText);
							if(accttype.getAcctTypeWords(Convert.ToInt32(reader[i].ChildNodes[2].InnerText))=="Accounts Payable")
								this.ARAccount.Items.Add(node.InnerText);
							break;
						}
						case "Description":
						{
							glAcctIDList.SetValue(node.InnerText,1,i);
							break;
						}
						case "Type":
						{
							glAcctIDList.SetValue(node.InnerText,2,i);
							break;
						}
					}
				}
			}
		}


		private void frmNewInvoices_Load(object sender, System.EventArgs e)
		{
			this.invoiceDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
			GetVendIDList();
			GetItemIDList();
			GetGLAccts();
		}

		private void custVendID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0;i<=vendIDList.GetUpperBound(0);i++)
			{
				if(vendIDList.GetValue(0,i).ToString() == this.custVendID.Text)
				{
					if(vendIDList.GetValue(1,i) != null)
						this.CustVendName.Text = vendIDList.GetValue(1,i).ToString();
					if(vendIDList.GetValue(2,i) != null)
						this.Add1.Text = vendIDList.GetValue(2,i).ToString();
					if(vendIDList.GetValue(3,i) != null)
						this.Add2.Text = vendIDList.GetValue(3,i).ToString();
					if(vendIDList.GetValue(4,i) != null)
						this.City.Text = vendIDList.GetValue(4,i).ToString();
					if(vendIDList.GetValue(5,i) != null)
						this.State.Text = vendIDList.GetValue(5,i).ToString();
					if(vendIDList.GetValue(6,i) != null)
						this.ZIP.Text = vendIDList.GetValue(6,i).ToString();
				}
			}
		}

		private void ARAccount_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= glAcctIDList.GetUpperBound(1); i++)
			{
				if(glAcctIDList.GetValue(0,i).ToString() == this.ARAccount.Text)
				{
					this.arAcctDesc.Text = glAcctIDList.GetValue(1,i).ToString();
				}
			}
		}


		private void itemID1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID1.Text)
				{
					this.Desc1.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice1.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID2.Text)
				{
					this.Desc2.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice2.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID3.Text)
				{
					this.Desc3.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice3.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID4.Text)
				{
					this.Desc4.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice4.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void itemID5_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			for(int i = 0; i <= itemIDList.GetUpperBound(1);i++)
			{
				if(itemIDList.GetValue(0,i).ToString() == this.itemID5.Text)
				{
					this.Desc5.Text = itemIDList.GetValue(1,i).ToString();
					this.unitprice5.Text = itemIDList.GetValue(2,i).ToString();
				}
			}
		}

		private void SaveButton_Click(object sender, System.EventArgs e)
		{
			CreateXMLFile();
			Importfile();
			ClearForm();
		}
		private void ClearForm()
		{
			foreach(Control ctrl in this.Controls)
			{
				if(ctrl.GetType().ToString() == "System.Windows.Forms.TextBox"
					|| ctrl.GetType().ToString() == "System.Windows.Forms.ComboBox")
				{
					ctrl.Text = "";
				}
			}
		}
		private void Importfile()
		{
			importer = (Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjPurchaseJournal);
			importer.ClearImportFieldList();
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_VendorId);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_VendorName);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Date);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_InvoiceNumber);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToAddressLine1);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToAddressLine2);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToCity);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToState);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToZip);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_APAccountId);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_APAmount);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_NumberOfDistributions);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Quantity);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ItemId);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Description);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_GLAccountId);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_UnitPrice);
			importer.AddToImportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Amount);
			importer.SetFilename(@"c:\XML\purchases.xml");
			importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			try
			{
				importer.Import();
			}
			catch(System.Exception e)
			{
				MessageBox.Show(e.Message);
			}		
		}
		private void CreateXMLFile()
		{
			XmlTextWriter Writer = new XmlTextWriter(@"c:\XML\purchases.xml",System.Text.Encoding.UTF8);
			Writer.WriteStartElement("PAW_Purchases");
			Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
			Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
			Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");
			Writer.WriteStartElement("PAW_Purchase");
			Writer.WriteAttributeString("xsi:type", "paw:Invoice");
			Writer.WriteStartElement("VendorID");
			Writer.WriteAttributeString("xsi:type", "paw:ID");
			Writer.WriteString(this.custVendID.Text);
			Writer.WriteEndElement();
			Writer.WriteElementString("VendorName",this.CustVendName.Text);
			Writer.WriteStartElement("Date");
			Writer.WriteAttributeString("xsi:type","paw:date");
			Writer.WriteString(this.invoiceDate.Text);
			Writer.WriteEndElement();	
			Writer.WriteElementString("Invoice_Number",this.invNum.Text);
			Writer.WriteElementString("Line1",this.Add1.Text);
			Writer.WriteElementString("Line2",this.Add2.Text);
			Writer.WriteElementString("City",this.City.Text);
			Writer.WriteElementString("State",this.State.Text);
			Writer.WriteElementString("Zip",this.State.Text);
			Writer.WriteStartElement("AP_Account");
			Writer.WriteAttributeString("xsi:type","paw:ID");
			Writer.WriteString(this.ARAccount.Text.ToString());
			Writer.WriteEndElement();
			Writer.WriteElementString("AP_Amount",this.invtotal.Text);
			Writer.WriteElementString("Number_of_Distributions","1");
			Writer.WriteStartElement("PurchaseLines");

			if(this.qty1.Text != "" && this.itemID1.Text != "" && this.Desc1.Text != "" &&
				this.glacct1.Text != "" && this.unitprice1.Text != "" && this.amount1.Text != "")
			{

				Writer.WriteStartElement("PurchaseLine");
				if(this.qty1.Text != "")
					Writer.WriteElementString("Quantity",this.qty1.Text);
				if(this.itemID1.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID1.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc1.Text != "")
					Writer.WriteElementString("Description",this.Desc1.Text);
				if(this.glacct1.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct1.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice1.Text != "")
					Writer.WriteElementString("Unit_Price",this.unitprice1.Text);
				if(this.amount1.Text != "")
					Writer.WriteElementString("Amount",this.amount1.Text);

				Writer.WriteEndElement();//closes the sales line element
			}

			if(this.qty2.Text != "" && this.itemID2.Text != "" && this.Desc2.Text != "" &&
				this.glacct2.Text != "" && this.unitprice2.Text != "" && this.amount2.Text != "")
			{

				Writer.WriteStartElement("PurchaseLine");
				if(this.qty1.Text != "")
					Writer.WriteElementString("Quantity",this.qty2.Text);
				if(this.itemID1.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID2.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc1.Text != "")
					Writer.WriteElementString("Description",this.Desc2.Text);
				if(this.glacct1.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct2.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice1.Text != "")
					Writer.WriteElementString("Unit_Price",this.unitprice2.Text);
				if(this.amount1.Text != "")
					Writer.WriteElementString("Amount",this.amount2.Text);

				Writer.WriteEndElement();//closes the sales line element
			}
			if(this.qty3.Text != "" && this.itemID3.Text != "" && this.Desc3.Text != "" &&
				this.glacct3.Text != "" && this.unitprice3.Text != "" && this.amount3.Text != "")
			{

				Writer.WriteStartElement("PurchaseLine");
				if(this.qty1.Text != "")
					Writer.WriteElementString("Quantity",this.qty3.Text);
				if(this.itemID1.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID3.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc1.Text != "")
					Writer.WriteElementString("Description",this.Desc3.Text);
				if(this.glacct1.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct3.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice1.Text != "")
					Writer.WriteElementString("Unit_Price",this.unitprice3.Text);
				if(this.amount1.Text != "")
					Writer.WriteElementString("Amount",this.amount3.Text);

				Writer.WriteEndElement();//closes the sales line element
			}
			if(this.qty4.Text != "" && this.itemID4.Text != "" && this.Desc4.Text != "" &&
				this.glacct4.Text != "" && this.unitprice4.Text != "" && this.amount4.Text != "")
			{

				Writer.WriteStartElement("PurchaseLine");
				if(this.qty1.Text != "")
					Writer.WriteElementString("Quantity",this.qty4.Text);
				if(this.itemID1.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID4.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc1.Text != "")
					Writer.WriteElementString("Description",this.Desc4.Text);
				if(this.glacct1.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct4.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice1.Text != "")
					Writer.WriteElementString("Unit_Price",this.unitprice4.Text);
				if(this.amount1.Text != "")
					Writer.WriteElementString("Amount",this.amount4.Text);

				Writer.WriteEndElement();//closes the sales line element
			}
			if(this.qty5.Text != "" && this.itemID5.Text != "" && this.Desc5.Text != "" &&
				this.glacct5.Text != "" && this.unitprice5.Text != "" && this.amount5.Text != "")
			{

				Writer.WriteStartElement("PurchaseLine");
				if(this.qty1.Text != "")
					Writer.WriteElementString("Quantity",this.qty5.Text);
				if(this.itemID1.Text != "")
				{
					Writer.WriteStartElement("Item_ID");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.itemID5.Text);
					Writer.WriteEndElement();
				}
				if(this.Desc1.Text != "")
					Writer.WriteElementString("Description",this.Desc5.Text);
				if(this.glacct1.Text != "")
				{
					Writer.WriteStartElement("GL_Account");
					Writer.WriteAttributeString("xsi:type", "paw:ID");
					Writer.WriteString(this.glacct5.Text);
					Writer.WriteEndElement();
				}
				if(this.unitprice1.Text != "")
					Writer.WriteElementString("Unit_Price",this.unitprice5.Text);
				if(this.amount1.Text != "")
					Writer.WriteElementString("Amount",this.amount5.Text);

				Writer.WriteEndElement();//closes the sales line element
			}
			Writer.WriteEndElement();//Closes the Sales Lines element

			Writer.WriteEndElement();//Closes the paw_invoice element
			
			Writer.WriteEndElement();//closes the paw_invoices element and ends the document
			
			Writer.Close();
		}
		private void qty1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}
		private void itemID1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}
		private void unitprice1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}
		private void amount1_leave(object sender, EventArgs e)
		{
			calcLine(amount1,qty1,unitprice1);
			CalcInvoice();
		}

		private void qty2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void itemID2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void unitprice2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void amount2_leave(object sender, EventArgs e)
		{
			calcLine(amount2,qty2,unitprice2);
			CalcInvoice();
		}
		private void qty3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void itemID3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void unitprice3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void amount3_leave(object sender, EventArgs e)
		{
			calcLine(amount3,qty3,unitprice3);
			CalcInvoice();
		}
		private void qty4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void itemID4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void unitprice4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void amount4_leave(object sender, EventArgs e)
		{
			calcLine(amount4,qty4,unitprice4);
			CalcInvoice();
		}
		private void qty5_leave(object sender, EventArgs e)
		{
			calcLine(amount5,qty5,unitprice5);
			CalcInvoice();
		}
		private void itemID5_leave(object sender, EventArgs e)
		{
			calcLine(amount5,qty5,unitprice5);
			CalcInvoice();
		}
		private void unitprice5_leave(object sender, EventArgs e)
		{
			if(Convert.ToDouble(qty5.Text) != 0)
			{
				calcLine(amount5,qty5,unitprice5);
				CalcInvoice();
			}
		}
		private void amount5_leave(object sender, EventArgs e)
		{
			calcLine(amount5,qty5,unitprice5);
			CalcInvoice();
		}
		private void calcLine(TextBox amount,TextBox qty,TextBox unitPrice)
		{
			double amt = 0;
			double units = 0;
			double uprice = 0;
        
			if(amount.Text != "")
				amt = Convert.ToDouble(amount.Text);

			if (qty.Text != "")
				units = Convert.ToDouble(qty.Text);
        
			if (unitPrice.Text != "")
				uprice = Convert.ToDouble(unitPrice.Text);
        
			if (amt == 0 && uprice != 0 && units != 0)
				amt = units * uprice;
			else if (amt != 0 && uprice != 0 && units == 0)
				units = amt / uprice;
			else if (amt != 0 && units != 0 && uprice == 0)
				uprice = amt / units;
			else if (amt != 0 && units != 0 && uprice != 0 && 
				amt != units * uprice)
				amt = units * uprice;
        
			if (amt != 0)
				amount.Text = amt.ToString("#,##0.00");

			if (units != 0)
				qty.Text = units.ToString("#,##0.00000");
        
			if (uprice != 0)
				unitPrice.Text = uprice.ToString("#,##0.00000");
        
			CalcInvoice();

		}
		private void CalcInvoice()
		{
			double total = 0;
			foreach(Control ctrl in this.Controls)
			{
				if(ctrl.Name.StartsWith("amount"))
				{
					if (ctrl.Text != "")
					{
						total += Convert.ToDouble(ctrl.Text);
					}
				}
			}
			invtotal.Text = total.ToString("#,##0.00");
		}
	}
}
