using Mniak.Network.Wrappers;
using System;
using System.Net.Sockets;
using System.Threading;
using Mniak.Network.Logging;

namespace Mniak.Network
{
    public class HeaderedWrapper : FilterableWrapper
    {
        private static readonly ILog logger = LogProvider.For<HeaderedWrapper>();
        private int headerLength = 4;
        public int HeaderLength
        {
            get { return headerLength; }
            set
            {
                if (value < 1 || value > 4)
                    throw new ArgumentOutOfRangeException(nameof(HeaderLength), "The value must be in the range of 1 to 4.");
                headerLength = value;
            }
        }

        public HeaderedWrapper(Socket socket)
            : base(socket)
        {

        }

        protected override void InnerSend(byte[] bytes)
        {
            bytes = ProcessFiltersSend(bytes);
            var header = bytes.GetHeader(HeaderLength);
            var all = header.Union(bytes);
            //FIX: Quando dá queda de conexão não está tratando exception
            socket.Send(all);
        }
        protected override byte[] Receive()
        {
            logger.Trace("Waiting for the message header");
            var length = ReadHeader();
            logger.Trace("Received message header: {ByteCount}. Waiting message.", length);
            var body = ReadBody(length);
            logger.Trace("Message received");

            if (body == null)
                return null;

            var data = ProcessFiltersReceive(body);
            return data;
        }

        private ulong ReadHeader()
        {
            var header = new byte[HeaderLength];
            while (running
                    && !socket.Poll(10, SelectMode.SelectRead) || socket.Available > 0
                    && socket.Available < HeaderLength)
            {
                Thread.Sleep(50);
            }

            // If it is not running then abort
            if (!running)
                return 0;
            int n = socket.Receive(header);

            if (n < HeaderLength)
                return 0;
            var length = header.AsInteger();
            return length;
        }
        private byte[] ReadBody(ulong length)
        {
            // If it is not running then abort
            if (!running || length == 0)
                return null;

            // Wait for all bytes available
            Helper.TimeoutLoop(() => running && (ulong)socket.Available < length, 5000);

            // If it is not running then abort
            if (!running)
                return null;

            var buffer = new byte[length];
            var n = socket.Receive(buffer);
            if (n < HeaderLength)
                return null;

            return buffer;
        }
    }
}
