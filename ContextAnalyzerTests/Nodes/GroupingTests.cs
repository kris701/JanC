using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using Nodes.ASTHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class GroupingTests : BaseRulesTests {

		public static List<object[]> TypeMatchesValueType_Data =>
			new List<object[]> {
				new object[] { Literal(1) },
				new object[] { Literal(1.0) },
				new object[] { Literal(true) },
				new object[] { Literal("string") },
			};
		[DataTestMethod]
		[DynamicData(nameof(TypeMatchesValueType_Data))]
		public void TypeMatchesValueType(IExpr value) {
			var grouping = new GroupingExprNode(null, value);
			global.With(grouping);

			ThrowErrorsIfAny();
			Assert.AreEqual(value.Type, grouping.Type);
		}

	}
}
