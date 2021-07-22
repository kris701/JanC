using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;
using System.Linq;

namespace ContextAnalyzer.SemanticErrors {
	internal class UndeclaredFunctionCalled : SemanticException, ISemanticError {
		public CallExprNode CallExpr { get; set; }

		public UndeclaredFunctionCalled(CallExprNode callExpr) {
			CallExpr = callExpr;
		}

		public override string GetDescription() {
			if (CallExpr.Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but context is null");

			return new StringBuilder()
				.AppendLine($"{Location(CallExpr.Context)}: error: no declaration found for {CallExpr.GetName()}({string.Join(',',CallExpr.Arguments.Select(i => i.ToString()))})")
				.Append(GetLineWithPointer(CallExpr.Context))
				.ToString();
		}

		public override string ToString() {
			return $"{Location(CallExpr.Context)}: error: no declaration found for {CallExpr.GetName()}({string.Join(',', CallExpr.Arguments.Select(i => i.ToString()))})";
		}
	}
}
