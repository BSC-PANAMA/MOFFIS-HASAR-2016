using System;
using Interop.PeachwServer;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace CSSDK
{
	public class Delete
	{
		Connect ptApp = new Connect();
		
		public Delete()
		{
		}
        public void DeleteTransaction(Interop.PeachwServer.PeachBusObjects module, Interop.PeachwServer.PeachObjectKey delBy, ref string[] recToDel)
		{
			try
			{
				ptApp.app.DeleteRecord(module,delBy,ref recToDel);
				MessageBox.Show("Recorded Deleted Successfully",ptApp.app.ProductName);
			}
			catch(COMException e)
			{
				MessageBox.Show(e.Message);
			}	
		}
	}
}
