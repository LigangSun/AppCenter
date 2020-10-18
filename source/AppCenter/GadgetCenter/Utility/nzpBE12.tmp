using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography.Xml;
using Microsoft.Win32;
using System.Security.AccessControl;

namespace GadgetCenter.Utility
{
    internal class DateInfo
    {
        internal DateTime startDate = DateTime.MinValue;
        internal int leftDays = 0;
        internal int maxDays = 30;
        internal string version = string.Empty;
        internal string hardwareId = string.Empty;
    }

    internal class LicenseManager
    {
        public static string companyName = string.Empty;
        public static string mail = string.Empty;

        private static string xmlKey = "<RSAKeyValue><Modulus>YfJEeMvZ08YPDjFJhtHJ0FxiNVHRdgUdtefEy/di9/79OwCHE7WwEpCsbw4CChLtZd4tD359ntZK0EdLsTtefKo8XSk4aKRpo48wBQfPhkMgRCcB4hlG/gSo+uPMu86zhCwDU84WtM/FxFOkhmIZLbuLm1HCxkvVDdMXzgE4/3g=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        internal static DateInfo FindFirstRunTime()
        {
            string encryptTime = string.Empty;
            DateInfo info = new DateInfo();
            try
            {
            //    LicenseTable licenseTable = new LicenseTable();

           //     encryptTime = licenseTable.GetDateInfo();

                if (!string.IsNullOrEmpty(encryptTime))
                {
                    string confuseString = string.Empty;
                    foreach (var key in GetKeys(0))
                    {
                        foreach (var key1 in DecryptDES(encryptTime, key))
                            confuseString = key1;
                    }

                    string data1 = confuseString.Substring(0, 32);
                    string timeString = confuseString.Remove(0, 32);
                    string data2 = timeString.Substring(4, 32);
                    timeString = timeString.Remove(4, 32);
                    string data3 = timeString.Substring(6, 32);

                    timeString = timeString.Remove(6, 32);
                    timeString = timeString.Remove(12, 32);

                    info.startDate = DateTime.Parse(timeString);
                    info.leftDays = Convert.ToInt32(GetString(data1));
                    info.maxDays = Convert.ToInt32(GetString(data2));
                    info.version = GetString(data3);
                }
                else
                {
                    DateTime currentTime = TimeHelper.DataStandardTime();
                    string timeString = currentTime.ToString();
                    string confuseString = timeString;

                    Version version = Assembly.GetExecutingAssembly().GetName().Version;
                    string versionStr = string.Format("{0}{1}{2}", version.Major.ToString("00"),
                        version.Minor.ToString("00"), version.Build.ToString("0000"));

                    confuseString = confuseString.Insert(0, GenerateString("030")); // left days
                    confuseString = confuseString.Insert(4 + 32, GenerateString("030")); // total trial days
                    confuseString = confuseString.Insert(6 + 32 + 32, GenerateString(versionStr));
                    confuseString = confuseString.Insert(12 + 32 + 32 + 32, Guid.NewGuid().ToString("N"));

                    info.startDate = currentTime;
                    info.leftDays = 30;
                    info.maxDays = 30;
                    info.version = versionStr;
                    foreach (string key in GetKeys(3))
                    {
                        foreach (string value in EncryptDES(Hardware.GetCpuID(), key))
                            info.hardwareId = value;
                    }

                    foreach (var key in GetKeys(0))
                    {
                    //    foreach (var value in EncryptDES(confuseString, key))
                    //        licenseTable.SetDataInfo(value);
                    }
                }
            }
            catch (Exception ex)
            {
            //    LogEx.Log(ex);
            }

            return info;
        }

        internal static string GenerateString(string partStr)
        {
            string temp = Guid.NewGuid().ToString("N");

            Random rand = new Random();
            int pos = rand.Next(6, 20);

            temp = temp.Remove(3, 3);
            temp = temp.Insert(3, pos.ToString("000"));

            temp = temp.Remove(pos, partStr.Length);
            temp = temp.Insert(pos, partStr);

            return temp;
        }

        internal static string GetString(string str)
        {
            string posStr = str.Substring(3, 3);
            int pos = int.Parse(posStr);

            return str.Substring(pos, 3);
        }

        internal static IEnumerable<string> GetKeys(int index)
        {
            if (index == 0)
            {
                var key = "ESQG#%98";
                yield return key;
            }
            else if (index == 1)
            {
                var key = "EduSharedQuestionGalleryKeyContainer";
                yield return key;
            }
            else if (index == 2)
            {
                var key = "#%@*Bgq&";
                yield return key;
            }
            else if (index == 3)
            {
                var key = "1)7*)2s$";
                yield return key;
            }
        }

