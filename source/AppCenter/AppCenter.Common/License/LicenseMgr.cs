using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace SoonLearning.AppCenter.License
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
        private string licenseFile;

        private LicenseMgr()
        {
        }

        public bool Init()
        {
            string appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            licenseFile = Path.Combine(appFolder, "license.lic");
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

        public bool IsLicenseForCurrentMachine(string appId)
        {
            string encryptAppId = EncryptHelper.GetMD5Hash(appId);
            if (this.gadgetLicenseDictionary.ContainsKey(encryptAppId))
            {
                string licenseString = this.gadgetLicenseDictionary[encryptAppId];
                return isLicenseForCurrentMachine(licenseString, appId);
            }

            return false;
        }

        public bool IsLicenseForCurrentMachine(string license, string appId)
        {
            string licenseString = license;
            foreach (string data in EncryptHelper.DecryptLicenseFile(licenseString))
                licenseString = data;

            foreach (string licenseBlock in GetLicenseBlock(licenseString))
            {
                string[] parts = licenseBlock.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return isLicenseForCurrentMachine(parts[1], appId);
            }

            return false;
        }

        private bool isLicenseForCurrentMachine(string license, string appId)
        {
            IntPtr licensePtr = IntPtr.Zero;
            try
            {
                byte[] licenseData = Convert.FromBase64String(license);
                licensePtr = Marshal.AllocHGlobal(licenseData.Length);
                Marshal.Copy(licenseData, 0, licensePtr, licenseData.Length);

                foreach (string hdId in EncryptHelper.GetHardwareIdMD5())
                {
                    IntPtr hdIdPtr = Marshal.StringToHGlobalAnsi(hdId);
                    int nRet = LicenseAPI.IsLicenseForCurrentMachine(licensePtr, licenseData.Length, hdIdPtr);
                    Marshal.FreeHGlobal(hdIdPtr);

                    if (nRet != 0)
                        return true;
                }
            }
            finally
            {
                if (licensePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(licensePtr);
            }

            return false;
        }

        public void SaveLicenseToFile(string license, string appId)
        {
            string encryptAppId = EncryptHelper.GetMD5Hash(appId);

            string decryptedLicense = string.Empty;
            foreach (string temp in EncryptHelper.DecryptLicenseFile(license))
                decryptedLicense = temp;

            string[] parts = decryptedLicense.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (this.gadgetLicenseDictionary.ContainsKey(encryptAppId))
            {
                this.gadgetLicenseDictionary[encryptAppId] = parts[1];
            }
            else
            {
                this.gadgetLicenseDictionary.Add(encryptAppId, parts[1]);
            }

            this.UpdateLicense(parts[1], encryptAppId);
        }

        private void UpdateLicense(string license, string appId)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (string key in this.gadgetLicenseDictionary.Keys)
            {
                strBuilder.AppendLine("****************************************************************");
                strBuilder.AppendLine(key);
                strBuilder.AppendLine(this.gadgetLicenseDictionary[key]);
                strBuilder.AppendLine("****************************************************************");
            }

            foreach (string result in EncryptHelper.EncryptLicenseFile(strBuilder.ToString()))
            {
                File.WriteAllText(this.licenseFile, result);
                break;
            }
        }

        private void AppendLicense(string license, string appId)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("****************************************************************");
            strBuilder.AppendLine(license);
            strBuilder.AppendLine("****************************************************************");

            File.AppendAllText(this.licenseFile, strBuilder.ToString());
        }

        public string GetHardwardId()
        {
            foreach (string hdId in EncryptHelper.GetHardwareIdDES())
            {
                return hdId;
            }

            return string.Empty;
        }
    }
}
