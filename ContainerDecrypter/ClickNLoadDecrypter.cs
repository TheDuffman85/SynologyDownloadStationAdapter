using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TheDuffman85.ContainerDecrypter
{
    /// <summary>
    /// Decrypts Click'N'Load content
    /// </summary>
    /// <remarks>
    /// Based on code created by bennyborn
    /// https://github.com/bennyborn/ClickNLoadDecrypt/
    /// </remarks>
    public class ClickNLoadDecrypter : DecrypterBase
    {
        private string _content;

        /// <summary>
        /// ClickNLoadDecrypter constructor
        /// </summary>
        /// <param name="content">Click'N'Load content</param>
        public ClickNLoadDecrypter(string content)
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

            // unescape content
            content =  Regex.Unescape(content);
            
            // get encrypted data
            Regex rgxData = new Regex("crypted=(.*?)(&|$)");
            String data = rgxData.Match(content).Groups[1].ToString();

            // get encrypted pass
            Regex rgxKey = new Regex("jk=(.*?){(.*?)}(;|&|$)");
            String key = rgxKey.Match(content).Groups[2].ToString();

            // get archive password
            Regex rgxPass = new Regex("passwords=(.*?)(&|$)");
            password = rgxPass.Match(content).Groups[1].ToString();

            var jsEngine = new Jurassic.ScriptEngine();
            key = jsEngine.Evaluate("(function (){" + key + "})()").ToString();
                                   
            // decode key
            key = key.ToUpper();
            String decKey = "";
            for (int i = 0; i < key.Length; i += 2)
            {
                decKey += (char)Convert.ToUInt16(key.Substring(i, 2), 16);
            }

            // decode data
            byte[] dataByte = Convert.FromBase64String(data);

            // decrypt that shit!
            RijndaelManaged rDel = new RijndaelManaged();
            System.Text.ASCIIEncoding aEc = new System.Text.ASCIIEncoding();
            rDel.Key = aEc.GetBytes(decKey);
            rDel.IV = aEc.GetBytes(decKey);
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.None;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(dataByte, 0, dataByte.Length);
            String rawLinks = aEc.GetString(resultArray);

            // replace empty paddings
            Regex rgx = new Regex("\u0000+$");
            String cleanLinks = rgx.Replace(rawLinks, "");

            // replace newlines
            rgx = new Regex("\n+");
            cleanLinks = rgx.Replace(cleanLinks, "\r\n");
            links = cleanLinks.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
