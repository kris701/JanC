using Antlr4.Runtime;
using Exceptions.Syntax;
using System.IO;

namespace Exceptions.ErrorListeners {
	public abstract class SyntaxErrorListener<T> : BaseErrorListener<SyntaxError<T>>, IAntlrErrorListener<T> {
		public void SyntaxError(TextWriter output, IRecognizer recognizer, T offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
			AddError(new SyntaxError<T>(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e));
		}
	}
}
