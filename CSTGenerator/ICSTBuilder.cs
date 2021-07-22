using Antlr4.Runtime;
using Exceptions.ErrorListeners;
using System.IO;
using Tools;

namespace CSTGenerator {
	public interface ICSTBuilder {
		public LexerErrorListener LexerErrorListener { get; }
		public ParserErrorListener ParserErrorListener { get; }
		public FileInfo SourceFile { get; }
		public string SourceString { get; }
		public ParserRuleContext CSTTree { get; }

		public ICSTBuilder ReadSource(FileInfo file);
		public ICSTBuilder ReadSource(string source);

		public ICSTBuilder BuildCST();
	}
}
