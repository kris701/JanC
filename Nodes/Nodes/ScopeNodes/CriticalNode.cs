using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public class CriticalNode : IImprContainer, IStmt {
		public CriticalNode(JanCParser.CriticalStmtContext context, IImpr body) {
			GenericContext = context;
			Body = body;
		}
		public IImpr Body { get; set; }
		public ParserRuleContext GenericContext { get; }
		public List<IASTNode> Children { get => NodesHelper.NewNodes.With(Body); }
	}
}
