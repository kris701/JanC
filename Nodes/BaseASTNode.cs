using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public abstract class BaseASTNode : IASTNode {
		public BaseASTNode(ParserRuleContext context) {
			GenericContext = context;
		}
		public ParserRuleContext GenericContext { get; }
		public abstract List<IASTNode> Children { get; }
	}
}
