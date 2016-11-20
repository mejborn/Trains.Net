using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Utility
{
    public static class FileIOUtils
    {
        public static void SaveObject<T>(T o,string fileName)
        {
            if (o == null)
                throw new Exception("Null object");
            XmlSerializer writer = new XmlSerializer(o.GetType());
            using (StreamWriter file = new StreamWriter(fileName))
            {
                writer.Serialize(file, o);
            }
        }

        public static T LoadObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }
            T obj = default(T);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            string xmlString = xmlDocument.OuterXml;
            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(T);
                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    obj = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
                read.Close();
            }
            return obj;
        }
    }
}
