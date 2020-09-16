using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FakeItEasy;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.Client.UnitTests
{
    [TestFixture]
    class XmlServiceTests
    {
        private const string testPath = @"C:\temp\DMS";
        private DateTime testDateTime = new DateTime(2020, 05, 04, 3, 5, 6);
        [Test]
        public void SerializeMetaDateItem_SerializeXml_ReturnsCorrectReslut()
        {
            //arrange
            var testMetadataItem = new MetadataItem("testName", "testTyp", "", "chs",
                testDateTime, testDateTime, testPath);
            var xmlService = new XmlService();
            string expectedResult =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><_bezeichnung>testName</_bezeichnung><_itemTyp>testTyp</_itemTyp><_stichwoerter /><_benutzer>chs</_benutzer><_path>C:\\temp\\DMS</_path><_valutaDatum>2020-05-04T03:05:06</_valutaDatum><_erfassungsdatum>2020-05-04T03:05:06</_erfassungsdatum></MetadataItem>";

            //act
            string result = xmlService.SerializeMetaDateItem(testMetadataItem);

            //assert
            Assert.That(result, Is.EquivalentTo(expectedResult));

        }


        //Test doesn't work yet..
        [Test]
        public void DeserializeMetadataItem_DeserializeXmlFile_ReturnsCorrectResult()
        {
            //arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {testPath + @"\" + "test.xml", new MockFileData( "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><_bezeichnung>testName</_bezeichnung><_itemTyp>testTyp</_itemTyp><_stichwoerter /><_benutzer>chs</_benutzer><_path>C:\\temp\\DMS</_path><_valutaDatum>2020-05-04T03:05:06</_valutaDatum><_erfassungsdatum>2020-05-04T03:05:06</_erfassungsdatum></MetadataItem>")},
                {testPath + @"\" + "test2.xml", new MockFileData("Testing is not...")}
            });
            var xmlService = new XmlService(fileSystem);
            var expectedResult = new MetadataItem("testName", "testTyp", "", "chs",
                testDateTime, testDateTime, testPath);

            //act
            var result = xmlService.DeserializeMetadataItem(testPath + @"\" + "test.xml");

            //assert
            Assert.AreEqual(result._bezeichnung, expectedResult._bezeichnung);
            Assert.AreEqual(result._itemTyp, expectedResult._itemTyp);
            Assert.AreEqual(result._stichwoerter, expectedResult._stichwoerter);
            Assert.AreEqual(result._benutzer, expectedResult._benutzer);
            Assert.AreEqual(result._path, expectedResult._path);
            Assert.AreEqual(result._valutaDatum, expectedResult._valutaDatum);
            Assert.AreEqual(result._erfassungsdatum, expectedResult._erfassungsdatum);

        }

        [Test]
        public void SaveXml_saving_IsSuccessfull()
        {
            //arrange
            string serializeXml =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Benutzer>TestUser</Benutzer><Bezeichnung>TestFile</Bezeichnung><Erfassungsdatum>2020-01-01T00:00:00</Erfassungsdatum><SelectedTypItem>Vertäge</SelectedTypItem><Stichwoerter>Ich bin ein Test</Stichwoerter><ValutaDatum>2020-02-02T00:00:00</ValutaDatum></MetadataItem>";
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(testPath);
            var XmlService = new XmlService(fileSystem);

            //act
            XmlService.SaveXml(serializeXml, testPath + @"\" + "test.xml");

            //assert
            Assert.That(fileSystem.File.Exists(testPath + @"\" + "test.xml"));
        }

    }
}
