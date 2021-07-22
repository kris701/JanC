using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;

namespace NodesTests
{
	[TestClass]
    public class TypeLiteralTypesTests
    {
		private static IEnumerable<object[]> PrimitiveTypeNodes => new List<object[]>() {
			new object[] { Invalid, new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Invalid) },
			new object[] { TypeNode.Void,	 new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Void) },
			new object[] { Int,	 new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer) },
			new object[] { TypeNode.String,  new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.String) },
			new object[] { Float,   new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Float) },
			new object[] { Bool,    new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool) }
		};
		[DataTestMethod]
		[DynamicData(nameof(PrimitiveTypeNodes))]
		public void TypeLiteralTypes_PropertiesReturnCorrespondingPrimitiveTypeNodeWithoutContext(PrimitiveTypeNode actual, PrimitiveTypeNode expected) {
			Assert.IsNull(expected.Context);
			Assert.IsTrue(actual.Equals(expected));
		}

		[TestMethod]
		public void Ref_ReturnsRefTypeNodeWithCorrectSubtypeAndNoContext() {
			var actual = Ref(Int);
			var expected = new RefTypeNode(context: null, subType: Int);
			Assert.IsNull(expected.Context);
			Assert.IsTrue(actual.Equals(expected));
		}

		[TestMethod]
		public void Struct_ReturnsUserDefinedTypeNodeWithCorrectNameAndNoContext() {
			var actual = Ref(Int);
			var expected = new RefTypeNode(context: null, subType: Int);
			Assert.IsNull(expected.Context);
			Assert.IsTrue(actual.Equals(expected));
		}

	}
}
