using Nodes;
using ContextAnalyzer.Tables;
using Tools;
using System;

namespace ContextAnalyzer.Listener {
	/// <summary>
	/// A listener to record all global declarations being made
	/// This makes so that we dont have to worry about for- or backwards declaration
	/// </summary>
	internal class GlobalsRecorderListener : ASTListener {
		private ContextSymbolTable _symbolTable;

		public GlobalsRecorderListener(ContextSymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		public void Enter(ITypeDecl node) {
			_symbolTable.Record(node);
		}
	}
}
