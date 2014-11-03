using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasySocket.Exceptions
{
	[Serializable]
	public class WrapperNotRunningException : System.Exception
	{
		public WrapperNotRunningException() { }
		public WrapperNotRunningException(string message) : base(message) { }
		public WrapperNotRunningException(string message, System.Exception inner) : base(message, inner) { }
		protected WrapperNotRunningException(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
