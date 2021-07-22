using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class StructLiteralTypeMismatch : SemanticException, ISemanticError {
		public StructLiteralNode StructLiteral { get; set; }
		public ITypeDecl Decl { get; set; }

		public StructLiteralTypeMismatch(StructLiteralNode structLiteral, ITypeDecl decl) {
			StructLiteral = structLiteral;
			Decl = decl;
		}

		public override string GetDescription() {
			return new StringBuilder()
				.AppendLine($"{Location(StructLiteral.Type.Context)}: error: the specified type of the struct literal '{StructLiteral.Type.Name}' was not a struct type.")
				.AppendLine(GetLineWithPointer(StructLiteral.Type.Context))
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}
