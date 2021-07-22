using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class CanOnlyCallFunction : SemanticException, ISemanticError {
		public CallExprNode Call { get; set; }

		public CanOnlyCallFunction(CallExprNode call) {
			Call = call;
		}

		public override string GetDescription() {
			if (Call.Context == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Call.Context)}: error: expected a function, not '{Call.Item.Type}' value")
				.AppendLine(GetLineWithPointer(Call.Context))
				.ToString();
		}

		public override string ToString() {
			return "expected a function";
		}
	}
}
