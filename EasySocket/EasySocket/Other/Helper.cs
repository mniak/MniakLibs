using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySocket.Exceptions;
using System.Threading;
using System.Net.Sockets;

namespace EasySocket
{
	internal static class Helper
	{
		public static UInt64 AsInteger(this byte[] bytes)
		{
			ulong result = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				result *= 256;
				result += bytes[i];
			}
			return result;
		}
		public static byte[] GetHeader(this byte[] bytes, int headerLength)
		{
			int length = bytes.Length;
			byte[] header = new byte[headerLength];
			for (int i = header.Length - 1; i >= 0; i--)
			{
				header[i] = (byte)(length % 256);
				length /= 256;
			}
			return header;
		}
		public static byte[] Union(this byte[] bytes1, byte[] bytes2)
		{
			byte[] buffer = new byte[bytes1.Length + bytes2.Length];
			Buffer.BlockCopy(bytes1, 0, buffer, 0, bytes1.Length);
			Buffer.BlockCopy(bytes2, 0, buffer, bytes1.Length, bytes2.Length);
			return buffer;
		}
		public static void TimeoutLoop(Func<bool> conditional, int timeout)
		{
			DateTime initial = DateTime.Now;
			while ((DateTime.Now - initial).Milliseconds < timeout && conditional())
			{
				Thread.Sleep(50);
			}
		}
	}
}
