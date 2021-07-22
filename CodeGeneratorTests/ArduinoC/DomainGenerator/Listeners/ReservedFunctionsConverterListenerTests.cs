using Nodes.ASTHelpers;
using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodes.ASTHelpers;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass()]
	public class ReservedFunctionsConverterListenerTests {
		#region Test Setup

		private ReservedFunctionsConverterListener reservedFunctionsConverterListener;

		[TestInitialize()]
		public void Setup() {
			reservedFunctionsConverterListener = new ReservedFunctionsConverterListener();
		}

		#endregion

		#region Enter(CallExprNode node)
		[TestMethod()]
		[DataRow("sleep", "vTaskDelay")]
		[DataRow("digitalRead", "digitalRead")]
		[DataRow("ref", "&")]
		public void ConvertsIfExist(string input, string output) {
			CallExprNode callExprNode = Call(input);

			reservedFunctionsConverterListener.Enter(callExprNode);

			Assert.AreEqual(output, ((IdentifierExpr)callExprNode.Item).Name);
		}
		#endregion
	}
}
