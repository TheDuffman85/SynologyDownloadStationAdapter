using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TheDuffman85.ContainerDecrypter
{
    /// <summary>
    /// "Decrypts" Plain Click'N'Load content
    /// </summary>
    public class PlainClickNLoadDecrypter : DecrypterBase
    {
        private string _content;

        /// <summary>
        /// PlainClickNLoadDecrypter constructor
        /// </summary>
        /// <param name="content">Plain Click'N'Load content</param>
        public PlainClickNLoadDecrypter(string content)
        {
            this._content = content;
        }

        protected override string LoadContent()
        {
            return this._content;
        } 
                
        protected override void Decrypt(string content, out string[] links, out string password)
        {
            links = null;
            password = null;
            
            // get encrypted data
            Regex rgxLinks = new Regex(@"urls=(.*?)(&|$)", RegexOptions.Singleline);
            String rawLinks = rgxLinks.Match(content).Groups[1].ToString();

            // get archive password
            Regex rgxPass = new Regex("passwords=(.*?)(&|$)");
            password = rgxPass.Match(content).Groups[1].ToString();
                       
            // replace empty paddings
            Regex rgx = new Regex("\u0000+$");
            String cleanLinks = rgx.Replace(rawLinks, "");

            // replace newlines
            rgx = new Regex("\n+");
            cleanLinks = rgx.Replace(cleanLinks, "\r\n");

            // replace tabs
            rgx = new Regex("\t");
            cleanLinks = rgx.Replace(cleanLinks, "");
           
            links = cleanLinks.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
