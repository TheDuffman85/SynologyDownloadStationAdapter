using Microsoft.Win32;
using SynologyAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheDuffman85.ContainerDecrypter;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    public class Adapter
    {
        #region Import

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        #endregion

        #region Constants

        private const string REG_KEY_NAME = "SynologyDownloadStationAdapter";
        private static readonly string[] FILE_TYPES = new string[] { ".dlc", ".ccf", ".rsdf" };

        #endregion

        #region Variables

        private static HttpListener _httpListener;
        private static frmSettings _frmSettings;
        private static frmAddLinks _frmAddLinks;
        private static frmDownloadStation _frmDownloadStation;
        private static frmSelectHoster _frmSelectHoster;
        
        #endregion   

        #region Properties

        public static frmSettings FrmSettings
        {
            get
            {
                if (_frmSettings == null)
                {
                    _frmSettings = new frmSettings();
                }

                return _frmSettings;
            }
        }

        public static frmAddLinks FrmAddLinks
        {
            get
            {
                if (_frmAddLinks == null ||
                    _frmAddLinks.IsDisposed)
                {
                    _frmAddLinks = new frmAddLinks();
                }

                return _frmAddLinks;
            }
        }

        public static frmDownloadStation FrmDownloadStation
        {
            get
            {
                if (_frmDownloadStation != null &&
                    _frmDownloadStation.Crashed)
                {
                    _frmDownloadStation.Dispose();
                }

                if (_frmDownloadStation == null ||
                    _frmDownloadStation.IsDisposed)
                {
                    _frmDownloadStation = new frmDownloadStation();
                }

                return _frmDownloadStation;
            }
        }

        public static frmSelectHoster FrmSelectHoster
        {
            get
            {
                if (_frmSelectHoster == null ||
                    _frmSelectHoster.IsDisposed)
                {
                    _frmSelectHoster = new frmSelectHoster();
                    _frmSelectHoster.Visible = true;
                    _frmSelectHoster.Visible = false;
                }

                return _frmSelectHoster;
            }
        }

        #endregion 

        #region Static Methods

        /// <summary>
        /// Starts the HttpListener
        /// </summary>
        public static void Start()
        {
            // Click'N'Load Listener
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://+:9666/");

            try
            {
                _httpListener.Start();
            }
            catch (HttpListenerException ex)
            {
                // Listen on port 9666 not allowed
                if (ex.ErrorCode == 5)
                {
                    // Allow listen
                    Adapter.AllowListener();

                    // Restart
                    Process.Start(Application.ExecutablePath);
                    Application.Exit();
                    return;
                }

                Adapter.ShowBalloonTip("Couldn't initialize Click'N'Load. Maybe another application is using port 9666?", ToolTipIcon.Warning);                        
            }

            if (_httpListener.IsListening)
            {
                IAsyncResult result = _httpListener.BeginGetContext(new AsyncCallback(WebRequestCallback), _httpListener);
            }
                      
        }

        /// <remarks>
        /// Based on code created by bennyborn
        /// https://github.com/bennyborn/ClickNLoadDecrypt/
        /// </remarks>
        private static void WebRequestCallback(IAsyncResult result)
        {
            HttpListenerContext context = _httpListener.EndGetContext(result);
            _httpListener.BeginGetContext(new AsyncCallback(WebRequestCallback), _httpListener);
            DecrypterBase decrypter = null;

            // build response data
            HttpListenerResponse response = context.Response;
            string responseString = "";
            response.StatusCode = 200;
            response.Headers.Add("Content-Type: text/html");
            // crossdomain.xml
            if (context.Request.RawUrl == "/crossdomain.xml")
            {
                responseString = "<?xml version=\"1.0\"?>"
                + "<!DOCTYPE cross-domain-policy SYSTEM \"http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd\">"
                + "<cross-domain-policy>"
                + "<allow-access-from domain=\"*\" />"
                + "</cross-domain-policy>";
            }
            else if (context.Request.RawUrl == "/jdcheck.js")
            {
                responseString = "jdownloader=true; var version='18507';";
            }
            else if (context.Request.RawUrl.StartsWith("/flash"))
            {
                if (context.Request.RawUrl.Contains("addcrypted2"))
                {
                    System.IO.Stream body = context.Request.InputStream;
                    System.IO.StreamReader reader = new System.IO.StreamReader(body, context.Request.ContentEncoding);
                    String requestBody = System.Web.HttpUtility.UrlDecode(reader.ReadToEnd());

                    if (!string.IsNullOrEmpty(requestBody))
                    {
                        decrypter = new ClickNLoadDecrypter(requestBody);
                    }

                    responseString = "success\r\n";
                }
                else
                {
                    responseString = "JDownloader";
                }
            }
            else if (context.Request.RawUrl.StartsWith("/OpenFile?"))
            {
                string filePath = System.Web.HttpUtility.UrlDecode(context.Request.RawUrl.Substring(10));
                                                
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (FILE_TYPES.Contains(Path.GetExtension(filePath).ToLower()))
                    {
                        decrypter = new ContainerDecrypter.DcryptItDecrypter(filePath);
                    }
                    else
                    {
                        ShowBalloonTip("Only file types dlc, ccf and rsdf are supported", ToolTipIcon.Warning);
                    }
                }

                responseString = "success\r\n";
            }
            else
            {
                response.StatusCode = 400;
            }
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            // output response
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            if (decrypter != null)
            {
                Adapter.AddLinksToDownloadStation(decrypter);
            }
        }

        /// <summary>
        /// Shows BallonTips
        /// </summary>
        /// <param name="msg">Message to show</param>
        /// <param name="icon">Icon of the ballon</param>
        public static void ShowBalloonTip(string msg, ToolTipIcon icon)
        {
            FrmSettings.NotifyIcon.ShowBalloonTip(3000, "Synology Download Station Adapter", msg, icon);
        }

        /// <summary>
        /// Is the current application running with administrative rights?
        /// </summary>
        /// <returns>Yes/No</returns>
        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Runs a new instance of the current application with administrative rights
        /// </summary>
        /// <param name="arg">Arguments</param>
        /// <returns>Did user allow the elevation?</returns>
        public static bool RunAsAdmin(string arg)
        {
            return RunAsAdmin(Application.ExecutablePath, arg);
        }

        /// <summary>
        /// Runs an application with administrative rights
        /// </summary>
        /// <param name="fileName">Application path</param>
        /// <param name="arg">Arguments</param>
        /// <returns></returns>
        public static bool RunAsAdmin(string fileName, string arg)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = fileName;
            startInfo.Verb = "runas";
            startInfo.Arguments = arg;

            try
            {
                Process process = Process.Start(startInfo);
                process.WaitForExit();

                return true;
            }
            catch
            {
                // The user refused the elevation. 
                return false;
            }
        }                
        
        /// <summary>
        /// Make the current application start with windows or not
        /// </summary>
        public static void ToggleAutoStart()
        {
            if (IsRunAsAdmin())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (!IsAutoStart())
                    {
                        key.SetValue(REG_KEY_NAME, Application.ExecutablePath);
                    }
                    else
                    {
                        key.DeleteValue(REG_KEY_NAME, false);
                    }
                }                   
            }
            else
            {
                RunAsAdmin("ToggleAutoStart");
            }
        }

        /// <summary>
        /// Does the current application start with windows?
        /// </summary>
        /// <returns>Yes/No</returns>
        public static bool IsAutoStart()
        {    
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
            {
                return (key.GetValue(REG_KEY_NAME) != null && key.GetValue(REG_KEY_NAME).ToString() == Application.ExecutablePath);
            }
        }

        /// <summary>
        /// Associate the application with .dlc, .ccf and .rsdf
        /// </summary>
        public static void AssociateFileTypes()
        {
            if (IsRunAsAdmin())
            {
                foreach (string fileType in FILE_TYPES)
                {
                    SetAssociation(fileType, "SynologyDownloadStationAdapter", Application.ExecutablePath, "Synolog Download Station Adapter");
                }

                // Tell explorer the file association has been changed
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                RunAsAdmin("AssociateFileTypes");
            }
        }
                
        private static void SetAssociation(string extension, string keyName, string openWith, string fileDescription)
        {
            using (RegistryKey baseKey = Registry.ClassesRoot.CreateSubKey(extension))
            using (RegistryKey openMethod = Registry.ClassesRoot.CreateSubKey(keyName))
            using (RegistryKey shell = openMethod.CreateSubKey("Shell"))
            using (RegistryKey currentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + extension, true))
            using (RegistryKey currentUser2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Roaming\\OpenWith\\FileExts\\" + extension, true))
            {
                baseKey.SetValue("", keyName);

                openMethod.SetValue("", fileDescription);
                openMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + openWith + "\",0");
                
                shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + openWith + "\"" + " \"%1\"");
                baseKey.Close();
                openMethod.Close();
                shell.Close();

                if (currentUser != null)
                {
                    currentUser.DeleteSubKey("UserChoice", false);
                    currentUser.Close();
                }

                if (currentUser2 != null)
                {
                    currentUser2.DeleteSubKey("UserChoice", false);
                    currentUser2.Close();
                }            
            }          
        }

        /// <summary>
        /// Allow listening on port 9666
        /// </summary>
        public static void AllowListener()
        {            
            RunAsAdmin("netsh", "http add urlacl url=http://+:9666/ user=" + WindowsIdentity.GetCurrent().Name);
        }               
                      
        /// <summary>
        /// Add links of a decrypted container to Download Station
        /// </summary>
        /// <param name="decrypter">ContainerDecrypter</param>
        /// <returns>Success?</returns>
        public static bool AddLinksToDownloadStation(DecrypterBase decrypter)
        {
            try
            {
                if (!decrypter.IsDecypted)
                {
                    decrypter.Decrypt();
                }

                return Adapter.AddLinksToDownloadStation(decrypter.Links.ToList());
            }
            catch (Exception ex)
            {
                Adapter.ShowBalloonTip(ex.Message, ToolTipIcon.Error);
            }

            return false;
        }

        /// <summary>
        /// Add links to Download Station
        /// </summary>
        /// <param name="links">Links to Download</param>
        /// <returns>Success?</returns>
        public static bool AddLinksToDownloadStation(List<string> links)
        {
            DownloadStation ds = null;

            try
            {
                ds = new DownloadStation(new Uri("http://" + Properties.Settings.Default.Address), Properties.Settings.Default.Username, Encoding.UTF8.GetString(Convert.FromBase64String(Properties.Settings.Default.Password)));

                // Login to Download Station
                if (ds.Login())
                {
                    links.RemoveAll(str => String.IsNullOrEmpty(str.Trim()));

                    Dictionary<string, List<string>> validHostLinks = new Dictionary<string, List<string>>();
                    List<string> corruptedLinks = new List<string>();   
                    Uri currentLink = null;
                    int validHostLinkCount = 0;

                    foreach (string link in links)
                    {
                        try
                        {
                            currentLink = new Uri(link);
                           
                            if (!validHostLinks.ContainsKey(currentLink.Host))
                            {
                                validHostLinks.Add(currentLink.Host, new List<string>());
                            }
                            
                            validHostLinks[currentLink.Host].Add(link);
                        }
                        catch
                        {
                            corruptedLinks.Add(link);
                        }                        
                    }

                    if (validHostLinks.Keys.Count > 1)
                    {
                        Adapter.FrmSelectHoster.SelectHoster(validHostLinks);
                    }                                        

                    foreach (var validHostLink in validHostLinks)
                    {
                        Adapter.ShowBalloonTip("Adding " + validHostLink.Value.Count + " links(s) (" + validHostLink.Key + ")", ToolTipIcon.Info);

                        validHostLinkCount += validHostLink.Value.Count;

                        foreach (string link in validHostLink.Value)
                        {
                            // Add link to Download Station
                            SynologyRestDAL.TResult<object> result = ds.CreateTask(link);
                                                        
                            if (!result.Success)
                            {
                                if (result.Error.Code == 406)
                                {
                                    throw new Exception("Couldn't add link. You have to choose a download folder for your Download Station.");
                                }
                                else
                                {
                                    throw new Exception("While adding a link '" + link + " error code " + result.Error.Code + " occurred");
                                }
                            }
                        }                        
                    }

                    string msg = validHostLinkCount + " link(s) added";

                    if (corruptedLinks.Count > 0)
                    {
                        msg += "\r\n" + corruptedLinks.Count + " links(s) were corrupted";
                    }

                    Adapter.ShowBalloonTip(msg, ToolTipIcon.Info);
                    return true;
                }
                else
                {
                    throw new Exception("Couldn't login to Download Station");
                }
            }
            catch (Exception ex)
            {
                Adapter.ShowBalloonTip(ex.Message, ToolTipIcon.Error);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Logout();
                }
            }
        }

        public static void OpenFileWithRunningInstance(string path)
        {            
            // Open file with running instance
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:9666/OpenFile?" + System.Web.HttpUtility.UrlEncode(path));
            request.Method = "GET";
            request.GetResponse();            
        }

        public static void OpenDownloadStation()
        {
            if (Properties.Settings.Default.ApplicationEnabled)
            {
                Adapter.FrmDownloadStation.Show();
                Adapter.FrmDownloadStation.Activate();
            }
            else
            {
                Process.Start("http://" + Properties.Settings.Default.Address);
            }
        }

        #endregion
    }
}
