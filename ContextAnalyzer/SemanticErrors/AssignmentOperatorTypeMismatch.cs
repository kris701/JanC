using Nodes;
using Exceptions.Exceptions;
using System;
using System.Text;

namespace Exceptions.Syntax.SemanticErrors {
	internal class AssignmentOperatorTypeMismatch : SemanticException, ISemanticError {
		public AssignStmtNode AssignStmt { get; set; }

		public AssignmentOperatorTypeMismatch(AssignStmtNode assignStmt) {
			AssignStmt = assignStmt;
		}

		public override string GetDescription() {
			if (AssignStmt.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(AssignStmt.Context)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(AssignStmt.Context)}: error: expected numeric left-side value but '{AssignStmt.Location.Type.Name}' was found")
				.AppendLine(GetLineWithPointer(AssignStmt.Context))
				.AppendLine("note: the assignment operators '+=', '-=', '*=', and '/=' can only be used with numeric types")
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}

