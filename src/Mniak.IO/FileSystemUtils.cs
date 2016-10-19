using System;
using System.IO;

namespace Mniak.IO
{
    public static class FileSystemUtils
    {
        public static void CreateDirectoryOfFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var dirPath = Path.GetDirectoryName(fileName);
            CreateDirectory(dirPath);
        }

        public static void CreateDirectory(string directoryPath)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(nameof(directoryPath));

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }
    }
}
