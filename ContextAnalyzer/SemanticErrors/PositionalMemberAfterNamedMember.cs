using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class PositionalMemberAfterNamedMember : SemanticException, ISemanticError {
		public StructLiteralMemberNode Member { get; set; }
		public PositionalMemberAfterNamedMember(StructLiteralMemberNode member) {
			Member = member;
		}

		public override string GetDescription() {
			if (Member.Context is null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Member.Context)}: error: positional members can only be used before named members")
				.AppendLine(GetLineWithPointer(Member.Context))
				.ToString();
		}

		public override string ToString() => GetDescription();
	}
}
