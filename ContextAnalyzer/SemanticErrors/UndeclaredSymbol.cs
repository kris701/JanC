using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class UndeclaredSymbol : SemanticException, ISemanticError {
		public IdentifierExpr Identifier { get; set; }

		public UndeclaredSymbol(IdentifierExpr identifier) {
			Identifier = identifier;
		}

		public override string GetDescription() {
			if (Identifier.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but context is null");

			return new StringBuilder()
				.AppendLine($"{Location(Identifier.Context)}: error: use of identifier '{Identifier.Name}' before declaration")
				.Append(GetLineWithPointer(Identifier.Context))
				.ToString();
		}

		public override string ToString() {
			return $"error: use of identifier '{Identifier.Name}' before declaration";
		}
	}
}
