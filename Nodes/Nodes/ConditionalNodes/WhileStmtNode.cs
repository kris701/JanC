using System.Collections.Generic;

namespace Nodes {
	public class WhileStmtNode : BaseConditionalNode {
		public WhileStmtNode(
			JanCParser.WhileStmtContext context,
			IExpr condition,
			IImpr body) : base(context, condition, body) {
			Context = context;
		}
		public JanCParser.WhileStmtContext Context { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Condition).With(Body); }
	}
}
