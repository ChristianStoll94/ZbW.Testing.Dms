using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;
using FakeItEasy;

namespace ZbW.Testing.Dms.Client.UnitTests
{
    [TestFixture]
    class DocumentServiceTests
    {
        private const string TestFilePath = @"C:\temp\testpath.xml";
        [Test]
        public void MetaDataFilter_HasValueType_HasValue()
        {
            //arrange
            var documentService = new DocumentService();
            documentService.MetaDataItems = A.CollectionOfDummy<MetadataItem>(10).ToList();
            documentService.MetaDataItems[0]._itemTyp = "";
            documentService.MetaDataItems[0]._bezeichnung = "Testvalue";
            var selectedTypItem = documentService.MetaDataItems[0]._itemTyp;
            var suchbegriff = documentService.MetaDataItems[0]._bezeichnung;

            //act
            var result = documentService.FilterMetadataItems(suchbegriff, selectedTypItem);

            //asset
            Assert.That(result, !Is.Empty);
        }

        [Test]
        public void AddFileToRepository_AddFile_IsCalled()
        {
            //arrange
            var fileServiceStub = A.Fake<IFileService>();
            var xmlServiceStub = A.Fake<IXmlService>();
            var documentService = new DocumentService(fileServiceStub, xmlServiceStub);
            var metadataItem = A.Fake<MetadataItem>();
            Guid guid = Guid.NewGuid();
            var newFileName = guid + "_" + "_Content";

            //act
            documentService.AddFileToRepository(metadataItem, TestFilePath, false, guid);

            //assert
            A.CallTo(() => fileServiceStub.AddFile(TestFilePath, newFileName, false)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetMetaDataItems_GetMetaDataItems_returnsCorrectAmountOfMetaDataItems()
        {
            //arrange
            var fileServiceStub = A.Fake<IFileService>();
            var xmlServiceStub = A.Fake<IXmlService>();
            var documentService = new DocumentService(fileServiceStub, xmlServiceStub);
            var metadataItem = A.Fake<MetadataItem>();
            List<MetadataItem> metadataItems = A.CollectionOfDummy<MetadataItem>(10).ToList();
            var TestFilePaths = new List<string>() { TestFilePath, TestFilePath};

            A.CallTo(() => fileServiceStub.getPaths()).Returns(TestFilePaths);
            A.CallTo(() => xmlServiceStub.DeserializeMetadataItem(TestFilePath)).Returns(metadataItem);

            //act
            var result = documentService.GetMetaDataItems();

            //assert
            Assert.That(result.Count, Is.EqualTo(TestFilePaths.Count));
        }

    }
}
