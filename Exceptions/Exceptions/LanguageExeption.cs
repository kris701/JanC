using System;

namespace Exceptions.Exceptions {
	public class LanguageException : Exception {
		public LanguageException() : base() { }
		public LanguageException(string message) : base(message) { }

		/// <summary>
		/// Return a human-readable string describing the error
		/// </summary>
		/// <returns></returns>
		public virtual string GetDescription() {
			return this.ToString();
		}
	}
}
