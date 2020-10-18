using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoonLearning.AppCenter.License
{
    internal class LicenseAPI
    {
        [DllImport("LicenseMgr")]
        internal extern static IntPtr DecryptPassword(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        internal extern static int IsTrialVersion(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        internal extern static int GetLeftTrialDays(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        internal extern static string GetAppTitle(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        internal extern static string GetAppId(IntPtr licensePtr, int len);

        [DllImport("LicenseMgr")]
        internal extern static int IsLicenseForCurrentMachine(IntPtr licensePtr, int len, IntPtr pszHardwareId);
    }
}
