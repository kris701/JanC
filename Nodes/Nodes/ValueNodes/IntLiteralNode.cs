using System.Collections.Generic;

namespace Nodes {
	public class IntLiteralNode : BaseExprNode, ILiteral {
		public IntLiteralNode(JanCParser.NumberLiteralContext context, int value) : base(context) {
			Context = context;
			Type = TypeNode.Int;
			Value = value;
		}
		public JanCParser.NumberLiteralContext Context { get; }
		public int Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type); }
	}
}
