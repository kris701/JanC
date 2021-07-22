using Antlr4.Runtime;

namespace Nodes {
	public abstract class BaseDeclNode : BaseASTNode, IDecl {
		public BaseDeclNode(ParserRuleContext context, string name) : base(context) {
			NameRef = new StringRef(name);
		}
		public StringRef NameRef { get; }
		public string Name { get => NameRef.Value; set => NameRef.Value = value; }
		// Useful for error messages
		public abstract IToken NameToken { get; }

		public override bool Equals(object obj) {
			if (obj is not BaseDeclNode decl)
				return false;

			return decl.Name == this.Name;
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}
	}
}
