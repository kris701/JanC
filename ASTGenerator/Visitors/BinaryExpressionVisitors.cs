using Nodes;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public override IASTNode VisitBinaryExpr(JanCParser.BinaryExprContext context) {
			IExpr left = (IExpr)Visit(context.left);
			IExpr right = (IExpr)Visit(context.right);

			return new BinaryExprNode(
				left: left,
				right: right,
				@operator: context.op.Text,
				context: context
			);
		}
	}
}
