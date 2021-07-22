using Nodes;
using System;
using System.Collections.Generic;
using static Nodes.TypeNode;

namespace Nodes.ASTHelpers {
	// Helps set up AST trees for testing
	public static class CommonAST {

		public static GlobalScopeNode Root() => new GlobalScopeNode(content: new List<IUnit>(), context: null);

		public static EveryTaskNode EveryTask(int delay) =>
			new EveryTaskNode(
				body: BlockNode.EmptyBlock,
				delay: delay.ToString(),
				context: null
			);

		public static OnceTaskNode OnceTask() =>
			new OnceTaskNode(
				body: BlockNode.EmptyBlock,
				context: null
			);

		public static OnTaskNode OnTask(IExpr condition) =>
			new OnTaskNode(
				body: BlockNode.EmptyBlock,
				context: null,
				condition: condition
			);

		public static IdleTaskNode IdleTask() =>
			new IdleTaskNode(
				body: BlockNode.EmptyBlock,
				context: null
			);

		public static ForStmtNode ForStmt(IImpr init, IExpr condition, IImpr update, IImpr body) => new ForStmtNode(null, condition, body, init, update);

		public static ReturnStmtNode Return() => new ReturnStmtNode(null, null);

		public static ReturnStmtNode Return(IExpr value) => new ReturnStmtNode(null, value);


		public static BlockNode Block(List<IUnit> content) {
			return new BlockNode(null, content);
		}

		public static BlockNode Block() {
			return new BlockNode(null, new List<IUnit>());
		}

		public static IntLiteralNode InvalidLiteral() {
			var literal = Literal(0);
			literal.Type = Invalid;
			return literal;
		}
		public static IntLiteralNode Literal(int value) =>
			new IntLiteralNode(value: value, context: null);
		public static FloatLiteralNode Literal(double value) =>
			new FloatLiteralNode(value: value, context: null);
		public static StringLiteralNode Literal(string value) =>
			new StringLiteralNode(value: value, context: null);
		public static BoolLiteralNode Literal(bool value) =>
			new BoolLiteralNode(value: value, context: null);
		public static VarDeclNode VarDecl(this ITypeLiteral type, string name, IExpr value = null) =>
			new VarDeclNode(null, name, type, value, false);
		public static VarDeclNode ConstVarDecl(this ITypeLiteral type, string name, IExpr value = null) =>
			new VarDeclNode(null, name, type, value, true);
		public static StructDeclNode StructDecl(string name) =>
			new StructDeclNode(null, name, new List<VarDeclNode>());
		public static VarDeclNode StructVarDecl(string structName, string varName) =>
			new VarDeclNode(null, varName, TypeNode.Struct(structName), null, false);
		public static VarDeclNode StructConstDecl(string structName, string varName) =>
			new VarDeclNode(null, varName, TypeNode.Struct(structName), null, true);
		public static StructLiteralMemberNode StructMember(int value, string name = null) =>
			new StructLiteralMemberNode(null, name, Literal(value));

		public static StructLiteralMemberNode StructMember(double value, string name = null) =>
			new StructLiteralMemberNode(null, name, Literal(value));

		public static StructLiteralMemberNode StructMember(string value, string name = null) =>
			new StructLiteralMemberNode(null, name, Literal(value));
		
		public static StructLiteralMemberNode StructMember(bool value, string name = null) =>
			new StructLiteralMemberNode(null, name, Literal(value));

		public static ModuleDeclNode Module(string name) =>
			new ModuleDeclNode(name, new List<IUnit>(), null);
	
		public static ModuleDeclNode With(this ModuleDeclNode module, IUnit unit) {
			module.Content.Add(unit);
			return module;
		}

		public static AssignStmtNode Assign(this IExpr node, IExpr value) =>
			new AssignStmtNode(null, node, AssignOperator.Assign, value);
		
		public static AssignStmtNode PlusAssign(this IExpr node, IExpr value) =>
			new AssignStmtNode(null, node, AssignOperator.PlusAssign, value);

		public static FuncDeclNode Function(this ITypeLiteral type, string name) =>
			new FuncDeclNode(
				name: name,
				returnType: type,
				parameters: new List<VarDeclNode>(),
				impr: BlockNode.EmptyBlock,
				context: null
			);

		/// <summary>
		/// Returns decorated argument for a value
		/// </summary>
		/// <param name="value">Value to be wrapped in ArgNode</param>
		public static ArgNode Argument(IExpr value) =>
			new ArgNode(null, value);
		public static ArgNode Argument(int value) =>
			new ArgNode(null, Literal(value));
		public static ArgNode Argument(double value) =>
			new ArgNode(null, Literal(value));
		public static ArgNode Argument(string value) =>
			new ArgNode(null, Literal(value));
		public static ArgNode Argument(bool value) =>
			new ArgNode(null, Literal(value));

		#region Helper Methods
		public static GlobalScopeNode With(this GlobalScopeNode root, IUnit unit) {
			root.Content.Add(unit);
			return root;
		}
		public static FuncDeclNode With(this FuncDeclNode func, VarDeclNode param) {
			func.Parameters.Add(param);
			return func;
		}
		public static CallExprNode With(this CallExprNode call, ArgNode arg) {
			call.Arguments.Add(arg);
			return call;
		}
		public static StructDeclNode With(this StructDeclNode decl, VarDeclNode member) {
			decl.Members.Add(member);
			return decl;
		}
		public static StructLiteralNode With(this StructLiteralNode literal, StructLiteralMemberNode member) {
			literal.Members.Add(member);
			return literal;
		}

		public static T PrependBody<T>(this T node, IImpr impr) where T : IImprContainer {
			if (node.Body is BlockNode body)
				body.PrependBody(impr);
			else
				node.Body = new BlockNode(null, new List<IUnit>() { node.Body, impr });
			return node;
		}

		public static T AppendBody<T>(this T node, IImpr impr) where T : IImprContainer {
			if (node.Body is BlockNode body)
				body.AppendBody(impr);
			else
				node.Body = new BlockNode(null, new List<IUnit>() { node.Body, impr });
			return node;
		}

		public static BlockNode PrependBody(this BlockNode node, IImpr impr) {
			node.Content.Insert(0, impr);
			return node;
		}

		public static BlockNode AppendBody(this BlockNode node, IImpr impr) {
			node.Content.Add(impr);
			return node;
		}

		public static UserDefinedTypeNode.Types GetMetaType(ITypeDecl decl) =>
			decl switch {
				ModuleDeclNode => UserDefinedTypeNode.Types.Module,
				FuncDeclNode => UserDefinedTypeNode.Types.Function,
				StructDeclNode => UserDefinedTypeNode.Types.Struct,
				_ => throw new InvalidOperationException()
			};

		#endregion
	}
}
