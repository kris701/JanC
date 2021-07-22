using System.Collections.Generic;

namespace Nodes {
	public class RefCallExprNode : BaseASTNode, IExpr {
		public RefCallExprNode(JanCParser.RefCallExprContext context, ArgNode argument) : base(context) {
			Context = context;
			Argument = argument;
		}
		public JanCParser.RefCallExprContext Context { get; set; }
		public virtual ITypeLiteral Type { get; set; }
		public ArgNode Argument { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Argument); }
	}
}
