using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mniak.IO
{
    public sealed class TempFileStream : Stream
    {
        private object streamLock = new object();
        private string tempFileName;
        private FileStream innerStream;

        public TempFileStream()
        {
            tempFileName = Path.GetTempFileName();
            innerStream = File.Create(tempFileName);
        }

        public override bool CanRead
        {
            get
            {
                lock (streamLock)
                {
                    return innerStream.CanRead;
                }
            }
        }

        public override bool CanSeek
        {
            get
            {
                lock (streamLock)
                {
                    return innerStream.CanSeek;
                }
            }
        }

        public override bool CanWrite
        {
            get
            {
                lock (streamLock)
                {
                    return innerStream.CanWrite;
                }
            }
        }

        public override long Length
        {
            get
            {
                lock (streamLock)
                {
                    return innerStream.Length;
                }
            }
        }

        public override long Position
        {
            get
            {
                lock (streamLock)
                {
                    return innerStream.Position;
                }
            }

            set
            {
                lock (streamLock)
                {
                    innerStream.Position = value;
                }
            }
        }

        public override void Flush()
        {
            lock (streamLock)
            {
                innerStream.Flush();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (streamLock)
            {
                return innerStream.Read(buffer, offset, count);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (streamLock)
            {
                return innerStream.Seek(offset, origin);
            }
        }

        public override void SetLength(long value)
        {
            lock (streamLock)
            {
                innerStream.SetLength(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (streamLock)
            {
                innerStream.Write(buffer, offset, count);
            }
        }
        protected override void Dispose(bool disposing)
        {
            lock (streamLock)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    innerStream.Dispose();
                    File.Delete(tempFileName);
                }
            }
        }
    }
}
