using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppAdminTool
{
    internal class AdminInformation
    {
        internal static readonly string adminUserName = "Admin";
        internal static readonly string adminPassword = Help.GetMD5Hash("SoonLearning_123");
        internal static readonly string adminKey = Help.GetMD5Hash("$eg@*d^&");
    }
}
