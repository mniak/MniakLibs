using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateReferences
{
    internal class ReversibleAttributes : IDisposable
    {
        FileAttributes attributes;
        private readonly string path;

        public ReversibleAttributes(string path)
        {
            this.path = path;
            this.attributes = File.GetAttributes(this.path);
            File.SetAttributes(this.path, this.attributes & ~FileAttributes.ReadOnly);
        }

        public void Dispose()
        {
            File.SetAttributes(this.path, this.attributes);
        }
    }
}
