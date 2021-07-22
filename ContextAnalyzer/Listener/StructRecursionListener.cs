using ContextAnalyzer.Tables;
using Exceptions.Syntax.SemanticErrors;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace ContextAnalyzer.Listener {
	/// <summary>
	/// Ensures that structs cannot contain themselves, resulting in infinite recursion.
	/// </summary>
	internal class StructRecursionListener : ASTListener {
		private readonly ContextSymbolTable _symbolTable;

		public StructRecursionListener(ContextSymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		public void Leave(StructDeclNode @struct) {
			RecursionCheck(@struct, null);
		}

		private void RecursionCheck(StructDeclNode @struct, HashSet<StructDeclNode> visitedStructs) {
			visitedStructs ??= new HashSet<StructDeclNode>();
			visitedStructs.Add(@struct);
			foreach (var member in @struct.Members) {
				if (member.Type.Name.Equals(@struct.Name))
						throw new StructCannotBeRecursive(@struct, member);
				var decl = _symbolTable.GetDeclaration(member.Type.Name);
				if (decl is StructDeclNode structDecl) {
					if (visitedStructs.Contains(structDecl)) {
						throw new StructsCannotBeMutuallyRecursive(@struct, @member);
					} else {
						RecursionCheck(structDecl, visitedStructs);
					}
				}
			}
		}
	}
}
