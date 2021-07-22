using System.Collections.Generic;

namespace Nodes {
	public class IdentifierExpr : BaseExprNode, IConstable {
		public IdentifierExpr(JanCParser.IdentifierContext context, string name, ITypeLiteral type = null) : base(context) {
			Context = context;
			NameRef = new StringRef(name);
			Type = type;
			IsConst = false;
	}
		public ITypedDecl Decl { get; set; } = null;
		public JanCParser.IdentifierContext Context { get; set; }
		private StringRef NameRef { get; set; }
		public string Name { get => NameRef.Value; }
		/// <summary>
		/// Links the name of this identifier to the name of the declaration.
		/// If the declaration is then renamed, this identifier would be renamed too.
		/// </summary>
		/// <param name="decl"></param>
		public void LinkNameTo(IDecl decl) {
			NameRef = decl.NameRef;
		}
		/// <summary>
		/// Renames only this identifier, without affecting the declaration and other identifiers.
		/// Unlinks the name if it was linked to a declaration before.
		/// </summary>
		/// <param name="name"></param>
		public void LocalRename(string name) {
			NameRef = new StringRef(name);
		}
		public bool? IsConst { get; set; }
		public override List<IASTNode> Children {
			get => NodesHelper.NewNodes.With(Type);
		}
	}
}
