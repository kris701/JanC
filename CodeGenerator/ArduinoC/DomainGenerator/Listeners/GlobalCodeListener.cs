using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using CodeGenerator.ArduinoC.Nodes;
using Nodes;
using System.Linq;
using Tools;
using System;
using System.Collections.Generic;
using Nodes.ASTHelpers;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	/// <summary>
	/// Listener to move imperative global code to the setup function and move declarative global code to the top of the global scope.
	/// </summary>
	internal class GlobalCodeListener : ASTListener {
		private int _libIndex = 0;
		private readonly ContextSymbolTable _symbolTable;
		public GlobalCodeListener(ContextSymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		/// <summary>
		/// Method to enter this listener
		/// </summary>
		/// <param name="global"></param>
		public void Enter(GlobalScopeNode global) {
			IUnit lastLibrary = global.Content.LastOrDefault(node => node is LibraryNode);
			if (lastLibrary != null)
				_libIndex = global.Content.LastIndexOf(lastLibrary) + 1;

			var units = GetAllUnits(global.Content);
			CopyImperativesFromGlobalToSetup(units, _symbolTable.SetupNode);
			RemoveGlobalImperatives(units);
			MoveDeclarationsToTopOfCode(units);
			ForwardDeclareDeclarations(units);
			global.Content = units;
		}

		private List<IUnit> GetAllUnits(List<IUnit> globals) {
			var output = new List<IUnit>();
			GetAllUnits(globals, output);
			return output;
		}

		private void GetAllUnits(List<IUnit> globals, List<IUnit> output) {
			foreach (var unit in globals) {
				if (unit is ModuleDeclNode module)
					GetAllUnits(module.Content, output);
				else
					output.Add(unit);
			}
		}



		/// <summary>
		/// Copy all imperative nodes from the global scope, e.g. variable declarations, into the setup scope
		/// Doing this will keep the declaration out in the global scope, but the assignment in the setup scope
		/// </summary>
		/// <param name="global"></param>
		/// <param name="setup"></param>
		private static void CopyImperativesFromGlobalToSetup(List<IUnit> units, FuncDeclNode setup) {
			foreach (var impr in units.OfType<IImpr>()) {
				if (impr is VarDeclNode varDecl) {
					if ((bool)!varDecl.IsConst)
						MoveAssignmentToSetupButKeepDeclarationInGlobal(setup, varDecl);
				} else
					setup.AppendBody(impr);
			}
		}

		/// <summary>
		/// Method to move assignements to setup, but keep declaration in global.
		/// </summary>
		/// <param name="setup"></param>
		/// <param name="decl"></param>
		private static void MoveAssignmentToSetupButKeepDeclarationInGlobal(FuncDeclNode setup, VarDeclNode decl) {
			var assignment = ExtractAssignmentFromDeclarationOrNull(decl);
			if (assignment is not null)
				setup.AppendBody(assignment);
		}

		/// <summary>
		/// Method to move declarations in global scope to the top of the document.
		/// </summary>
		/// <param name="global"></param>
		private void MoveDeclarationsToTopOfCode(List<IUnit> units) {
			// This order ensures that type declarations come before variable declarations.
			MoveToTop<VarDeclNode>(units);
			MoveToTop<StructDeclNode>(units);
		}

		private void MoveToTop<T>(List<IUnit> units) where T : IUnit {
			foreach (var unit in units.OfType<T>().Reverse().ToList()) {
				units.Remove(unit);
				units.Insert(_libIndex, unit);
			}
		}

		/// <summary>
		/// Method to create forward declarations for functions and structs
		/// </summary>
		/// <param name="global"></param>
		private void ForwardDeclareDeclarations(List<IUnit> units) {
			foreach (var funcDecl in units.OfType<FuncDeclNode>().ToList()) {
				units.Insert(_libIndex, new FunctionForwardDeclaration(funcDecl));
			}
			foreach (var structDecl in units.OfType<StructDeclNode>().ToList()) {
					units.Insert(_libIndex, new StructForwardDeclaration(structDecl));
			}
		}

		/// <summary>
		/// Checks if there is an assignment to a declaration
		/// </summary>
		/// <param name="decl"></param>
		/// <returns></returns>
		private static AssignStmtNode ExtractAssignmentFromDeclarationOrNull(VarDeclNode decl) {
			var assignment = GetAssignmentFromDeclarationOrNull(decl);
			decl.Value = null;
			return assignment;
		}

		/// <summary>
		/// Method to get all assignments from a declaration
		/// </summary>
		/// <param name="decl"></param>
		/// <returns></returns>
		private static AssignStmtNode GetAssignmentFromDeclaration(VarDeclNode decl) {
			return new AssignStmtNode(
				location: new IdentifierExpr(
					name: decl.Name,
					context: null
				),
				@operator: AssignOperator.Assign,
				value: decl.Value,
				context: null
			);
		}

		/// <summary>
		/// A try version of the <seealso cref="GetAssignmentFromDeclaration"/> function 
		/// </summary>
		/// <param name="decl"></param>
		/// <returns></returns>
		private static AssignStmtNode GetAssignmentFromDeclarationOrNull(VarDeclNode decl) {
			if (decl.Value == null)
				return null;
			return GetAssignmentFromDeclaration(decl);
		}

		/// <summary>
		/// Method to remove all imperative statements from global scope
		/// </summary>
		/// <param name="global"></param>
		private static void RemoveGlobalImperatives(List<IUnit> units) {
			units.RemoveAll(node => node is IImpr && !(node is VarDeclNode));
		}
	}
}
