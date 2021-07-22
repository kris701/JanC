using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class InvalidVoidUsage : SemanticException, ISemanticError {
		public ITypeLiteral Type { get; set; }

		public InvalidVoidUsage(ITypeLiteral type) {
			Type = type;
		}
		public override string GetDescription() {
			if (Type.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but context is null");

			return new StringBuilder()
				.AppendLine($"{Location(Type.Context)}: error: only functions can be void")
				.Append(GetLineWithPointer(Type.Context))
				.ToString();
		}
		public override string ToString() {
			return "error: only functions can be void";
		}
	}
}
