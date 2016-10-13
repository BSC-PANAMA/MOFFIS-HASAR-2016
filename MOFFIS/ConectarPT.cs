using System;
using System.Windows.Forms;
using Interop.PeachwServer;

namespace MOFFIS
{
    public class ConectarPT
    {                
        public Interop.PeachwServer.Application app;
        //public Interop.PeachwServer.Login login = new Interop.PeachwServer.LoginClass();
        public Interop.PeachwServer.Login login = new Interop.PeachwServer.Login();

        public ConectarPT()
		{            
            try
            {
                //app = (Interop.PeachwServer.Application)login.GetApplication(frmMain.sName, frmMain.sPassword);
                app = (Interop.PeachwServer.Application)login.GetApplication("Business Software Consulting INC", "1N1123QFY132X2I");
            }
            catch (System.UnauthorizedAccessException e )
            {
                MessageBox.Show(e.Message);
            }
		}
    }
}
