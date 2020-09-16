using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.Client.UnitTests
{

    [TestFixture]
    class FileServiceTests
    {
        private const string savingpath = @"C:\temp\DMS"; //destination path
        private const string filepath = @"C:\Temp\TestFile.txt"; //source file
        const string newFileName = "newNamedTestFile.txt"; //new name
        const string directory = @"C:\temp"; //new name
        private readonly string _currentYear = DateTime.Now.Year.ToString();


        [Test]
        public void AddFile_CopyFile_IsSuccessful()
        {
            //arrange
            string savingfilepath = savingpath + @"\" + _currentYear; //new file
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {filepath, new MockFileData("Testing is fun")}
            });
            var fileService = new FileService(fileSystem, savingfilepath, "irrelevant");

            //act
            fileService.AddFile(filepath, newFileName, false);

            //assert
            Assert.That(fileSystem.File.Exists(savingfilepath + @"\" + newFileName), Is.True);
            Assert.That(fileSystem.File.Exists(filepath), Is.True);
        }

        [Test]
        public void AddFile_MoveFile_IsSuccessful()
        {
            //arrange
            string savingfilepath = savingpath + @"\" + _currentYear; //new file
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {filepath, new MockFileData("Testing is fun")}
            });
            var fileService = new FileService(fileSystem, savingpath, "irrelevant");

            //act
            fileService.AddFile(filepath, newFileName, true);

            //assert
            Assert.That(fileSystem.File.Exists(savingpath + @"\" + newFileName), Is.True);
            Assert.That(fileSystem.File.Exists(filepath), Is.False);
        }

        [Test]
        public void CopyFile_Copy_CreateFolderStructureIsSuccessful()
        {
            //arrange
            string savingfilepath = savingpath + @"\" + _currentYear; //new file
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {filepath, new MockFileData("Testing is...")}
            });
            var fileService = new FileService(fileSystem, savingpath, "irrelevant");

            //act
            fileService.AddFile(filepath, newFileName, true);

            //assert
            Assert.That(fileSystem.Directory.Exists(savingpath), Is.True);
        }

        [Test]
        public void GetPaths_GetPaths_ReturnsXmlFile()
        {
            string testdirectory = directory + @"\" + _currentYear; //new file
            //arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {testdirectory + @"\" + "testfile.xml", new MockFileData("Testing is fun")},
            });

            var fileService = new FileService(fileSystem, "irrelevant", directory);
            string[] expectedResult = { testdirectory + @"\" + "testfile.xml"};

            //act
            var result = fileService.getPaths();

            //assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }

    }
}
