using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GadgetCenter.License
{
    internal class LicenseAPI
    {
        [DllImport("LicenseMgr")]
        public extern static IntPtr DecryptPassword(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        public extern static int IsTrialVersion(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        public extern static int GetLeftTrialDays(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        public extern static string GetAppTitle(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        public extern static string GetAppId(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        public extern static string IsLicenseForCurrentMachine(IntPtr licensePtr, int len, IntPtr pszHardwareId);
    }
}
