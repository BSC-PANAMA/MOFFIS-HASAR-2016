using System;
using System.Windows.Forms;

namespace MOFFIS
{

	public class GLInformationsss
	{
		public GLInformationsss()
		{

		}
		public string getAcctTypeWords(int acctTypeNum)
		{
			switch (acctTypeNum)
			{
				case 0:
					return "Cash";
				case 1:
					return "Accounts Receivable";
				case 2:
					return "Inventory";
				case 3:
					return "Receivables Retainage (PPAC Only)";
				case 4:
					return "Other Current Assets";
				case 5:
					return "Fixed Asset";
				case 6:
					return "Accumulated Depreciation";
				case 8:
					return "Other Asset";
				case 10:
					return "Accounts Payable";
				case 11:
					return "Payables Retainage (PPAC Only)";
				case 12:
					return "Other Current Liabilities";
				case 14:
					return "Long Term Liabilities";
				case 16:
					return "Equity - Doesn't Close";
				case 18:
					return "Equity - Retained Earnings";
				case 19:
					return "Equity - Gets Closed";
				case 21:
					return "Income";
				case 23:
					return "Cost of Sales";
				case 24:
					return "Expenses";
				default:
					return "Invalid Account Type";
			}
		}
		public void fillAcctTypeList(ComboBox ctl)
		{
			ctl.Items.Add("Cash");
			ctl.Items.Add("Accounts Receivable");
			ctl.Items.Add("Inventory");
			ctl.Items.Add("Receivables Retainage (PPAC Only)");
			ctl.Items.Add("Other Current Assets");
			ctl.Items.Add("Fixed Assets");
			ctl.Items.Add("Accumlated Depreciation");
			ctl.Items.Add("Other Asset");
			ctl.Items.Add("Accounts Payable");
			ctl.Items.Add("Payables Retainage (PPAC Only)");
			ctl.Items.Add("Other Current Liabilities");
			ctl.Items.Add("Long Term Liabilties");
			ctl.Items.Add("Equity - Doesn't Close");
			ctl.Items.Add("Equity - Retained Earnings");
			ctl.Items.Add("Equity - Gets Closed");
			ctl.Items.Add("Income");
			ctl.Items.Add("Cost of Sales");
			ctl.Items.Add("Expenses");
		}
	
		public int getAcctTypeWords(string acctTypeName)
		{
			switch (acctTypeName)
			{
				case "Cash":
					return 0;
				case "Accounts Receivable":
					return 1;
				case "Inventory":
					return 2;
				case "Receivables Retainage (PPAC Only)":
					return 3;
				case "Other Current Assets":
					return 4;
				case "Fixed Asset":
					return 5;
				case "Accumulated Depreciation":
					return 6;
				case "Other Asset":
					return 8;
				case "Accounts Payable":
					return 10;
				case "Payables Retainage (PPAC Only)":
					return 11;
				case "Other Current Liabilities":
					return 12;
				case "Long Term Liabilities":
					return 14;
				case "Equity - Doesn't Close":
					return 16;
				case "Equity - Retained Earnings":
					return 18;
				case "Equity - Gets Closed":
					return 19;
				case "Income":
					return 21;
				case "Cost of Sales":
					return 23;
				case "Expenses":
					return 24;
				default:
					return 0;
			}
		}
	}
}
