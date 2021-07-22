using System.Collections.Generic;

namespace Nodes {
	public class NotNode : BaseExprNode, IUnary {
		public NotNode(JanCParser.NotNodeContext context, IExpr value) : base(context) {
			Context = context;
			Value = value;
		}
		public JanCParser.NotNodeContext Context { get; }
		public IExpr Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Value); }
	}
}
