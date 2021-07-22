using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;

namespace NodesTests.TypeNodes {
	[TestClass]
	public class RefTypeNodeTests {
		[TestMethod]
		public void Constructor_SubtypeIsNotref_BottomTypeIsSatAsParameter() {
			var expected = Int;

			var rtn = new RefTypeNode(null, expected);
			var actual = rtn.BottomType;

			Assert.AreEqual(actual, expected);
		}
		[TestMethod]
		public void Constructor_SubtypeIsNotref_RefDepthIsSetToOne() {
			var rtn = new RefTypeNode(null, Int);
			int actual = rtn.RefDepth;
			int expected = 1;

			Assert.AreEqual(actual, expected);
		}
		[TestMethod]
		public void Constructor_SubtypeIsref_RefDepthIsSetToTwo() {
			var rtn1 = new RefTypeNode(null, Int);
			var rtn2 = new RefTypeNode(null, rtn1);
			int actual = rtn2.RefDepth;
			int expected = 2;

			Assert.AreEqual(actual, expected);
		}
		[TestMethod]
		public void Constructor_SubtypeIsref_BottomType() {
			var rtn1 = new RefTypeNode(null, Int);
			var rtn2 = new RefTypeNode(null, rtn1);

			var expected = Int;
			var actual = rtn2.BottomType;
			
			Assert.AreEqual(actual, expected);
		}
		[TestMethod]
		public void Constructor_NameIsSatWhenSubTypeIsRef() {
			var rtn1 = new RefTypeNode(null, Int);
			var rtn2 = new RefTypeNode(null, rtn1);

			var actual = rtn2.Name;

			Assert.IsNotNull(actual);
		}
		[TestMethod]
		public void Constructor_NameIsSatWhenSubTypeIsNotRef() {
			var rtn1 = new RefTypeNode(null, Int);

			var actual = rtn1.Name;

			Assert.IsNotNull(actual);
		}
		[TestMethod]
		public void Equals_ReturnsTrueWhenOtherRefTypeNodeHasSameSubtype() {
			var rtn1 = new RefTypeNode(null, Int);
			var rtn2 = new RefTypeNode(null, Int);

			Assert.IsTrue(rtn1.Equals(rtn2));
		}
		[TestMethod]
		public void Equals_ReturnsFalseWhenOtherRefTypeNodeHasDifferentSubtype() {
			var rtn1 = new RefTypeNode(null, Int);
			var rtn2 = new RefTypeNode(null, TypeNode.String);

			Assert.IsFalse(rtn1.Equals(rtn2));
		}
		[TestMethod]
		public void Equals_ReturnsFalseWhenOtherIsDifferentType() {
			var rtn1 = new RefTypeNode(null, Int);
			var rtn2 = new ArgNode(null, null);

			Assert.IsFalse(rtn1.Equals(rtn2));
		}
		[TestMethod]
		public void GetHashCode_ReturnsHashOfName() {
			var rtn1 = new RefTypeNode(null, Int);

			int actual = rtn1.GetHashCode();
			int expected = rtn1.Name.GetHashCode();

			Assert.AreEqual(actual, expected);
		}
	}
}
