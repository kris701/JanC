using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tables;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners.Tests {
	[TestClass()]
	public class SetupStructLiteralVarListenerTests {
		#region Test Setup

		private SymbolTable symbolTable;
		private SetupStructLiteralVarListener setupStructLiteralVarListener;

		[TestInitialize()]
		public void Setup() {
			symbolTable = new SymbolTable();
			setupStructLiteralVarListener = new SetupStructLiteralVarListener(symbolTable);
		}

		#endregion

		#region Constructor Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfConstructorItemsAreNull() {
			new SetupStructLiteralVarListener(null);
		}
		#endregion

		#region Enter(BlockNode block) Tests
		// Unsure about what these do

		#endregion

		#region Enter(GlobalScopeNode globalScope) Tests
		// Unsure about what these do

		#endregion
	}
}
