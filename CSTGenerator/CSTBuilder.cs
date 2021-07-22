using Antlr4.Runtime;
using Exceptions.ErrorListeners;
using System;
using System.IO;
using Tools;

namespace CSTGenerator {
	public class CSTBuilder : ICSTBuilder {
		public LexerErrorListener LexerErrorListener { get; internal set; }
		public ParserErrorListener ParserErrorListener { get; internal set; }
		public FileInfo SourceFile { get; internal set; }
		public string SourceString { get; internal set; }
		public ParserRuleContext CSTTree { get; internal set; }

		public CSTBuilder(LexerErrorListener lexerErrorListener = null, ParserErrorListener parserErrorListener = null) {
			if (lexerErrorListener == null)
				LexerErrorListener = new LexerErrorListener();
			else
				LexerErrorListener = lexerErrorListener;

			if (parserErrorListener == null)
				ParserErrorListener = new ParserErrorListener();
			else
				ParserErrorListener = parserErrorListener;
		}

		public ICSTBuilder ReadSource(FileInfo file) {
			SourceFile = file;
			SourceString = File.ReadAllText(file.FullName);
			return this;
		}
		public ICSTBuilder ReadSource(string source) {
			SourceString = source;
			return this;
		}

		public ICSTBuilder BuildCST() {
			if (SourceString == null)
				throw new ArgumentNullException("Source string was null!");

			var lexer = new JanCLexer(new AntlrInputStream(SourceString));
			lexer.RemoveErrorListeners();
			lexer.AddErrorListener(LexerErrorListener);

			var parser = new JanCParser(new CommonTokenStream(lexer));
			parser.RemoveErrorListeners();
			parser.BuildParseTree = true;
			parser.AddErrorListener(ParserErrorListener);

			CSTTree = parser.compileUnit();

			if (LexerErrorListener.HadErrors || ParserErrorListener.HadErrors)
				CSTTree = null;

			return this;
		}
	}
}
