using Antlr4.Runtime.Misc;
using Nodes;
using System.Collections.Generic;
using System.Linq;

namespace ASTGenerator.Visitors {
	internal partial class CSTVisitors : JanCBaseVisitor<IASTNode> {
		public override IASTNode VisitVarDecl(JanCParser.VarDeclContext context) {
			ITypeLiteral type = (ITypeLiteral)base.Visit(context.type);
			IExpr value = null;
			if (context.value != null)
				value = (IExpr)Visit(context.value);

			return new VarDeclNode(
				type: type,
				name: context.name.Text,
				value: value,
				isConst: context.CONST() is not null,
				context: context
			);
		}

		public override IASTNode VisitBlockStmt(JanCParser.BlockStmtContext context) {
			List<IUnit> contentList = context.impr().Select(child => (IUnit)Visit(child)).ToList();

			return new BlockNode(
				content: contentList,
				context: context
			);
		}

		public override IASTNode VisitFuncDecl(JanCParser.FuncDeclContext context) {
			IImpr impr = (IImpr)Visit(context.impr());
			var parameters = new List<VarDeclNode>();
			if (context.parameters() != null) {
				parameters = context.parameters().varDecl().Select(param => (VarDeclNode)Visit(param)).ToList();
			}
			ITypeLiteral type = (ITypeLiteral)base.Visit(context.type);

			return new FuncDeclNode(
				name: context.name.Text,
				impr: impr,
				parameters: parameters,
				context: context,
				returnType: type
			);
		}

		public override IASTNode VisitStructDecl(JanCParser.StructDeclContext context) {
			List<VarDeclNode> memberList = context.varDecl().Select(node => (VarDeclNode)Visit(node)).ToList();

			return new StructDeclNode(
				name: context.name.Text,
				members: memberList,
				context: context
			);
		}

		public override IASTNode VisitModuleDecl(JanCParser.ModuleDeclContext context) {
			return new ModuleDeclNode(
				name: context.name.Text,
				content: context.unit().Select(unit => (IUnit)Visit(unit)).ToList(),
				context: context
			);
		}
	}
}
