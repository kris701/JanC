using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ExpectedLValueOnLeftSideOfAssignment : SemanticException, ISemanticError {
		public AssignStmtNode Assignment { get; set; }

		public ExpectedLValueOnLeftSideOfAssignment(AssignStmtNode assignment) {
			Assignment = assignment;
		}

		public override string GetDescription() {
			if (Assignment.Context == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Assignment.Context)}: error: l-value required as left side of assignment")
				.AppendLine(GetLineWithPointer(Assignment.Location.GenericContext))
				.AppendLine("note: l-values are values with a persistent location, such as variables or struct members")
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}
