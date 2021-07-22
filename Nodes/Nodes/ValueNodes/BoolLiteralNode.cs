using System.Collections.Generic;

namespace Nodes {
	public class BoolLiteralNode : BaseASTNode, ILiteral {
		public BoolLiteralNode(JanCParser.BoolLiteralContext context, bool value) : base(context) {
			Context = context;
			Type = TypeNode.Bool;
			Value = value;
		}
		public JanCParser.BoolLiteralContext Context { get; }
		public ITypeLiteral Type { get; set; }
		public bool Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type); }
	}
}
