using Exceptions.Exceptions;
using Nodes;
using System;
using System.Text;

namespace Exceptions.Syntax.SemanticErrors {
	internal class CannotAssignToConst : SemanticException, ISemanticError {
		public AssignStmtNode AssignStmt { get; set; }

		public CannotAssignToConst(AssignStmtNode assignStmt) {
			AssignStmt = assignStmt;
		}

		public override string GetDescription() {
			if (AssignStmt.Context == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(AssignStmt.Context)}: error: cannot assign a new value to a constant")
				.AppendLine(GetLineWithPointer(AssignStmt.Context))
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}

