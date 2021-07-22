using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using Nodes;
using System;
using System.Collections.Generic;
using Tables;
using Tools;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	/// <summary>
	/// Listener to transparently rewrite names
	/// </summary>
	class PrefixNamesListener : ASTListener {
		private readonly string uservarPrefix = "JanC_";

		public PrefixNamesListener() {	}

		public void Enter(ITypedDecl decl) {
			RewriteName(decl);
		}

		private void RewriteName(ITypedDecl node) {
			string newName = uservarPrefix + node.Name;
			node.Name = newName;
		}
	}
}
