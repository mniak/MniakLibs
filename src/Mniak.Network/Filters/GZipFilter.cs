using System;
using System.IO;
using System.IO.Compression;

namespace Mniak.Network.Filters
{
	public class GZipFilter : IFilter
	{
		public bool Reverse { get; set; }

		public byte[] FilterSend(byte[] data)
		{
			if (!Reverse)
				return Compress(data);
			else
				return Decompress(data);
		}
		public byte[] FilterReceive(byte[] data)
		{
			if (!Reverse)
				return Decompress(data);
			else
				return Compress(data);
		}

		private byte[] Compress(byte[] data)
		{
			if (data == null)
				return null;

			using (MemoryStream compressed = new MemoryStream())
			using (GZipStream gzipStream = new GZipStream(compressed, CompressionMode.Compress))
			{
				gzipStream.Write(data, 0, data.Length);
				gzipStream.Close();
				return compressed.ToArray();
			}
		}
		private byte[] Decompress(byte[] data)
		{
			if (data == null)
				return null;

			try
			{
				byte[] buffer = new byte[4096];
				using (MemoryStream compressed = new MemoryStream(data))
				using (GZipStream gzipStream = new GZipStream(compressed, CompressionMode.Decompress))
				using (MemoryStream uncompressed = new MemoryStream())
				{
					int num;
					while ((num = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						uncompressed.Write(buffer, 0, num);
					}
					return uncompressed.ToArray();
				}
			}
			catch (Exception ex)
			{
				throw new FilterException("Data is not a valid GZip package.", ex);
			}
		}
	}
}
