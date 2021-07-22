using Antlr4.Runtime.Misc;
using Nodes;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public override IASTNode VisitEveryTaskNode(JanCParser.EveryTaskNodeContext context) {
			IImpr impr = (IImpr)Visit(context.impr());

			return new EveryTaskNode(
				delay: context.delay.Text,
				body: impr,
				context: context
			);
		}

		public override IASTNode VisitOnTaskNode(JanCParser.OnTaskNodeContext context) {
			IExpr condition = (IExpr)Visit(context.condition);
			IImpr body = (IImpr)Visit(context.impr());

			return new OnTaskNode(
				context: context,
				condition: condition,
				body: body
			);
		}

		public override IASTNode VisitOnceTaskNode(JanCParser.OnceTaskNodeContext context) {
			IImpr body = (IImpr)Visit(context.impr());

			return new OnceTaskNode(
				context: context,
				body: body
			);
		}

		public override IASTNode VisitIdleTaskNode(JanCParser.IdleTaskNodeContext context) {
			IImpr body = (IImpr)Visit(context.impr());

			return new IdleTaskNode(
				context: context,
				body: body
			);
		}

		public override IASTNode VisitCriticalStmt(JanCParser.CriticalStmtContext context) {
			IImpr body = (IImpr)Visit(context.impr());

			return new CriticalNode(
				context: context,
				body: body
			);
		}
	}
}
