using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class AssignmentTypeMismatch : SemanticException, ISemanticError {
		public AssignStmtNode AssignStmt { get; set; }

		public AssignmentTypeMismatch(AssignStmtNode assignment) {
			AssignStmt = assignment;
		}

		public override string GetDescription() {
			if (AssignStmt.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(AssignStmt.GenericContext)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(AssignStmt.Context)}: error: the type of the left side '{AssignStmt.Location.Type.Name}' does not match the type of the right side '{AssignStmt.Value.Type.Name}'")
				.AppendLine(GetLineWithPointer(AssignStmt.Context))
				.ToString();
		}
	}
}
