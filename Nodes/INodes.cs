using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public interface IASTNode {
		// Used to easily navigate AST tree.
		public List<IASTNode> Children { get; }
		public ParserRuleContext GenericContext { get; }
	}
}
