using System;

namespace Exceptions.Exceptions.Base {
	public class AlreadyHandledException : Exception {
		public AlreadyHandledException() : base() { }
		public AlreadyHandledException(string message) : base(message) { }
		public AlreadyHandledException(string message, Exception innerException) : base(message, innerException) { }
	}
}
