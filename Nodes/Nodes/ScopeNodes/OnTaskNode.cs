
using System.Collections.Generic;

namespace Nodes {
	public class OnTaskNode : BaseTaskNode {
		public OnTaskNode(JanCParser.OnTaskNodeContext context, IExpr condition, IImpr body) : base(context, "OnTask", body) {
			Context = context;
			Condition = condition;
			Body = body;
		}
		public IExpr Condition { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Body).With(Condition); }
	}
}
