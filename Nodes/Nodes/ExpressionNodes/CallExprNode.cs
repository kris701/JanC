using System;
using System.Collections.Generic;

namespace Nodes {
	public class CallExprNode : BaseASTNode, IExpr {
		public CallExprNode(JanCParser.CallExprContext context, IExpr item, List<ArgNode> arguments, ITypeLiteral type = null) : base(context) {
			Context = context;
			Item = item;
			Type = type;
			Arguments = arguments;
		}
		public JanCParser.CallExprContext Context { get; set; }
		public virtual ITypeLiteral Type { get; set; }
		public IExpr Item { get; set; }
		public List<ArgNode> Arguments { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type).With(Item).With(Arguments); }

		public string GetName() => GetName((dynamic)Item);
		private string GetName(IdentifierExpr expr) => expr.Name;
		private string GetName(MemberAccessNode expr) => GetName((dynamic)expr.Item);
		private string GetName(IExpr expr) => throw new NotImplementedException();
	}
}
