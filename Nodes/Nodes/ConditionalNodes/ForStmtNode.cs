using System.Collections.Generic;

namespace Nodes {
	public class ForStmtNode : BaseConditionalNode {
		public ForStmtNode(
			JanCParser.ForStmtContext context,
			IExpr condition,
			IImpr body,
			IImpr init,
			IImpr update) : base(context, condition, body) {
			Context = context;
			Init = init;
			Update = update;
		}
		public IImpr Init { get; }
		public IImpr Update { get; }
		public JanCParser.ForStmtContext Context { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Init).With(Condition).With(Update).With(Body); }
	}
}
