using Nodes;
using System.Collections.Generic;
using System.Linq;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public override IASTNode VisitIfStmt(JanCParser.IfStmtContext context) {
			IExpr condition = (IExpr)Visit(context.condition);
			IImpr thenBody = (IImpr)Visit(context.thenBody);
			IImpr elseBody = null;
			if (context.elseBody != null)
				elseBody = (IImpr)Visit(context.elseBody);

			return new IfStmtNode(
				condition: condition,
				thenBody: thenBody,
				elseBody: elseBody,
				context: context
			);
		}

		public override IASTNode VisitWhileStmt(JanCParser.WhileStmtContext context) {
			IExpr condition = (IExpr)Visit(context.condition);
			IImpr body = (IImpr)Visit(context.body);

			return new WhileStmtNode(
				condition: condition,
				body: body,
				context: context
			);
		}

		public override IASTNode VisitForStmt(JanCParser.ForStmtContext context) {
			IImpr init = (IImpr)Visit(context.init);
			IExpr condition = (IExpr)Visit(context.condition);
			IImpr update = (IImpr)Visit(context.iteration);
			IImpr body = (IImpr)Visit(context.body);

			return new ForStmtNode(
				init: init,
				condition: condition,
				update: update,
				body: body,
				context: context
			);
		}

		public override IASTNode VisitSwitchStmt(JanCParser.SwitchStmtContext context) {
			IExpr value = (IExpr)Visit(context.value);
			List<SwitchCaseStmtNode> casesList = context.switchCaseStmt().Select(i => (SwitchCaseStmtNode)Visit(i)).ToList();
			IImpr defaultCase = null;
			if (context.defaultCase != null)
				defaultCase = (IImpr)Visit(context.defaultCase);

			return new SwitchStmtNode(
				value: value,
				cases: casesList,
				defaultCase: defaultCase,
				context: context
			);
		}

		public override IASTNode VisitSwitchCaseStmt(JanCParser.SwitchCaseStmtContext context) {
			IExpr value = (IExpr)Visit(context.value);
			IImpr body = (IImpr)Visit(context.impr());

			return new SwitchCaseStmtNode(
				value: value,
				body: body,
				context: context
			);
		}
	}
}
