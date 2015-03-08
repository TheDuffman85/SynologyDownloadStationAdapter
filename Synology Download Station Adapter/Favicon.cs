using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheDuffman85.Tools
{
    /// <summary>
    /// This class provides the favicon of a given url.
    /// </summary>
    public class Favicon
    {
        #region Events

        /// <summary>
        /// Occurs when GetFromUrlAsync is completed.
        /// </summary>
        public event AsyncCompletedEventHandler GetFromUrlAsyncCompleted;

        #endregion

        #region Constances

        private const int DEFAULT_TIMEOUT = 5 * 1000;

        #endregion

        #region Variables

        private int _timeout = 5 * 1000;
        private Image _icon;
        private object _tag;

        #endregion

        #region Properties

        /// <summary>
        /// The favicon of the given url. Use GetFromUrl or GetFromUrlAsync to fill this property.
        /// </summary>
        public Image Icon
        {
            get { return _icon; }
        }

        /// <summary>
        /// Gets or sets the object that contains supplemental data.
        /// </summary>
        /// <remarks>
        /// Will be null if no favicon can be found.
        /// </remarks>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public Favicon()
        {
        }

        private Favicon(Image icon)
        {
            _icon = icon;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets the favicon from a given url.
        /// </summary>
        /// <remarks>
        /// The property Icon will be null if no favicon can be found.
        /// </remarks>
        /// <param name="url">The url to get the favicon from.</param>
        /// <param name="timeout">Timeout for web requests. The timeout will not raise an exception. The default value is 5 seconds.</param>
        /// <returns>Favicon</returns>        
        public static Favicon GetFromUrl(string url, int timeout = DEFAULT_TIMEOUT)
        {
            return GetFromUrl(new Uri(url), timeout);
        }

        /// <summary>
        /// Gets the favicon from a given url.
        /// </summary>
        /// <remarks>
        /// The property Icon will be null if no favicon can be found.
        /// </remarks>
        /// <param name="url">The url to get the favicon from.</param>
        /// <param name="timeout">Timeout for web requests. The timeout will not raise an exception. The default value is 5 seconds.</param>
        /// <returns>Favicon</returns>
        public static Favicon GetFromUrl(Uri url, int timeout = DEFAULT_TIMEOUT)
        {
            Favicon favicon = new Favicon();

            favicon._timeout = timeout;
            favicon._icon = favicon.GetIcon(url);

            return favicon;
        }

        /// <summary>
        /// Gets the favicon asynchronously from a given url.
        /// </summary>
        /// <remarks>
        /// The property Icon will be null if no favicon can be found.
        /// </remarks>
        /// <param name="url">The url to get the favicon from.</param>
        /// <param name="timeout">Timeout for web requests. The timeout will not raise an exception. The default value is 5 seconds.</param>
        public void GetFromUrlAsync(string url, int timeout = DEFAULT_TIMEOUT)
        {
            GetFromUrlAsync(new Uri(url), timeout);
        }

        /// <summary>
        /// Gets the favicon asynchronously from a given url.
        /// </summary>
        /// <remarks>
        /// The property Icon will be null if no favicon can be found.
        /// </remarks>
        /// <param name="url">The url to get the favicon from.</param>
        /// <param name="timeout">Timeout for web requests. The timeout will not raise an exception. The default value is 5 seconds.</param>
        public void GetFromUrlAsync(Uri url, int timeout = DEFAULT_TIMEOUT)
        {
            new Task(() =>
            {
                this._timeout = timeout;

                try
                {
                    this._icon = GetIcon(url);

                    if (GetFromUrlAsyncCompleted != null)
                    {
                        GetFromUrlAsyncCompleted(this, new AsyncCompletedEventArgs(null, false, null));
                    }
                }
                catch (Exception ex)
                {
                    if (GetFromUrlAsyncCompleted != null)
                    {
                        GetFromUrlAsyncCompleted(this, new AsyncCompletedEventArgs(ex, false, null));
                    }
                }
            }
            ).Start();
        }

        #endregion

        #region Private Methods

        private Image GetIcon(Uri url)
        {
            Image icon = null;

            try
            {
                icon = DownloadFavicon(url, "/favicon.ico");

                if (icon == null)
                {
                    string iconUrl = ExtractFavIconUrl(url);

                    if (iconUrl != null)
                    {
                        icon = DownloadFavicon(url, iconUrl);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.Timeout)
                {
                    throw;
                }
            }

            return icon;
        }

        private string ExtractFavIconUrl(Uri url)
        {
            WebClient client = new WebClientExtended(_timeout);
            string iconUrl = null;

            string html = client.DownloadString(url);

            Match match;

            // Link
            foreach (Match m in Regex.Matches(html, "<link[^>]*(rel=\"icon\"|rel=\"shortcut icon\"|rel=\"apple-touch-icon\"|rel=\"apple-touch-icon-precomposed\")[^>]*[\\/]?>", RegexOptions.IgnoreCase))
            {
                match = Regex.Match(m.Value, "href=\"([^\"]*)\"", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            // Meta
            foreach (Match m in Regex.Matches(html, "<meta[^>]*(itemprop=\"image\")[^>]*[\\/]?>", RegexOptions.IgnoreCase))
            {
                match = Regex.Match(m.Value, "content=\"([^\"]*)\"", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return iconUrl;
        }

        private Image DownloadFavicon(Uri baseUrl, string iconUrl)
        {
            Image icon = null;
            WebClient client = new WebClientExtended(_timeout);

            try
            {
                // Download fav icon
                if (!string.IsNullOrEmpty(iconUrl))
                {
                    Uri faviconUrl;

                    if (Uri.TryCreate(iconUrl, UriKind.RelativeOrAbsolute, out faviconUrl))
                    {
                        if (!faviconUrl.IsAbsoluteUri)
                        {
                            faviconUrl = new Uri(baseUrl, iconUrl);
                        }

                        Stream dataStream = client.OpenRead(faviconUrl);
                        icon = Image.FromStream(dataStream);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }
            }

            return icon;
        }

        #endregion
    }
}
