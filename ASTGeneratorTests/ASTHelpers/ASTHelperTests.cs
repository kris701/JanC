//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ASTGenerator.ASTHelpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Nodes;

//namespace ASTGenerator.ASTHelpers.Tests {
//	[TestClass()]
//	public class ASTHelperTests {
//		#region Node Generation

//		private static IEnumerable<object[]> AllDataTypes => new List<object[]>() {
//			new object[] { Root(), typeof(GlobalScopeNode) },
//			new object[] { ASTHelper.EveryTask(1), typeof(EveryTaskNode) },
//			new object[] { ASTHelper.OnceTask(), typeof(OnceTaskNode) },
//			new object[] { ASTHelper.OnTask(null), typeof(OnTaskNode) },
//			new object[] { ASTHelper.IdleTask(), typeof(IdleTaskNode) },
//			new object[] { Int.Function(""), typeof(FuncDeclNode) },
//			new object[] { Float.Function(""), typeof(FuncDeclNode) },
//			new object[] { TypeNode.String.Function(""), typeof(FuncDeclNode) },
//			new object[] { TypeNode.Void.Function(""), typeof(FuncDeclNode) },
//			new object[] { ASTHelper.BoolFunction(""), typeof(FuncDeclNode) },
//			new object[] { ASTHelper.IntParameter(""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.FloatParameter(""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.StringParameter(""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.VoidCall(""), typeof(CallExprNode) },
//			new object[] { Call(""), typeof(CallExprNode) },
//			new object[] { ASTHelper.FloatCall(""), typeof(CallExprNode) },
//			new object[] { ASTHelper.StringCall(""), typeof(CallExprNode) },
//			new object[] { ASTHelper.BoolCall(""), typeof(CallExprNode) },
//			new object[] { Argument(0), typeof(ArgNode) },
//			new object[] { Argument(0), typeof(ArgNode) },
//			new object[] { Argument(""), typeof(ArgNode) },
//			new object[] { Literal(0), typeof(IntLiteralNode) },
//			new object[] { Literal(0), typeof(FloatLiteralNode) },
//			new object[] { Literal(""), typeof(StringLiteralNode) },
//			new object[] { Int.VarDecl(""), typeof(VarDeclNode) },
//			new object[] { Int.VarDecl("", 2), typeof(VarDeclNode) },
//			new object[] { Int.ConstVarDecl(""), typeof(VarDeclNode) },
//			new object[] { Int.ConstVarDecl("", 2), typeof(VarDeclNode) },
//			new object[] { Float.VarDecl(""), typeof(VarDeclNode) },
//			new object[] { Float.VarDecl("", 2), typeof(VarDeclNode) },
//			new object[] { ASTHelper.FloatConstDecl(""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.FloatConstDecl("", 2), typeof(VarDeclNode) },
//			new object[] { TypeNode.String.VarDecl(""), typeof(VarDeclNode) },
//			new object[] { TypeNode.String.VarDecl("", ""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.StringConstDecl(""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.StringConstDecl("", ""), typeof(VarDeclNode) },
//			new object[] { StructDecl(""), typeof(StructDeclNode) },
//			new object[] { ASTHelper.StructConstDecl("",""), typeof(VarDeclNode) },
//			new object[] { ASTHelper.StructVarDecl("",""), typeof(VarDeclNode) },
//			new object[] { StructLiteral(""), typeof(StructLiteralNode) },
//			new object[] { StructMember(5), typeof(StructLiteralMemberNode) },
//			new object[] { StructMember(5, ""), typeof(StructLiteralMemberNode) },
//			new object[] { StructMember(5), typeof(StructLiteralMemberNode) },
//			new object[] { StructMember(5, ""), typeof(StructLiteralMemberNode) },
//			new object[] { StructMember(""), typeof(StructLiteralMemberNode) },
//			new object[] { StructMember("", ""), typeof(StructLiteralMemberNode) },
//			new object[] { ASTHelper.MemberAccess("",""), typeof(MemberAccessNode) },
//			new object[] { Identifier(""), typeof(IdentifierExpr) },
//			new object[] { ASTHelper.BinaryOp(null, "", 5), typeof(BinaryExprNode) },
//			new object[] { ASTHelper.BinaryOp(null, "", 5f), typeof(BinaryExprNode) },
//			new object[] { ASTHelper.Assign(null, null), typeof(AssignStmtNode) },
//			new object[] { ASTHelper.PlusAssign(null, null), typeof(AssignStmtNode) },
//		};
//		[DataTestMethod]
//		[DynamicData(nameof(AllDataTypes))]
//		public void ReturnsCorrectNodeType(IASTNode node, Type type) {
//			// ARRANGE
//			IASTNode checknode = null;

