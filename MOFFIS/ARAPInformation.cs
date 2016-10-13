using System;

namespace MOFFIS
{
	
	public class ARAPInformation
	{
        private Interop.PeachwServer.Application app;
        private Interop.PeachwServer.Login login = new Interop.PeachwServer.Login();
        
        //private Interop.PeachwServer.Login login = new Interop.PeachwServer.LoginClass();
		private AccountingPeriods lastperiod = new AccountingPeriods();
//		private Array invoices = Array.CreateInstance(typeof(string),0,9);
		

		public ARAPInformation()
		{
            //app = (Interop.PeachwServer.Application)login.GetApplication(frmMain.sName, frmMain.sPassword);
		}
		public void ARUnpaidInv(string ID, string invNum,out double amtPaid, out bool outstanding)
		{
			DateTime[] duedate = new DateTime[1];
            string[] invoicenum = new string[1];
            string[] custname = new string[1];
            decimal[] amount = new decimal[1];
            string[] custID = new string[1];
            int[] PostOrder = new int[1];
            decimal[] POA = new decimal[1];
            DateTime[] transdate = new DateTime[1];
            bool[] begbal = new bool[1];
			amtPaid = 0;
			outstanding = false;


			app.GetOverdueInvoicesByDate(lastperiod.getLastOpenDay(),1,
				out duedate,out invoicenum,out custname,
				out amount,out custID,out PostOrder,
				out POA,out transdate,out begbal);

			for(int i = 0;i <= invoicenum[0].Length -1;i++)
			{
				if(custID[i] == ID && invoicenum[i] == invNum)
				{
					amtPaid = Convert.ToDouble(POA[i]);
					outstanding = true;
					break;
				}
				else
				{
					amtPaid = 0;
					outstanding = false;
				}
			}
		}
		public void ApUnpaidInv(string ID, string invNum,out double amtPaid, out bool outstanding)
		{
            DateTime[] duedate = new DateTime[1];
            string[] invoicenum = new string[1];
            string[] vendname = new string[1];
            decimal[] amount = new decimal[1];
            string[] vendID = new string[1];
            int[] PostOrder = new int[1];
            decimal[] POA = new decimal[1];
            DateTime[] transdate = new DateTime[1];
            bool[] begbal = new bool[1];
            bool[] prepay = new bool[1];
            amtPaid = 0;
            outstanding = false;


			app.GetOverduePurchasesByDate(lastperiod.getLastOpenDay(),1,
				out duedate,out invoicenum,out vendname,
				out amount,out vendID,out PostOrder,
                out POA, out transdate, out begbal, out prepay);
			for(int i = 0;i <= invoicenum[0].Length -1;i++)
			{
				if(vendID[i] == ID && invoicenum[i] == invNum)
				{
					amtPaid = Convert.ToDouble(POA[i]);
					outstanding = true;
					break;
				}
				else
				{
					amtPaid = 0;
					outstanding = false;
				}
			}
		}

	}
}
