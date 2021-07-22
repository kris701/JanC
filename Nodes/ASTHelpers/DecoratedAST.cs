using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.ASTHelpers.CommonAST;

namespace Nodes.ASTHelpers {
	/// <summary>
	/// Generates decorated AST. Especially useful for CodeAnalyzer tests.
	/// </summary>
	public static class DecoratedAST {

		/// <summary>
		/// Returns a decorated addition of the two expressions
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public static BinaryExprNode DPlus(this IExpr left, IExpr right) {
			if (!left.Type.Equals(right.Type))
				throw new InvalidOperationException();
			return new BinaryExprNode(null, left, "+", right) { Type = left.Type };
		}
		public static VarDeclNode VarDecl(string name, IExpr expr) {
			return new VarDeclNode(null, name, expr.Type, expr, false);
		}
		public static VarDeclNode ConstVarDecl(string name, IExpr expr) {
			return new VarDeclNode(null, name, expr.Type, expr, true);
		}


		/// <summary>
		/// Returns decorated ref call to given expression.
		/// </summary>
		/// <param name="value"></param>
		public static RefCallExprNode Ref(IExpr value) =>
			new RefCallExprNode(null, Argument(value)) { Type = TypeNode.Ref(value.Type) };

		/// <summary>
		/// Returns call to given function that must be available in current scope.
		/// </summary>
		/// <param name="function">Function to call</param>
		public static CallExprNode Call(FuncDeclNode function) => NewCall(Identifier(function));

		/// <summary>
		/// Returns a decorated call to a function using an accessor expression. Useful for module functions.
		/// Example: Call(moduleDecl1.Access(moduleDecl2).Access(funcDecl)))
		/// </summary>
		/// <param name="functionAccessor">Expression that accesses the function declaration</param>
		public static CallExprNode Call(IExpr functionAccessor) => NewCall(functionAccessor);

		/// <summary>
		/// Accesses type member of module
		/// Example: moduleDecl.AccessType(structDecl)
		/// </summary>
		/// <param name="module"></param>
		/// <param name="member"></param>
		public static UserDefinedTypeNode AccessDecoratedType(this ModuleDeclNode module, ITypeDecl member) {
			UserDefinedTypeNode.Types metaType = GetMetaType(member);
			return new UserDefinedTypeNode(null, member.Name, metaType, new List<string> { module.Name });
		}

		/// <summary>
		/// Accesses module member of module type reference
		/// Example: moduleDecl1.AccessType(moduleDecl2)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="module"></param>
		public static UserDefinedTypeNode AccessDecoratedType(this UserDefinedTypeNode type, ModuleDeclNode module) {
			var newModuleNames = type.ModuleNames.ToList();
			newModuleNames.Add(module.Name);
			return new UserDefinedTypeNode(null, module.Name, UserDefinedTypeNode.Types.Module, newModuleNames);
		}
		/// <summary>
		/// Accesses type inside module type reference
		/// Example: moduleDecl1.AccessType(moduleDecl2).AccessType(structDecl)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="decl"></param>
		public static UserDefinedTypeNode AccessDecoratedType(this UserDefinedTypeNode type, ITypeDecl decl) {
			if (decl is ModuleDeclNode moduleDecl) {
				return type.AccessDecoratedType(moduleDecl);
			} else {
				UserDefinedTypeNode.Types metaType = GetMetaType(decl);
				return new UserDefinedTypeNode(null, decl.Name, metaType, type.ModuleNames);
			}
		}

		/// <summary>
		/// Accesses a module available to current scope
		/// </summary>
		/// <param name="struct">Module to be accessed in current scope</param>
		/// <param name="field">Member to be retrieved</param>
		public static MemberAccessNode DecoratedAccess(this ModuleDeclNode module, ITypedDecl member) =>
			new MemberAccessNode(null, Identifier(module), member.Name) { Type = member.Type };

		/// <summary>
		/// Accesses a struct available to current scope
		/// </summary>
		/// <param name="struct">Struct to be accessed in current scope</param>
		/// <param name="field">Field to be retrieved</param>
		public static MemberAccessNode DecoratedAccess(this StructDeclNode @struct, VarDeclNode field) =>
			new MemberAccessNode(null, Identifier(@struct), field.Name) { Type = field.Type };

		/// <summary>
		/// Accesses item to retrieve given member. Can be used on Module or Struct types.
		/// </summary>
		/// <param name="item">Item to be accessed</param>
		/// <param name="decl">Declaration to be retrieved</param>
		public static MemberAccessNode DecoratedAccess(this IExpr item, ITypedDecl decl) =>
			new MemberAccessNode(null, item, decl.Name) { Type = decl.Type };

		/// <summary>
		/// Returns an undecorated type identifier for the given in-scope declaration.
		/// If the declaration is inside a module, you should use module.Access instead.
		/// Example: TypeIdentifier(structDecl)
		/// </summary>
		/// <param name="decl">The in-scope declaration to be referred to</param>
		public static UserDefinedTypeNode TypeIdentifier(ITypeDecl decl) =>
			new UserDefinedTypeNode(null, decl.Name, GetMetaType(decl));

		/// <summary>
		/// Returns a decorated identifier for the given in-scope declaration.
		/// If the declaration is inside a module, you should use module.DAccess instead.
		/// Example: Identifier(varDecl)
		/// </summary>
		public static IdentifierExpr Identifier(ITypedDecl decl) =>
			new IdentifierExpr(null, decl.Name, decl.Type);

		/// <summary>
		/// Returns a reference to a variable holding the given value
		/// </summary>
		public static RefCallExprNode RefValue(dynamic value) {
			ILiteral literal = Literal(value);
			var location = literal.Type.VarDecl("ref_location");
			var reference = Ref(Identifier(location));
			return reference;
		}

		/// <summary>
		/// Returns a decorated literal of the given struct
		/// </summary>
		/// <param name="structAccessor"></param>
		public static StructLiteralNode StructLiteral(UserDefinedTypeNode structAccessor) =>
			new StructLiteralNode(null, structAccessor, new List<StructLiteralMemberNode>());

		/// <summary>
		/// Returns a decorated comparison of the left and right expression
		/// </summary>
		public static BinaryExprNode DEquals(this IExpr left, IExpr right) =>
			new BinaryExprNode(null, left, "==", right) { Type = left.Type };

		// Private helpers
		private static CallExprNode NewCall(IExpr item) =>
			new CallExprNode(
				item: item,
				type: item.Type,
				arguments: new List<ArgNode>(),
				context: null
			);
	}
}
