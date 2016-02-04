using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mniak.IO
{
    public sealed class TempDirectory : IDisposable
    {
        private bool disposed = false;
        public TempDirectory(string path)
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
                    throw new ObjectDisposedException("TemporaryDirectory");

                return _path;
            }
        }
    }
}
