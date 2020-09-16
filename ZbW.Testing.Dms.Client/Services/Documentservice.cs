using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions.Behaviors;
using ZbW.Testing.Dms.Client.Model;

namespace ZbW.Testing.Dms.Client.Services
{
    public class DocumentService : BindableBase
    {
        private readonly string _savingPath;
        private readonly string _directory;
        private List<MetadataItem> _metaDataItems;
        private readonly IFileService _fileService;
        private readonly IXmlService _xmlService;
        private readonly string _currentYear = DateTime.Now.Year.ToString();

        public List<MetadataItem> MetaDataItems
        {
            get => _metaDataItems;
            set => SetProperty(ref _metaDataItems, value);
        }


        public DocumentService()
        {
            _directory = ConfigurationManager.AppSettings["RepositoryDir"];
            _savingPath = _directory + @"\" + _currentYear;
            _fileService = new FileService(_savingPath, _directory);
            _xmlService = new XmlService();
        }

        public DocumentService(IFileService fileService, IXmlService xmlService)
        {
            _directory = ConfigurationManager.AppSettings["RepositoryDir"];
            _savingPath = _directory + @"\" + _currentYear;
            _fileService = fileService;
            _xmlService = xmlService;
        }

        public void AddFileToRepository(MetadataItem metadataItem, string filepath, bool isRemoveFileEnabled, Guid guid)
        {
            var newFileName = guid + "_" + "_Content" + _fileService.GetFileExtension(filepath);
            var xmlFileName = @"\" + guid + "_Metadata.xml";
            var xmlFilePath = _savingPath + xmlFileName;

            metadataItem._path = _savingPath + @"\" + newFileName;

            _fileService.AddFile(filepath, newFileName, isRemoveFileEnabled);
            _xmlService.SaveXml(_xmlService.SerializeMetaDateItem(metadataItem), xmlFilePath);
        }

        public void OpenFile(string filePath)
        {
            System.Diagnostics.Process.Start(filePath);
        }


        public List<MetadataItem> GetMetaDataItems()
        {
            var metaDataItems = new List<MetadataItem>();
            var paths = _fileService.getPaths();

            foreach (var path in paths)
            {
                metaDataItems.Add(_xmlService.DeserializeMetadataItem(path));
            }

            return metaDataItems;
        }

        public List<MetadataItem> FilterMetadataItems(string suchbegriff, string selectedTypItem)
        {

            var MetaDataItems = this.MetaDataItems;

            var filteredMetaDataItems = new List<MetadataItem>();

            if (string.IsNullOrEmpty(suchbegriff))
                suchbegriff = "";

            foreach (var metaDataItem in MetaDataItems)
            {
                if (metaDataItem._bezeichnung != null)
                {
                    if ((metaDataItem._bezeichnung.Contains(suchbegriff) || metaDataItem._stichwoerter.Contains(suchbegriff))
                        && (metaDataItem._itemTyp.Equals(selectedTypItem)  || string.IsNullOrEmpty(selectedTypItem)))
                {
                    filteredMetaDataItems.Add(metaDataItem);
                }
                }
            }

            return filteredMetaDataItems;
        }
    }
}
