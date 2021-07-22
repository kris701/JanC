using Exceptions.Syntax;
using Nodes;
using System;
using System.Collections.Generic;

namespace Tools {
	/// <summary>
	/// A class used to describe a phase of the decorator
	/// </summary>
	public class Phase {
		public Action<IASTNode, List<ASTListener>, SemanticErrorListener> Strategy { get; set; }
		public List<ASTListener> Listeners { get; set; }

		public Phase(Action<IASTNode, List<ASTListener>, SemanticErrorListener> strategy, List<ASTListener> listeners) {
			Strategy = strategy;
			Listeners = listeners;
		}

		public void Execute(IASTNode node, SemanticErrorListener errorListener) {
			Strategy(node, Listeners, errorListener);
		}
	}
}
