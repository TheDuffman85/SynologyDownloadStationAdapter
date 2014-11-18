using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheDuffman85.ContainerDecrypter
{
    /// <summary>
    /// Base class for container decrypters
    /// </summary>
    public abstract class DecrypterBase
    {
        private string _path;
        private string _content;
        private string[] _links;
        private string _password;
        private bool _isDecrypted;

        /// <summary>
        /// DecrypterBase standard constructor
        /// </summary>
        public DecrypterBase()
        {
        }

        /// <summary>
        /// DecrypterBase constructor
        /// </summary>
        /// <param name="path">Path to container file</param>
        public DecrypterBase(string path)
        {
            this._path = path;            
        }

        /// <summary>
        /// Read the content of the container
        /// </summary>
        /// <returns>Content of the container</returns>
        protected virtual string LoadContent()
        {
            using (StreamReader containerStream = new StreamReader(this._path))
            {
                return containerStream.ReadToEnd();
            }
        }

        /// <summary>
        /// The file name
        /// </summary>
        public string Name
        {
            get 
            {
                return Path.GetFileName(this._path);
            }
        }

        /// <summary>
        /// The contained links
        /// </summary>
        public string[] Links
        {
            get
            {
                if (!this._isDecrypted)
                {
                    throw new InvalidOperationException("You have to decrypt the container first!");
                }

                return this._links;
            }
        }

        /// <summary>
        /// The contained Password
        /// </summary>
        public string Password
        {
            get
            {
                if (!this._isDecrypted)
                {
                    throw new InvalidOperationException("You have to decrypt the container first!");
                }

                return this._password;
            }
        }

        /// <summary>
        /// Indicates whether the container is decrypted or not
        /// </summary>
        public bool IsDecypted
        {
            get
            {
                return this._isDecrypted;
            }
        }

        /// <summary>
        /// Decrypts the container
        /// </summary>
        public void Decrypt()
        {
            if (this._isDecrypted)
            {
                throw new InvalidOperationException("The container was allready decrypted!");
            }
            
            this._content = LoadContent();
            Decrypt(this._content, out this._links, out this._password);

            this._isDecrypted = true;            
        }

        protected abstract void Decrypt(string content, out string[] links, out string password);
    }
}
