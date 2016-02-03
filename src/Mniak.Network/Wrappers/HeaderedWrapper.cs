using System;
using System.Threading;
using Mniak.Network.Filters;
using System.Collections.Generic;
using Mniak.Network.Wrappers;
using System.Net.Sockets;

namespace Mniak.Network
{
    public class HeaderedWrapper : FilterableWrapper
    {
        private int headerLength = 4;
        public int HeaderLength
        {
            get { return headerLength; }
            set
            {
                if (value < 1 || value > 4)
                    throw new ArgumentOutOfRangeException("The value of HeaderLength must be in the range of 1 to 4.");
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
            byte[] header = bytes.GetHeader(HeaderLength);
            byte[] all = header.Union(bytes);
            //FIX: Quando dá queda de conexão não está tratando exception
            socket.Send(all);
        }
        protected override byte[] Receive()
        {
            ulong length = ReadHeader();
            byte[] body = ReadBody(length);

            if (body == null)
                return null;

            byte[] data = ProcessFiltersReceive(body);
            return data;
        }

        private ulong ReadHeader()
        {
            byte[] header = new byte[HeaderLength];
            while (running
                    && !socket.Poll(10, SelectMode.SelectRead) || socket.Available > 0
                    && socket.Available < HeaderLength)
            {
                Thread.Sleep(100);
            }

            // If it is not running then abort
            if (!running)
                return 0;
            int n = socket.Receive(header);

            if (n < HeaderLength)
                return 0;
            ulong tamanho = header.AsInteger();

            return tamanho;
        }
        private byte[] ReadBody(ulong length)
        {
            // If it is not running then abort
            if (!running || length < 1)
                return null;

            // Wait for all bytes available
            Helper.TimeoutLoop(() => running && (ulong)socket.Available < length, 5000);

            // If it is not running then abort
            if (!running)
                return null;

            byte[] buffer = new byte[length];
            int n = socket.Receive(buffer);
            if (n < HeaderLength)
                return null;

            return buffer;
        }
    }
}
