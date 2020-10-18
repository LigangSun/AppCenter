using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    internal class AdminInformation
    {
        internal static string adminUserName = "Admin";
        internal static string adminPassword = string.Empty;
        internal static readonly string adminKey = LoginInfo.GetMD5Hash("$eg@*d^&");
    }
}
