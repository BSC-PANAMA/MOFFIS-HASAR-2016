using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Interop.PeachwServer;

namespace CSSDK
{
	public class frmInvoices : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Label lblCustVendID;
		public System.Windows.Forms.Label lblCustVendName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox custVendID;
		private System.Windows.Forms.TextBox CustVendName;
		private System.Windows.Forms.TextBox Add1;
		private System.Windows.Forms.TextBox Add2;
		private System.Windows.Forms.TextBox City;
		private System.Windows.Forms.TextBox State;
		private System.Windows.Forms.ComboBox invNumDD;
		private System.Windows.Forms.TextBox ZIP;
		private System.Windows.Forms.TextBox invoiceDate;
		private System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox Subtotal;
		public System.Windows.Forms.TextBox TAX;
		private System.Windows.Forms.TextBox InvoiceTotal;
		private System.Windows.Forms.TextBox AmountPaid;
		private System.Windows.Forms.TextBox BalanceDue;
		public System.Windows.Forms.Label lblSubTotal;
		public System.Windows.Forms.Label lblSalesTax;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ColumnHeader itemID;
		private System.Windows.Forms.ColumnHeader Description;
		private System.Windows.Forms.ColumnHeader UnitPrice;
		private System.Windows.Forms.ColumnHeader amt;
		private System.Windows.Forms.ColumnHeader blankcolumn;
		private System.Windows.Forms.ColumnHeader qty;
		private Connect ptApp = new Connect();
        private Interop.PeachwServer.Export exporter;
		private XmlImplementation imp;
		private XmlDocument doc;
		private XmlNodeList reader;
		private XmlNodeList saleslinereader;
		private string[,] invarray;
		private AccountingPeriods ap = new AccountingPeriods();
		private System.Windows.Forms.ListView invDetails;
		private int taxlines = 0;
		private double POA;
		public System.Windows.Forms.Button button1;
        private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmInvoices()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
            this.lblCustVendID = new System.Windows.Forms.Label();
            this.lblCustVendName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.custVendID = new System.Windows.Forms.TextBox();
            this.CustVendName = new System.Windows.Forms.TextBox();
            this.Add1 = new System.Windows.Forms.TextBox();
            this.Add2 = new System.Windows.Forms.TextBox();
            this.City = new System.Windows.Forms.TextBox();
            this.State = new System.Windows.Forms.TextBox();
            this.invNumDD = new System.Windows.Forms.ComboBox();
            this.invDetails = new System.Windows.Forms.ListView();
            this.blankcolumn = new System.Windows.Forms.ColumnHeader();
            this.qty = new System.Windows.Forms.ColumnHeader();
            this.itemID = new System.Windows.Forms.ColumnHeader();
            this.Description = new System.Windows.Forms.ColumnHeader();
            this.UnitPrice = new System.Windows.Forms.ColumnHeader();
            this.amt = new System.Windows.Forms.ColumnHeader();
            this.ZIP = new System.Windows.Forms.TextBox();
            this.invoiceDate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Subtotal = new System.Windows.Forms.TextBox();
            this.TAX = new System.Windows.Forms.TextBox();
            this.InvoiceTotal = new System.Windows.Forms.TextBox();
            this.AmountPaid = new System.Windows.Forms.TextBox();
            this.BalanceDue = new System.Windows.Forms.TextBox();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblSalesTax = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCustVendID
            // 
            this.lblCustVendID.Location = new System.Drawing.Point(6, 76);
            this.lblCustVendID.Name = "lblCustVendID";
            this.lblCustVendID.Size = new System.Drawing.Size(83, 21);
            this.lblCustVendID.TabIndex = 0;
            this.lblCustVendID.Text = "ID";
            this.lblCustVendID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCustVendName
            // 
            this.lblCustVendName.Location = new System.Drawing.Point(272, 6);
            this.lblCustVendName.Name = "lblCustVendName";
            this.lblCustVendName.Size = new System.Drawing.Size(80, 20);
            this.lblCustVendName.TabIndex = 1;
            this.lblCustVendName.Text = "Name";
            this.lblCustVendName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(272, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(272, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Address";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "Invoice Number";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "Invoice Date";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // custVendID
            // 
            this.custVendID.Location = new System.Drawing.Point(93, 76);
            this.custVendID.Name = "custVendID";
            this.custVendID.Size = new System.Drawing.Size(100, 20);
            this.custVendID.TabIndex = 7;
            // 
            // CustVendName
            // 
            this.CustVendName.Location = new System.Drawing.Point(355, 5);
            this.CustVendName.Name = "CustVendName";
            this.CustVendName.Size = new System.Drawing.Size(292, 20);
            this.CustVendName.TabIndex = 8;
            // 
            // Add1
            // 
            this.Add1.Location = new System.Drawing.Point(355, 29);
            this.Add1.Name = "Add1";
            this.Add1.Size = new System.Drawing.Size(292, 20);
            this.Add1.TabIndex = 9;
            // 
            // Add2
            // 
            this.Add2.Location = new System.Drawing.Point(355, 53);
            this.Add2.Name = "Add2";
            this.Add2.Size = new System.Drawing.Size(292, 20);
            this.Add2.TabIndex = 10;
            // 
            // City
            // 
            this.City.Location = new System.Drawing.Point(355, 77);
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(223, 20);
            this.City.TabIndex = 11;
            // 
            // State
            // 
            this.State.Location = new System.Drawing.Point(582, 77);
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(24, 20);
            this.State.TabIndex = 12;
            // 
            // invNumDD
            // 
            this.invNumDD.Location = new System.Drawing.Point(93, 28);
            this.invNumDD.Name = "invNumDD";
            this.invNumDD.Size = new System.Drawing.Size(102, 21);
            this.invNumDD.TabIndex = 13;
            this.invNumDD.SelectedIndexChanged += new System.EventHandler(this.invNumDD_SelectedIndexChanged);
            // 
            // invDetails
            // 
            this.invDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.blankcolumn,
            this.qty,
            this.itemID,
            this.Description,
            this.UnitPrice,
            this.amt});
            this.invDetails.FullRowSelect = true;
            this.invDetails.GridLines = true;
            this.invDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.invDetails.HideSelection = false;
            this.invDetails.HoverSelection = true;
            this.invDetails.LabelWrap = false;
            this.invDetails.Location = new System.Drawing.Point(8, 109);
            this.invDetails.MultiSelect = false;
            this.invDetails.Name = "invDetails";
            this.invDetails.Size = new System.Drawing.Size(639, 128);
            this.invDetails.TabIndex = 14;
            this.invDetails.UseCompatibleStateImageBehavior = false;
            this.invDetails.View = System.Windows.Forms.View.Details;
            // 
            // blankcolumn
            // 
            this.blankcolumn.Text = "";
            this.blankcolumn.Width = 0;
            // 
            // qty
            // 
            this.qty.Text = "Quantity";
            this.qty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.qty.Width = 56;
            // 
            // itemID
            // 
            this.itemID.Text = "Item";
            this.itemID.Width = 57;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 376;
            // 
            // UnitPrice
            // 
            this.UnitPrice.Text = "Unit Price";
            this.UnitPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.UnitPrice.Width = 63;
            // 
            // amt
            // 
            this.amt.Text = "Amount";
            this.amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amt.Width = 58;
            // 
            // ZIP
            // 
            this.ZIP.Location = new System.Drawing.Point(606, 77);
            this.ZIP.Name = "ZIP";
            this.ZIP.Size = new System.Drawing.Size(40, 20);
            this.ZIP.TabIndex = 15;
            // 
            // invoiceDate
            // 
            this.invoiceDate.Location = new System.Drawing.Point(93, 52);
            this.invoiceDate.Name = "invoiceDate";
            this.invoiceDate.Size = new System.Drawing.Size(100, 20);
            this.invoiceDate.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(272, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "City, State, ZIP";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Subtotal
            // 
            this.Subtotal.Location = new System.Drawing.Point(546, 243);
            this.Subtotal.Name = "Subtotal";
            this.Subtotal.Size = new System.Drawing.Size(100, 20);
            this.Subtotal.TabIndex = 18;
            this.Subtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TAX
            // 
            this.TAX.Location = new System.Drawing.Point(546, 265);
            this.TAX.Name = "TAX";
            this.TAX.Size = new System.Drawing.Size(100, 20);
            this.TAX.TabIndex = 19;
            this.TAX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // InvoiceTotal
            // 
            this.InvoiceTotal.Location = new System.Drawing.Point(546, 287);
            this.InvoiceTotal.Name = "InvoiceTotal";
            this.InvoiceTotal.Size = new System.Drawing.Size(100, 20);
            this.InvoiceTotal.TabIndex = 20;
            this.InvoiceTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AmountPaid
            // 
            this.AmountPaid.Location = new System.Drawing.Point(546, 309);
            this.AmountPaid.Name = "AmountPaid";
            this.AmountPaid.Size = new System.Drawing.Size(100, 20);
            this.AmountPaid.TabIndex = 21;
            this.AmountPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BalanceDue
            // 
            this.BalanceDue.Location = new System.Drawing.Point(546, 331);
            this.BalanceDue.Name = "BalanceDue";
            this.BalanceDue.Size = new System.Drawing.Size(100, 20);
            this.BalanceDue.TabIndex = 22;
            this.BalanceDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Location = new System.Drawing.Point(442, 243);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(100, 20);
            this.lblSubTotal.TabIndex = 23;
            this.lblSubTotal.Text = "Sub-Total";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalesTax
            // 
            this.lblSalesTax.Location = new System.Drawing.Point(442, 265);
            this.lblSalesTax.Name = "lblSalesTax";
            this.lblSalesTax.Size = new System.Drawing.Size(100, 20);
            this.lblSalesTax.TabIndex = 24;
            this.lblSalesTax.Text = "Sales Tax";
            this.lblSalesTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(442, 287);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "Invoice Total";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(442, 309);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 20);
            this.label11.TabIndex = 26;
            this.label11.Text = "Amount Paid";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(442, 331);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 20);
            this.label12.TabIndex = 27;
            this.label12.Text = "Balance Due";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(167, 260);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 69);
            this.button1.TabIndex = 28;
            this.button1.Text = "Delete Invoice";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Select in the drop list current Invoices";
            // 
            // frmInvoices
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(655, 358);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblSalesTax);
            this.Controls.Add(this.lblSubTotal);
            this.Controls.Add(this.BalanceDue);
            this.Controls.Add(this.AmountPaid);
            this.Controls.Add(this.InvoiceTotal);
            this.Controls.Add(this.TAX);
            this.Controls.Add(this.Subtotal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.invoiceDate);
            this.Controls.Add(this.ZIP);
            this.Controls.Add(this.invDetails);
            this.Controls.Add(this.invNumDD);
            this.Controls.Add(this.State);
            this.Controls.Add(this.City);
            this.Controls.Add(this.Add2);
            this.Controls.Add(this.Add1);
            this.Controls.Add(this.CustVendName);
            this.Controls.Add(this.custVendID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCustVendName);
            this.Controls.Add(this.lblCustVendID);
            this.Name = "frmInvoices";
            this.Text = "Customer Invoices";
            this.Load += new System.EventHandler(this.frmInvoices_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void frmInvoices_Load(object sender, System.EventArgs e)
		{
			fillInvoiceList();
		}
		private void fillInvoiceList()
		{
			AccountingPeriods ap = new AccountingPeriods();
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesJournal);
			
			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
			//exporter.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange,DateTime.Parse(ap.getFirstOpenDay().ToString("MM/dd/yyyy")),DateTime.Parse(ap.getLastOpenDay().ToString("MM/dd/yyyy")));
			exporter.SetFilename(@"c:\XML\sales.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\sales.xml");
			reader = doc.GetElementsByTagName("PAW_Invoice");
			invarray = new string[2,reader.Count];
			int skippedrecords = 0;
			this.invNumDD.Items.Clear();
			this.invNumDD.Text = "";
			for(int i = 0;i <= reader.Count -1;i++)
			{
				if(reader[i].ChildNodes.Count > 1)
				{
					this.invNumDD.Items.Add(reader[i].ChildNodes[1].InnerText);
					invarray.SetValue(reader[i].ChildNodes[0].InnerText,0,i - skippedrecords);
					invarray.SetValue(reader[i].ChildNodes[1].InnerText,1,i - skippedrecords);
				}
				else
					skippedrecords += 1;
			}

			doc = null;
			imp = null;
			reader = null;
		}

		private void invNumDD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
			clearform();
			int recindex = this.invNumDD.SelectedIndex;
			taxlines = 0;
			
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjSalesJournal);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerId);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_CustomerName);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_InvoiceNumber);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Date);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quote);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_DropShip);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine1);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToAddressLine2);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToCity);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToState);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ShipToZip);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAccountId);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ARAmount);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_BeginningBalanceTransaction);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_IsCreditMemo);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_NumberOfDistributions);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesOrderDistNum);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Quantity);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_ItemId);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Description);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_GLAccountId);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_UnitPrice);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_TaxType);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_Amount);
			exporter.AddToExportFieldList((short)PeachwIEObjSalesJournalField.peachwIEObjSalesJournalField_SalesTaxAuthority);
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.SetFilename(@"c:\XML\sales.xml");
			exporter.SetFilterValue((short)PeachwIEObjSalesJournalFilter.peachwIEObjSalesJournalFilter_InvoiceNumber,PeachwIEFilterOperation.peachwIEFilterOperationRange, invarray.GetValue(1,recindex).ToString(), invarray.GetValue(1,recindex).ToString());
			exporter.SetFilterValue((short)PeachwIEObjSalesJournalFilter.peachwIEObjSalesJournalFilter_CustomerId,PeachwIEFilterOperation.peachwIEFilterOperationRange,	invarray.GetValue(0,recindex).ToString(), invarray.GetValue(0,recindex).ToString());
			exporter.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange,ap.getFirstOpenDay(), ap.getLastOpenDay());
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\sales.xml");
			reader = doc.GetElementsByTagName("PAW_Invoice");
			saleslinereader = doc.GetElementsByTagName("Sales_Lines");

			foreach(XmlNode node in reader[0])
			{
				switch(node.Name)
				{
					case "Date":
						this.invoiceDate.Text = node.InnerText;
						break;
					case "Customer_ID":
						this.custVendID.Text = node.InnerText;
						break;
					case "Customer_Name":
						this.CustVendName.Text = node.InnerText;
						break;
					case "Line1":
						this.Add1.Text = node.InnerText;
						break;
					case "Line2":
						this.Add2.Text = node.InnerText;
						break;
					case "City":
						this.City.Text = node.InnerText;
						break;
					case "State":
						this.State.Text = node.InnerText;
						break;
					case "Zip":
						this.ZIP.Text = node.InnerText;
						break;
					case "Accounts_Receivable_Amount":
						this.InvoiceTotal.Text = Convert.ToSingle(node.InnerText).ToString("#,##0.00");
						break;
					case "SalesLines":
						int rowcount = 0;
						XmlNodeList taxAmount = doc.GetElementsByTagName("Amount");
						XmlNodeList salesLine = doc.GetElementsByTagName("SalesLine");
						for(int s = 0; s <= salesLine.Count -1;s++)
						{
							for(int a = 0;a <= salesLine[s].ChildNodes.Count -1;a++)
							{
								if(salesLine[s].LastChild.Name == "Sales_Tax_Authority")
								{
									if(this.TAX.Text != "")
										this.TAX.Text = (Convert.ToDouble(this.TAX.Text) + (Convert.ToSingle(taxAmount[s].InnerText)* -1)).ToString("#,##0.00");
									else
										this.TAX.Text = (Convert.ToDouble(taxAmount[s].InnerText)*-1).ToString("#,##0.00");
									taxlines += (a + 1);
									break;
								}
								else
								{
									rowcount += 1;
									if(salesLine[s].ChildNodes[a].Name == "Quantity")
									{																											
										this.invDetails.Items.Add("");
										this.invDetails.Items[s-taxlines].SubItems.AddRange(new string[]{"","","","",""});
									}
									switch(salesLine[s].ChildNodes[a].Name)
									{
										case "Quantity":
											this.invDetails.Items[s-taxlines].SubItems[1].Text = salesLine[s].ChildNodes[a].InnerText;
											break;
										case "ItemID":
											this.invDetails.Items[s-taxlines].SubItems[2].Text = salesLine[s].ChildNodes[a].InnerText;
											break;
										case "Description":
											this.invDetails.Items[s-taxlines].SubItems[3].Text = salesLine[s].ChildNodes[a].InnerText;
											break;
										case "Unit_Price":
											this.invDetails.Items[s-taxlines].SubItems[4].Text = salesLine[s].ChildNodes[a].InnerText;
											break;
										case "Amount":
											this.invDetails.Items[s-taxlines].SubItems[5].Text = (Convert.ToSingle(salesLine[s].ChildNodes[a].InnerText)*-1).ToString("#,##0.00");
											break;
									}
								}
							}
						}
						break;
				}
			}
			this.Subtotal.Text = calcInv();
			this.AmountPaid.Text = GetAmtPaid().ToString();
			this.BalanceDue.Text = (Convert.ToSingle(this.InvoiceTotal.Text) - Convert.ToSingle(this.AmountPaid.Text)).ToString("#,##0.00");
            this.Cursor = Cursors.Default;
		}
		private void clearform()
		{
			this.invDetails.Items.Clear();
			foreach(Control ctrl in this.Controls)
			{
				if(ctrl.GetType().ToString() == "System.Windows.Forms.TextBox")
					ctrl.Text = "";
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Delete delinvoice = new Delete();
			string[] recToDel = new string[2];
			recToDel[0] = this.custVendID.Text;
			recToDel[1] = this.invNumDD.Text;
			delinvoice.DeleteTransaction(PeachBusObjects.pboSalesEntry,PeachObjectKey.pboKey_ByCustomerIDByNumber,ref recToDel);
			clearform();
			fillInvoiceList();

		}
		private string calcInv()
		{
			Single invsubtot = 0;
			foreach(ListViewItem row in invDetails.Items)
			{
				invsubtot += Convert.ToSingle(row.SubItems[5].Text);
			}
			return invsubtot.ToString("#,##0.00");
		}
		private string GetAmtPaid()
		{
			double amtPaid;
			bool Unpaid;
			ARAPInformation arinfo = new ARAPInformation();
			arinfo.ARUnpaidInv(this.custVendID.Text,this.invNumDD.Text,out amtPaid,out Unpaid);
			if(Unpaid == false)
				POA = Convert.ToDouble(this.InvoiceTotal.Text);
			else
				POA = amtPaid;
																			 

			return POA.ToString("#,##0.00");
		}


	}

	public class frmPurchases : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Label lblCustVendID;
		public System.Windows.Forms.Label lblCustVendName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox custVendID;
		private System.Windows.Forms.TextBox CustVendName;
		private System.Windows.Forms.TextBox Add1;
		private System.Windows.Forms.TextBox Add2;
		private System.Windows.Forms.TextBox City;
		private System.Windows.Forms.TextBox State;
		private System.Windows.Forms.ComboBox invNumDD;
		private System.Windows.Forms.TextBox ZIP;
		private System.Windows.Forms.TextBox invoiceDate;
		private System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox Subtotal;
		public System.Windows.Forms.TextBox TAX;
		private System.Windows.Forms.TextBox InvoiceTotal;
		private System.Windows.Forms.TextBox AmountPaid;
		private System.Windows.Forms.TextBox BalanceDue;
		public System.Windows.Forms.Label lblSubTotal;
		public System.Windows.Forms.Label lblSalesTax;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ColumnHeader itemID;
		private System.Windows.Forms.ColumnHeader Description;
		private System.Windows.Forms.ColumnHeader UnitPrice;
		private System.Windows.Forms.ColumnHeader amt;
		private System.Windows.Forms.ColumnHeader blankcolumn;
		private System.Windows.Forms.ColumnHeader qty;
		private Connect ptApp = new Connect();
        private Interop.PeachwServer.Export exporter;
		private XmlImplementation imp;
		private XmlDocument doc;
		private XmlNodeList reader;
		private XmlNodeList Purchaselinereader;
		private string[,] invarray;
		private AccountingPeriods ap = new AccountingPeriods();
		private System.Windows.Forms.ListView invDetails;
		private double POA;
		public System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmPurchases()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			this.lblCustVendID = new System.Windows.Forms.Label();
			this.lblCustVendName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.custVendID = new System.Windows.Forms.TextBox();
			this.CustVendName = new System.Windows.Forms.TextBox();
			this.Add1 = new System.Windows.Forms.TextBox();
			this.Add2 = new System.Windows.Forms.TextBox();
			this.City = new System.Windows.Forms.TextBox();
			this.State = new System.Windows.Forms.TextBox();
			this.invNumDD = new System.Windows.Forms.ComboBox();
			this.invDetails = new System.Windows.Forms.ListView();
			this.blankcolumn = new System.Windows.Forms.ColumnHeader();
			this.qty = new System.Windows.Forms.ColumnHeader();
			this.itemID = new System.Windows.Forms.ColumnHeader();
			this.Description = new System.Windows.Forms.ColumnHeader();
			this.UnitPrice = new System.Windows.Forms.ColumnHeader();
			this.amt = new System.Windows.Forms.ColumnHeader();
			this.ZIP = new System.Windows.Forms.TextBox();
			this.invoiceDate = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.Subtotal = new System.Windows.Forms.TextBox();
			this.TAX = new System.Windows.Forms.TextBox();
			this.InvoiceTotal = new System.Windows.Forms.TextBox();
			this.AmountPaid = new System.Windows.Forms.TextBox();
			this.BalanceDue = new System.Windows.Forms.TextBox();
			this.lblSubTotal = new System.Windows.Forms.Label();
			this.lblSalesTax = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblCustVendID
			// 
			this.lblCustVendID.Location = new System.Drawing.Point(6, 56);
			this.lblCustVendID.Name = "lblCustVendID";
			this.lblCustVendID.Size = new System.Drawing.Size(83, 21);
			this.lblCustVendID.TabIndex = 0;
			this.lblCustVendID.Text = "ID";
			this.lblCustVendID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustVendName
			// 
			this.lblCustVendName.Location = new System.Drawing.Point(272, 6);
			this.lblCustVendName.Name = "lblCustVendName";
			this.lblCustVendName.Size = new System.Drawing.Size(80, 20);
			this.lblCustVendName.TabIndex = 1;
			this.lblCustVendName.Text = "Name";
			this.lblCustVendName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(272, 29);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Address";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(272, 53);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 20);
			this.label4.TabIndex = 3;
			this.label4.Text = "Address";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 21);
			this.label5.TabIndex = 4;
			this.label5.Text = "Invoice Number";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(83, 21);
			this.label6.TabIndex = 5;
			this.label6.Text = "Invoice Date";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// custVendID
			// 
			this.custVendID.Location = new System.Drawing.Point(93, 56);
			this.custVendID.Name = "custVendID";
			this.custVendID.TabIndex = 7;
			this.custVendID.Text = "";
			// 
			// CustVendName
			// 
			this.CustVendName.Location = new System.Drawing.Point(355, 5);
			this.CustVendName.Name = "CustVendName";
			this.CustVendName.Size = new System.Drawing.Size(292, 20);
			this.CustVendName.TabIndex = 8;
			this.CustVendName.Text = "";
			// 
			// Add1
			// 
			this.Add1.Location = new System.Drawing.Point(355, 29);
			this.Add1.Name = "Add1";
			this.Add1.Size = new System.Drawing.Size(292, 20);
			this.Add1.TabIndex = 9;
			this.Add1.Text = "";
			// 
			// Add2
			// 
			this.Add2.Location = new System.Drawing.Point(355, 53);
			this.Add2.Name = "Add2";
			this.Add2.Size = new System.Drawing.Size(292, 20);
			this.Add2.TabIndex = 10;
			this.Add2.Text = "";
			// 
			// City
			// 
			this.City.Location = new System.Drawing.Point(355, 77);
			this.City.Name = "City";
			this.City.Size = new System.Drawing.Size(223, 20);
			this.City.TabIndex = 11;
			this.City.Text = "";
			// 
			// State
			// 
			this.State.Location = new System.Drawing.Point(582, 77);
			this.State.Name = "State";
			this.State.Size = new System.Drawing.Size(24, 20);
			this.State.TabIndex = 12;
			this.State.Text = "";
			// 
			// invNumDD
			// 
			this.invNumDD.Location = new System.Drawing.Point(93, 8);
			this.invNumDD.Name = "invNumDD";
			this.invNumDD.Size = new System.Drawing.Size(102, 21);
			this.invNumDD.TabIndex = 13;
			this.invNumDD.SelectedIndexChanged += new System.EventHandler(this.invNumDD_SelectedIndexChanged);
			// 
			// invDetails
			// 
			this.invDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						 this.blankcolumn,
																						 this.qty,
																						 this.itemID,
																						 this.Description,
																						 this.UnitPrice,
																						 this.amt});
			this.invDetails.FullRowSelect = true;
			this.invDetails.GridLines = true;
			this.invDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.invDetails.HideSelection = false;
			this.invDetails.HoverSelection = true;
			this.invDetails.LabelWrap = false;
			this.invDetails.Location = new System.Drawing.Point(8, 109);
			this.invDetails.MultiSelect = false;
			this.invDetails.Name = "invDetails";
			this.invDetails.Size = new System.Drawing.Size(639, 128);
			this.invDetails.TabIndex = 14;
			this.invDetails.View = System.Windows.Forms.View.Details;
			// 
			// blankcolumn
			// 
			this.blankcolumn.Text = "";
			this.blankcolumn.Width = 0;
			// 
			// qty
			// 
			this.qty.Text = "Quantity";
			this.qty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.qty.Width = 56;
			// 
			// itemID
			// 
			this.itemID.Text = "Item";
			this.itemID.Width = 57;
			// 
			// Description
			// 
			this.Description.Text = "Description";
			this.Description.Width = 376;
			// 
			// UnitPrice
			// 
			this.UnitPrice.Text = "Unit Price";
			this.UnitPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.UnitPrice.Width = 63;
			// 
			// amt
			// 
			this.amt.Text = "Amount";
			this.amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.amt.Width = 58;
			// 
			// ZIP
			// 
			this.ZIP.Location = new System.Drawing.Point(606, 77);
			this.ZIP.Name = "ZIP";
			this.ZIP.Size = new System.Drawing.Size(40, 20);
			this.ZIP.TabIndex = 15;
			this.ZIP.Text = "";
			// 
			// invoiceDate
			// 
			this.invoiceDate.Location = new System.Drawing.Point(93, 32);
			this.invoiceDate.Name = "invoiceDate";
			this.invoiceDate.TabIndex = 16;
			this.invoiceDate.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(272, 77);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 20);
			this.label7.TabIndex = 17;
			this.label7.Text = "City, State, ZIP";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Subtotal
			// 
			this.Subtotal.Location = new System.Drawing.Point(546, 243);
			this.Subtotal.Name = "Subtotal";
			this.Subtotal.TabIndex = 18;
			this.Subtotal.Text = "";
			this.Subtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TAX
			// 
			this.TAX.Location = new System.Drawing.Point(546, 265);
			this.TAX.Name = "TAX";
			this.TAX.TabIndex = 19;
			this.TAX.Text = "";
			this.TAX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// InvoiceTotal
			// 
			this.InvoiceTotal.Location = new System.Drawing.Point(546, 287);
			this.InvoiceTotal.Name = "InvoiceTotal";
			this.InvoiceTotal.TabIndex = 20;
			this.InvoiceTotal.Text = "";
			this.InvoiceTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// AmountPaid
			// 
			this.AmountPaid.Location = new System.Drawing.Point(546, 309);
			this.AmountPaid.Name = "AmountPaid";
			this.AmountPaid.TabIndex = 21;
			this.AmountPaid.Text = "";
			this.AmountPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// BalanceDue
			// 
			this.BalanceDue.Location = new System.Drawing.Point(546, 331);
			this.BalanceDue.Name = "BalanceDue";
			this.BalanceDue.TabIndex = 22;
			this.BalanceDue.Text = "";
			this.BalanceDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblSubTotal
			// 
			this.lblSubTotal.Location = new System.Drawing.Point(442, 243);
			this.lblSubTotal.Name = "lblSubTotal";
			this.lblSubTotal.Size = new System.Drawing.Size(100, 20);
			this.lblSubTotal.TabIndex = 23;
			this.lblSubTotal.Text = "Sub-Total";
			this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSalesTax
			// 
			this.lblSalesTax.Location = new System.Drawing.Point(442, 265);
			this.lblSalesTax.Name = "lblSalesTax";
			this.lblSalesTax.Size = new System.Drawing.Size(100, 20);
			this.lblSalesTax.TabIndex = 24;
			this.lblSalesTax.Text = "Sales Tax";
			this.lblSalesTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(442, 287);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 20);
			this.label10.TabIndex = 25;
			this.label10.Text = "Invoice Total";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(442, 309);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(100, 20);
			this.label11.TabIndex = 26;
			this.label11.Text = "Amount Paid";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(442, 331);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(100, 20);
			this.label12.TabIndex = 27;
			this.label12.Text = "Balance Due";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(167, 260);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 69);
			this.button1.TabIndex = 28;
			this.button1.Text = "Delete Invoice";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmInvoices
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(655, 358);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.lblSalesTax);
			this.Controls.Add(this.lblSubTotal);
			this.Controls.Add(this.BalanceDue);
			this.Controls.Add(this.AmountPaid);
			this.Controls.Add(this.InvoiceTotal);
			this.Controls.Add(this.TAX);
			this.Controls.Add(this.Subtotal);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.invoiceDate);
			this.Controls.Add(this.ZIP);
			this.Controls.Add(this.invDetails);
			this.Controls.Add(this.invNumDD);
			this.Controls.Add(this.State);
			this.Controls.Add(this.City);
			this.Controls.Add(this.Add2);
			this.Controls.Add(this.Add1);
			this.Controls.Add(this.CustVendName);
			this.Controls.Add(this.custVendID);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblCustVendName);
			this.Controls.Add(this.lblCustVendID);
			this.Name = "frmInvoices";
			this.Text = "Vendor Invoices";
			this.Load += new System.EventHandler(this.frmInvoices_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmInvoices_Load(object sender, System.EventArgs e)
		{
			fillInvoiceList();
		}
		private void fillInvoiceList()
		{
			AccountingPeriods ap = new AccountingPeriods();
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjPurchaseJournal);
			
			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_InvoiceNumber);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_VendorId);
			exporter.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange,DateTime.Parse(ap.getFirstOpenDay().ToString("MM/dd/yyyy")),DateTime.Parse(ap.getLastOpenDay().ToString("MM/dd/yyyy")));
			exporter.SetFilename(@"c:\XML\sales.xml");
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\XML\sales.xml");
			reader = doc.GetElementsByTagName("PAW_Purchase");
			invarray = new string[2,reader.Count];
			int skippedrecords = 0;
			this.invNumDD.Items.Clear();
			this.invNumDD.Text = "";
			for(int i = 0;i <= reader.Count -1;i++)
			{
				if(reader[i].ChildNodes.Count > 1)
				{
					this.invNumDD.Items.Add(reader[i].ChildNodes[1].InnerText);
					invarray.SetValue(reader[i].ChildNodes[0].InnerText,0,i - skippedrecords);
					invarray.SetValue(reader[i].ChildNodes[1].InnerText,1,i - skippedrecords);
				}
				else
					skippedrecords += 1;
			}

			doc = null;
			imp = null;
			reader = null;
		}

		private void invNumDD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
			clearform();
			int recindex = this.invNumDD.SelectedIndex;
			
			exporter = (Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjPurchaseJournal);

			exporter.ClearExportFieldList();
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_VendorId);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_VendorName);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToAddressLine1);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToAddressLine2);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToCity);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToState);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ShipToZip);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_InvoiceNumber);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Date);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_DropShip);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_APAccountId);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_APAmount);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_BeginningBalanceTransaction);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_IsCreditMemo);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_NumberOfDistributions);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_PurchaseOrderDistNum);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Quantity);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_ItemId);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Description);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_GLAccountId);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_UnitPrice);
			exporter.AddToExportFieldList((short)PeachwIEObjPurchaseJournalField.peachwIEObjPurchaseJournalField_Amount);
			exporter.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			exporter.SetFilename(@"c:\purchases.xml");
			exporter.SetFilterValue((short)PeachwIEObjPurchaseJournalFilter.peachwIEObjPurchaseJournalFilter_InvoiceNumber,PeachwIEFilterOperation.peachwIEFilterOperationRange, invarray.GetValue(1,recindex).ToString(), invarray.GetValue(1,recindex).ToString());
			exporter.SetFilterValue((short)PeachwIEObjPurchaseJournalFilter.peachwIEObjPurchaseJournalFilter_VendorId,PeachwIEFilterOperation.peachwIEFilterOperationRange,	invarray.GetValue(0,recindex).ToString(), invarray.GetValue(0,recindex).ToString());
			exporter.SetDateFilterValue(PeachwIEDateFilterOperation.peachwIEDateFilterOperationRange,ap.getFirstOpenDay(), ap.getLastOpenDay());
			exporter.Export();

			imp = new XmlImplementation();
			doc = imp.CreateDocument();
			doc.Load(@"c:\purchases.xml");
			reader = doc.GetElementsByTagName("PAW_Purchase");
			Purchaselinereader = doc.GetElementsByTagName("Purchase_Lines");

			foreach(XmlNode node in reader[0])
			{
				switch(node.Name)
				{
					case "Date":
						this.invoiceDate.Text = node.InnerText;
						break;
					case "VendorID":
						this.custVendID.Text = node.InnerText;
						break;
					case "VendorName":
						this.CustVendName.Text = node.InnerText;
						break;
					case "Line1":
						this.Add1.Text = node.InnerText;
						break;
					case "Line2":
						this.Add2.Text = node.InnerText;
						break;
					case "City":
						this.City.Text = node.InnerText;
						break;
					case "State":
						this.State.Text = node.InnerText;
						break;
					case "Zip":
						this.ZIP.Text = node.InnerText;
						break;
					case "AP_Amount":
						this.InvoiceTotal.Text = (Convert.ToSingle(node.InnerText)*-1).ToString("#,##0.00");
						break;
					case "PurchaseLines":
						XmlNodeList taxAmount = doc.GetElementsByTagName("Amount");
						XmlNodeList purchaseLine = doc.GetElementsByTagName("PurchaseLine");
						for(int s = 0; s <= purchaseLine.Count -1;s++)
						{
							for(int a = 0;a <= purchaseLine[s].ChildNodes.Count -1;a++)
							{
								switch(purchaseLine[s].ChildNodes[a].Name)
								{
									case "Quantity":
										this.invDetails.Items.Add("");
										this.invDetails.Items[s].SubItems.AddRange(new string[]{"","","","",""});
										this.invDetails.Items[s].SubItems[1].Text = purchaseLine[s].ChildNodes[a].InnerText;
										break;
									case "ItemID":
										this.invDetails.Items[s].SubItems[2].Text = purchaseLine[s].ChildNodes[a].InnerText;
										break;
									case "Description":
										this.invDetails.Items[s].SubItems[3].Text = purchaseLine[s].ChildNodes[a].InnerText;
										break;
									case "Unit_Price":
										this.invDetails.Items[s].SubItems[4].Text = purchaseLine[s].ChildNodes[a].InnerText;
										break;
									case "Amount":
										this.invDetails.Items[s].SubItems[5].Text = Convert.ToSingle(purchaseLine[s].ChildNodes[a].InnerText).ToString("#,##0.00");
										break;
								}
									
							}
						}
						break;
				}
			}
			this.Subtotal.Text = calcInv();
			this.AmountPaid.Text = GetAmtPaid().ToString();
			this.BalanceDue.Text = (Convert.ToSingle(this.InvoiceTotal.Text) - Convert.ToSingle(this.AmountPaid.Text)).ToString("#,##0.00");

            this.Cursor = Cursors.Default;
		}
		private void clearform()
		{
			this.invDetails.Items.Clear();
			foreach(Control ctrl in this.Controls)
			{
				if(ctrl.GetType().ToString() == "System.Windows.Forms.TextBox")
					ctrl.Text = "";
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Delete delinvoice = new Delete();
			string[] recToDel = new string[2];
			recToDel[0] = this.custVendID.Text;
			recToDel[1] = this.invNumDD.Text;
			delinvoice.DeleteTransaction(PeachBusObjects.pboSalesEntry,PeachObjectKey.pboKey_ByCustomerIDByNumber,ref recToDel);
			clearform();
			fillInvoiceList();

		}
		private string calcInv()
		{
			Single invsubtot = 0;
			foreach(ListViewItem row in invDetails.Items)
			{
				invsubtot += Convert.ToSingle(row.SubItems[5].Text);
			}
			return invsubtot.ToString("#,##0.00");
		}
		private string GetAmtPaid()
		{
			double amtPaid;
			bool Unpaid;
			ARAPInformation arinfo = new ARAPInformation();
			arinfo.ARUnpaidInv(this.custVendID.Text,this.invNumDD.Text,out amtPaid,out Unpaid);
			if(Unpaid == false)
				POA = Convert.ToDouble(this.InvoiceTotal.Text);
			else
				POA = amtPaid;
																			 

			return POA.ToString("#,##0.00");
		}
	}

}
