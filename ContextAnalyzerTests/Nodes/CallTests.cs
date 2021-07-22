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
	public class CallTests : BaseRulesTests {
		public static IEnumerable<object[]> CanCallFunction_Data => new List<object[]> {
			new object[]{ Int.Function("get_int_value") },
			new object[]{ Float.Function("float_function") },
			new object[]{ TypeNode.String.Function("read_message") },
		};
		[TestMethod]
		[DynamicData(nameof(CanCallFunction_Data), DynamicDataSourceType.Property)]
		public void CanCallFunction(FuncDeclNode function) {
			global = Root()
				.With(function)
				.With(Call(function));

			ThrowErrorsIfAny();
		}

		public static IEnumerable<object[]> CannotCallLiteral_Data => new List<object[]> {
			new object[]{ Literal(5) },
			new object[]{ Literal(0.5) },
			new object[]{ Literal("text") },
		};
		[TestMethod]
		[ExpectedException(typeof(CanOnlyCallFunction))]
		[DynamicData(nameof(CannotCallLiteral_Data))]
		public void CannotCallLiteral(ILiteral literal) {
			global
				.With(new CallExprNode(null, literal, new List<ArgNode>()));

			ThrowErrorsIfAny();
		}

		public static IEnumerable<object[]> CannotCallVariable_Data => new List<object[]> {
			new object[]{ Int.VarDecl("my_int", Literal(5)) },
			new object[]{ Float.VarDecl("my_float") },
			new object[]{ TypeNode.String.VarDecl("my_string") },
		};
		[TestMethod]
		[ExpectedException(typeof(CanOnlyCallFunction))]
		[DynamicData(nameof(CannotCallVariable_Data))]
		public void CannotCallVariable(VarDeclNode varDecl) {
			global
				.With(varDecl)
				.With(new CallExprNode(null, Identifier(varDecl), new List<ArgNode>()));

			ThrowErrorsIfAny();
		}

		public static IEnumerable<object[]> CanCallMemberAccessedFunction_Data => new List<object[]> {
			new object[]{ "module1", Int.Function("function") },
			new object[]{ "module_name", Float.Function("function") },
			new object[]{ "TemperatureSensor", TypeNode.String.Function("function") },
		};
		[TestMethod]
		[DynamicData(nameof(CanCallMemberAccessedFunction_Data))]
		public void CanCallMemberAccessedFunction(string moduleName, FuncDeclNode function) {
			var module = Module(moduleName).With(function);
			global
				.With(module);
				//.With(MemberAccess(Identifier(module.Name), function.Name));

			ThrowErrorsIfAny();
		}

		public static IEnumerable<object[]> CannotCallMemberAccessedVariable_Data => new List<object[]> {
			new object[]{ "module1", Int.VarDecl("some_int") },
			new object[]{ "module_name", Float.VarDecl("some_float") },
			new object[]{ "TemperatureSensor", TypeNode.String.VarDecl("some_string") },
		};
		[TestMethod]
		[DynamicData(nameof(CannotCallMemberAccessedVariable_Data))]
		public void CannotCallMemberAccessedVariable(string moduleName, VarDeclNode varDecl) {
			var module = Module(moduleName).With(varDecl);
			global
				.With(module)
				.With(module.UAccess(varDecl));

			ThrowErrorsIfAny();
		}
	}
}
