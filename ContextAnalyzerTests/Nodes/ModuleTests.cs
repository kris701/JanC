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

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class ModuleTests : BaseRulesTests {
		public ModuleDeclNode module;

		[TestInitialize]
		public override void Setup() {
			base.Setup();
			module = Module("my_module");
			global.With(module);
		}

		[TestMethod]
		[DataRow("some_int")]
		[DataRow("x")]
		[DataRow("name")]
		public void ModuleLocalsCanBeInternallyAccessedByName(string name) {
			module
				.With(Int.VarDecl(name))
				.With(Identifier(name));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredSymbol))]
		[DataRow("some_int")]
		[DataRow("x")]
		[DataRow("name")]
		public void ModuleLocalsCanbotBeExternallyAccessedByName(string name) {
			module
				.With(Int.VarDecl(name));
			global
				.With(Identifier(name));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow("some_int")]
		[DataRow("x")]
		[DataRow("name")]
		public void ModuleLocalsCanBeInternallyAccessedByMemberAccess(string name) {
			module
				.With(Int.VarDecl(name))
				.With(module.UAccess(name));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow("some_int")]
		[DataRow("x")]
		[DataRow("name")]
		public void ModuleLocalsCanBeExternallyAccessedByMemberAccess(string name) {
			module
				.With(Int.VarDecl(name));
			global
				.With(module.UAccess(name));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredSymbol))]
		public void ModulesCannotReferToEachOthersLocalsByName() {
			module
				.With(Int.VarDecl("my_int"));
			global
				.With(Module("other_module").With(Identifier("my_int")));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void ModulesCanReferToEachOthersLocalsByMemberAccess() {
			VarDeclNode my_int = Int.VarDecl("my_int");
			VarDeclNode other_int = Int.VarDecl("other_int");
			ModuleDeclNode other_module = Module("other_module");
			module
				.With(my_int)
				.With(other_module.UAccess(other_int));
			global
				.With(other_module.With(other_int)
					.With(module.UAccess(my_int)));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow("some_int")]
		[DataRow("x")]
		[DataRow("name")]
		public void ModulesCanReferToOuterNames(string name) {
			VarDeclNode decl = Int.VarDecl(name);
			global = Root()
				.With(decl)
				.With(module);
			IdentifierExpr identifier = Identifier(name);
			module
				.With(identifier);

			ThrowErrorsIfAny();

			Assert.AreEqual(decl.Type, identifier.Type);
		}

		[TestMethod]
		[ExpectedException(typeof(SymbolAlreadyDeclared))]
		public void ModulesCannotHaveTheSameName() {
			ModuleDeclNode othermoduleDeclNode = Module(module.Name);
			global.With(othermoduleDeclNode);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void ModuleStructsCanBeUsedExternally() {
			module
				.With(StructDecl("Point").With(Int.VarDecl("x")));
			UserDefinedTypeNode PointType = new UserDefinedTypeNode(null, "Point", null, new List<string> { module.Name });
			global
				.With(new VarDeclNode(null, "point", PointType, null, false))
				.With(Identifier("point")
					.Assign(new StructLiteralNode(null, PointType, new List<StructLiteralMemberNode>())));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void ModuleFunctionsCanBeCalledExternally() {
			module
				.With(TypeNode.String.Function("get_message"));
			global
				.With(TypeNode.String.VarDecl("message"))
				.With(Identifier("message")
					.Assign(Call(module.UAccess("get_message"))));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidUsageOfStaticDeclaration))]
		public void ModulesCannotBeInstantiated() {
			global
				.With(new VarDeclNode(null, "module_instance", new UserDefinedTypeNode(null, module.Name), null, false));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredSymbol))]
		public void CannotAccessNonExistentModuleSymbol() {
			global
				.With(UAccess("non_module", "name"));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(UndeclaredType))]
		public void CannotAccessNonExistentModuleType() {
			global
				.With(new StructLiteralNode(null, new UserDefinedTypeNode(null, "structName", null, new List<string> { "non_module" }), new List<StructLiteralMemberNode>()));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		public void CanAccessModuleWithinModuleFromOutside() {
			VarDeclNode item = TypeNode.String.VarDecl("item");
			ModuleDeclNode innerModule = Module("innerModule")
				.With(item);
			module
				.With(innerModule);
			MemberAccessNode innerAccess = new MemberAccessNode(null, new MemberAccessNode(null, Identifier(module.Name), innerModule.Name), item.Name);
			global
				.With(innerAccess);

			ThrowErrorsIfAny();

			Assert.AreEqual(item.Type, innerAccess.Type);
		}
	}
}
