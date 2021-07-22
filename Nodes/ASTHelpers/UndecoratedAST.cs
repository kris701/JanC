using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.ASTHelpers.CommonAST;

namespace Nodes.ASTHelpers {
	/// <summary>
	/// Generates undecorated AST. Especially useful for ContextGenerator tests.
	/// </summary>
	public static class UndecoratedAST {

		/// <summary>
		/// Returns addition of the left and right expression
		/// </summary>
		public static BinaryExprNode UPlus(this IExpr left, IExpr right) =>
			new BinaryExprNode(null, left, "+", right);

		/// <summary>
		/// Returns a literal of the given struct which should be available in current scope
		/// </summary>
		/// <param name="structAccessor"></param>
		/// <returns></returns>
		public static StructLiteralNode StructLiteral(string name) =>
			new StructLiteralNode(null, new UserDefinedTypeNode(null, name), new List<StructLiteralMemberNode>());

		/// <summary>
		/// Returns an undecorated literal of the given struct
		/// </summary>
		public static StructLiteralNode StructLiteral(UserDefinedTypeNode structAccessor) {
			var clearedType = new UserDefinedTypeNode(null, structAccessor.Name, null, structAccessor.ModuleNames);
			return new StructLiteralNode(null, clearedType, new List<StructLiteralMemberNode>());
		}

		/// <summary>
		/// Returns an undecorated comparison of the left and right expression
		/// </summary>
		public static BinaryExprNode UEquals(this IExpr left, IExpr right) =>
			new BinaryExprNode(null, left, "==", right);

		/// <summary>
		/// Returns ref call to given expression.
		/// </summary>
		public static RefCallExprNode Ref(IExpr value) =>
			new RefCallExprNode(null, Argument(value));

		/// <summary>
		/// Returns call to given function that should be available in current scope.
		/// </summary>
		/// <param name="function">Function to call</param>
		public static CallExprNode Call(FuncDeclNode function) => NewCall(item: Identifier(function));

		/// <summary>
		/// Returns call to given name.
		/// </summary>
		/// <param name="function">Function to call</param>
		public static CallExprNode Call(string name) => NewCall(name: name);


		/// <summary>
		/// Returns a decorated call to a function using an accessor expression. Useful for module functions.
		/// Example: Call(moduleDecl1.Access(moduleDecl2).Access(funcDecl)))
		/// </summary>
		/// <param name="functionAccessor">Expression that accesses the function declaration</param>
		public static CallExprNode Call(IExpr functionAccessor) => NewCall(item: functionAccessor);

		/// <summary>
		/// Accesses type member of module
		/// Example: moduleDecl.AccessType(structDecl)
		/// </summary>
		/// <param name="module"></param>
		/// <param name="member"></param>
		public static UserDefinedTypeNode UAccessType(this ModuleDeclNode module, ITypeDecl member) {
			return new UserDefinedTypeNode(null, member.Name, null, new List<string> { module.Name });
		}

		/// <summary>
		/// Accesses module member of module type reference
		/// Example: moduleDecl1.AccessType(moduleDecl2)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="module"></param>
		public static UserDefinedTypeNode UAccessType(this UserDefinedTypeNode type, ModuleDeclNode module) {
			var newModuleNames = type.ModuleNames.ToList();
			newModuleNames.Add(module.Name);
			return new UserDefinedTypeNode(null, module.Name, null, newModuleNames);
		}
		/// <summary>
		/// Accesses type inside module type reference
		/// Example: moduleDecl1.AccessUndecoratedType(moduleDecl2).AccessUndecoratedType(structDecl)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="decl"></param>
		public static UserDefinedTypeNode UAccessType(this UserDefinedTypeNode type, ITypeDecl decl) {
			if (decl is ModuleDeclNode moduleDecl)
				return type.UAccessType(moduleDecl);
			else
				return new UserDefinedTypeNode(null, decl.Name, null, type.ModuleNames);			
		}

