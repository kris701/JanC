using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodesTests.TypeNodes {
	[TestClass]
	public class PrimitiveTypeNodeTests {
		[TestMethod]
		public void Name_GetsStringRepresentationOfType() {
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool);

			Assert.AreEqual("Bool", ptn.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Name_SettingThrowsInvalidOperationException() {
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool);

			ptn.Name = "stuff";
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IsNumeric_ThrowsForTypeInvalid() {
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Invalid);
			bool numeric;

			numeric = ptn.IsNumeric;
		}

		public static IEnumerable<object[]> returnsTrueForIntegerAndFloat => new List<object[]>() {
			new object[] { PrimitiveTypeNode.Types.Integer },
			new object[] { PrimitiveTypeNode.Types.Float }
		};
		[DataTestMethod]
		[DynamicData(nameof(returnsTrueForIntegerAndFloat))]
		public void IsNumeric_returnsTrueForIntegerAndfloat(PrimitiveTypeNode.Types type) {
			var ptn = new PrimitiveTypeNode(null, type);

			Assert.IsTrue(ptn.IsNumeric);
		}

		public static IEnumerable<object[]> returnsFalseForVoidAndStringAndBool => new List<object[]>() {
			new object[] { PrimitiveTypeNode.Types.Void },
			new object[] { PrimitiveTypeNode.Types.String },
			new object[] { PrimitiveTypeNode.Types.Bool },
		};
		[DataTestMethod]
		[DynamicData(nameof(returnsFalseForVoidAndStringAndBool))]
		public void IsNumeric_returnsFalseForVoidAndStringAndBool(PrimitiveTypeNode.Types type) {
			var ptn = new PrimitiveTypeNode(null, type);

			Assert.IsFalse(ptn.IsNumeric);
		}

		[TestMethod]
		public void IsInvalid_returnsTrueForTypeInvalid() {
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Invalid);

			Assert.IsTrue(ptn.IsInvalid);
		}

		[TestMethod]
		public void IsInvalid_returnsFalseForTypeNotInvalid() {
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);

			Assert.IsFalse(ptn.IsInvalid);
		}

		[TestMethod]
		public void Equals_ReturnsFalseForOtherOfDifferentType() {
			var ptn1 = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);
			var nodeDiffType = new ArgNode(null, null);

			Assert.IsFalse(ptn1.Equals(nodeDiffType));
		}

		[TestMethod]
		public void Equals_ReturnsFalseForOtherPrimitiveTypeNodeWithDifferentPrimitiveType() {
			var ptn1 = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);
			var ptn2 = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Bool);

			Assert.IsFalse(ptn1.Equals(ptn2));
		}

		[TestMethod]
		public void Equals_ReturnsTrueForOtherPrimitiveTypeNodeWithSamePrimitiveType() {
			var ptn1 = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);
			var ptn2 = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);

			Assert.IsTrue(ptn1.Equals(ptn2));
		}

		[TestMethod]
		public void GetHashCode_ReturnsHashOfTypeField() {
			var ptn = new PrimitiveTypeNode(null, PrimitiveTypeNode.Types.Integer);
			int expected = PrimitiveTypeNode.Types.Integer.GetHashCode();
			int actual = ptn.GetHashCode();

			Assert.AreEqual(actual, expected);
		}

	}
}
