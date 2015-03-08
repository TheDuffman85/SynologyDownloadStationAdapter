using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TheDuffman85.Tools
{
    public class WebClientExtended : WebClient
    {
        #region Variables

        private int _timeout = 60 * 1000;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the WebClient class.
        /// </summary>
        /// <param name="timeout">Timeout for the WebRequest</param>
        public WebClientExtended(int timeout)
        {
            _timeout = timeout;
        }

        #endregion

        #region Methods

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest request = base.GetWebRequest(uri);
            request.Timeout = _timeout;

            // Set default proxy
            IWebProxy webProxy = WebRequest.DefaultWebProxy;
            webProxy.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy = webProxy;

            return request;
        }

        #endregion
    }
}
