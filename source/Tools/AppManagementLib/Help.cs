using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SoonLearning.AppManagementLib
{
    internal class Help
    {
        internal static void WriteString(Stream stream, string text)
        {
            byte[] textData = Encoding.UTF8.GetBytes(text);
            byte[] textDataLength = BitConverter.GetBytes(textData.Length);
            stream.Write(textDataLength, 0, 4);
            stream.Write(textData, 0, textData.Length);
        }

        internal static void WriteBytes(Stream stream, byte[] bytes)
        {
            byte[] byteLen = BitConverter.GetBytes(bytes.Length);
            stream.Write(byteLen, 0, 4);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
