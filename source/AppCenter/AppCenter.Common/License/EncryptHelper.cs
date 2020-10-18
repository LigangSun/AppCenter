using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.License
{
    internal class EncryptHelper
    {
        private static IEnumerable<byte> GetKeys()
        {
            yield return 0xBA;
            yield return 0xFA;
            yield return 0xA6;
            yield return 0xCE;
            yield return 0x97;
            yield return 0xCB;
            yield return 0xAD;
            yield return 0x1F;
        }

        private static IEnumerable<char> GetLicensePassword()
        {
            yield return 'g';
            yield return 'h';
            yield return 'S';
            yield return 'a';
            yield return 'T';
            yield return 'h';
            yield return 'Q';
            yield return 'a';
            yield return '9';
            yield return 'L';
            yield return 'g';
            yield return '=';
        }

        internal static IEnumerable<string> DecryptLicenseFile(string decryptString)
        {
            string decryptKey = string.Empty;
            foreach (char c in GetLicensePassword())
                decryptKey += c;

            byte[] passwordData = Convert.FromBase64String(decryptKey);
            IntPtr passwordPtr = Marshal.AllocHGlobal(passwordData.Length);
            Marshal.Copy(passwordData, 0, passwordPtr, passwordData.Length);
            IntPtr decryptPtr = LicenseAPI.DecryptPassword(passwordPtr, passwordData.Length);
            Marshal.FreeHGlobal(passwordPtr);

            byte[] decryptData = new byte[8];
            Marshal.Copy(decryptPtr, decryptData, 0, 8);

            decryptKey = Encoding.ASCII.GetString(decryptData);

            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);

            List<byte> rgbIVList = new List<byte>();
            foreach (byte key in GetKeys())
                rgbIVList.Add(key);

            byte[] rgbIV = Enumerable.ToArray<byte>(rgbIVList);

            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            string ret = Encoding.UTF8.GetString(mStream.ToArray());

            cStream.Close();
            mStream.Close();

            yield return ret;
        }

        internal static IEnumerable<string> EncryptLicenseFile(string encryptString)
        {
            string encryptKey = string.Empty;
            foreach (char c in GetLicensePassword())
                encryptKey += c;

            byte[] passwordData = Convert.FromBase64String(encryptKey);
            IntPtr passwordPtr = Marshal.AllocHGlobal(passwordData.Length);
            Marshal.Copy(passwordData, 0, passwordPtr, passwordData.Length);
            IntPtr enctryptPtr = LicenseAPI.DecryptPassword(passwordPtr, passwordData.Length);
            Marshal.FreeHGlobal(passwordPtr);

            byte[] enctyptData = new byte[8];
            Marshal.Copy(enctryptPtr, enctyptData, 0, 8);

            encryptKey = Encoding.ASCII.GetString(enctyptData);

            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);

            List<byte> rgbIVList = new List<byte>();
            foreach (byte key in GetKeys())
                rgbIVList.Add(key);

            byte[] rgbIV = Enumerable.ToArray<byte>(rgbIVList);
            
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            yield return Convert.ToBase64String(mStream.ToArray());
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

        internal static IEnumerable<string> ReadFileData(string file)
        {
            yield return File.ReadAllText(file);
        }

        internal static IEnumerable<string> GetHardwareIdMD5()
        {
            foreach (string hdId in Hardware.GetCpuIDs())
            {
                yield return GetMD5Hash(hdId);
            }
        }

        internal static IEnumerable<string> GetHardwareIdDES()
        {
            foreach (string hdId in Hardware.GetCpuIDs())
            {
                foreach (string ret in EncryptLicenseFile(hdId))
                    yield return ret;
            }
        }
    }
}
