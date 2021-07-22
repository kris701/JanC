using Exceptions.Exceptions;
using Exceptions.Exceptions.Base;
using Exceptions.Syntax;
using Nodes;
using System.Collections.Generic;

namespace Tools {
	public static class DefaultPhasesMethods {
		/// <summary>
		/// Strategy for the first and last phase of the decorator
		/// </summary>
		/// <param name="node"></param>
		/// <param name="listeners"></param>
		/// <param name="errorListener"></param>
		public static void VisitGlobals(IASTNode node, List<ASTListener> listeners, SemanticErrorListener errorListener) {
			NotifyEnter(node, listeners, errorListener);
			BaseASTNode globalScope = (BaseASTNode)node;
			foreach (IASTNode unit in globalScope.Children) {
				NotifyEnter(unit, listeners, errorListener);
				NotifyLeave(unit, listeners, errorListener);
			}
			NotifyLeave(node, listeners, errorListener);
		}

		/// <summary>
		/// Strategy for the second phase of the decorator
		/// </summary>
		/// <param name="node"></param>
		/// <param name="listeners"></param>
		/// <param name="errorListener"></param>
		public static void VisitDepthFirst(IASTNode node, List<ASTListener> listeners, SemanticErrorListener errorListener) {
			NotifyEnter(node, listeners, errorListener);
			foreach (IASTNode child in node.Children) {
				if (child is null) { throw new UnreachableException(); }
				VisitDepthFirst(child, listeners, errorListener);
			}
			NotifyLeave(node, listeners, errorListener);
		}

		public static void VisitDepthFirstDeclarations(IASTNode node, List<ASTListener> listeners, SemanticErrorListener errorListener) {
			NotifyEnter(node, listeners, errorListener);
			foreach (IASTNode child in node.Children) {
				if (child is ITypedDecl || child is BaseTaskNode) {
					if (child is null) { throw new UnreachableException(); }
					VisitDepthFirst(child, listeners, errorListener);
				}
			}
			NotifyLeave(node, listeners, errorListener);
		}

		/// <summary>
		/// Method for notifying where the semantic listener should be 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="listeners"></param>
		/// <param name="errorListener"></param>
		private static void NotifyEnter(IASTNode node, List<ASTListener> listeners, SemanticErrorListener errorListener) {
			// The dynamic casts ensures that the node is always handled by the most specific available method.
			foreach (dynamic listener in listeners) {
				try {
					listener.Enter((dynamic)node);
				}
				catch (SemanticException e) {
					errorListener.Update(e);
				}
				catch (AlreadyHandledException) { }
			}
		}

		/// <summary>
		/// Method for notifying where the semantic listener should be
		/// </summary>
		/// <param name="node"></param>
		/// <param name="listeners"></param>
		/// <param name="errorListener"></param>
		private static void NotifyLeave(IASTNode node, List<ASTListener> listeners, SemanticErrorListener errorListener) {
			foreach (dynamic listener in listeners) {
				try {
					listener.Leave((dynamic)node);
				}
				catch (SemanticException e) {
					errorListener.Update(e);
				}
				catch (AlreadyHandledException) { }
			}
		}
	}
}
