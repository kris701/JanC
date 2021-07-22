using System.Collections.Generic;

namespace Nodes {
	public class SwitchStmtNode : BaseASTNode, IStmt {
		public SwitchStmtNode(
			JanCParser.SwitchStmtContext context,
			IExpr value,
			List<SwitchCaseStmtNode> cases,
			IImpr defaultCase
		) : base(context) {
			Context = context;
			Value = value;
			Cases = cases;
			DefaultCase = defaultCase;
		}
		public JanCParser.SwitchStmtContext Context { get; }
		public IExpr Value { get; set; }
		public List<SwitchCaseStmtNode> Cases { get; set; }
		public IImpr DefaultCase { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Value).With(Cases).With(DefaultCase); }
	}
}