//			// ACT
//			checknode = node;

//			// ASSERT
//			Assert.IsInstanceOfType(checknode, type);
//		}

//		[TestMethod()]
//		public void Root_IsValidContext() {
//			// ARRANGE

//			// ACT
//			GlobalScopeNode node = Root();

//			// ASSERT
//			Assert.IsInstanceOfType(node.Content, typeof(List<IUnit>));
//			Assert.IsNull(node.Context);
//		}

//		[TestMethod()]
//		public void EveryTask_IsValidContext() {
//			// ARRANGE
//			int taskDelay = 5;

//			// ACT
//			EveryTaskNode node = ASTHelper.EveryTask(taskDelay);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(taskDelay.ToString(), node.Delay);
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void OnceTask_IsValidContext() {
//			// ARRANGE

//			// ACT
//			OnceTaskNode node = ASTHelper.OnceTask();

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void OnTask_IsValidContext() {
//			// ARRANGE
//			IExpr condiditon = null;

//			// ACT
//			OnTaskNode node = ASTHelper.OnTask(condiditon);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(condiditon, node.Condition);
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void IdleTask_IsValidContext() {
//			// ARRANGE

//			// ACT
//			IdleTaskNode node = ASTHelper.IdleTask();

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.IsNull(node.Context);
//		}

//		[TestMethod()]
//		public void IntFunction_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			FuncDeclNode node = Int.Function(name);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Int, node.ReturnType);
//			Assert.IsInstanceOfType(node.Parameters, typeof(List<VarDeclNode>));
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void FloatFunction_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			FuncDeclNode node = Float.Function(name);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Float, node.ReturnType);
//			Assert.IsInstanceOfType(node.Parameters, typeof(List<VarDeclNode>));
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void StringFunction_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			FuncDeclNode node = TypeNode.String.Function(name);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.String, node.ReturnType);
//			Assert.IsInstanceOfType(node.Parameters, typeof(List<VarDeclNode>));
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void VoidFunction_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			FuncDeclNode node = TypeNode.Void.Function(name);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Void, node.ReturnType);
//			Assert.IsInstanceOfType(node.Parameters, typeof(List<VarDeclNode>));
//			Assert.IsNull(node.Context);
//		}
//		[TestMethod()]
//		public void BoolFunction_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			FuncDeclNode node = ASTHelper.BoolFunction(name);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Body, typeof(BlockNode));
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Bool, node.ReturnType);
//			Assert.IsInstanceOfType(node.Parameters, typeof(List<VarDeclNode>));
//			Assert.IsNull(node.Context);
//		}

//		[TestMethod()]
//		public void IntParameter_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = ASTHelper.IntParameter(name);

//			// ASSERT
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Int, node.Type);
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void FloatParameter_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = ASTHelper.FloatParameter(name);

//			// ASSERT
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Float, node.Type);
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void StringParameter_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = ASTHelper.StringParameter(name);

//			// ASSERT
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.String, node.Type);
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//		}

//		/*
//		[TestMethod()]
//		public void VoidCall_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			CallExprNode node = ASTHelper.VoidCall(name);

