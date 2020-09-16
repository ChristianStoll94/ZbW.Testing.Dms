using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace ZbW.Testing.Dms.Client.Services
{

    public interface IFileService
    {
        void AddFile(string filepath, string newFileName, bool isRemoveEnabled);

        string GetFileExtension(string filepath);

        List<string> getPaths();

        

    }

    public class FileService : IFileService
    {
        private readonly IFileSystem _filesystem;

        private readonly string _savingPath;

        private readonly string _directory;


        public FileService(IFileSystem fileSystem, string savingPath, string directory)
        {
            _filesystem = fileSystem;
            _savingPath = savingPath;
            _directory = directory;
        }

        public FileService(string savingPath, string directory)
        {
            _filesystem = new FileSystem();
            _savingPath = savingPath;
            _directory = directory;
        }

        public void AddFile(string filepath, string newFileName, bool isRemoveEnabled)
        {
            if (!_filesystem.Directory.Exists(_savingPath))
            {
                _filesystem.Directory.CreateDirectory(_savingPath);
            }

            var newFilePath = _savingPath + @"\" + newFileName;

            if (isRemoveEnabled)
            {
                _filesystem.File.Move(filepath, newFilePath);
            }
            else
            {
                _filesystem.File.Copy(filepath, newFilePath);
            }
        }

        public string GetFileExtension(string filepath)
        {
            return _filesystem.Path.GetExtension(filepath);
        }


        public List<string> getPaths()
        {
            var paths = new List<string>();
            string[] subDirectories = _filesystem.Directory.GetDirectories(_directory);
            foreach (var subDirectory in subDirectories)
            {
                foreach (var xmlFile in _filesystem.Directory.GetFiles(subDirectory, "*.xml"))
                {
                    paths.Add(xmlFile);
                }
            }
            return paths;
        }
    }
}
