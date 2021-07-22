using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class CannotHaveMoreLiteralMembersThanStructMembers : SemanticException, ISemanticError {
		public StructLiteralNode Literal { get; set; }
		public StructDeclNode Decl { get; set; }
		public CannotHaveMoreLiteralMembersThanStructMembers(StructLiteralNode literal, StructDeclNode decl) {
			Literal = literal;
			Decl = decl;
		}

		public override string GetDescription() {
			return new StringBuilder()
					.AppendLine($"{Location(Literal.Context)}: error: '{Literal.Name}' literal has '{Literal.Members.Count}' members but its declaration only has '{Decl.Members.Count}' members")
					.AppendLine(GetLineWithPointer(Literal.Context))
					.AppendLine($"{Location(Decl.Context)}: note: declaration of struct '{Literal.Name}'")
					.AppendLine(GetLineWithPointer(Decl.Context))
					.ToString();
		}

		public override string ToString() => GetDescription();
	}
}
