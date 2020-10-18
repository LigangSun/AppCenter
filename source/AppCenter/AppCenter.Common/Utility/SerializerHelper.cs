using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SoonLearning.AppCenter.Utility
{
    public static class SerializerHelper<T>
    {
        private static XmlSerializer xmlSerializer;

        private static XmlSerializer GetXmlSerializer()
        {
            if (xmlSerializer == null)
                xmlSerializer = new XmlSerializer(typeof(T));
            return xmlSerializer;
        }

        public static void XmlSerialize(Stream stream, T obj)
        {
            XmlSerializer serializer = GetXmlSerializer();
            serializer.Serialize(stream, obj);
        }

        public static void XmlSerialize(string file, T obj)
        {
            using (FileStream fs = File.OpenWrite(file))
            {
                XmlSerialize(fs, obj);
            }
        }

        public static void CompressSerialize(string file, T obj)
        {
            string tempFile = Path.GetTempFileName();
            XmlSerialize(tempFile, obj);

            Compression.CompressFile(tempFile, file);

            try
            {
                File.Delete(tempFile);
            }
            catch
            {
            }
        }

        public static T XmlDeserialize(Stream stream)
        {
            XmlSerializer serializer = GetXmlSerializer();
            return (T)serializer.Deserialize(stream);
        }

        public static T XmlDeserialize(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                return XmlDeserialize(fs);
            }
        }

        public static T DecompressDeserialize(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                MemoryStream ms = new MemoryStream();
                Compression.DecompressStream(fs, ms);
                return XmlDeserialize(ms);
            }
        }
    }
}