		/// <summary>
		/// Accesses a module available to current scope
		/// </summary>
		/// <param name="struct">Module to be accessed in current scope</param>
		/// <param name="field">Member to be retrieved</param>
		public static MemberAccessNode UAccess(this ModuleDeclNode module, ITypedDecl member) =>
			new MemberAccessNode(null, Identifier(module), member.Name);
		public static MemberAccessNode UAccess(this ModuleDeclNode module, string member) =>
			new MemberAccessNode(null, Identifier(module), member);


		/// <summary>
		/// Accesses a module available to current scope
		/// </summary>
		/// <param name="itemName">Module to be accessed in current scope</param>
		/// <param name="field">Member to be retrieved</param>
		public static MemberAccessNode UAccess(string itemName, ITypedDecl member) =>
			new MemberAccessNode(null, Identifier(itemName), member.Name);

		/// <summary>
		/// Accesses a struct available to current scope
		/// </summary>
		/// <param name="struct">Struct to be accessed in current scope</param>
		/// <param name="field">Field to be retrieved</param>
		public static MemberAccessNode UAccess(this StructDeclNode @struct, VarDeclNode field) =>
			new MemberAccessNode(null, Identifier(@struct), field.Name);

		/// <summary>
		/// Accesses a module available to current scope
		/// </summary>
		/// <param name="itemName">Module to be accessed in current scope</param>
		/// <param name="field">Member to be retrieved</param>
		public static MemberAccessNode UAccess(string itemName, string name) =>
			new MemberAccessNode(null, Identifier(itemName), name);


		/// <summary>
		/// Accesses item to retrieve given member. Can be used on Module or Struct types.
		/// </summary>
		/// <param name="item">Item to be accessed</param>
		/// <param name="decl">Declaration to be retrieved</param>
		public static MemberAccessNode UAccess(this IExpr item, ITypedDecl decl) =>
			new MemberAccessNode(null, item, decl.Name);

		/// <summary>
		/// Accesses item to retrieve given member. Can be used on Module or Struct types.
		/// </summary>
		/// <param name="item">Item to be accessed</param>
		/// <param name="decl">Declaration to be retrieved</param>
		public static MemberAccessNode UAccess(this IExpr item, string name) =>
			new MemberAccessNode(null, item, name);

		/// <summary>
		/// Returns an undecorated type identifier for the given in-scope declaration.
		/// If the declaration is inside a module, you should use module.Access instead.
		/// Example: TypeIdentifier(structDecl)
		/// </summary>
		/// <param name="decl">The in-scope declaration to be referred to</param>
		public static UserDefinedTypeNode TypeIdentifier(ITypeDecl decl) =>
			new UserDefinedTypeNode(null, decl.Name);

		/// <summary>
		/// Returns an undecorated type identifier for the given in-scope declaration.
		/// If the declaration is inside a module, you should use module.Access instead.
		/// Example: TypeIdentifier(structDecl)
		/// </summary>
		/// <param name="name">The in-scope declaration to be referred to</param>

		public static UserDefinedTypeNode TypeIdentifier(string name) =>
			new UserDefinedTypeNode(null, name);


		/// <summary>
		/// Returns an undecorated identifier for the given in-scope declaration.
		/// If the declaration is inside a module, you should use module.Access instead.
		/// Example: Identifier(varDecl)
		/// </summary>
		/// <param name="decl">The in-scope declaration to be referred to</param>
		public static IdentifierExpr Identifier(ITypedDecl decl) =>
			new IdentifierExpr(null, decl.Name);

		/// <summary>
		/// Returns an undecorated identifier for the given in-scope declaration name.
		/// If the declaration is inside a module, you should use module.Access instead.
		/// Example: Identifier("varDecl")
		/// </summary>
		/// <param name="decl">The in-scope declaration to be referred to</param>
		public static IdentifierExpr Identifier(string name) =>
			new IdentifierExpr(null, name);


		// Private helpers
		private static CallExprNode NewCall(string name=null, IExpr item=null) =>
			new CallExprNode(
				item: item ?? Identifier(name),
				arguments: new List<ArgNode>(),
				context: null
			);
	}
}
