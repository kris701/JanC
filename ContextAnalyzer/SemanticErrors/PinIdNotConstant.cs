using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class PinIdNotConstant : SemanticException, ISemanticError {
		public JanCParser.ArgumentContext Context { get; set; }

		public PinIdNotConstant(JanCParser.ArgumentContext context) {
			Context = context;
		}

		public override string GetDescription() {
			if (Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Context)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(Context)}: error: expected pin id to be a compile-time constant")
				.Append(GetLineWithPointer(Context))
				.ToString();
		}

		public override string ToString() {
			return "error: expected pin id to be a compile-time constant";
		}
	}
}
