using ContextAnalyzer.Tables;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tables;
using Tools;

namespace ContextAnalyzer.Listener {
	/// <summary>
	/// A listener to enter/exit scopes and record functions
	/// </summary>
	internal class ScopeListener : ASTListener {
		private readonly ContextSymbolTable _symbolTable;

		public ScopeListener(ContextSymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		#region FuncDeclNode
		public void Enter(FuncDeclNode node) {
			_symbolTable.EnterScope(node);
		}
		public void Leave(FuncDeclNode node) {
			_symbolTable.LeaveScope();
		}
		#endregion

		#region ForStmtNode
		public void Enter(ForStmtNode node) {
			_symbolTable.EnterScope();
		}
		public void Leave(ForStmtNode node) {
			_symbolTable.LeaveScope();
		}
		#endregion

		#region BlockNode
		public void Enter(BlockNode node) {
			_symbolTable.EnterScope();
		}
		public void Leave(BlockNode node) {
			_symbolTable.LeaveScope();
		}
		#endregion

		#region StructDeclNode
		public void Enter(StructDeclNode node) {
			_symbolTable.EnterScope();
		}
		public void Leave(StructDeclNode node) {
			_symbolTable.LeaveScope();
		}
		#endregion

		#region ModuleDeclNode
		public void Enter(ModuleDeclNode node) {
			_symbolTable.EnterScope();
			ForwardDeclareModuleDeclarations(node);
		}
		public void Leave(ModuleDeclNode node) {
			_symbolTable.LeaveScope();
		}
		private void ForwardDeclareModuleDeclarations(ModuleDeclNode node) {
			foreach (var decl in node.Content.OfType<ITypeDecl>())
				_symbolTable.Record(decl);
		}
		#endregion

		#region VarDeclNode
		public void Leave(VarDeclNode node) {
			_symbolTable.Record(node);
		}
		#endregion
	}
}
