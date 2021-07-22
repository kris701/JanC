using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodesTests {
	[TestClass]
	public class NodesHelperTests {
		[TestMethod]
		public void NewNodes_ReturnsListOfIastNode() {
			var actual = NodesHelper.NewNodes;

			Assert.IsTrue(actual is List<IASTNode>);
		}

		[TestMethod]
		public void With_AddsItemToList() {
			var list = new List<IASTNode>();
			var node = new NotNode(null, null);
			list.With(node);

			Assert.IsTrue(list.Contains(node));
		}

		[TestMethod]
		public void With_DoesNotAddNull() {
			var list = new List<IASTNode>();
			NotNode node = null;
			list.With(node);

			Assert.IsFalse(list.Contains(node));
		}

		[TestMethod]
		public void With_TakingSingleNodeReturnsRefToSameList() {
			var list = new List<IASTNode>();
			NotNode node = null;
			var newList = list.With(node);

			Assert.IsTrue(ReferenceEquals(list, newList));
		}

		[TestMethod]
		public void With_AddsAllElementsFromAListToAnother() {
			NotNode node1 = null;
			NotNode node2 = null;
			NotNode node3 = null;
			var list = new List<IASTNode> { node1 };
			var otherList = new List<IASTNode> { node2, node3 };

			list.With(otherList);
			int expected = 3;
			int actual = list.Count;
			Assert.AreEqual(actual, expected);
		}

		[TestMethod]
		public void With_TakingListOfNodesReturnsRefToSameList() {
			var list = new List<IASTNode>();
			var otherList = new List<IASTNode>();

			var newList = list.With(otherList);

			Assert.IsTrue(ReferenceEquals(list, newList));
		}
	}
}
