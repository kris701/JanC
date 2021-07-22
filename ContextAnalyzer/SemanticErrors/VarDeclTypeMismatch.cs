using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class VarDeclTypeMismatch : SemanticException, ISemanticError {
		public VarDeclNode VarDecl { get; set; }

		public VarDeclTypeMismatch(VarDeclNode varDecl) {
			VarDecl = varDecl;
		}

		public override string GetDescription() {
			if (VarDecl.Context == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(VarDecl.Context)}: error: the type of the left side '{VarDecl.Type.Name}' does not match the type of the right side '{VarDecl.Value.Type.Name}'")
				.AppendLine(GetLineWithPointer(VarDecl.Context))
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}
