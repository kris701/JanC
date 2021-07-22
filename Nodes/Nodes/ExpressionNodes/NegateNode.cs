using System.Collections.Generic;

namespace Nodes {
	public class NegateNode : BaseExprNode, IUnary {
		public NegateNode(JanCParser.UnaryExprContext context, IExpr value) : base(context) {
			Context = context;
			Value = value;
		}
		public JanCParser.UnaryExprContext Context { get; }
		public IExpr Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Value); }
	}
}
