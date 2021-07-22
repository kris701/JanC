using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ASTGenerator.Visitors;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTGeneratorTests {
	public abstract class BaseASTGeneratorTests {

		public static GlobalScopeNode Parse(string input) {
			var lexer = new JanCLexer(new AntlrInputStream(input));
			var parser = new JanCParser(new CommonTokenStream(lexer));
			var compileUnitContext = parser.compileUnit();
			var global = (GlobalScopeNode)new CSTVisitors().Visit((IParseTree)compileUnitContext);
			return global;
		}

		public static T Parse<T>(string input) where T : IASTNode {
			var lexer = new JanCLexer(new AntlrInputStream(input));
			var parser = new JanCParser(new CommonTokenStream(lexer));
			var compileUnitContext = parser.compileUnit();
			var global = (GlobalScopeNode)new CSTVisitors().Visit((IParseTree)compileUnitContext);			
			return global.Content.OfType<T>().First();
		}
	}
}
