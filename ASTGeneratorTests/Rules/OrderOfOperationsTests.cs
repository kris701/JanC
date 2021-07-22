using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTGeneratorTests.Rules {
	[TestClass]
	public class OrderOfOperationsTests : BaseASTGeneratorTests {

		[TestMethod]
		[DataRow("+")]
		[DataRow("-")]
		[DataRow("*")]
		[DataRow("/")]
		[DataRow("==")]
		[DataRow("!=")]
		[DataRow(">")]
		[DataRow(">=")]
		[DataRow("<")]
		[DataRow("<=")]
		public void OperatorsParsesFromLeftToRight(string op) {
			Assert.AreEqual(Order.SameOrderLeftAssociative, DetermineOrder(op, op));
		}

		[TestMethod]
		public void PlusAndMinusHaveSameOrder() {
			Assert.AreEqual(Order.SameOrderLeftAssociative, DetermineOrder("+", "-"));
		}

		[TestMethod]
		public void MultiplyAndDivideHaveSameOrder() {
			Assert.AreEqual(Order.SameOrderLeftAssociative, DetermineOrder("+", "-"));
		}

		[TestMethod]
		[DataRow("+", "*")]
		[DataRow("+", "/")]
		[DataRow("-", "*")]
		[DataRow("-", "/")]
		public void MultiplicationAndDivisionPrecedesAdditionAndSubtraction(string addSub, string multDiv) {
			Assert.AreEqual(Order.BPrecedesA, DetermineOrder(addSub, multDiv));
		}

		[TestMethod]
		[DataRow("==", "!=")]
		[DataRow("!=", "<")]
		[DataRow("<", "<=")]
		[DataRow("<=", ">")]
		[DataRow(">", ">=")]
		[DataRow(">=", "==")]
		public void ComparisonOperatorsHaveSameOrder(string a, string b) {
			Assert.AreEqual(Order.SameOrderLeftAssociative, DetermineOrder(a, b));
		}

		[TestMethod]
		[DataRow("+", "==")]
		[DataRow("-", "!=")]
		[DataRow("*", "<")]
		[DataRow("/", "<=")]
		[DataRow("+", ">")]
		[DataRow("-", ">=")]
		public void ArithmeticOperatorsPrecedesComparisonOperators(string arithmetic, string comparator) {
			Assert.AreEqual(Order.APrecedesB, DetermineOrder(arithmetic, comparator));
		}

		[TestMethod]
		public void MemberAccessPrecedesArithmeticOperators() {
			var input = @"struct B { int c }
B b
a / b.c";
			var global = Parse(input);
			var expression = global.Content[2];
			Assert.IsInstanceOfType(expression, typeof(BinaryExprNode));
		}


		private static Order DetermineOrder(string a, string b) {
			var possibleOrders = DeterminePossibleOrdersWhenAIsLeftAndBIsRight(a, b);
			possibleOrders.IntersectWith(DeterminePossibleOrdersWhenBIsLeftAndAIsRight(a, b));

			Assert.AreEqual(1, possibleOrders.Count);
			return possibleOrders.First();
		}

		private static HashSet<Order> DeterminePossibleOrdersWhenAIsLeftAndBIsRight(string a, string b) {
			var outerBinary = Parse<BinaryExprNode>($"1{a}2{b}3");
			if (a.Equals(b))
				return new HashSet<Order> { outerBinary.Left is BinaryExprNode ? Order.SameOrderLeftAssociative : Order.SameOrderRightAssociative };
			else if (outerBinary.Operator.Equals(a))
				return new HashSet<Order> { Order.BPrecedesA, Order.SameOrderRightAssociative };
			else
				return new HashSet<Order> { Order.APrecedesB, Order.SameOrderLeftAssociative };
		}

		private static HashSet<Order> DeterminePossibleOrdersWhenBIsLeftAndAIsRight(string a, string b) {
			var outerBinary = Parse<BinaryExprNode>($"1{b}2{a}3");
			if (a.Equals(b))
				return new HashSet<Order> { outerBinary.Left is BinaryExprNode ? Order.SameOrderLeftAssociative : Order.SameOrderRightAssociative };
			else if (outerBinary.Operator.Equals(a))
				return new HashSet<Order> { Order.BPrecedesA, Order.SameOrderLeftAssociative };
			else
				return new HashSet<Order> { Order.APrecedesB, Order.SameOrderRightAssociative };
		}

		public enum Order {
			APrecedesB,
			BPrecedesA,
			SameOrderLeftAssociative,
			SameOrderRightAssociative,
		}
		private static HashSet<Order> OrderPossibilities =>
			((Order[])Enum.GetValues(typeof(Order))).ToHashSet();

	}
}
