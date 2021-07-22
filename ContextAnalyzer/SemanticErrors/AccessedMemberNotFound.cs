using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class AccessedMemberNotFound : SemanticException, ISemanticError {
		public MemberAccessNode MemberAccess { get; set; }
		public AccessedMemberNotFound(MemberAccessNode memberAccess) {
			MemberAccess = memberAccess;
		}

		public override string GetDescription() {
			return new StringBuilder()
					.AppendLine($"{Location(MemberAccess.Context)}: error: member '{MemberAccess.MemberName}' does not exist in '{MemberAccess.Item.Type.Name}'")
					.AppendLine(GetLineWithPointer(MemberAccess.Context))
					.ToString();
		}

		public override string ToString() => GetDescription();
	}
}
