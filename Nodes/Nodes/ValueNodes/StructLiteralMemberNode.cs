using System.Collections.Generic;

namespace Nodes {
	public class StructLiteralMemberNode : BaseASTNode {
		public StructLiteralMemberNode(JanCParser.StructLiteralMemberContext context, string name, IExpr value) : base(context) {
			NameRef = new StringRef(name);
			Value = value;
			Context = context;
		}
		public string Name { get => NameRef.Value; set => NameRef.Value = value; }
		public StringRef NameRef { get; private set; }
		public void LinkNameTo(VarDeclNode decl) {
			NameRef = decl.NameRef;
		}

		public IExpr Value { get; }
		public JanCParser.StructLiteralMemberContext Context { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Value); }
	}
}
