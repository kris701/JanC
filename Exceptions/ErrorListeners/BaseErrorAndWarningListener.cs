using Exceptions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exceptions.ErrorListeners {
	public abstract class BaseErrorAndWarningListener<ErrorType, WarningType> : BaseErrorListener<ErrorType> where ErrorType : LanguageException where WarningType : LanguageException {
		public List<WarningType> Warnings { get; private set; } = new List<WarningType>();
		public bool HadWarnings => Warnings.Count > 0;

		protected void AddWarning(WarningType warning) {
			Warnings.Add(warning);
		}

		internal virtual void ThrowIfWarning() {
			if (Warnings.Count > 0)
				ThrowException(GetWarningString());
		}

		protected abstract void ThrowWarning(string message);

		internal string GetWarningString() {
			return string.Join(Environment.NewLine, Warnings.Select(i => i.ToString()));
		}
	}
}
