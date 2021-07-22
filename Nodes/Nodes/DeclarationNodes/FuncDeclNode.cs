using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public class FuncDeclNode : BaseDeclNode, IStaticTypeDecl, IImprContainer {
		public FuncDeclNode(JanCParser.FuncDeclContext context,
			string name,
			ITypeLiteral returnType,
			List<VarDeclNode> parameters,
			IImpr impr) : base(context, name) {
			Context = context;
			Parameters = parameters;
			Body = impr;
			ReturnType = returnType;
			Type = new UserDefinedTypeNode(null, name, UserDefinedTypeNode.Types.Function) {Decl = this };
			((UserDefinedTypeNode)Type).LinkNameTo(this);
		}
		public ITypeLiteral Type { get; }
		public ITypeLiteral ReturnType { get; }
		public JanCParser.FuncDeclContext Context { get; set; }
		public List<VarDeclNode> Parameters { get; }
		public IImpr Body { get; set; }
		public override IToken NameToken => Context?.name;
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(ReturnType).With(Parameters).With(Body); }

		public override bool Equals(object obj) {

			if(obj is not FuncDeclNode funcDecl)
				return false;

			if (funcDecl.Name != this.Name)
				return false;

			if (funcDecl.Parameters.Count != this.Parameters.Count)
				return false;

			for(int i=0; i<funcDecl.Parameters.Count; i++) {
				if (!funcDecl.Parameters[i].Equals(this.Parameters[i]))
					return false;
			}

			return true;
		}

		public override int GetHashCode() {
			return Name.GetHashCode() ^ Parameters.GetHashCode();
		}
	}
}
