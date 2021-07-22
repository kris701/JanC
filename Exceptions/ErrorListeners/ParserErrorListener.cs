using Antlr4.Runtime;
using Exceptions.Exceptions;

namespace Exceptions.ErrorListeners {
	public class ParserErrorListener : SyntaxErrorListener<IToken> {
		protected override void ThrowException(string message) {
			throw new ParserException(GetErrorString());
		}
	}
}
