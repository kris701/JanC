using System.Collections.Generic;
using System.Linq;

namespace Nodes {
	public class AssignStmtNode : BaseASTNode, IStmt, IAssign {
		public AssignStmtNode(JanCParser.AssignStmtContext context, IExpr location, AssignOperator @operator, IExpr value) : base(context) {
			Context = context;
			Location = location;
			Operator = @operator;
			Value = value;
		}
		public JanCParser.AssignStmtContext Context { get; set; }
		public IExpr Location { get; set; }
		public AssignOperator Operator { get; set; }
		public IExpr Value { get; set; }
		public bool HasNumericOperator {
			get => Operator != AssignOperator.Assign;
		}
		public override List<IASTNode> Children {
			get => NodesHelper.NewNodes.With(Location).With(Value);
		}

	}

	public enum AssignOperator {
		Assign,
		PlusAssign,
		MinusAssign,
		MultiplyAssign,
		DivideAssign
	}
}
