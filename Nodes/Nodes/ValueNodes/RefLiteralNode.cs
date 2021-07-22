using System.Collections.Generic;

namespace Nodes {
	public class RefLiteralNode : BaseExprNode, ILiteral {
		public RefLiteralNode(JanCParser.LiteralTypeContext context, ILiteral value) : base(context) {
			Context = context;
			Type = TypeNode.Ref(value.Type);
			Value = value;
		}
		public JanCParser.LiteralTypeContext Context { get; }
		public ILiteral Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type); }
	}
}
