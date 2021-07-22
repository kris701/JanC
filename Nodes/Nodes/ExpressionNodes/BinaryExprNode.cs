using System.Collections.Generic;

namespace Nodes {
	public class BinaryExprNode : BaseExprNode {
		public BinaryExprNode(JanCParser.BinaryExprContext context, IExpr left, string @operator, IExpr right) : base(context) {
			Context = context;
			Left = left;
			Operator = @operator;
			Right = right;
		}
		public JanCParser.BinaryExprContext Context { get; }
		public string Operator { get; }
		public IExpr Left { get; }
		public IExpr Right { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Left).With(Right); }
	}
}
