using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using Nodes.ASTHelpers;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class FunctionTests : BaseRulesTests {
		private CallExprNode call;
		private FuncDeclNode function;
		private FuncDeclNode voidFunction;

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			function = TypeNode.Void.Function("function");
			voidFunction = TypeNode.Void.Function("voidFunction");
			call = Call("function");
		}

		[TestMethod]
		public void CanCallFunctionBeforeDeclaration() {
			global
				.With(call)
				.With(function);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanCallFunctionAfterDeclaration() {
			global
				.With(function)
				.With(call);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredSymbol))]
		public void CannotCallUndeclaredFunction() {
			global
				.With(call);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanBeRecursive() {
			global
				.With(function
					.AppendBody(call));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanBeMutuallyRecursive() {
			var function2 = TypeNode.Void.Function("function2");
			var call2 = Call("function2");
			global
				.With(function
					.AppendBody(call2))
				.With(function2
					.AppendBody(call));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void FunctionsCannotBeInstanced() {
			global
				.With(function)
				.With(new VarDeclNode(null, "var", new UserDefinedTypeNode(null, function.Name), null, false));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void VoidFunctionCanOmitReturn() {
			global.With(voidFunction);

			ThrowErrorsIfAny();
		}

		public static List<object[]> MatchingValueAndReturnTypeWorks_Data =>
			new List<object[]> {
				new object[] { Int, Literal(-5) },
				new object[] { Int, Literal(0) },
				new object[] { Int, Literal(10) },
				new object[] { Bool, Literal(true) },
				new object[] { Bool, Literal(false) },
				new object[] { Float, Literal(0.0) },
				new object[] { Float, Literal(-1.0) },
				new object[] { Float, Literal(1.0) },
				new object[] { TypeNode.String, Literal("") },
				new object[] { TypeNode.String, Literal("hello world!") },
			};
		[DataTestMethod]
		[DynamicData(nameof(MatchingValueAndReturnTypeWorks_Data))]
		public void MatchingValueAndReturnTypeWorks(ITypeLiteral type, IExpr value) {
			global.With(BuildFunctionReturningValue(type, value));

			ThrowErrorsIfAny();
		}

		public static List<object[]> ReturnValueCannotMismatchReturnType_Data =>
			new List<object[]> {
				new object[] { Float, Literal(-5) },
				new object[] { Bool, Literal(0) },
				new object[] { TypeNode.String, Literal(10) },
				new object[] { Int, Literal(true) },
				new object[] { Float, Literal(false) },
				new object[] { Int, Literal(0.0) },
				new object[] { TypeNode.String, Literal(-1.0) },
				new object[] { Bool, Literal(1.0) },
				new object[] { Int, Literal("") },
				new object[] { Float, Literal("hello world!") },
				new object[] { TypeNode.Void, Literal(1) },
			};
		[DataTestMethod]
		[DynamicData(nameof(ReturnValueCannotMismatchReturnType_Data))]
		[ExpectedException(typeof(ReturnTypeMismatch))]
		public void ReturnValueCannotMismatchReturnType(ITypeLiteral type, IExpr value) {
			global.With(BuildFunctionReturningValue(type, value));

			ThrowErrorsIfAny();
		}

		public static List<object[]> InvalidValueCanMismatchReturnType_Data =>
			new List<object[]> {
				new object[] { Int, InvalidLiteral() },
				new object[] { Float, InvalidLiteral() },
				new object[] { TypeNode.String, InvalidLiteral() },
				new object[] { Bool, InvalidLiteral() },
				new object[] { Ref(Int), InvalidLiteral() },
				new object[] { Ref(TypeNode.String), InvalidLiteral() },
			};
		[DataTestMethod]
		[DynamicData(nameof(InvalidValueCanMismatchReturnType_Data))]
		public void InvalidValueCanMismatchReturnType(ITypeLiteral type, IExpr value) {
			global.With(BuildFunctionReturningValue(type, value));

			ThrowErrorsIfAny();
		}

		public static List<object[]> InvalidReturnTypeCanMismatchValue_Data =>
			new List<object[]> {
				new object[] { Invalid, Literal(1) },
				new object[] { Invalid, Literal(1.0) },
				new object[] { Invalid, Literal(true) },
				new object[] { Invalid, Literal("text") },
			};
		[DataTestMethod]
		[DynamicData(nameof(InvalidReturnTypeCanMismatchValue_Data))]
		public void InvalidReturnTypeCanMismatchValue(ITypeLiteral type, IExpr value) {
			global.With(BuildFunctionReturningValue(type, value));

			ThrowErrorsIfAny();
		}


		[TestMethod]
		public void FunctionCanReturnReference() {
			var variable = Int.VarDecl("x");
			var reference = Ref(Identifier(variable));
			global.With(variable);
			global.With(BuildFunctionReturningValue(Ref(Int), reference));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void FunctionCanReturnStruct() {
			var @struct = StructDecl("Point").With(Int.VarDecl("x"));
			var literal = StructLiteral(@struct.Type);
			global.With(@struct);
			global.With(BuildFunctionReturningValue(@struct.Type, literal));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void FunctionCannotReturnModule() {
			var module = Module("Controller");
			var function = TypeIdentifier(module).Function("moduleFunction");
			global.With(module).With(function);
			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void FunctionCannotReturnAFunction() {
			var functionA = Int.Function("functionA");
			var functionB = TypeIdentifier(functionA).Function("functionB");
			global.With(functionA);
			global.With(functionB);
			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(ExpectedLValue))]
		public void RefFunctionMustReturnLValue() {
			global.With(BuildFunctionReturningValue(Ref(Int), Literal(2)));
			ThrowErrorsIfAny();
		}

		private static FuncDeclNode BuildFunctionReturningValue(ITypeLiteral type, IExpr value) =>
			type.Function("function")
					.AppendBody(Return(value));
	}
}