//			// ASSERT
//			Assert.AreEqual(name, ((IdentifierExpr)node.Item).Name);
//			Assert.AreEqual(TypeNode.Void, node.Item.Type);
//			Assert.AreEqual(null, node.Item.Context);
//			Assert.AreEqual(TypeNode.Void, node.Type);
//			Assert.IsInstanceOfType(node.Arguments, typeof(List<ArgNode>));
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void IntCall_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			CallExprNode node = Call(name);

//			// ASSERT
//			Assert.AreEqual(name, ((IdentifierExpr)node.Item).Name);
//			Assert.AreEqual(Int, node.Item.Type);
//			Assert.AreEqual(null, node.Item.Context);
//			Assert.AreEqual(Int, node.Type);
//			Assert.IsInstanceOfType(node.Arguments, typeof(List<ArgNode>));
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void FloatCall_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			CallExprNode node = ASTHelper.FloatCall(name);

//			// ASSERT
//			Assert.AreEqual(name, node.Identifier.Name);
//			Assert.AreEqual(Float, node.Identifier.Type);
//			Assert.AreEqual(null, node.Identifier.Context);
//			Assert.AreEqual(Float, node.Type);
//			Assert.IsInstanceOfType(node.Arguments, typeof(List<ArgNode>));
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void StringCall_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			CallExprNode node = ASTHelper.StringCall(name);

//			// ASSERT
//			Assert.AreEqual(name, node.Identifier.Name);
//			Assert.AreEqual(TypeNode.String, node.Identifier.Type);
//			Assert.AreEqual(null, node.Identifier.Context);
//			Assert.AreEqual(TypeNode.String, node.Type);
//			Assert.IsInstanceOfType(node.Arguments, typeof(List<ArgNode>));
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void BoolCall_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			CallExprNode node = ASTHelper.BoolCall(name);

//			// ASSERT
//			Assert.AreEqual(name, node.Identifier.Name);
//			Assert.AreEqual(Bool, node.Identifier.Type);
//			Assert.AreEqual(null, node.Identifier.Context);
//			Assert.AreEqual(Bool, node.Type);
//			Assert.IsInstanceOfType(node.Arguments, typeof(List<ArgNode>));
//			Assert.AreEqual(null, node.Context);
//		}*/

//		[TestMethod()]
//		public void IntArgument_IsValidContext() {
//			// ARRANGE
//			int value = 25;

