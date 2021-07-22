using System.Collections.Generic;

namespace Nodes {
	public class ReturnStmtNode : BaseASTNode, IStmt {
		public ReturnStmtNode(JanCParser.ReturnStmtContext context, IExpr value) : base(context) {
			Context = context;
			Value = value;
		}
		public JanCParser.ReturnStmtContext Context { get; set; }
		public IExpr Value { get; set; }
		public ITypeLiteral Type {
			get {
				if (Value?.Type is null)
					return TypeNode.Void;
				return Value.Type;
			}
		}
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Value); }
	}
}
