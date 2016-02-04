using System;
using System.IO;

namespace Mniak.IO
{
    public sealed class TempFile : IDisposable
    {
        private bool disposed = false;

        public TempFile() : this(Path.GetTempFileName())
        {

        }
        public TempFile(string path)
        {
            this._filename = path;
            FileSystemUtils.CreateDirectoryOfFile(Filename);
        }
        public void Dispose()
        {
            if (disposed)
                return;

            File.Delete(Filename);
        }
        private string _filename;
        public string Filename
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(TempFile));

                return _filename;
            }
        }
    }
}
