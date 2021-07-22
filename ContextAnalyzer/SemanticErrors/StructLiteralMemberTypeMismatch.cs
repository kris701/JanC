using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class StructLiteralMemberTypeMismatch : SemanticException, ISemanticError {
		public StructLiteralMemberNode LiteralMember { get; set; }
		public VarDeclNode DeclMember { get; set; }

		public StructLiteralMemberTypeMismatch(StructLiteralMemberNode literalMember, VarDeclNode declMember) {
			LiteralMember = literalMember;
			DeclMember = declMember;
		}
		public override string GetDescription() {
			if (LiteralMember.Context is null)
				throw new InvalidOperationException();
			if (DeclMember.Context is null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(LiteralMember.Context)}: error: actual '{LiteralMember.Value.Type.Name}' struct member did not match formal '{DeclMember.Type.Name}' struct member '{DeclMember.Name}'")
				.AppendLine(GetLineWithPointer(LiteralMember.Context))
				.AppendLine($"{Location(DeclMember.Context)}: note: declaration of formal member '{DeclMember.Name}'")
				.Append(GetLineWithPointer(DeclMember.Context))
				.ToString();
		}

		public override string ToString() {
			return GetDescription();
		}
	}
}
