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

            try
            {
                Directory.Delete(Path, true);
            }
            catch (UnauthorizedAccessException)
            {
                var files = new DirectoryInfo(Path).EnumerateFiles("*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    f.Attributes = f.Attributes & ~FileAttributes.ReadOnly;
                    f.Delete();
                }
                Directory.Delete(Path, true);
            }
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
