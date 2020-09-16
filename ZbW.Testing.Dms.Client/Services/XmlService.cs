using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.Client.Services
{
    public interface IXmlService
    {
        string SerializeMetaDateItem(MetadataItem metaDataItem);
        void SaveXml(string serializeXml, string path);
        MetadataItem DeserializeMetadataItem(string path);
    }

    public class XmlService : IXmlService
    {
        private readonly IFileSystem _filesystem;

        public XmlService()
        {
            _filesystem = new FileSystem();
        }

        public XmlService(IFileSystem fileSystem)
        {
            _filesystem = fileSystem;
        }


        public string SerializeMetaDateItem(MetadataItem metaDataItem)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MetadataItem));
            StringWriter stringWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter);

            xmlSerializer.Serialize(writer, metaDataItem);
            var serializeXml = stringWriter.ToString();
            writer.Close();
            return serializeXml;
        }

        public void SaveXml(string serializeXml, string path)
        {
            using (Stream fs = (Stream) _filesystem.FileStream.Create(path, FileMode.CreateNew))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(serializeXml);
                xmlDoc.Save(fs);
                fs.Flush();
                fs.Position = 0;
            }
        }

        public MetadataItem DeserializeMetadataItem(string path)
        {
            {
                using (Stream fs = (Stream) _filesystem.FileStream.Create(path, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MetadataItem));
                    StreamReader reader = new StreamReader(fs);

                    var metadataItem = (MetadataItem) serializer.Deserialize(reader);
                    reader.Close();

                    return metadataItem;
                }
            }
        }
    }





}
