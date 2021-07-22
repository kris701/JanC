using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ArgumentCountMismatch : SemanticException, ISemanticError {
		public JanCParser.CallExprContext Context { get; set; }
		public JanCParser.FuncDeclContext FuncDeclContext { get; set; }
		public string FunctionName { get; set; }
		public int ExpectedArgs { get; set; }
		public int ActualArgs { get; set; }

		public ArgumentCountMismatch(string functionName, int actualArgs, int expectedArgs, JanCParser.CallExprContext context, JanCParser.FuncDeclContext funcDeclContext) {
			FunctionName = functionName;
			ActualArgs = actualArgs;
			ExpectedArgs = expectedArgs;
			Context = context;
			FuncDeclContext = funcDeclContext;
		}

		public override string GetDescription() {
			if (Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Context)} is null");
			if (FuncDeclContext == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(FuncDeclContext)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(Context)}: error: '{FunctionName}()' expects {ExpectedArgs} arguments but {ActualArgs} were given")
				.AppendLine(GetLineWithPointer(Context))
				.AppendLine($"{Location(FuncDeclContext)}: note: declaration of '{FunctionName}()'")
				.Append(GetLineWithPointer(FuncDeclContext.parameters()))
				.ToString();
		}

		public override string ToString() {
			return $"error: '{FunctionName}()' expects {ExpectedArgs} arguments but {ActualArgs} were given";
		}
	}
}
