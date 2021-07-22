using Nodes;
using ContextAnalyzer.SemanticErrors;
using ContextAnalyzer.Tables;
using Exceptions.Exceptions.Base;
using Exceptions.Syntax.SemanticErrors;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using Tables;

namespace ContextAnalyzer.Listener {
	/// <summary>
	/// A listener to make sure types dont get mixed together
	/// </summary>
	internal class TypeEvaluatorListener : ASTListener {
		private readonly ContextSymbolTable _symbolTable;

		public TypeEvaluatorListener(ContextSymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		#region Declarations
		public void Leave(FuncDeclNode node) {
			CheckVoidableType(node.ReturnType);
		}

		public void Leave(VarDeclNode node) {
			// Check after recording, so we don't confuse the user with errors about undeclared variables.
			if (node.Value is not null) {
				ITypeLiteral expectedType = node.Type;
				ITypeLiteral actualType = node.Value.Type;

				if (expectedType is RefTypeNode && !IsLValue(node.Value))
					throw new ExpectedLValueOnRightSide(node);
				if (!CompareTypes(expectedType, actualType))
					throw new VarDeclTypeMismatch(node);
			}
			CheckUnvoidableType(node.Type);
		}

		private void CheckUnvoidableType(ITypeLiteral type) {
			if (type is PrimitiveTypeNode primitive)
				if (primitive.Type == PrimitiveTypeNode.Types.Void)
					throw new InvalidVoidUsage(type);

			CheckVoidableType(type);
		}

		#endregion

		#region Expressions
		public void Leave(BinaryExprNode node) {
			if (CompareTypes(node.Left.Type, node.Right.Type)) {
				node.Type = DerefType(node.Left.Type);
			} else {
				if (node.Left.Type.IsInvalid || node.Right.Type.IsInvalid) {
					node.Type = TypeNode.Invalid;
				} else {
					throw new BinaryExprTypeMismatch(
						leftType: node.Left.Type,
						@operator: node.Operator,
						rightType: node.Right.Type,
						context: node.Context
					);
				}
			}
		}

		public void Leave(IUnary node) {
			node.Type = DerefType(node.Value.Type);
		}

		public void Leave(CallExprNode node) {
			node.Type = TypeNode.Invalid;
			FuncDeclNode func = GetFunctionDeclaration(node, node.Arguments);
			if (func is null)
				throw new UndeclaredFunctionCalled(node);
			node.Type = func.ReturnType;
		}

		/// <summary>
		/// Gets a function declaration from a node
		/// </summary>
		/// <param name="node"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		private FuncDeclNode GetFunctionDeclaration(CallExprNode node, List<ArgNode> arguments) {
			foreach (FuncDeclNode function in GetFunctionDeclarations(node)) {
				if (CompareArgumentsAndParameters(arguments, function.Parameters))
					return function;
			}
			return null;
		}

