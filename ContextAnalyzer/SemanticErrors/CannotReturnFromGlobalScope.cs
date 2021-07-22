using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class CannotReturnFromGlobalScope : SemanticException, ISemanticError {
		public ReturnStmtNode ReturnStmt { get; set; }
		public CannotReturnFromGlobalScope(ReturnStmtNode returnStmt) {
			ReturnStmt = returnStmt;
		}

		public override string GetDescription() {
			if (ReturnStmt.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(ReturnStmt.Context)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(ReturnStmt.Context)}: error: cannot return from global scope")
				.AppendLine(GetLineWithPointer(ReturnStmt.Context))
				.AppendLine($"note: if you want to stop the remaining code from running, try wrapping it in an if-statement.")
				.ToString();
		}

		public override string ToString() {
			return "error: cannot return from global scope";
		}
	}
}
