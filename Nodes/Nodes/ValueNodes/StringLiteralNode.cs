using System.Collections.Generic;

namespace Nodes {
	public class StringLiteralNode : BaseExprNode, ILiteral {
		public StringLiteralNode(JanCParser.StringLiteralContext context, string value) : base(context) {
			Context = context;
			Type = TypeNode.String;
			Value = value;
		}
		public JanCParser.StringLiteralContext Context { get; }
		public string Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type); }
	}
}
