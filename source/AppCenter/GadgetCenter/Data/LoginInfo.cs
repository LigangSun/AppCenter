using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Data
{
    public class LoginInfo : NotifyPropertyChanged
    {
        private string loginId;
        private string password;
        private bool remember;
        private bool autoLogin;

        private string id;

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string LoginId
        {
            get { return this.loginId; }
            set
            {
                this.loginId = value;
                this.OnPropertyChanged("LoginId");
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
                this.OnPropertyChanged("Password");
            }
        }

        public bool Remember
        {
            get { return this.remember; }
            set
            {
                this.remember = value;
                this.OnPropertyChanged("Remember");
            }
        }

        public bool AutoLogin
        {
            get { return this.autoLogin; }
            set
            {
                this.autoLogin = value;
                this.OnPropertyChanged("AutoLogin");
            }
        }

        public LoginInfo()
        {
            this.Id = "0";
        }

        internal void Save()
        {
            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                string basePath = Path.GetDirectoryName(assembly.Location);
                string infoFile = Path.Combine(basePath, "UserInfo.xml");
                if (File.Exists(infoFile))
                    File.Delete(infoFile);
                SerializerHelper<LoginInfo>.XmlSerialize(infoFile, this);
            }
            catch
            {
            }
        }

        internal static LoginInfo Load()
        {
            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                string basePath = Path.GetDirectoryName(assembly.Location);
                string infoFile = Path.Combine(basePath, "UserInfo.xml");
                if (File.Exists(infoFile))
                    return SerializerHelper<LoginInfo>.XmlDeserialize(infoFile);
            }
            catch
            {
            }

            return new LoginInfo();
        }

        internal static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
    }
}
