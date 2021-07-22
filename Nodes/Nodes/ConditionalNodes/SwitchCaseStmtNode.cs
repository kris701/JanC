using System.Collections.Generic;

namespace Nodes {
	public class SwitchCaseStmtNode : BaseASTNode, IStmt {
		public SwitchCaseStmtNode(
			JanCParser.SwitchCaseStmtContext context,
			IExpr value,
			IImpr body
		) : base(context) {
			Context = context;
			Value = value;
			Body = body;
		}
		public JanCParser.SwitchCaseStmtContext Context { get; }
		public IExpr Value { get; set; }
		public IImpr Body { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Value).With(Body); }
	}
}
