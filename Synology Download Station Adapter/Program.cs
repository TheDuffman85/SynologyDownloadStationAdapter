using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace TheDuffman85.SynologyDownloadStationAdapter
{
    static class Program
    {
        #region Variables

        #endregion

        #region Methods

        [STAThread]
        static void Main(string[] args)
        {            
            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "AssociateFileTypes":
                        Adapter.AssociateFileTypes();
                        return;
                    case "ToggleAutoStart":
                        Adapter.ToggleAutoStart();
                        return;
                    default:
                        break;
                }
            }

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "SynologyDownloadStationAdapter", out createdNew))
            {
                //No instance running create new
                if (createdNew)
                {                    
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Start the listener
                    Adapter.Start();
                                                       
                    Application.Run(Adapter.FrmSettings);
                }
                // An instance is allready running
                else
                {
                    if (args.Length == 1)
                    {                        
                        try
                        {
                            // Does the file exists?
                            if (File.Exists(args[0]))
                            {
                                Adapter.OpenFileWithRunningInstance(args[0]);
                            }
                        }
                        catch
                        {
                            // throw no error here
                        }                        
                    }
                }
            }            
        }
           
        #endregion
    }
}
