using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Mniak.Network.Sockets
{
    public interface ISocket
    {
        AddressFamily AddressFamily { get; }
        int Available { get; }
        bool Blocking { get; set; }
        bool Connected { get; }
        bool DontFragment { get; set; }
        bool EnableBroadcast { get; set; }
        bool ExclusiveAddressUse { get; set; }
        IntPtr Handle { get; }
        bool IsBound { get; }
        LingerOption LingerState { get; set; }
        EndPoint LocalEndPoint { get; }
        bool MulticastLoopback { get; set; }
        bool NoDelay { get; set; }
        ProtocolType ProtocolType { get; }
        int ReceiveBufferSize { get; set; }
        int ReceiveTimeout { get; set; }
        EndPoint RemoteEndPoint { get; }
        int SendBufferSize { get; set; }
        int SendTimeout { get; set; }
        SocketType SocketType { get; }
        SslProtocols SslProtocols { get; set; }
        short Ttl { get; set; }
        bool UseOnlyOverlappedIO { get; set; }

        SecureSocket Accept();
        Task<SecureSocket> AcceptAsync();
        IAsyncResult BeginAccept(AsyncCallback callback, object state);
        IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state);
        IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state);
        IAsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncCallback callback, object state);
        void Bind(EndPoint localEP);
        void Close();
        void Close(int timeout);
        void Connect(string host, int port);
        Task<bool> ConnectAsync(string host, int port);
        void Disconnect(bool reuseSocket);
        void Dispose();
        SecureSocket EndAccept(IAsyncResult asyncResult);
        bool EndConnect(IAsyncResult asyncResult);
        int EndReceive(IAsyncResult asyncResult);
        int EndSend(IAsyncResult asyncResult);
        object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName);
        byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength);
        void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        void Listen(int backlog);
        void LoadClientCertificate(X509Certificate2 certificate);
        void LoadServerCertificate(X509Certificate2 certificate);
        int Receive(byte[] buffer);
        int Receive(byte[] buffer, int offset, int count);
        int Send(byte[] buffer);
        int Send(byte[] buffer, int offset, int size);
        void Shutdown(SocketShutdown how);
    }
}