using Antlr4.Runtime;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public IASTNode BuildASTTree(ParserRuleContext rootContext) {
			return Visit(rootContext);
		}

		public override IASTNode VisitCompileUnit(JanCParser.CompileUnitContext context) {
			List<IUnit> units = context.unit().Select(child => (IUnit)Visit(child)).ToList();

			return new GlobalScopeNode(
				content: units,
				context: context
			);
		}

		public override IASTNode VisitUnit(JanCParser.UnitContext context) {
			return Visit(context.GetChild(0));
		}

		public override IASTNode VisitLiteralTypeAccess(JanCParser.LiteralTypeAccessContext context) {
			var accessNames = new List<string>();
			var item = context;
			do {
				var userType = (UserDefinedTypeNode)Visit(item.typeLiteral());
				accessNames.Add(userType.Name);
				var nextItem = item.typeLiteral();
				if (nextItem is JanCParser.LiteralTypeAccessContext nextItemAccess)
					item = nextItemAccess;
				else
					item = null;
			} while (item is not null);
			var memberName = context.memberName.Text;
			accessNames.Reverse();
			return new UserDefinedTypeNode(context, memberName, null, accessNames);
		}

		public override IASTNode VisitLiteralType(JanCParser.LiteralTypeContext context) {
			PrimitiveTypeNode.Types? baseType = StringToTypePrimitive(context.baseType.Text);

			if(baseType != null) {
				return new PrimitiveTypeNode(context, baseType.Value);
			}
			else {
				return new UserDefinedTypeNode(context, context.baseType.Text);
			}
		}

		public override IASTNode VisitLiteralTypeRef(JanCParser.LiteralTypeRefContext context) {
			ITypeLiteral subType = (ITypeLiteral)base.Visit(context.typeLiteral());

			return new RefTypeNode(context, subType);
		}

		private static PrimitiveTypeNode.Types? StringToTypePrimitive(string baseType) {
			return baseType switch {
				"str" => PrimitiveTypeNode.Types.String,
				"int" => PrimitiveTypeNode.Types.Integer,
				"bool" => PrimitiveTypeNode.Types.Bool,
				"float" => PrimitiveTypeNode.Types.Float,
				"void" => PrimitiveTypeNode.Types.Void,
				_ => null,
			};
		}

	}
}
