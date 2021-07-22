using Antlr4.Runtime.Misc;
using Exceptions.Exceptions.Base;
using Nodes;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public override IASTNode VisitGroupingExpr(JanCParser.GroupingExprContext context) {
			IExpr value = (IExpr)Visit(context.expr());

			return new GroupingExprNode(
				value: value,
				context: context
			);
		}

		public override IASTNode VisitAssignStmt(JanCParser.AssignStmtContext context) {
			IExpr location = (IExpr)Visit(context.location);
			IExpr value = (IExpr)Visit(context.value);
			AssignOperator? @operator = context.op.Text switch {
				"=" => AssignOperator.Assign,
				"+=" => AssignOperator.PlusAssign,
				"-=" => AssignOperator.MinusAssign,
				"*=" => AssignOperator.MultiplyAssign,
				"/=" => AssignOperator.DivideAssign,
				_ => null
			};
			if (@operator is null) {
				throw new UnreachableException();
			}
			return new AssignStmtNode(
				location: location,
				value: value,
				@operator: @operator.Value,
				context: context
			);
		}

		public override IASTNode VisitIdentifierExpr(JanCParser.IdentifierExprContext context) {
			return new IdentifierExpr(
				name: context.identifier().id.Text,
				context: context.identifier()
			);
		}

		public override IASTNode VisitCallExpr(JanCParser.CallExprContext context) {
			List<ArgNode> arguments = new List<ArgNode>();
			if (context.arguments() is not null) {
				arguments = context.arguments().argument()
					.Select(callArg => (ArgNode)Visit(callArg)).ToList();
			}

			return new CallExprNode(
				item: (IExpr)Visit(context.item),
				arguments: arguments,
				context: context
			);
		}
		
		public override IASTNode VisitRefCallExpr(JanCParser.RefCallExprContext context) {
			return new RefCallExprNode(
				context: context,
				argument: (ArgNode)Visit(context.argument())
			);
		}

		public override IASTNode VisitArgument(JanCParser.ArgumentContext context) {
			IExpr value = (IExpr)Visit(context.expr());

			return new ArgNode(
				value: value,
				context: context
			);
		}

		public override IASTNode VisitBoolLiteral(JanCParser.BoolLiteralContext context) {
			return new BoolLiteralNode(
				value: context.value.Text == "true",
				context: context
			);
		}

		public override IASTNode VisitNumberLiteral(JanCParser.NumberLiteralContext context) {
			if (int.TryParse(context.value.Text, out int intVal)) {
				return new IntLiteralNode(
					value: intVal,
					context: context
				);
			}
			else if (double.TryParse(context.value.Text, NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out double floatVal)) {
				return new FloatLiteralNode(
					value: floatVal,
					context: context
				);
			}
			else {
				throw new UnreachableException("Number was neither integer nor float.");
			}
		}

		public override IASTNode VisitStringLiteral(JanCParser.StringLiteralContext context) {
			string withoutQuoteMarks = context.value.Text.Substring(1, context.value.Text.Length - 2);

			return new StringLiteralNode(
				value: withoutQuoteMarks,
				context: context
			);
		}

		public override IASTNode VisitStructLiteral(JanCParser.StructLiteralContext context) {
			UserDefinedTypeNode type = (UserDefinedTypeNode)Visit(context.typeLiteral());
			List<StructLiteralMemberNode> memberList = context.structLiteralMember().Select(member => (StructLiteralMemberNode)Visit(member)).ToList();

			return new StructLiteralNode(
				type: type,
				members: memberList,
				context: context
			);
		}

		public override IASTNode VisitStructLiteralMember(JanCParser.StructLiteralMemberContext context) {
			IExpr value = (IExpr)Visit(context.value);

			return new StructLiteralMemberNode(
				name: context.name?.Text,
				value: value,
				context: context
			);
		}

		public override IASTNode VisitMemberAccess(JanCParser.MemberAccessContext context) {
			return new MemberAccessNode(
				item: (IExpr)Visit(context.structExpr),
				memberName: context.memberName.GetText(),
				context: context
			);
		}

		public override IASTNode VisitReturnStmt(JanCParser.ReturnStmtContext context) {
			IExpr value = null;
			if (context.value != null)
				value = (IExpr)Visit(context.value);

			return new ReturnStmtNode(
				value: value,
				context: context
			);
		}
	}
}
