using Exceptions.ErrorListeners;
using Exceptions.Exceptions;
using System;

namespace Exceptions.Syntax {
	public class SemanticErrorListener : BaseErrorAndWarningListener<SemanticException, SemanticException> {
		public void Update(SemanticException e) {
			if (e is ISemanticError) {
				AddError(e);
			}
			else if (e is ISemanticWarning) {
				AddWarning(e);
			}
			else {
				throw new ArgumentException($"Unhandled subtype {e.GetType()} of {nameof(SemanticException)}");
			}
		}

		protected override void ThrowException(string message) {
			throw new SemanticException(message);
		}

		protected override void ThrowWarning(string message) {
			throw new SemanticException(message);
		}

	}
}
