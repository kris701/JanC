using Antlr4.Runtime;
using Exceptions.Exceptions;
using System.IO;

namespace Exceptions.Syntax {
	public class SyntaxError<T> : LanguageException {
		public TextWriter Output { get; set; }
		public IRecognizer Recognizer { get; set; }
		public T OffendingSymbol { get; set; }
		public int Line { get; set; }
		public int CharPositionInLine { get; set; }
		public string Msg { get; set; }
		public RecognitionException RecognitionException { get; set; }

		public SyntaxError(TextWriter output, IRecognizer recognizer, T offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException recognitionException) {
			Output = output;
			Recognizer = recognizer;
			OffendingSymbol = offendingSymbol;
			Line = line;
			CharPositionInLine = charPositionInLine;
			Msg = msg;
			RecognitionException = recognitionException;
		}

		public override string ToString() {
			// Example message when writing 'evefry' instead of 'every':
			// line 3:0 extraneous input 'evefry' expecting {<EOF>, 'every', 'int', 'str'}
			return $"Line {Line}:{CharPositionInLine} {Msg}";
		}
	}
}
