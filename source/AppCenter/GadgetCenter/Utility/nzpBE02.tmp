using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Collections;

namespace GadgetCenter.Utility
{
    public class Hardware
    {
        internal static string GetLocalMacAddress()
        {
            ManagementClass mc;
            ManagementObjectCollection moc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            moc = mc.GetInstances();
            string strRet = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    strRet = mo["MacAddress"].ToString();

            }

            return strRet;
        }

        static internal string GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }

        static internal IEnumerable<string> GetCpuIDs()
        {
            List<string> cpuIdList = new List<string>();
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    cpuIdList.Add(mo.Properties["ProcessorId"].Value.ToString());
                }
            }
            catch
            {
            }

            foreach (string value in cpuIdList)
                yield return value;
        }

        internal static string GetHardDiskID()
        {
            string HDid = "";
            try
            {

                ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    HDid = (string)mo.Properties["Model"].Value;


                }
                return HDid;
            }
            catch
            {
                return "";
            }
        }

        internal static string GetKeyboard()
        {
            string HDid = "";
            try
            {

                ManagementClass cimobject1 = new ManagementClass("Win32_Keyboard");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    foreach (PropertyData pd in mo.Properties)
                    {
                        HDid = pd.Value as string;
                    }
                }
                return HDid;
            }
            catch
            {
                return "";
            }
        }
    }
}
