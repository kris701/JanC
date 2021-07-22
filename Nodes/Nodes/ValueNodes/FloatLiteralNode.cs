using System.Collections.Generic;

namespace Nodes {
	public class FloatLiteralNode : BaseExprNode, ILiteral {
		public FloatLiteralNode(JanCParser.NumberLiteralContext context, double value) : base(context) {
			Context = context;
			Type = TypeNode.Float;
			Value = value;
		}
		public JanCParser.NumberLiteralContext Context { get; }
		public double Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type); }
	}
}
