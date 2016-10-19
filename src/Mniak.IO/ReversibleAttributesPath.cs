using System;
using System.IO;

namespace Mniak.IO
{
    public class ReversibleAttributesPath : IDisposable
    {
        FileAttributes attributes;
        private readonly string path;

        public ReversibleAttributesPath(string path)
        {
            this.path = path;
            this.attributes = File.GetAttributes(this.path);
        }

        public ReversibleAttributesPath RemoveAttribute(FileAttributes attribute)
        {
            File.SetAttributes(this.path, this.attributes & ~attribute);
            return this;
        }
        public ReversibleAttributesPath RemoveReadOnly()
        {
            return RemoveAttribute(FileAttributes.ReadOnly);
        }

        public void Dispose()
        {
            File.SetAttributes(this.path, this.attributes);
        }
    }
}
