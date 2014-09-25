using System;
namespace EasySocket
{
	public class SimpleWrapper : Wrapper
	{
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
