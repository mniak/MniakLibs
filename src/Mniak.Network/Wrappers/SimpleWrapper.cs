using System.Net.Sockets;
namespace Mniak.Network
{
    public class SimpleWrapper : Wrapper
    {
        public SimpleWrapper(Socket socket)
            : base(socket)
        {

        }
        protected override void InnerSend(byte[] bytes)
        {
            socket.Send(bytes);
        }

        protected override byte[] Receive()
        {
            Helper.TimeoutLoop(() => socket.Available == 0, 30000);
            byte[] buffer = new byte[socket.Available];
            socket.Receive(buffer);
            return buffer;
        }
    }
}