//			// ACT
//			ArgNode node = Argument(value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(IntLiteralNode));
//			Assert.AreEqual(value, ((IntLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, ((IntLiteralNode)node.Value).Context);
//			Assert.AreEqual(TypeNode.Int, node.Type);
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void FloatArgument_IsValidContext() {
//			// ARRANGE
//			int value = 25;

//			// ACT
//			ArgNode node = Argument(value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(FloatLiteralNode));
//			Assert.AreEqual(value, ((FloatLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, ((FloatLiteralNode)node.Value).Context);
//			Assert.AreEqual(TypeNode.Float, node.Type);
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void StringArgument_IsValidContext() {
//			// ARRANGE
//			string value = "25";

//			// ACT
//			ArgNode node = Argument(value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(StringLiteralNode));
//			Assert.AreEqual(value, ((StringLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, ((StringLiteralNode)node.Value).Context);
//			Assert.AreEqual(TypeNode.String, node.Type);
//			Assert.AreEqual(null, node.Context);
//		}

//		[TestMethod()]
//		public void IntLiteral_IsValidContext() {
//			// ARRANGE
//			int value = 25;

//			// ACT
//			IntLiteralNode node = Literal(value);

//			// ASSERT
//			Assert.AreEqual(value, node.Value);
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void FloatLiteral_IsValidContext() {
//			// ARRANGE
//			int value = 25;

//			// ACT
//			FloatLiteralNode node = Literal(value);

//			// ASSERT
//			Assert.AreEqual(value, node.Value);
//			Assert.AreEqual(null, node.Context);
//		}
//		[TestMethod()]
//		public void StringLiteral_IsValidContext() {
//			// ARRANGE
//			string value = "25";

//			// ACT
//			StringLiteralNode node = Literal(value);

//			// ASSERT
//			Assert.AreEqual(value, node.Value);
//			Assert.AreEqual(null, node.Context);
//		}

//		[TestMethod()]
//		public void IntDecl_IsValidContext() {
//			// ARRANGE
//			string name = "testname";
//			int value = 25;

//			// ACT
//			VarDeclNode node = Int.VarDecl(name, value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(IntLiteralNode));
//			Assert.AreEqual(value, ((IntLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Int, node.Type);
//		}
//		[TestMethod()]
//		public void IntDecl_IsValidContext_Null() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = Int.VarDecl(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Int, node.Type);
//		}
//		[TestMethod()]
//		public void IntConstDecl_IsValidContext() {
//			// ARRANGE
//			string name = "testname";
//			int value = 25;

//			// ACT
//			VarDeclNode node = Int.ConstVarDecl(name, value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(IntLiteralNode));
//			Assert.AreEqual(value, ((IntLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(true, node.IsConst);
//			Assert.AreEqual(TypeNode.Int, node.Type);
//		}
//		[TestMethod()]
//		public void IntConstDecl_IsValidContext_Null() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = Int.ConstVarDecl(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(true, node.IsConst);
//			Assert.AreEqual(TypeNode.Int, node.Type);
//		}
//		[TestMethod()]
//		public void FloatDecl_IsValidContext() {
//			// ARRANGE
//			string name = "testname";
//			int value = 25;

//			// ACT
//			VarDeclNode node = Float.VarDecl(name, value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(FloatLiteralNode));
//			Assert.AreEqual(value, ((FloatLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Float, node.Type);
//		}
//		[TestMethod()]
//		public void FloatDecl_IsValidContext_Null() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = Float.VarDecl(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.Float, node.Type);
//		}
//		[TestMethod()]
//		public void FloatConstDecl_IsValidContext() {
//			// ARRANGE
//			string name = "testname";
//			int value = 25;

//			// ACT
//			VarDeclNode node = ASTHelper.FloatConstDecl(name, value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(FloatLiteralNode));
//			Assert.AreEqual(value, ((FloatLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(true, node.IsConst);
//			Assert.AreEqual(TypeNode.Float, node.Type);
//		}
//		[TestMethod()]
//		public void FloatConstDecl_IsValidContext_Null() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = ASTHelper.FloatConstDecl(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(true, node.IsConst);
//			Assert.AreEqual(TypeNode.Float, node.Type);
//		}
//		[TestMethod()]
//		public void StringDecl_IsValidContext() {
//			// ARRANGE
//			string name = "testname";
//			string value = "25";

//			// ACT
//			VarDeclNode node = TypeNode.String.VarDecl(name, value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(StringLiteralNode));
//			Assert.AreEqual(value, ((StringLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.String, node.Type);
//		}
//		[TestMethod()]
//		public void StringDecl_IsValidContext_Null() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = TypeNode.String.VarDecl(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.String, node.Type);
//		}
//		[TestMethod()]
//		public void StringConstDecl_IsValidContext() {
//			// ARRANGE
//			string name = "testname";
//			string value = "25";

//			// ACT
//			VarDeclNode node = ASTHelper.StringConstDecl(name, value);

//			// ASSERT
//			Assert.IsInstanceOfType(node.Value, typeof(StringLiteralNode));
//			Assert.AreEqual(value, ((StringLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(true, node.IsConst);
//			Assert.AreEqual(TypeNode.String, node.Type);
//		}
//		[TestMethod()]
//		public void StringConstDecl_IsValidContext_Null() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			VarDeclNode node = ASTHelper.StringConstDecl(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Value);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(true, node.IsConst);
//			Assert.AreEqual(TypeNode.String, node.Type);
//		}
//		[TestMethod()]
//		public void StructVarDecl_IsValidContext() {
//			// ARRANGE
//			string structname = "testname";
//			string varname = "varname";

//			// ACT
//			VarDeclNode node = ASTHelper.StructVarDecl(structname, varname);

//			// ASSERT
//			Assert.AreEqual(varname, node.Name);
//			Assert.AreEqual(null, node.Type.Context);
//			Assert.IsInstanceOfType(node.Type, typeof(UserDefinedTypeNode));
//			Assert.AreEqual(structname, node.Type.Name);
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(null, node.Value);
//		}

//		[TestMethod()]
//		public void StructDecl_IsValidContext() {
//			// ARRANGE
//			string structname = "testname";

//			// ACT
//			StructDeclNode node = StructDecl(structname);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(structname, node.Name);
//			Assert.IsInstanceOfType(node.Members, typeof(List<VarDeclNode>));
//		}
//		[TestMethod()]
//		public void StructLiteral_IsValidContext() {
//			// ARRANGE
//			string structname = "testname";

//			// ACT
//			StructLiteralNode node = StructLiteral(structname);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(null, node.Type.Context);
//			Assert.IsInstanceOfType(node.Type, typeof(UserDefinedTypeNode));
//			Assert.AreEqual(structname, node.Type.Name);
//			Assert.IsInstanceOfType(node.Members, typeof(List<StructLiteralMemberNode>));
//		}
//		[TestMethod()]
//		public void IntStructMember_IsValidContext() {
//			// ARRANGE
//			int value = 5;

//			// ACT
//			StructLiteralMemberNode node = StructMember(value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(null, node.Name);
//			Assert.IsInstanceOfType(node.Value, typeof(IntLiteralNode));
//			Assert.AreEqual(value, ((IntLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, ((IntLiteralNode)node.Value).Context);
//		}
//		[TestMethod()]
//		public void FloatStructMember_IsValidContext() {
//			// ARRANGE
//			float value = 5;

//			// ACT
//			StructLiteralMemberNode node = StructMember(value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(null, node.Name);
//			Assert.IsInstanceOfType(node.Value, typeof(FloatLiteralNode));
//			Assert.AreEqual(value, ((FloatLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, ((FloatLiteralNode)node.Value).Context);
//		}
//		[TestMethod()]
//		public void StringStructMember_IsValidContext() {
//			// ARRANGE
//			string value = "5";

//			// ACT
//			StructLiteralMemberNode node = StructMember(value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(null, node.Name);
//			Assert.IsInstanceOfType(node.Value, typeof(StringLiteralNode));
//			Assert.AreEqual(value, ((StringLiteralNode)node.Value).Value);
//			Assert.AreEqual(null, ((StringLiteralNode)node.Value).Context);
//		}
//		[TestMethod()]
//		public void MemberAccess_IsValidContext_Name() {
//			// ARRANGE
//			string name = "5";
//			string membername = "5";

//			// ACT
//			MemberAccessNode node = ASTHelper.MemberAccess(name, membername);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(membername, node.MemberName);
//			Assert.IsInstanceOfType(node.Item, typeof(IdentifierExpr));
//			Assert.AreEqual(null, ((IdentifierExpr)node.Item).Context);
//		}
//		[TestMethod()]
//		public void MemberAccess_IsValidContext_Item() {
//			// ARRANGE
//			IdentifierExpr identifier = new IdentifierExpr(null, "name");
//			string membername = "5";

//			// ACT
//			MemberAccessNode node = ASTHelper.MemberAccess(identifier, membername);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(membername, node.MemberName);
//			Assert.IsInstanceOfType(node.Item, typeof(IdentifierExpr));
//			Assert.AreEqual(null, ((IdentifierExpr)node.Item).Context);
//		}
//		/*
//		[TestMethod()]
//		public void IntIdentifier_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			IdentifierExpr node = Identifier(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(Int, node.Type);
//		}
//		[TestMethod()]
//		public void FloatIdentifier_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			IdentifierExpr node = ASTHelper.FloatIdentifier(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(Float, node.Type);
//		}
//		[TestMethod()]
//		public void StringIdentifier_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			IdentifierExpr node = ASTHelper.StringIdentifier(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(TypeNode.String, node.Type);
//		}
//		[TestMethod()]
//		public void BoolIdentifier_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			IdentifierExpr node = ASTHelper.BoolIdentifier(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(Bool, node.Type);
//		}
//		*/
//		[TestMethod()]
//		public void Identifier_IsValidContext() {
//			// ARRANGE
//			string name = "testname";

//			// ACT
//			IdentifierExpr node = Identifier(name);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(name, node.Name);
//			Assert.AreEqual(null, node.Type);
//		}

//		[TestMethod()]
//		public void BinaryOp_IsValidContext_Int() {
//			// ARRANGE
//			IExpr inNode = null;
//			string op = "+";
//			int value = 5;

//			// ACT
//			BinaryExprNode node = ASTHelper.BinaryOp(inNode, op, value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(inNode, node.Left);
//			Assert.AreEqual(op, node.Operator);
//			Assert.IsInstanceOfType(node.Right, typeof(IntLiteralNode));
//			Assert.AreEqual(value, ((IntLiteralNode)node.Right).Value);
//		}
//		[TestMethod()]
//		public void BinaryOp_IsValidContext_Float() {
//			// ARRANGE
//			IExpr inNode = null;
//			string op = "+";
//			float value = 5;

//			// ACT
//			BinaryExprNode node = ASTHelper.BinaryOp(inNode, op, value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(inNode, node.Left);
//			Assert.AreEqual(op, node.Operator);
//			Assert.IsInstanceOfType(node.Right, typeof(FloatLiteralNode));
//			Assert.AreEqual(value, ((FloatLiteralNode)node.Right).Value);
//		}

//		[TestMethod()]
//		public void Assign_IsValidContext() {
//			// ARRANGE
//			IExpr inNode = null;
//			IExpr value = null;

//			// ACT
//			AssignStmtNode node = ASTHelper.Assign(inNode, value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(inNode, node.Location);
//			Assert.AreEqual(AssignOperator.Assign, node.Operator);
//			Assert.AreEqual(value, node.Value);
//		}
//		[TestMethod()]
//		public void PlusAssign_IsValidContext() {
//			// ARRANGE
//			IExpr inNode = null;
//			IExpr value = null;

//			// ACT
//			AssignStmtNode node = ASTHelper.PlusAssign(inNode, value);

//			// ASSERT
//			Assert.AreEqual(null, node.Context);
//			Assert.AreEqual(inNode, node.Location);
//			Assert.AreEqual(AssignOperator.PlusAssign, node.Operator);
//			Assert.AreEqual(value, node.Value);
//		}

//		#endregion

//		[TestMethod()]
//		public void GlobalScopeNode_With() {
//			// ARRANGE
//			GlobalScopeNode node = Root();
//			IUnit node2 = Int.Function("test");

//			// ACT
//			node.With(node2);

//			// ASSERT
//			Assert.AreEqual(node2, node.Content[0]);
//		}
//		[TestMethod()]
//		public void FuncDeclNode_With() {
//			// ARRANGE
//			FuncDeclNode node = TypeNode.Void.Function("abc");
//			VarDeclNode node2 = Int.VarDecl("test");

//			// ACT
//			node.With(node2);

//			// ASSERT
//			Assert.AreEqual(node2, node.Parameters[0]);
//		}
//		[TestMethod()]
//		public void CallExprNode_With() {
//			// ARRANGE
//			CallExprNode node = Call("abc");
//			ArgNode node2 = Argument(5);

//			// ACT
//			node.With(node2);

//			// ASSERT
//			Assert.AreEqual(node2, node.Arguments[0]);
//			Assert.AreEqual(node2.Value, node.Arguments[0].Value);
//		}
//		[TestMethod()]
//		public void StructDeclNode_With() {
//			// ARRANGE
//			StructDeclNode node = StructDecl("abc");
//			VarDeclNode node2 = Int.VarDecl("a", 5);

//			// ACT
//			node.With(node2);

//			// ASSERT
//			Assert.AreEqual(node2, node.Members[0]);
//			Assert.AreEqual(node2.Value, node.Members[0].Value);
//		}
//		[TestMethod()]
//		public void StructLiteralNode_With() {
//			// ARRANGE
//			StructLiteralNode node = StructLiteral("abc");
//			StructLiteralMemberNode node2 = StructMember(5, "a");

//			// ACT
//			node.With(node2);

//			// ASSERT
//			Assert.AreEqual(node2, node.Members[0]);
//			Assert.AreEqual(node2.Value, node.Members[0].Value);
//		}

//		[TestMethod()]
//		public void PrependBody_With() {
//			// ARRANGE
//			FuncDeclNode funcDeclNode1 = Int.Function("a");
//			VarDeclNode varDeclNode1 = Int.VarDecl("a");
//			VarDeclNode varDeclNode2 = Float.VarDecl("a");

//			// ACT
//			funcDeclNode1.PrependBody(varDeclNode1);
//			funcDeclNode1.PrependBody(varDeclNode2);

//			// ASSERT
//			Assert.AreEqual(varDeclNode1, ((BlockNode)funcDeclNode1.Children[1]).Content[1]);
//			Assert.AreEqual(varDeclNode2, ((BlockNode)funcDeclNode1.Children[1]).Content[0]);
//		}
//		[TestMethod()]
//		public void PrependBody_With_WithoutBody() {
//			// ARRANGE
//			FuncDeclNode funcDeclNode1 = Int.Function("a");
//			VarDeclNode varDeclNode1 = Int.VarDecl("a");
//			funcDeclNode1.Body = varDeclNode1;
//			VarDeclNode varDeclNode2 = Float.VarDecl("a");

//			// ACT
//			funcDeclNode1.PrependBody(varDeclNode2);

//			// ASSERT
//			Assert.AreEqual(varDeclNode1, ((BlockNode)funcDeclNode1.Children[1]).Content[0]);
//			Assert.AreEqual(varDeclNode2, ((BlockNode)funcDeclNode1.Children[1]).Content[1]);
//		}
//		[TestMethod()]
//		public void AppendBody_With() {
//			// ARRANGE
//			FuncDeclNode funcDeclNode1 = Int.Function("a");
//			VarDeclNode varDeclNode1 = Int.VarDecl("a");
//			VarDeclNode varDeclNode2 = Float.VarDecl("a");

//			// ACT
//			funcDeclNode1.AppendBody(varDeclNode1);
//			funcDeclNode1.AppendBody(varDeclNode2);

//			// ASSERT
//			Assert.AreEqual(varDeclNode1, ((BlockNode)funcDeclNode1.Children[1]).Content[0]);
//			Assert.AreEqual(varDeclNode2, ((BlockNode)funcDeclNode1.Children[1]).Content[1]);
//		}
//		[TestMethod()]
//		public void AppendBody_With_WithoutBody() {
//			// ARRANGE
//			FuncDeclNode funcDeclNode1 = Int.Function("a");
//			VarDeclNode varDeclNode1 = Int.VarDecl("a");
//			funcDeclNode1.Body = varDeclNode1;
//			VarDeclNode varDeclNode2 = Float.VarDecl("a");

//			// ACT
//			funcDeclNode1.AppendBody(varDeclNode2);

//			// ASSERT
//			Assert.AreEqual(varDeclNode1, ((BlockNode)funcDeclNode1.Children[1]).Content[0]);
//			Assert.AreEqual(varDeclNode2, ((BlockNode)funcDeclNode1.Children[1]).Content[1]);
//		}
//	}
//}
