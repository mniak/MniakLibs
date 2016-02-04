using System.IO;

namespace Mniak.IO
{
    public sealed class HybridStream : Stream
    {
        private const long DEFAULT_THRESHOLD = 1 * 1024 * 1024; // 1 MB

        private object streamLock = new object();
        private bool usingMemory;
        private Stream innerStream;

        public HybridStream(long threshold = DEFAULT_THRESHOLD)
        {
            innerStream = new MemoryStream();
            usingMemory = true;
            Threshold = threshold;
        }
        public HybridStream(byte[] initialData, long threshold = DEFAULT_THRESHOLD) : this(threshold)
        {
            this.Write(initialData, 0, initialData.Length);
        }
        public long Threshold { get; private set; }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return true;
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
                var result = innerStream.Read(buffer, offset, count);
                return result;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (streamLock)
            {
                var result = innerStream.Seek(offset, origin);
                return result;
            }
        }

        public override void SetLength(long value)
        {
            lock (streamLock)
            {
                InnerSetLength(value);
            }
        }

        private void InnerSetLength(long value)
        {
            if (usingMemory && value > innerStream.Length && value > Threshold)
            {
                usingMemory = false;
                var newStream = new TempFileStream();

                long oldPosition = innerStream.Position;
                innerStream.Position = 0;

                newStream.SetLength(value);
                innerStream.CopyTo(newStream);
                innerStream.Dispose();

                newStream.Position = oldPosition;
                innerStream = newStream;
            }
            else if (!usingMemory && value < innerStream.Length && value <= Threshold)
            {
                usingMemory = true;
                var newStream = new MemoryStream();

                long oldPosition = innerStream.Position;
                innerStream.Position = 0;

                newStream.SetLength(value);
                innerStream.CopyTo(newStream);
                innerStream.Dispose();

                newStream.Position = oldPosition;
                innerStream = newStream;
            }
            else
            {
                innerStream.SetLength(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (streamLock)
            {
                InnerSetLength(Position + count);
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
                }
            }
        }
    }
}
