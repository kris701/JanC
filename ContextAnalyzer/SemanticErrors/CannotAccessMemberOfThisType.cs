using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class CannotAccessMemberOfThisType : SemanticException, ISemanticError {
		public CannotAccessMemberOfThisType(MemberAccessNode memberAccess) {
			MemberAccess = memberAccess;
		}

		public MemberAccessNode MemberAccess { get; set; }

		public override string GetDescription() {
			return new StringBuilder()
					.AppendLine($"{Location(MemberAccess.Context)}: error: tried to access member of '{MemberAccess.Item.Type}' value but can only access members of modules, struct values or enum types")
					.AppendLine(GetLineWithPointer(MemberAccess.Item.GenericContext))
					.ToString();
		}

		public override string ToString() => GetDescription();
	}
}
