using System.Collections.Generic;

namespace Nodes {
	public class StructLiteralNode : BaseExprNode, ILiteral {
		public StructLiteralNode(JanCParser.StructLiteralContext context, UserDefinedTypeNode type, List<StructLiteralMemberNode> members) : base(context) {
			Type = type;
			Members = members;
			Context = context;
		}

		public new UserDefinedTypeNode Type {
			get => (UserDefinedTypeNode)base.Type;
			set => base.Type = value;
		}
		public string Name => Type.Name;
		public string VariableName { get; set; } // Initialized and used during code generation. Holds the name of the variable that contains the struct data.
		public List<StructLiteralMemberNode> Members { get; }
		public JanCParser.StructLiteralContext Context { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Members); }
	}
}
