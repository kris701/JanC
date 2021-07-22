using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tables {
	/// <summary>
	/// Lookup table for symbols
	/// </summary>
	public class SymbolTable {
		// Cannot be easily changed to a dictionary, because symbols can change names
		protected readonly Stack<List<ITypedDecl>> _scopes = new Stack<List<ITypedDecl>>();
		protected List<ITypedDecl> _currentScope { get => _scopes.Peek(); }
		public int ScopeCount { get => _scopes.Count; }
		public FuncDeclNode CurrentFunction { get; private set; }
		public bool IsAlreadyDeclaredInCurrentScope(ITypedDecl decl) => GetDeclarationsInScope(_scopes.First(), decl.Name).Any(i => i.Equals(decl));
		public bool IsAlreadyDeclaredInCurrentScope(string name) => GetDeclarationsInScope(_scopes.First(), name).Any();

		public SymbolTable() {
			// Global scope.
			EnterScope();
		}

		/// <summary>
		/// Method to enter a scope
		/// </summary>
		public void EnterScope(ITypedDecl typedDecl = null) {
			_scopes.Push(new List<ITypedDecl>());
			if (typedDecl is FuncDeclNode function)
				CurrentFunction = function;
		}

		/// <summary>
		/// Method to leave a scope
		/// </summary>
		public void LeaveScope() {
			_scopes.Pop();
			CurrentFunction = null;
		}

		/// <summary>
		/// Method to record a declaration, and add it to the symbol table (If it havent already been declared, if so throw an error)
		/// </summary>
		/// <param name="decl"></param>
		public void Record(ITypedDecl decl) {
			if (!IsAlreadyDeclaredInCurrentScope(decl)) {
				_currentScope.Add(decl);
			}
		}

		/// <summary>
		/// Method to get a declaration node by name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ITypedDecl GetDeclaration(string name) {
			return GetDeclarations(name).FirstOrDefault();
		}

		/// <summary>
		/// Method to get a declaration node by name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<ITypedDecl> GetDeclarations(string name) {
			foreach (List<ITypedDecl> scope in _scopes) {
				foreach (var decl in GetDeclarationsInScope(scope, name))
					yield return decl;
			}
		}

		/// <summary>
		/// Method to get a declaration node by name
		/// </summary>
		/// <param name="scope"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<ITypedDecl> GetDeclarationsInScope(List<ITypedDecl> scope, string name) {
			foreach (var decl in scope.Where(decl => decl.Name == name))
				yield return decl;
		}
	}
}
