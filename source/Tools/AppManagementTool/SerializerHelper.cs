using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SoonLearning.AppManagementTool
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
    }
}
