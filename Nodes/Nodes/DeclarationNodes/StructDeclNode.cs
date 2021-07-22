using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public class StructDeclNode : BaseDeclNode, ITypeDecl {
		public StructDeclNode(JanCParser.StructDeclContext context, string name, List<VarDeclNode> members) : base(context, name) {
			Name = name;
			Members = members;
			Context = context;
			Members = members;
			Type = new UserDefinedTypeNode(null, name);
			Type.LinkNameTo(this);
		}
		public JanCParser.StructDeclContext Context { get; }
		public UserDefinedTypeNode Type { get; set; }
		ITypeLiteral ITypedDecl.Type => Type;

		public List<VarDeclNode> Members { get; set; }
		public override IToken NameToken => Context?.name;
		public override List<IASTNode> Children => NodesHelper.NewNodes.With(Type).With(Members);
	}
}
