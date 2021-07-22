using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class UndeclaredType : SemanticException, ISemanticError {
		public ITypeLiteral Type { get; set; }

		public UndeclaredType(ITypeLiteral type) {
			Type = type;
		}
		public override string GetDescription() {
			if (Type.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but context is null");

			return new StringBuilder()
				.AppendLine($"{Location(Type.Context!)}: error: use of type '{Type.Name}' before declaration")
				.Append(GetLineWithPointer(Type.Context))
				.ToString();
		}

		public override string ToString() {
			return $"error: use of type '{Type.Name}' before declaration";
		}
	}
}
