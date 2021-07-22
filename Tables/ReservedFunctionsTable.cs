using Nodes.ASTHelpers;
using Nodes;
using System.Collections.Generic;

namespace Tables {
	public static class ReservedFunctionsTable {

		private static readonly List<FuncDeclNode> builtInFunctions = new List<FuncDeclNode>() {
			BuiltinAST.DigitalReadDecl,
			BuiltinAST.DigitalWriteDecl,
			BuiltinAST.AnalogReadDecl,
			BuiltinAST.AnalogWriteDecl,
			BuiltinAST.SleepDecl
		};

		public static void InsertBuiltInFunctions(SymbolTable symbolTable) {
			foreach (var funcDecl in builtInFunctions)
				symbolTable.Record(funcDecl);
		}
	}
}
