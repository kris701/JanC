using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class InvalidUsageOfStaticDeclaration : SemanticException, ISemanticError {
		public UserDefinedTypeNode Type { get; set; }

		public InvalidUsageOfStaticDeclaration(UserDefinedTypeNode type) {
			Type = type;
		}

		public override string GetDescription() {
			if (Type.Context == null)
				throw new InvalidOperationException();

			var sb = new StringBuilder()
				.AppendLine($"{Location(Type.Context)}: error: {Type.Name} is a static type and cannot be instantiated as a variable, function parameter or function return type")
				.AppendLine(GetLineWithPointer(Type.Context));
				if (Type.Decl is not null & Type.Decl.GenericContext is not null) {
					sb.AppendLine($"{Location(Type.Decl.GenericContext)}: note: declaration of {Type.Name}")
					.AppendLine(GetLineWithPointer(Type.Decl.GenericContext));
			}
				return sb.ToString();
		}

		public override string ToString() {
			return "cannot instantiate static declaration";
		}
	}
}