		private static bool CompareArgumentsAndParameters(List<ArgNode> arguments, List<VarDeclNode> parameters) {
			if (arguments.Count != parameters.Count)
				return false;
			foreach (var (arg, param) in arguments.Zip(parameters)) {
				if (!CompareTypes(arg.Type, param.Type)) {
					if (arg.Type.IsInvalid || param.Type.IsInvalid)
						throw new AlreadyHandledException();
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Gets all function declarations with the same name as node
		/// </summary>
		private IEnumerable<FuncDeclNode> GetFunctionDeclarations(IASTNode node) {

			if (node is CallExprNode call) {
				if (call.Item.Type.IsInvalid)
					throw new AlreadyHandledException();
				if (!(call.Item.Type is UserDefinedTypeNode userType && userType.Type.Equals(UserDefinedTypeNode.Types.Function)))
					throw new CanOnlyCallFunction(call);

				foreach (var item in GetFunctionDeclarations(call.Item))
					yield return item;
			}
			else if (node is MemberAccessNode memberAccess) {
				foreach (var item in GetFunctionDeclarations(GetMember(memberAccess)))
					yield return item;
			}
			else if (node is IdentifierExpr identifier) {
				var decls = _symbolTable.GetDeclarations(identifier.Name);
				foreach (var decl in decls)
					foreach (var item in GetFunctionDeclarations(decl))
						yield return item;
			}
			else if (node is FuncDeclNode funcDecl) {
				yield return funcDecl;
			}
			else {
				throw new UnreachableException();
			}
		}

		public void Leave(RefCallExprNode node) {
			node.Type = TypeNode.Invalid;
			CheckUnvoidableType(node.Argument.Type);
			if (!IsLValue(node.Argument.Value))
				throw new ExpectedLValue(node.Argument.Value);			
			node.Type = TypeNode.Ref(node.Argument.Type);
		}

		public void Leave(IdentifierExpr node) {
			ITypedDecl decl = _symbolTable.GetDeclaration(node.Name);
			if (decl is not null) {
				node.Type = decl.Type;
				node.LinkNameTo(decl);
				if (decl is VarDeclNode varDecl)
					node.IsConst = varDecl.IsConst;
			}
			else {
				node.Type = TypeNode.Invalid;
				throw new UndeclaredSymbol(node);
			}
		}

		public void Leave(StructLiteralNode node) {
			if (node.Type.IsInvalid)
				throw new AlreadyHandledException();
			if (node.Type.Decl is not StructDeclNode structDecl)
				throw new StructLiteralTypeMismatch(node, node.Type.Decl);

			foreach (var (literalMember, declMember) in GetMemberPairs(node, structDecl)) {
				if (!literalMember.Value.Type.Equals(declMember.Type))
					throw new StructLiteralMemberTypeMismatch(literalMember, declMember);

				if (literalMember.Name is not null)
					literalMember.LinkNameTo(declMember);
			}
		}
		private static IEnumerable<Tuple<StructLiteralMemberNode, VarDeclNode>> GetMemberPairs(StructLiteralNode node, StructDeclNode structDecl) {
			if (node.Members.Count > structDecl.Members.Count)
				throw new CannotHaveMoreLiteralMembersThanStructMembers(node, structDecl);

			int index = 0;
			bool hasHadNamedParameter = false;
			foreach (var member in node.Members) {
				if (member.Name is null) {
					if (hasHadNamedParameter)
						throw new PositionalMemberAfterNamedMember(member);
					yield return new Tuple<StructLiteralMemberNode, VarDeclNode>(member, structDecl.Members[index]);
				}
				else {
					hasHadNamedParameter = true;
					VarDeclNode declMember = structDecl.Members.FirstOrDefault(m => m.Name.Equals(member.Name));
					if (declMember is null)
						throw new StructLiteralMemberNotFound(member, structDecl);

					yield return new Tuple<StructLiteralMemberNode, VarDeclNode>(member, declMember);
				}
				index += 1;
			}
		}

		public void Leave(MemberAccessNode node) {
			node.Type = TypeNode.Invalid;
			if (node.Item is IConstable constableItem)
				node.IsConst = Or(node.IsConst, constableItem.IsConst);
			ITypedDecl member = GetMember(node);
			if (member is IConstable constableMember)
				node.IsConst = Or(node.IsConst, constableMember.IsConst);
			node.Type = member.Type;
			node.LinkMemberName(member);
		}
		private bool? Or(bool? a, bool? b) {
			bool? c = a ?? b;
			if (b.HasValue)
				c = c.Value || b.Value;
			return c;
		}

		#endregion

		#region Statements
		public void Leave(ReturnStmtNode @return) {
			var function = _symbolTable.CurrentFunction;
			if (function is null)
				throw new CannotReturnFromGlobalScope(@return);
			if (!@return.Type.IsInvalid
				&& !function.ReturnType.IsInvalid) {
				if (!CompareTypes(@return.Type, function.ReturnType))
					throw new ReturnTypeMismatch(@return, function);
				if (function.ReturnType is RefTypeNode && @return.Type is not RefTypeNode && !IsLValue(@return.Value))
					throw new ExpectedLValue(@return.Value);
			}
		}
		public void Leave(AssignStmtNode node) {
			if (node.Location is IConstable location && location.IsConst.HasValue && location.IsConst.Value)
				throw new CannotAssignToConst(node);

			if (!IsLValue(node.Location))
				throw new ExpectedLValueOnLeftSideOfAssignment(node);

			ITypeLiteral leftType = node.Location.Type;
			ITypeLiteral rightType = node.Value.Type;

			if (leftType.IsInvalid || rightType == null || rightType.IsInvalid)
				throw new AlreadyHandledException();

			else if (!CompareTypes(leftType, rightType)) {
				throw new AssignmentTypeMismatch(node);
			}
			else if (node.HasNumericOperator && !leftType.IsNumeric) {
				throw new AssignmentOperatorTypeMismatch(node);
			}
		}
		private static bool IsLValue(IExpr value) =>
			value switch {
				IdentifierExpr => true,
				MemberAccessNode => true,
				_ => false
			};

		#endregion

		#region Types
		public void Leave(UserDefinedTypeNode node) {
			ModuleDeclNode module = GetUserDefinedTypeModule(node);
			ITypeDecl decl;
			if (module is null)
				decl = _symbolTable.GetDeclaration(node.Name) as ITypeDecl;
			else
				decl = GetMember(module, node.Name) as ITypeDecl;
			if (decl is not null) {
				node.Decl = decl;
				switch (decl) {
					case StructDeclNode: node.Type = UserDefinedTypeNode.Types.Struct; break;
					case ModuleDeclNode: node.Type = UserDefinedTypeNode.Types.Module; break;
					case FuncDeclNode: node.Type = UserDefinedTypeNode.Types.Function; break;
					default: throw new UnreachableException();
				}
				node.LinkNameTo(decl);
			}
			else
				throw new UndeclaredType(node);
		}
		private ModuleDeclNode GetUserDefinedTypeModule(UserDefinedTypeNode node) {
			string firstModuleName = node.ModuleNames.FirstOrDefault();
			ModuleDeclNode module = null;
			if (firstModuleName is not null) {
				module = _symbolTable.GetDeclaration(firstModuleName) as ModuleDeclNode;
				foreach (var moduleName in node.ModuleNames.Skip(1)) {
					module = GetMember(module, moduleName) as ModuleDeclNode;
				}
			}
			return module;
		}

		public void Leave(RefTypeNode node) {
			CheckUnvoidableType(node.SubType);
		}

		#endregion

		/// <summary>
		/// Method to leave this visitor
		///		Note: All expressions must be type-evaluated.
		/// </summary>
		/// <param name="node"></param>
		public void Leave(IExpr node) {
			throw new UnreachableException($"TypeEvaluator did not have a method for expression node: {node.GetType()}");
		}

		public void Leave(ILiteral node) {
			// Type already known.
		}

		#region Common Private methods

		private void CheckVoidableType(ITypeLiteral type) {
			if (type is UserDefinedTypeNode userType) {
				if (userType.Decl is null)
					throw new AlreadyHandledException();
				if (userType.Decl is IStaticTypeDecl)
					throw new InvalidUsageOfStaticDeclaration(userType);
			}
		}

		private static bool CompareTypes(ITypeLiteral leftType, ITypeLiteral rightType) =>
			DerefType(leftType).Equals(DerefType(rightType));

		private static ITypeLiteral DerefType(ITypeLiteral type) =>
			type is RefTypeNode refType ? refType.BottomType : type;

		/// <summary>
		/// A method to get a function declaration from a node
		/// </summary>
		private FuncDeclNode GetFunctionDeclaration(IASTNode node) {
			if (node is CallExprNode call) {
				if (call.Item.Type.IsInvalid)
					throw new AlreadyHandledException();
				if (!(call.Item.Type is UserDefinedTypeNode userType && userType.Type.Equals(UserDefinedTypeNode.Types.Function)))
					throw new CanOnlyCallFunction(call);
				return GetFunctionDeclaration(call.Item);
			}
			else if (node is MemberAccessNode memberAccess) {
				return GetFunctionDeclaration(GetMember(memberAccess));
			}
			else if (node is IdentifierExpr identifier) {
				var decl = _symbolTable.GetDeclaration(identifier.Name);
				return GetFunctionDeclaration(decl);
			}
			else if (node is FuncDeclNode funcDecl) {
				return funcDecl;
			}
			else {
				throw new UnreachableException();
			}
		}
		private ITypedDecl GetMember(MemberAccessNode node) {
			var itemType = DerefType(node.Item.Type);
			if (itemType.IsInvalid) {
				throw new AlreadyHandledException();
			}
			if (itemType is UserDefinedTypeNode userType) {
				ITypedDecl member = userType.Decl switch {
					StructDeclNode @struct => @struct.Members.FirstOrDefault(m => m.Name.Equals(node.MemberName)),
					//EnumDeclNode @enum => ...
					ModuleDeclNode module => GetMember(module, node.MemberName),
					_ => throw new CannotAccessMemberOfThisType(node)
				};
				if (member is null)
					throw new AccessedMemberNotFound(node);
				return member;
			}
			else {
				throw new CannotAccessMemberOfThisType(node);
			}
		}
		private static ITypedDecl GetMember(ModuleDeclNode module, string name) =>
			module.Content.OfType<ITypedDecl>().FirstOrDefault(m => m.Name.Equals(name));
		#endregion
	}
}
