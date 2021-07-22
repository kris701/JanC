using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ArgumentCountMismatchBuildInFunction : SemanticException, ISemanticError {
		public JanCParser.CallExprContext Context { get; set; }
		public string FunctionName { get; set; }
		public int ExpectedArgs { get; set; }
		public int ActualArgs { get; set; }

		public ArgumentCountMismatchBuildInFunction(string functionName, int actualArgs, int expectedArgs, JanCParser.CallExprContext context) {
			FunctionName = functionName;
			ActualArgs = actualArgs;
			ExpectedArgs = expectedArgs;
			Context = context;
		}

		public override string GetDescription() {
			if (Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Context)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(Context)}: error: '{FunctionName}()' expects {ExpectedArgs} arguments but {ActualArgs} were given")
				.AppendLine(GetLineWithPointer(Context))
				.ToString();
		}

		public override string ToString() {
			return $"error: '{FunctionName}()' expects {ExpectedArgs} arguments but {ActualArgs} were given";
		}
	}
}
