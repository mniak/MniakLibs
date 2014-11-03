using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasySocket.Filters
{
	public interface IFilter
	{
		byte[] FilterSend(byte[] data);
		byte[] FilterReceive(byte[] data);
	}
}
