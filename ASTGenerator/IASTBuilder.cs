using Antlr4.Runtime;
using Exceptions.Syntax;
using Nodes;
using System.Collections.Generic;
using System.IO;
using Tools;

namespace ASTGenerator {
	public interface IASTBuilder {
		public ParserRuleContext SourceCSTTree { get; }
		public IASTNode ASTTree { get; }

		public IASTBuilder ReadSource(ParserRuleContext source);

		public IASTBuilder BuildAST();
	}
}
