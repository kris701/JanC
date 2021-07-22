using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ExpectedLValue : SemanticException, ISemanticError {
		public IExpr Value { get; set; }

		public ExpectedLValue(IExpr value) {
			Value = value;
		}

		public override string GetDescription() {
			if (Value.GenericContext == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Value.GenericContext)}: error: expected l-value")
				.AppendLine(GetLineWithPointer(Value.GenericContext))
				.AppendLine("note: l-values are values with a persistent location, such as variables or struct members")
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}
