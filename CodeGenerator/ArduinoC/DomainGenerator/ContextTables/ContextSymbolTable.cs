using Nodes;
using System.Linq;
using Tables;

namespace CodeGenerator.ArduinoC.DomainGenerator.ContextTables {
	/// <summary>
	/// Lookup table for symbols
	/// </summary>
	internal class ContextSymbolTable : SymbolTable {
		public FuncDeclNode SetupNode {
			get => _scopes.First()
					.Where(decl => decl is FuncDeclNode)
					.Select(decl => (FuncDeclNode)decl).First(decl => decl.Name == "setup");
		}

		public ContextSymbolTable() {
			// Global scope.
			EnterScope();
		}
	}
}
