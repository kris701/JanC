using System.Collections.Generic;

namespace Nodes {
	public class IfStmtNode : BaseConditionalNode {
		public IfStmtNode(
			JanCParser.IfStmtContext context,
			IExpr condition,
			IImpr thenBody,
			IImpr elseBody) : base(context, condition, thenBody) {
			Context = context;
			ElseBody = elseBody;
		}
		public JanCParser.IfStmtContext Context { get; }
		public IImpr ElseBody { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Condition).With(Body).With(ElseBody); }
	}
}
