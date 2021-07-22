using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class StructLiteralMemberNotFound : SemanticException, ISemanticError {
		public StructLiteralMemberNode Member { get; set; }
		public StructDeclNode Struct { get; set; }
		public StructLiteralMemberNotFound(StructLiteralMemberNode member, StructDeclNode @struct) {
			Member = member;
			Struct = @struct;
		}

		public override string GetDescription() {
			if (Member.Context is null)
				throw new InvalidOperationException();
			if (Struct.Context is null)
				throw new InvalidOperationException();

			return new StringBuilder()
					.AppendLine($"{Location(Member.Context)}: error: member '{Member.Name}' does not exist in the struct '{Struct.Name}'")
					.AppendLine(GetLineWithPointer(Member.Context))
					.ToString();
		}

		public override string ToString() => GetDescription();
	}
}
