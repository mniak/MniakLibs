using System;
using System.IO;

namespace Mniak.IO
{
    public sealed class TempFile : IDisposable
    {
        private bool disposed = false;

        public TempFile() : this(System.IO.Path.GetTempFileName())
        {

        }
        public TempFile(params string[] paths) : this(System.IO.Path.Combine(paths))
        {
        }
        public TempFile(string path)
        {
            this._path = path;
            FileSystemUtils.CreateDirectoryOfFile(Path);
        }
        public void Dispose()
        {
            if (disposed)
                return;

            File.Delete(Path);
        }
        private string _path;
        public string Path
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(TempFile));

                return _path;
            }
        }
    }
}
