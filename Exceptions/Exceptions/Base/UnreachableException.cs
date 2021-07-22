using System;

namespace Exceptions.Exceptions.Base {
	// The program has entered a state that was assumed to be unreachable.
	// This exception always indicates a bug in the compiler, not a run-time issue.
	public class UnreachableException : Exception {
		public UnreachableException() : base() { }
		public UnreachableException(string message) : base(message) { }
		public UnreachableException(string message, Exception innerException) : base(message, innerException) { }
	}
}
