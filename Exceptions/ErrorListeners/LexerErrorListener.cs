using Exceptions.Exceptions;

namespace Exceptions.ErrorListeners {
	public class LexerErrorListener : SyntaxErrorListener<int> {
		protected override void ThrowException(string message) {
			throw new LexerException(GetErrorString());
		}
	}
}
