using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.IO;
using GadgetCenter.Utility;
using System.Runtime.InteropServices;

namespace GadgetCenter.License
{
    public class LicenseMgr
    {
        private static LicenseMgr instance;

        public static LicenseMgr Instance
        {
            get
            {
                Interlocked.CompareExchange<LicenseMgr>(ref instance, new LicenseMgr(), null);
                return instance;
            }
        }

        private Dictionary<string, string> gadgetLicenseDictionary = new Dictionary<string, string>();

        private LicenseMgr()
        {
        }

        internal bool Init()
        {
            string appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string licenseFile = Path.Combine(appFolder, "license.lic");
            if (!File.Exists(licenseFile))
                return false;

            try
            {
                this.LoadLicense(licenseFile);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void LoadLicense(string file)
        {
            string licenseData = string.Empty;
            foreach (string data in EncryptHelper.ReadFileData(file))
            {
                licenseData = data;
            }

            foreach (string data in EncryptHelper.DecryptLicenseFile(licenseData))
                licenseData = data;

            foreach (string licenseBlock in GetLicenseBlock(licenseData))
            {
                string[] parts = licenseBlock.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                this.gadgetLicenseDictionary.Add(parts[0], parts[1]);
            }
        }

        private IEnumerable<string> GetLicenseBlock(string licenseData)
        {
            string[] licenseList = licenseData.Split(new string[] { "****************************************************************" },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string license in licenseList)
            {
                string temp = license.TrimStart(new char[] { '\r', '\n' });
                if (!string.IsNullOrEmpty(temp))
                    yield return temp;
            }
        }

        public bool IsTrialVersion(string gadgetId)
        {
            string encryptGadgetId = EncryptHelper.GetMD5Hash(gadgetId);
            if (this.gadgetLicenseDictionary.ContainsKey(encryptGadgetId))
            {
                string licenseString = this.gadgetLicenseDictionary[encryptGadgetId];
                byte[] licenseData = Convert.FromBase64String(licenseString);
                IntPtr licensePtr = Marshal.AllocHGlobal(licenseData.Length);
                Marshal.Copy(licenseData, 0, licensePtr, licenseData.Length);
                return LicenseAPI.IsTrialVersion(licensePtr, licenseData.Length) != 0;
            }

            return false;
        }
    }
}
