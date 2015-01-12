using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TheDuffman85.SynologyDownloadStationAdapter
{
    /// <summary>
    /// Generic class that provides a singletone patern for forms
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonForm<T> : Form where T : Form 
    {
        #region Variables

        private static T _instance;
        private bool _newInstance = false;

        #endregion

        #region Properties

        /// <summary>
        /// Instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance != null &&
                    ((SingletonForm<T>)(object)_instance)._newInstance)
                {
                    _instance.Dispose();
                }

                if (_instance == null ||
                    _instance.IsDisposed)
                {
                    _instance = (T)Activator.CreateInstance(typeof(T));
                }

                return _instance;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Shows the instance
        /// </summary>
        public static void ShowInstance()
        {
            ((T)SingletonForm<T>.Instance).Show();
            ((T)SingletonForm<T>.Instance).Activate();
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance on next call
        /// </summary>
        protected void NewIntance()
        {
            ((SingletonForm<T>)(object)_instance)._newInstance = true;
        }

        #endregion
    }
}
