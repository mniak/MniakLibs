using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mniak.Network.Filters
{
	[Serializable]
	public class FilterException : Exception
	{
		public FilterException() { }
		public FilterException(string message) : base(message) { }
		public FilterException(string message, Exception inner) : base(message, inner) { }
		protected FilterException(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
