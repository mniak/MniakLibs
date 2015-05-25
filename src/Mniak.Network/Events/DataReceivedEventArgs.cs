using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mniak.Network.Events
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(byte[] dataBytes, Encoding encoding)
        {
            this.DataBytes = dataBytes;
            this.DataString = encoding.GetString(dataBytes);
        }
        public DataReceivedEventArgs(string dataString, Encoding encoding)
        {
            this.DataBytes = encoding.GetBytes(dataString);
            this.DataString = dataString;
        }
        public DataReceivedEventArgs(byte[] dataBytes, string dataString)
        {
            this.DataBytes = dataBytes;
            this.DataString = dataString;
        }
        public byte[] DataBytes { get; private set; }
        public string DataString { get; private set; }
    }
}