        internal static void ModifyLeftDays(int leftDays, int maxDays)
        {
            DateTime currentTime = TimeHelper.DataStandardTime();
            string timeString = currentTime.ToString();
            string confuseString = timeString;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionStr = string.Format("{0}{1}{2}", version.Major.ToString("00"),
                version.Minor.ToString("00"), version.Build.ToString("0000"));

            confuseString = confuseString.Insert(0, GenerateString(leftDays.ToString("000"))); // left days
            confuseString = confuseString.Insert(4 + 32, GenerateString(maxDays.ToString("000"))); // total trial days
            confuseString = confuseString.Insert(6 + 32 + 32, GenerateString(versionStr));
            confuseString = confuseString.Insert(12 + 32 + 32 + 32, Guid.NewGuid().ToString("N"));

        //    LicenseTable licenseTable = new LicenseTable();

            foreach (var key in GetKeys(0))
            {
            //    foreach (var value in EncryptDES(confuseString, key))
            //        licenseTable.SetDataInfo(value);
            }
        }

        internal static bool VerifyLicense()
        {
        //    LicenseTable licenseTable = new LicenseTable();

            string license = string.Empty;// licenseTable.GetLicense();

            if (string.IsNullOrEmpty(license))
                return false;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionStr = string.Format("{0}{1}{2}", version.Major.ToString("00"),
                version.Minor.ToString("00"), version.Build.ToString("0000"));

            foreach (string cpuId in Hardware.GetCpuIDs())
            {
                List<char> keys = new List<char>();
                foreach (char key in GetConfuseString(cpuId, versionStr))
                {
                    keys.Add(key);
                }

                foreach (var value in getMd5(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(keys.ToArray()))))
                {
                    foreach (string key in GetKeys(3))
                    {
                        foreach (string temp in EncryptDES(value, key))
                        {
                            if (license == temp)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static IEnumerable<char> GetConfuseString(string cpuId, string versionStr)
        {
            yield return '4';
            yield return versionStr[0];
            yield return cpuId[0];
            yield return '#';
            yield return cpuId[1];
            yield return 'D';
            yield return cpuId[2];
            yield return versionStr[1];
            yield return '%';
            yield return cpuId[3];
            yield return cpuId[4];
            yield return cpuId[5];
            yield return cpuId[6];
            yield return '^';
            yield return versionStr[2];
            yield return versionStr[3];
            yield return cpuId[7];
            yield return cpuId[8];
            yield return versionStr[4];
            yield return cpuId[9];
            yield return cpuId[10];
            yield return '&';
            yield return cpuId[11];
            yield return versionStr[5];
            yield return cpuId[12];
            yield return versionStr[6];
            yield return cpuId[13];
            yield return cpuId[14];
            yield return cpuId[15];
            yield return versionStr[7];
            yield return '}';
        }

        internal static bool UpdateLicense(string license)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionStr = string.Format("{0}{1}{2}", version.Major.ToString("00"),
                version.Minor.ToString("00"), version.Build.ToString("0000"));

            foreach (string cpuId in Hardware.GetCpuIDs())
            {
                List<char> keys = new List<char>();
                foreach (char key in GetConfuseString(cpuId, versionStr))
                {
                    keys.Add(key);
                }

                foreach (var value in getMd5(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(keys.ToArray()))))
                {
                    foreach (string key in GetKeys(3))
                    {
                        foreach (string temp in EncryptDES(value, key))
                        {
                            if (license == temp)
                            {
                            //    LicenseTable licenseTable = new LicenseTable();
                            //    licenseTable.SetLicense(license);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 0 indicate expired, -1 indicate regist version
        /// </summary>
        /// <param name="licenseFile"></param>
        /// <returns></returns>
        internal static DateInfo GetLeftDay(string licenseFile)
        {
            // Get the XML content from the embedded XML public key.
            Stream s = null;
            string xmlkey = xmlKey;

            DateInfo dateInfo = new DateInfo();
            dateInfo.leftDays = 0;

            // Create an RSA crypto service provider from the embedded
            // XML document resource (the public key).
            CspParameters parms = new CspParameters(1);			// PROV_RSA_FULL
            parms.Flags = CspProviderFlags.UseMachineKeyStore;	// Use Machine store
            foreach (var key in GetKeys(1))
                parms.KeyContainerName = key;
            parms.KeyNumber = 2;								// AT_SIGNATURE
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(parms);

            // Load the signed XML license file.
            XmlDocument xmldoc = new XmlDocument();

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
                return dateInfo;

            FileStream licenseStream = File.Open(licenseFile, FileMode.Open);
            if (licenseStream == null)
                return dateInfo;

            byte[] licenseData = new byte[licenseStream.Length];
            licenseStream.Read(licenseData, 0, licenseData.Length);
            string encryptString = Encoding.ASCII.GetString(licenseData);
            foreach (var key in GetKeys(0))
            {
                foreach (var key1 in DecryptDES(encryptString, key))
                    encryptString = key1;
            }

            StringReader sr = new StringReader(encryptString);
            xmldoc.Load(sr);

            licenseStream.Close();

            // Create the signed XML object.
            SignedXml sxml = new SignedXml(xmldoc);

            try
            {
                // Get the XML Signature node and load it into the signed XML object.
                XmlNode dsig = xmldoc.GetElementsByTagName("Signature",
                    SignedXml.XmlDsigNamespaceUrl)[0];
                sxml.LoadXml((XmlElement)dsig);
            }
            catch
            {
                Console.Error.WriteLine("Error: no signature found.");
                return dateInfo;
            }

            // Verify the signature.
            if (sxml.CheckSignature(csp))
            {
                XmlNode cpuNode = xmldoc.GetElementsByTagName("CPUID")[0];

                string currentCPUInfo = string.Empty;
                foreach (var key in GetKeys(2))
                {
                    foreach (var key1 in EncryptDES(Hardware.GetCpuID(), key))
                        currentCPUInfo = key1;
                }

                foreach (string value in getMd5(currentCPUInfo))
                {
                    if (cpuNode.InnerText == value.Trim())
                    {
                        dateInfo.leftDays = -1;
                        return dateInfo;
                    }
                }
            }

            XmlNode trialDaysNode = xmldoc.GetElementsByTagName("TrialDays")[0];
            XmlNode startDateNode = xmldoc.GetElementsByTagName("StartDate")[0];

            dateInfo.startDate = Convert.ToDateTime(startDateNode.InnerText);
            dateInfo.leftDays = Convert.ToInt32(trialDaysNode.InnerText);

            return dateInfo;
        }

        internal static void ModifyLeftDay(string licenseFile, DateTime? startDate, int leftDay)
        {
            // Get the XML content from the embedded XML public key.
            Stream s = null;
            string xmlkey = xmlKey;

            DateInfo dateInfo = new DateInfo();
            dateInfo.leftDays = 0;

            try
            {
                // Create an RSA crypto service provider from the embedded
                // XML document resource (the public key).
                CspParameters parms = new CspParameters(1);			// PROV_RSA_FULL
                parms.Flags = CspProviderFlags.UseMachineKeyStore;	// Use Machine store
                foreach (var key in GetKeys(1))
                    parms.KeyContainerName = key;
                parms.KeyNumber = 2;								// AT_SIGNATURE
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider(parms);

                // Load the signed XML license file.
                XmlDocument xmldoc = new XmlDocument();

                Assembly entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null)
                    return;

                FileStream licenseStream = File.Open(licenseFile, FileMode.Open);
                if (licenseStream == null)
                    return;

                byte[] licenseData = new byte[licenseStream.Length];
                licenseStream.Read(licenseData, 0, licenseData.Length);
                string encryptString = Encoding.ASCII.GetString(licenseData);
                foreach (var key in GetKeys(0))
                {
                    foreach (var key1 in DecryptDES(encryptString, key))
                        encryptString = key1;
                }
                StringReader sr = new StringReader(encryptString);
                xmldoc.Load(sr);

                licenseStream.Close();

                // Create the signed XML object.
                SignedXml sxml = new SignedXml(xmldoc);

                try
                {
                    // Get the XML Signature node and load it into the signed XML object.
                    XmlNode dsig = xmldoc.GetElementsByTagName("Signature",
                        SignedXml.XmlDsigNamespaceUrl)[0];
                    sxml.LoadXml((XmlElement)dsig);
                }
                catch
                {
                    Console.Error.WriteLine("Error: no signature found.");
                    return;
                }

                XmlNode trialDaysNode = xmldoc.GetElementsByTagName("TrialDays")[0];
                XmlNode startDateNode = xmldoc.GetElementsByTagName("StartDate")[0];

                trialDaysNode.InnerText = leftDay.ToString();
                if (startDate != null)
                    startDateNode.InnerText = startDate.Value.Date.ToString();

                MemoryStream ms = new MemoryStream();
                xmldoc.Save(ms);

                string encryptResult = string.Empty;
                foreach (var key in GetKeys(2))
                {
                    foreach (var key1 in EncryptDES(Encoding.UTF8.GetString(ms.GetBuffer()), key))
                        encryptResult = key1;
                }
                File.WriteAllText(licenseFile, encryptResult);
                ms.Close();
            }
            catch (System.Exception ex)
            {
            }
        }

        //默认密钥向量
        private static byte[] Keys = { 0xBA, 0xFA, 0xA6, 0xCE, 0x97, 0xCB, 0xAD, 0x1F };

        internal static IEnumerable<string> EncryptDES(string encryptString, string encryptKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            yield return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        internal static IEnumerable<string> DecryptDES(string decryptString, string decryptKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            yield return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public static IEnumerable<string> getMd5(string md5)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] value, hash;
            value = System.Text.Encoding.UTF8.GetBytes(md5);
            hash = md.ComputeHash(value);
            md.Clear();
            string temp = "";
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                temp += hash[i].ToString("x").PadLeft(2, '0');
            }

            yield return temp;
        }
    }
}
