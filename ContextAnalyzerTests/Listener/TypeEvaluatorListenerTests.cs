using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContextAnalyzer.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tables;
using ContextAnalyzer.Tables;
using Nodes;
using ContextAnalyzer.SemanticErrors;
using Exceptions.Syntax.SemanticErrors;
using Exceptions.Exceptions.Base;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using static Nodes.TypeNode;
using Nodes.ASTHelpers;

namespace ContextAnalyzer.Listener.Tests {
	[TestClass()]
	public class TypeEvaluatorListenerTests {
		#region Test Setup
		private ContextSymbolTable contextSymbolTable;
		private TypeEvaluatorListener typeEvaluatorListener;

		[TestInitialize]
		public virtual void Setup() {
			contextSymbolTable = new ContextSymbolTable();
			typeEvaluatorListener = new TypeEvaluatorListener(contextSymbolTable);
		}
		#endregion

		#region Constructor Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ThrowsIfArguemntsNullTest1() {
			new TypeEvaluatorListener(null);
		}
		#endregion

		#region Enter(FuncDeclNode node) Tests
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_FuncDeclNode_ThrowIfAlreadyHandled() {
			FuncDeclNode funcDeclNode = new FuncDeclNode(null, "test", new UserDefinedTypeNode(null, "structname"), new List<VarDeclNode>(), new BlockNode(null, new List<IUnit>()));
			contextSymbolTable.Record(funcDeclNode);

			typeEvaluatorListener.Leave(funcDeclNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void Leave_FuncDeclNode_ThrowIfInstanciatedStaticDecl() {
			FuncDeclNode funcDeclNode = new FuncDeclNode(null, "test", new UserDefinedTypeNode(null, "structname"), new List<VarDeclNode>(), new BlockNode(null, new List<IUnit>()));
			contextSymbolTable.Record(funcDeclNode);
			((UserDefinedTypeNode)funcDeclNode.ReturnType).Decl = Int.Function("a");

			typeEvaluatorListener.Leave(funcDeclNode);
		}
		#endregion

		#region Leave(BinaryExprNode node) Tests
		[TestMethod()]
		public void Leave_BinaryExprNode_SetNodeTypeIfEqualTypes() {
			BinaryExprNode binaryExprNode = new BinaryExprNode(null, Literal(1), "+", Literal(1));

			typeEvaluatorListener.Leave(binaryExprNode);

			Assert.AreEqual(Int, binaryExprNode.Type);
		}
		[TestMethod()]
		public void Leave_BinaryExprNode_SetNodeTypeIfValidRefAssignment_BothIsRef1() {
			BinaryExprNode binaryExprNode = DecoratedAST.RefValue(1).UPlus(DecoratedAST.RefValue(1));

			typeEvaluatorListener.Leave(binaryExprNode);

			Assert.AreEqual(Int, binaryExprNode.Type);
		}
		[TestMethod()]
		[ExpectedException(typeof(BinaryExprTypeMismatch))]
		public void Leave_BinaryExprNode_SetNodeTypeIfValidRefAssignment_BothIsRef2() {
			BinaryExprNode binaryExprNode = DecoratedAST.RefValue("1").UPlus(DecoratedAST.RefValue(1));

			typeEvaluatorListener.Leave(binaryExprNode);
		}
		[TestMethod()]
		public void Leave_BinaryExprNode_SetNodeTypeIfValidRefAssignment_LeftIsRef() {
			BinaryExprNode binaryExprNode = DecoratedAST.RefValue(1).UPlus(Literal(1));

			typeEvaluatorListener.Leave(binaryExprNode);

			Assert.AreEqual(Int, binaryExprNode.Type);
		}
		[TestMethod()]
		public void Leave_BinaryExprNode_SetNodeTypeIfValidRefAssignment_RightIsRef() {
			BinaryExprNode binaryExprNode = Literal(1).UPlus(DecoratedAST.RefValue(1));

			typeEvaluatorListener.Leave(binaryExprNode);

			Assert.AreEqual(Int, binaryExprNode.Type);
		}
		[TestMethod()]
		public void Leave_BinaryExprNode_SetNodeTypeToInvalidIfAnySubTypesAre1() {
			BinaryExprNode binaryExprNode = new BinaryExprNode(null, Literal(1), "+", Literal(1));
			binaryExprNode.Left.Type = Invalid;

			typeEvaluatorListener.Leave(binaryExprNode);

			Assert.AreEqual(Invalid, binaryExprNode.Type);
		}
		[TestMethod()]
		public void Leave_BinaryExprNode_SetNodeTypeToInvalidIfAnySubTypesAre2() {
			BinaryExprNode binaryExprNode = new BinaryExprNode(null, Literal(1), "+", Literal(1));
			binaryExprNode.Right.Type = Invalid;

			typeEvaluatorListener.Leave(binaryExprNode);

			Assert.AreEqual(Invalid, binaryExprNode.Type);
		}
		[TestMethod()]
		[ExpectedException(typeof(BinaryExprTypeMismatch))]
		public void Leave_BinaryExprNode_ThrowIfTypeMismatch() {
			BinaryExprNode binaryExprNode = new BinaryExprNode(null, Literal("1"), "+", Literal(1));

			typeEvaluatorListener.Leave(binaryExprNode);
		}
		#endregion

		#region Leave(IUnary node) Tests
		[TestMethod()]
		public void Leave_IUnary_SetsTypeFromValue() {
			IUnary iUnaryNode = new NegateNode(null, Identifier("a"));

			typeEvaluatorListener.Leave(iUnaryNode);

			Assert.AreEqual(iUnaryNode.Type, iUnaryNode.Value.Type);
		}
		#endregion

		#region Leave(CallExprNode node) Tests
		[TestMethod()]
		public void Leave_CallExprNode_SetsTypeFromReturnType() {
			FuncDeclNode funcDeclNode = Int.Function("func");
			CallExprNode callExprNode = Call("func");
			contextSymbolTable.Record(funcDeclNode);
			((IdentifierExpr)callExprNode.Item).Type = funcDeclNode.Type;
			((IdentifierExpr)callExprNode.Item).LinkNameTo(funcDeclNode);

			typeEvaluatorListener.Leave(callExprNode);

			Assert.AreEqual(callExprNode.Type, funcDeclNode.ReturnType);
		}
		[TestMethod()]
		public void Leave_CallExprNode_SetsTypeFromReturnType_WithOverload() {
			// func with no params
			FuncDeclNode funcDeclNode = Int.Function("func");
			CallExprNode callExprNode = Call("func");
			contextSymbolTable.Record(funcDeclNode);

			// func with intParam
			FuncDeclNode funcDeclNode2 = Float.Function("func").With(Int.VarDecl("param"));
			var intArg = Argument(6);
			CallExprNode callExprNode2 = Call("func").With(intArg);
			contextSymbolTable.Record(funcDeclNode2);

			typeEvaluatorListener.Leave(funcDeclNode);
			typeEvaluatorListener.Leave(intArg);
			typeEvaluatorListener.Leave(funcDeclNode2);


			((IdentifierExpr)callExprNode.Item).Type = funcDeclNode.Type;
			((IdentifierExpr)callExprNode.Item).LinkNameTo(funcDeclNode);

			((IdentifierExpr)callExprNode2.Item).Type = funcDeclNode2.Type;
			((IdentifierExpr)callExprNode2.Item).LinkNameTo(funcDeclNode2);


			typeEvaluatorListener.Leave(callExprNode);
			typeEvaluatorListener.Leave(callExprNode2);

			Assert.AreEqual(callExprNode.Type, funcDeclNode.ReturnType);
			Assert.AreEqual(callExprNode2.Type, funcDeclNode2.ReturnType);
		}
		[TestMethod()]
		[ExpectedException(typeof(UndeclaredFunctionCalled))]
		public void Leave_CallExprNode_ThrowIfIncorrectParamsCount() {
			FuncDeclNode funcDeclNode = new FuncDeclNode(new JanCParser.FuncDeclContext(new JanCParser.DeclContext()), "func", Int, new List<VarDeclNode>(), new BlockNode(null, new List<IUnit>()));
			CallExprNode callExprNode = Call("func").With(Argument(1));
			contextSymbolTable.Record(funcDeclNode);
			((IdentifierExpr)callExprNode.Item).Type = funcDeclNode.Type;
			((IdentifierExpr)callExprNode.Item).LinkNameTo(funcDeclNode);

			typeEvaluatorListener.Leave(callExprNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(UndeclaredFunctionCalled))]
		public void Leave_CallExprNode_ThrowIfIncorrectParamsCountBuildInFunction() {
			CallExprNode callExprNode = Call("digitalWrite").With(Argument(1));
			ReservedFunctionsTable.InsertBuiltInFunctions(contextSymbolTable);
			((IdentifierExpr)callExprNode.Item).Type = contextSymbolTable.GetDeclaration("digitalWrite").Type;
			((IdentifierExpr)callExprNode.Item).LinkNameTo(contextSymbolTable.GetDeclaration("digitalWrite"));

			typeEvaluatorListener.Leave(callExprNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(UndeclaredFunctionCalled))]
		public void Leave_CallExprNode_ThrowIfArgumentTypeMismatch() {
			FuncDeclNode funcDeclNode = new FuncDeclNode(new JanCParser.FuncDeclContext(new JanCParser.DeclContext()), "func", Int, new List<VarDeclNode>() { Int.VarDecl("a") }, new BlockNode(null, new List<IUnit>()));
			CallExprNode callExprNode = Call("func").With(Argument("1"));
			contextSymbolTable.Record(funcDeclNode);
			((IdentifierExpr)callExprNode.Item).Type = funcDeclNode.Type;
			((IdentifierExpr)callExprNode.Item).LinkNameTo(funcDeclNode);

			typeEvaluatorListener.Leave(callExprNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_CallExprNode_ThrowAlreadyHandledIfArgumentTypeIsInvalid1() {
			FuncDeclNode function = Int.Function("func").With(Int.VarDecl("a"));
			function.Context = new JanCParser.FuncDeclContext(new JanCParser.DeclContext());
			CallExprNode call = Call("func").With(Argument(InvalidLiteral()));
			contextSymbolTable.Record(function);
			((IdentifierExpr)call.Item).Type = function.Type;
			((IdentifierExpr)call.Item).LinkNameTo(function);

			typeEvaluatorListener.Leave(call);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_CallExprNode_ThrowAlreadyHandledIfArgumentTypeIsInvalid2() {
			FuncDeclNode function = Int.Function("func").With(Int.VarDecl("a"));
			function.Context = new JanCParser.FuncDeclContext(new JanCParser.DeclContext());
			CallExprNode call = Call("func").With(Argument(InvalidLiteral()));
			contextSymbolTable.Record(function);
			((IdentifierExpr)call.Item).Type = function.Type;
			((IdentifierExpr)call.Item).LinkNameTo(function);

			typeEvaluatorListener.Leave(call);
		}
		#endregion

		#region Leave(RefCallExprNode node) Tests
		[TestMethod()]
		public void Leave_RefCallExprNode_SetsRefTypeFromValue() {
			RefCallExprNode refCall = DecoratedAST.RefValue(1);
			refCall.Type = null;

			typeEvaluatorListener.Leave(refCall);

			Assert.AreEqual(((RefTypeNode)refCall.Type).SubType, refCall.Argument.Type);
		}
		#endregion

		#region Leave(IdentifierExpr node) Tests
		[TestMethod()]
		[ExpectedException(typeof(UndeclaredSymbol))]
		public void Leave_IdentifierExpr_ThrowsIfIdentifierNotDeclared() {
			IdentifierExpr identifierExpr = Identifier("a");

			typeEvaluatorListener.Leave(identifierExpr);
		}
		[TestMethod()]
		public void Leave_IdentifierExpr_SetsTypeIfDeclared() {
			VarDeclNode varDeclNode = Int.VarDecl("a");
			contextSymbolTable.Record(varDeclNode);
			IdentifierExpr identifierExpr = Identifier("a");

			typeEvaluatorListener.Leave(identifierExpr);

			Assert.AreEqual(identifierExpr.Type, varDeclNode.Type);
		}
		[TestMethod()]
		public void Leave_IdentifierExpr_SetsTypeIfConstDeclared() {
			VarDeclNode varDeclNode = Int.ConstVarDecl("a");
			contextSymbolTable.Record(varDeclNode);
			IdentifierExpr identifierExpr = Identifier("a");

			typeEvaluatorListener.Leave(identifierExpr);

			Assert.AreEqual(true, identifierExpr.IsConst);
		}
		#endregion

		#region Leave(ReturnStmtNode @return) Tests
		[TestMethod()]
		[ExpectedException(typeof(CannotReturnFromGlobalScope))]
		public void Leave_ReturnStmtNode_ThrowsIfLatestFuncDeclIsNull() {
			ReturnStmtNode returnStmtNode = new ReturnStmtNode(null, Literal(1));

			typeEvaluatorListener.Leave(returnStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(ReturnTypeMismatch))]
		public void Leave_ReturnStmtNode_ThrowsIfReturnTypeNotSameAsFunc() {
			FuncDeclNode funcDeclNode = TypeNode.String.Function("func");
			ReturnStmtNode returnStmtNode = new ReturnStmtNode(null, Literal(1));
			contextSymbolTable.EnterScope(funcDeclNode);

			typeEvaluatorListener.Leave(returnStmtNode);
		}
		#endregion

		#region Leave(VarDeclNode node) Tests
		[TestMethod()]
		[ExpectedException(typeof(InvalidVoidUsage))]
		public void Leave_VarDeclNode_ThrowIfVarIsVoid() {
			VarDeclNode varDeclNode = Int.VarDecl("a");
			varDeclNode.Type = TypeNode.Void;
			contextSymbolTable.Record(varDeclNode);

			typeEvaluatorListener.Leave(varDeclNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_VarDeclNode_ThrowIfVarNotDeclared() {
			VarDeclNode varDeclNode = new VarDeclNode(null, "test", new UserDefinedTypeNode(null, "structname"), null, false);

			typeEvaluatorListener.Leave(varDeclNode);
		}
		/*[TestMethod()]
		[ExpectedException(typeof(VarDeclRefDeclaredToNonIdentifier))]
		public void Leave_VarDeclNode_ThrowsIfRefIsNonIdentifier() {
			VarDeclNode varDeclNode = Ref(Int).VarDecl("a", Literal(1));
			varDeclNode.Value = Literal(1);
			contextSymbolTable.Record(varDeclNode);

			typeEvaluatorListener.Leave(varDeclNode);
		}*/

		[TestMethod()]
		[ExpectedException(typeof(VarDeclTypeMismatch))]
		public void Leave_VarDeclNode_ThrowIfTypeMismatch1() {
			VarDeclNode varDeclNode = Ref(TypeNode.String).VarDecl("a");
			varDeclNode.Value = new IdentifierExpr(null, "some_ref", Ref(Int));
			contextSymbolTable.Record(varDeclNode);

			typeEvaluatorListener.Leave(varDeclNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(VarDeclTypeMismatch))]
		public void Leave_VarDeclNode_ThrowIfTypeMismatch2() {
			VarDeclNode varDeclNode = TypeNode.String.VarDecl("a");
			varDeclNode.Value = Literal(1);
			contextSymbolTable.Record(varDeclNode);

			typeEvaluatorListener.Leave(varDeclNode);
		}
		#endregion

		#region Leave(AssignStmtNode node) Tests
		[TestMethod()]
		[ExpectedException(typeof(CannotAssignToConst))]
		public void Leave_AssignStmtNode_ThrowIfLocationConst() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(Identifier("b"));
			((IConstable)assignStmtNode.Location).IsConst = true;

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(ExpectedLValueOnLeftSideOfAssignment))]
		public void Leave_AssignStmtNode_ThrowIfLocationNotLValue() {
			AssignStmtNode assignStmtNode = Literal(1).Assign(Identifier("b"));

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_AssignStmtNode_ThrowIfTypeAlreadyHandled1() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(Identifier("b"));
			assignStmtNode.Location.Type = Invalid;

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_AssignStmtNode_ThrowIfTypeAlreadyHandled2() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(Identifier("b"));
			assignStmtNode.Location.Type = Int;
			assignStmtNode.Value.Type = null;

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_AssignStmtNode_ThrowIfTypeAlreadyHandled3() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(Identifier("b"));
			assignStmtNode.Location.Type = Int;
			assignStmtNode.Value.Type = Invalid;

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AssignmentTypeMismatch))]
		public void Leave_AssignStmtNode_ThrowIfRefTypeMismatch1() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(DecoratedAST.RefValue("str"));
			assignStmtNode.Location.Type = Int;
			assignStmtNode.Value.Type = Ref(TypeNode.String);

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AssignmentTypeMismatch))]
		public void Leave_AssignStmtNode_ThrowIfRefTypeMismatch2() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(Literal("1"));
			assignStmtNode.Location.Type = Int;
			assignStmtNode.Value.Type = Ref(TypeNode.String);

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AssignmentTypeMismatch))]
		public void Leave_AssignStmtNode_ThrowIfTypeMismatch() {
			AssignStmtNode assignStmtNode = Identifier("a").Assign(Literal("1"));
			assignStmtNode.Location.Type = Int;
			assignStmtNode.Value.Type = TypeNode.String;

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AssignmentOperatorTypeMismatch))]
		public void Leave_AssignStmtNode_ThrowIfLeftValueNotNumeric() {
			AssignStmtNode assignStmtNode = Identifier("a").PlusAssign(Identifier("a"));
			assignStmtNode.Location.Type = TypeNode.String;
			assignStmtNode.Value.Type = TypeNode.String;

			typeEvaluatorListener.Leave(assignStmtNode);
		}
		#endregion

		#region Leave(StructLiteralNode node) Tests

		[TestMethod()]
		[ExpectedException(typeof(StructLiteralTypeMismatch))]
		public void Leave_StructLiteralNode_ThrowsIfDeclNotStruct() {
			FuncDeclNode funcDeclNode = TypeNode.Void.Function("structname");
			contextSymbolTable.Record(funcDeclNode);
			StructLiteralNode structLiteralNode = StructLiteral("structname");
			structLiteralNode.Type.Decl = funcDeclNode;

			typeEvaluatorListener.Leave(structLiteralNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(CannotHaveMoreLiteralMembersThanStructMembers))]
		public void Leave_StructLiteralNode_ThrowsIfLiteralMembersNotSame() {
			StructDeclNode structDecl = StructDecl("structname");
			contextSymbolTable.Record(structDecl);
			StructLiteralNode structLiteralNode = StructLiteral("structname").With(StructMember(1));
			structLiteralNode.Type.Decl = structDecl;

			typeEvaluatorListener.Leave(structLiteralNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(StructLiteralMemberTypeMismatch))]
		public void Leave_StructLiteralNode_ThrowsIfLiteralMembersTypeNotSame() {
			StructDeclNode structDecl = StructDecl("structname").With(TypeNode.String.VarDecl("a"));
			contextSymbolTable.Record(structDecl);
			StructLiteralNode structLiteralNode = StructLiteral("structname").With(StructMember(1));
			structLiteralNode.Type.Decl = structDecl;

			typeEvaluatorListener.Leave(structLiteralNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(PositionalMemberAfterNamedMember))]
		public void Leave_StructLiteralNode_ThrowsIfPositionalMemberAfterNamedMember() {
			StructDeclNode structDecl = StructDecl("structname").With(Int.VarDecl("a")).With(TypeNode.String.VarDecl("b"));
			contextSymbolTable.Record(structDecl);
			StructLiteralNode structLiteralNode = StructLiteral("structname").With(StructMember(1, "a")).With(StructMember("strr"));
			structLiteralNode.Type.Decl = structDecl;

			typeEvaluatorListener.Leave(structLiteralNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(StructLiteralMemberNotFound))]
		public void Leave_StructLiteralNode_ThrowsIfNamesLiteralNotFound() {
			StructDeclNode structDecl = StructDecl("structname").With(Int.VarDecl("a"));
			contextSymbolTable.Record(structDecl);
			StructLiteralNode structLiteralNode = StructLiteral("structname").With(StructMember(1, "b"));
			structLiteralNode.Type.Decl = structDecl;

			typeEvaluatorListener.Leave(structLiteralNode);
		}
		#endregion

		#region Leave(MemberAccessNode node) Tests
		[TestMethod()]
		public void Leave_MemberAccessNode_SetTypeToMemberType_Module() {
			VarDeclNode varDeclNode = Int.VarDecl("a");
			ModuleDeclNode moduleDeclNode = Module("mod").With(varDeclNode);
			MemberAccessNode memberAccessNode = UAccess("mod", "a");
			memberAccessNode.Item.Type = moduleDeclNode.Type;

			typeEvaluatorListener.Leave(memberAccessNode);

			Assert.AreEqual(varDeclNode.Type, memberAccessNode.Type);
		}
		[TestMethod()]
		public void Leave_MemberAccessNode_SetTypeToMemberType_Struct() {
			VarDeclNode varDeclNode = Int.VarDecl("a");
			StructDeclNode structDeclNode = StructDecl("mod").With(varDeclNode);
			structDeclNode.Type.Decl = structDeclNode;
			MemberAccessNode memberAccessNode = UAccess("mod", "a");
			memberAccessNode.Item.Type = structDeclNode.Type;

			typeEvaluatorListener.Leave(memberAccessNode);

			Assert.AreEqual(varDeclNode.Type, memberAccessNode.Type);
		}
		[TestMethod()]
		public void Leave_MemberAccessNode_SetTypeToMemberTypeConstable() {
			VarDeclNode varDeclNode = Int.ConstVarDecl("a");
			ModuleDeclNode moduleDeclNode = Module("mod").With(varDeclNode);
			MemberAccessNode memberAccessNode = UAccess("mod", "a");
			memberAccessNode.Item.Type = moduleDeclNode.Type;

			typeEvaluatorListener.Leave(memberAccessNode);

			Assert.AreEqual(true, memberAccessNode.IsConst);
		}
		[TestMethod()]
		[ExpectedException(typeof(AccessedMemberNotFound))]
		public void Leave_MemberAccessNode_ThrowsIfMemberNotFound() {
			VarDeclNode varDeclNode = Int.VarDecl("a");
			ModuleDeclNode moduleDeclNode = Module("mod").With(varDeclNode);
			MemberAccessNode memberAccessNode = UAccess("mod", "b");
			memberAccessNode.Item.Type = moduleDeclNode.Type;

			typeEvaluatorListener.Leave(memberAccessNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(CannotAccessMemberOfThisType))]
		public void Leave_MemberAccessNode_ThrowsIfItemNotStructOrModule() {
			FuncDeclNode funcDeclNode = TypeNode.Void.Function("func");
			MemberAccessNode memberAccessNode = UAccess("mod", "a");
			memberAccessNode.Item.Type = funcDeclNode.Type;

			typeEvaluatorListener.Leave(memberAccessNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(AlreadyHandledException))]
		public void Leave_MemberAccessNode_ThrowsIfAlreadyHandled() {
			MemberAccessNode memberAccessNode = UAccess("mod", "a");
			memberAccessNode.Item.Type = Invalid;

			typeEvaluatorListener.Leave(memberAccessNode);
		}
		[TestMethod()]
		[ExpectedException(typeof(CannotAccessMemberOfThisType))]
		public void Leave_MemberAccessNode_ThrowsIfMemberAnotherType() {
			MemberAccessNode memberAccessNode = UAccess("mod", "a");
			memberAccessNode.Item.Type = Int;

			typeEvaluatorListener.Leave(memberAccessNode);
		}
		#endregion

		#region Leave(UserDefinedTypeNode node) Tests
		[TestMethod()]
		public void Leave_UserDefinedTypeNode_LinksDeclIfModule() {
			ModuleDeclNode moduleDeclNode = Module("modulename");
			contextSymbolTable.Record(moduleDeclNode);
			UserDefinedTypeNode userDefinedTypeNode = new UserDefinedTypeNode(null, "modulename", UserDefinedTypeNode.Types.Module);

			typeEvaluatorListener.Leave(userDefinedTypeNode);

			Assert.AreEqual(moduleDeclNode, userDefinedTypeNode.Decl);
		}
		[TestMethod()]
		public void Leave_UserDefinedTypeNode_LinksDeclIfStruct() {
			StructDeclNode structDeclNode = StructDecl("structname");
			contextSymbolTable.Record(structDeclNode);
			UserDefinedTypeNode userDefinedTypeNode = new UserDefinedTypeNode(null, "structname", UserDefinedTypeNode.Types.Struct);

			typeEvaluatorListener.Leave(userDefinedTypeNode);

			Assert.AreEqual(structDeclNode, userDefinedTypeNode.Decl);
		}
		[TestMethod()]
		public void Leave_UserDefinedTypeNode_LinksDeclIfNestedModule1() {
			FuncDeclNode funcDeclNode = Int.Function("b");
			ModuleDeclNode innerModuleDeclNode = Module("a").With(funcDeclNode);
			ModuleDeclNode moduleDeclNode = Module("modulename").With(innerModuleDeclNode);
			contextSymbolTable.Record(moduleDeclNode);
			contextSymbolTable.Record(innerModuleDeclNode);
			contextSymbolTable.Record(funcDeclNode);
			UserDefinedTypeNode userDefinedTypeNode = new UserDefinedTypeNode(null, "b", UserDefinedTypeNode.Types.Function, new List<string>() { "modulename", "a" });

			typeEvaluatorListener.Leave(userDefinedTypeNode);

			Assert.AreEqual(funcDeclNode, userDefinedTypeNode.Decl);
		}
		[TestMethod()]
		public void Leave_UserDefinedTypeNode_LinksDeclIfNestedModule2() {
			ModuleDeclNode innerModuleDeclNode = Module("a");
			ModuleDeclNode moduleDeclNode = Module("modulename").With(innerModuleDeclNode);
			contextSymbolTable.Record(moduleDeclNode);
			contextSymbolTable.Record(innerModuleDeclNode);
			UserDefinedTypeNode userDefinedTypeNode = new UserDefinedTypeNode(null, "a", UserDefinedTypeNode.Types.Function, new List<string>() { "modulename" });

			typeEvaluatorListener.Leave(userDefinedTypeNode);

			Assert.AreEqual(innerModuleDeclNode, userDefinedTypeNode.Decl);
		}
		[TestMethod()]
		public void Leave_UserDefinedTypeNode_LinksDeclIfNestedStruct() {
			StructDeclNode innerStructDeclNode = StructDecl("a");
			ModuleDeclNode moduleDeclNode = Module("modulename").With(innerStructDeclNode);
			contextSymbolTable.Record(moduleDeclNode);
			contextSymbolTable.Record(innerStructDeclNode);
			UserDefinedTypeNode userDefinedTypeNode = new UserDefinedTypeNode(null, "a", UserDefinedTypeNode.Types.Struct, new List<string>() { "modulename", "a" });

			typeEvaluatorListener.Leave(userDefinedTypeNode);

			Assert.AreEqual(innerStructDeclNode, userDefinedTypeNode.Decl);
		}
		[TestMethod()]
		[ExpectedException(typeof(UndeclaredType))]
		public void Leave_UserDefinedTypeNode_ThrowsIfUndeclaredType() {
			UserDefinedTypeNode userDefinedTypeNode = new UserDefinedTypeNode(null, "a", UserDefinedTypeNode.Types.Struct);

			typeEvaluatorListener.Leave(userDefinedTypeNode);
		}
		#endregion

		#region Leave(IExpr node) Tests
		[TestMethod()]
		[ExpectedException(typeof(UnreachableException))]
		public void Leave_IExpr_ThrowsIfCalled() {
			IExpr expr = Literal(1).UPlus(null);

			typeEvaluatorListener.Leave(expr);
		}
		#endregion
	}
}
