using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public class VarDeclNode : BaseDeclNode, ITypedDecl, IConstable, IStmt, IAssign {
		public VarDeclNode(JanCParser.VarDeclContext context, string name, ITypeLiteral type, IExpr value, bool isConst) : base(context, name) {
			Value = value;
			Initialized = value is not null;
			Context = context;
			Type = type;
			IsConst = isConst;
		}
		public JanCParser.VarDeclContext Context { get; }
		public IExpr Value { get; set; }
		public virtual ITypeLiteral Type { get; set; }
		public override IToken NameToken => Context?.name;
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Value); }
		public bool Initialized { get; set; }
		public bool? IsConst { get; set; }
	}
}
