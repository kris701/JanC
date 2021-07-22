using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodesTests.TypeNodes {
	[TestClass]
	public class UserDefinedTypeNodeTests {
		[TestMethod]
		public void LinkNameTo_TakesNameRefOfTypeDecl() {
			var udtn = new UserDefinedTypeNode(null, "name of userdefinedtypenode", null);
			var tdn = new StructDeclNode(null, "name of StructDeclNode", null);

			udtn.LinkNameTo(tdn);

			Assert.IsTrue(ReferenceEquals(udtn.NameRef, tdn.NameRef));
		}

		[TestMethod]
		public void Equals_ReturnsFalseForOtherOfDifferentType() {
			var udtn = new UserDefinedTypeNode(null, "name of userdefinedtypenode", null);
			var tdn = new StructDeclNode(null, "name of StructDeclNode", null);

			bool expected = false;
			bool actual = udtn.Equals(tdn);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Equals_ReturnsTrueForOtherOfSameTypeAndNameAndValidity() {
			JanCParser.TypeLiteralContext ctx = null;
			string name = "name";
			var type = UserDefinedTypeNode.Types.Struct;

			var udtn1 = new UserDefinedTypeNode(ctx, name, type);
			var udtn2 = new UserDefinedTypeNode(ctx, name, type);

			Assert.IsTrue(udtn1.Equals(udtn2));
		}
	}
}
