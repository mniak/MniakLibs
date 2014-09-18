using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasySocket.Exception
{
	[Serializable]
	public class EasySocketException : System.Exception
	{
		public EasySocketException() { }
		public EasySocketException(string message) : base(message) { }
		public EasySocketException(string message, System.Exception inner) : base(message, inner) { }
		protected EasySocketException(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
