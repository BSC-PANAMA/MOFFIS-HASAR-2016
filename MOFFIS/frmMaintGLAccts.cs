using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Interop.PeachwServer;
using System.Xml;




namespace MOFFIS
{
	public class frmMaintGLAccts : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		public System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.ComponentModel.Container components = null;
        public Interop.PeachwServer.Export exporter;
        private ConectarPT ptApp = new ConectarPT();
		public System.Windows.Forms.Button button1;
		public System.Windows.Forms.Button button2;
		public System.Windows.Forms.Button button3;
		Array coa;
        private Interop.PeachwServer.Import importer;

		public frmMaintGLAccts()
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Account ID";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 21);
			this.label2.TabIndex = 1;
			this.label2.Text = "Account Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 62);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(108, 21);
			this.label3.TabIndex = 2;
			this.label3.Text = "Account Type";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(118, 13);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(216, 21);
			this.comboBox1.TabIndex = 3;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(118, 37);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(215, 20);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "";
			// 
			// comboBox2
			// 
			this.comboBox2.Location = new System.Drawing.Point(118, 62);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(217, 21);
			this.comboBox2.TabIndex = 5;
			// 
			// checkBox1
			// 
			this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.Location = new System.Drawing.Point(59, 84);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(72, 24);
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "In Active";
			this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(353, 10);
			this.button1.Name = "button1";
			this.button1.TabIndex = 7;
			this.button1.Text = "Add";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(353, 37);
			this.button2.Name = "button2";
			this.button2.TabIndex = 8;
			this.button2.Text = "Modify";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(353, 64);
			this.button3.Name = "button3";
			this.button3.TabIndex = 9;
			this.button3.Text = "Delete";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// frmMaintGLAccts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(439, 112);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "frmMaintGLAccts";
			this.Text = "Maintain General Ledger Accounts";
			this.Load += new System.EventHandler(this.frmMaintGLAccts_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmMaintGLAccts_Load(object sender, System.EventArgs e)
		{
			exportList();
			fillIDlist();
			GLInformationsss acctType = new GLInformationsss();
			acctType.fillAcctTypeList(comboBox2);
		}
		private void exportList()
		{
            exporter = (Interop.PeachwServer.Export)ptApp.app.CreateExporter(PeachwIEObj.peachwIEObjChartOfAccounts);
			exporter.ClearExportFieldList();
            exporter.AddToExportFieldList((short)Interop.PeachwServer.PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerId);
            exporter.AddToExportFieldList((short)Interop.PeachwServer.PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerDescription);
            exporter.AddToExportFieldList((short)Interop.PeachwServer.PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Type);
            exporter.AddToExportFieldList((short)Interop.PeachwServer.PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Inactive);
            exporter.SetFileType(Interop.PeachwServer.PeachwIEFileType.peachwIEFileTypeXML);
			exporter.SetFilename("c:\\XML\\coa.xml");
			exporter.Export();
		}
		private void fillIDlist()
		{
			XmlImplementation imp = new XmlImplementation();
			XmlDocument doc = imp.CreateDocument();
			doc.Load("c:\\XML\\coa.xml");
			XmlNodeList reader = doc.GetElementsByTagName("PAW_Account");
			XmlNode node = reader[0];
			int aLength = reader.Count;

			coa = Array.CreateInstance(typeof(String),4,aLength);	
			for(int i = 0;i <= aLength -1;i++)
			{
				node = reader[i];
				this.comboBox1.Items.Add(node.ChildNodes[0].InnerText);
				coa.SetValue(node.ChildNodes[0].InnerText,0,i);
				coa.SetValue(node.ChildNodes[1].InnerText,1,i);
				coa.SetValue(node.ChildNodes[2].InnerText,2,i);
				coa.SetValue(node.ChildNodes[3].InnerText,3,i);
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			GLInformationsss acctTypes = new GLInformationsss();
			this.textBox1.Text = coa.GetValue(1,comboBox1.SelectedIndex).ToString();
			this.comboBox2.SelectedItem = acctTypes.getAcctTypeWords(Convert.ToInt32(coa.GetValue(2,comboBox1.SelectedIndex).ToString()));
			this.checkBox1.Checked = Convert.ToBoolean(coa.GetValue(3,comboBox1.SelectedIndex).ToString());
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			Delete delRec = new Delete();
			string[] recToDel = new string[1];
			recToDel[0] = this.comboBox1.SelectedItem.ToString();
            delRec.DeleteTransaction(Interop.PeachwServer.PeachBusObjects.pboAccount, Interop.PeachwServer.PeachObjectKey.pboKey_ByID, ref recToDel);
			clearForm();
		}
		private void clearForm()
		{
			this.comboBox1.Text = "";
			TextBox tb;
			ComboBox cb;
			foreach(Control txt in this.Controls)
			{
				if(txt.GetType().ToString() == "System.Windows.Forms.TextBox")
				{
					tb = (TextBox)txt;
					tb.Text = "";
				}
				if(txt.GetType().ToString() == "System.Windows.Forms.ComboBox")
				{
					cb = (ComboBox)txt;
					cb.Text = "";
				}
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			saveRecord();
		}
		private void saveRecord()
		{
			XmlTextWriter Writer = new XmlTextWriter(@"c:\XML\coa.xml",System.Text.Encoding.UTF8);
			
			Writer.WriteStartElement("PAW_Accounts");
			Writer.WriteAttributeString("xmlns:paw", "urn:schemas-peachtree-com/paw8.02-datatypes");
			Writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2000/10/XMLSchema-instance");
			Writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2000/10/XMLSchema-datatypes");

			Writer.WriteStartElement("PAW_Account");
			Writer.WriteAttributeString("xsi:type", "paw:account");

			Writer.WriteStartElement("ID");
			Writer.WriteAttributeString("xsi:type", "paw:ID");
			Writer.WriteString(this.comboBox1.Text);
			Writer.WriteEndElement();

			Writer.WriteElementString("Description", this.textBox1.Text);
            
			GLInformationsss accttype = new GLInformationsss();
			
			Writer.WriteElementString("Type",accttype.getAcctTypeWords(this.comboBox2.Text).ToString());
			Writer.WriteElementString("isInactive",this.checkBox1.Checked.ToString());
			Writer.Close();

            importer = (Interop.PeachwServer.Import)ptApp.app.CreateImporter(PeachwIEObj.peachwIEObjChartOfAccounts);
			importer.ClearImportFieldList();
			importer.AddToImportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerId);
			importer.AddToImportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_GeneralLedgerDescription);
			importer.AddToImportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Type);
			importer.AddToImportFieldList((short)PeachwIEObjChartOfAccountsField.peachwIEObjChartOfAccountsField_Inactive);
			importer.SetFilename(@"C:\XML\coa.xml");
			importer.SetFileType(PeachwIEFileType.peachwIEFileTypeXML);
			importer.Import();
			clearForm();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			saveRecord();
		}
	}
}
