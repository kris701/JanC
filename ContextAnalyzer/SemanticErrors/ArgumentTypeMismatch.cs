using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Linq;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ArgumentTypeMismatch : SemanticException, ISemanticError {
		public CallExprNode CallExpr { get; set; }
		public FuncDeclNode FuncDecl { get; set; } // Context is null for built-in functions
		public ArgNode Argument { get; set; }
		public VarDeclNode Parameter { get; set; } // Context is null for built-in functions
		public int ArgumentIndex { get; set; }

		public ArgumentTypeMismatch(CallExprNode call, int argumentIndex, ArgNode argument, VarDeclNode parameter, FuncDeclNode function) {
			CallExpr = call;
			Argument = argument;
			Parameter = parameter;
			ArgumentIndex = argumentIndex;
			FuncDecl = function;
		}
		public override string GetDescription() {
			var sb = new StringBuilder()
				.AppendLine($"{Location(Argument.Context!)}: error: '{Argument.Type.Name}' argument did not match '{Parameter.Type.Name}' parameter '{Parameter.Name}' in call to '{FuncDecl.Name}'")
				.AppendLine(GetLineWithPointer(Argument.Context));
			if (Parameter.Context is not null) {
				sb.AppendLine($"{Location(Parameter.Context)}: note: declaration of '{FuncDecl.Name}'")
				.Append(GetLineWithPointer(Parameter.Context));
			}
			return sb.ToString();
		}

		public override string ToString() {
			return new StringBuilder()
				.AppendLine($"error: '{Argument.Type.Name}' argument did not match '{Parameter.Type.Name}' parameter '{Parameter.Name}' in call to '{FuncDecl.Name}'")
				.Append($"{FuncDecl.Type.Name} {FuncDecl.Name}(")
				.Append(string.Join(", ", FuncDecl.Parameters.Select(param => $"{param.Type.Name} {param.Name}")))
				.AppendLine(")")
				.ToString();
		}
	}
}
