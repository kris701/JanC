using Nodes;
using System.Collections.Generic;

namespace Nodes {
	public class MemberAccessNode : BaseASTNode, IConstable, IExpr {
		public MemberAccessNode(JanCParser.MemberAccessContext context, IExpr item, string memberName) : base(context) {
			Context = context;
			Item = item;
			MemberNameRef = new StringRef(memberName);
		}
		public JanCParser.MemberAccessContext Context { get; }
		public ITypeLiteral Type { get; set; }
		public IExpr Item { get; }
		private StringRef MemberNameRef { get; set; }
		public string MemberName { get => MemberNameRef.Value; }
		public void LinkMemberName(IDecl decl) {
			MemberNameRef = decl.NameRef;
		}
		public bool? IsConst { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Item); }
	}
}
