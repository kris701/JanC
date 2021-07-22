using Exceptions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exceptions.ErrorListeners {
	public abstract class BaseErrorListener<ErrorType> where ErrorType : LanguageException {
		public List<ErrorType> Errors { get; private set; } = new List<ErrorType>();
		public bool HadErrors => Errors.Count > 0;
		public bool ThrowErrorsImmediately { get; set; }
		protected BaseErrorListener(bool throwImmediately = false) {
			ThrowErrorsImmediately = throwImmediately;
		}

		protected void AddError(ErrorType error) {
			Errors.Add(error);
			if (ThrowErrorsImmediately)
				ThrowIfError();
		}

		public virtual void ThrowIfError() {
			if (Errors.Count > 0)
				ThrowException(GetErrorString());
		}

		protected abstract void ThrowException(string message);

		public string GetErrorString() {
			return string.Join(Environment.NewLine, Errors.Select(i => i.ToString()));
		}
	}
}
