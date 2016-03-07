using System;
using System.IO;

namespace Mniak.IO
{
    public sealed class TempDir : IDisposable
    {
        private bool disposed = false;
        public TempDir() : this(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString())
        {

        }
        public TempDir(params string[] paths) : this(System.IO.Path.Combine(paths))
        {

        }
        public TempDir(string path)
        {
            this._path = path;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }
        public void Dispose()
        {
            if (disposed)
                return;

            Directory.Delete(Path, true);
        }
        private string _path;
        public string Path
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(TempDir));

                return _path;
            }
        }
    }
}
