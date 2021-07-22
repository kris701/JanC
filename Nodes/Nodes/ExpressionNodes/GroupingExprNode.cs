using System.Collections.Generic;

namespace Nodes {
	public class GroupingExprNode : BaseExprNode, IUnary {
		public GroupingExprNode(JanCParser.GroupingExprContext context, IExpr value) : base(context) {
			Context = context;
			Value = value;
		}
		public JanCParser.GroupingExprContext Context { get; }
		public override ITypeLiteral Type => Value.Type;
		public IExpr Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Value); }
	}
}
