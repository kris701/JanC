using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ExpectedLValueOnRightSide : SemanticException, ISemanticError {
		public IAssign Assignment { get; set; }

		public ExpectedLValueOnRightSide(IAssign assignment) {
			Assignment = assignment;
		}

		public override string GetDescription() {
			if (Assignment.GenericContext == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Assignment.GenericContext)}: error: expected l-value on right side, as the left side is a reference")
				.AppendLine(GetLineWithPointer(Assignment.GenericContext))
				.AppendLine("note: l-values are values with a persistent location, such as variables or struct members")
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}
