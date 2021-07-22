using Nodes;
using System;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public override IASTNode VisitUnaryExpr(JanCParser.UnaryExprContext context) {
			switch (context.op.Type) {
				case JanCLexer.PLUS:
					// We need a UnaryPlusNode to check whether value is a numeric type.
					// I removed unary plus from grammar so far
					return Visit(context.expr());
				case JanCLexer.MINUS:
					return new NegateNode(
						value: (IExpr)Visit(context.expr()),
						context: context
					);
				default:
					throw new NotSupportedException();
			}
		}

		public override IASTNode VisitNotNode(JanCParser.NotNodeContext context) {
			IExpr value = (IExpr)Visit(context.expresion);

			return new NotNode(
				context: context,
				value: value
			);
		}
	}
}
