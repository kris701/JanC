using Nodes;
using ContextAnalyzer.SemanticErrors;
using System.Linq;
using Tables;

namespace ContextAnalyzer.Tables {
	/// <summary>
	/// Lookup table for symbols
	/// </summary>
	public class ContextSymbolTable : SymbolTable {
		public ContextSymbolTable() {
			// Global scope.
			EnterScope();
		}

		/// <summary>
		/// Method to record a declaration, and add it to the symbol table (If it havent already been declared, if so throw an error)
		/// </summary>
		/// <param name="decl"></param>
		public new void Record(ITypedDecl decl) {
			if (IsAlreadyDeclaredInCurrentScope(decl)) {
				throw new SymbolAlreadyDeclared(
					current: decl,
					prev: GetDeclaration(decl.Name)!
				);
			}
			else {
				_currentScope.Add(decl);
			}
		}
	}
}
