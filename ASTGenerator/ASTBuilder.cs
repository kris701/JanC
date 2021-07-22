using Antlr4.Runtime;
using ASTGenerator.Visitors;
using Exceptions.Syntax;
using Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using Tools;

namespace ASTGenerator {
	public class ASTBuilder : IASTBuilder {
		public ParserRuleContext SourceCSTTree { get; internal set; }
		public IASTNode ASTTree { get; internal set; }

		public ASTBuilder() { }

		public IASTBuilder ReadSource(ParserRuleContext source) {

			SourceCSTTree = source;

			return this;
		}

		public IASTBuilder BuildAST() {

			if (SourceCSTTree == null)
				throw new ArgumentNullException();

			CSTVisitors cSTVisitors = new CSTVisitors();
			ASTTree = cSTVisitors.Visit(SourceCSTTree);

			return this;
		}
	}
}
