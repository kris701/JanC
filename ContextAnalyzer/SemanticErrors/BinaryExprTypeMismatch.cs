using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class BinaryExprTypeMismatch : SemanticException, ISemanticError {
		public string Operator { get; set; }
		public JanCParser.BinaryExprContext Context { get; set; }
		public ITypeLiteral LeftType { get; set; }
		public ITypeLiteral RightType { get; set; }

		public BinaryExprTypeMismatch(ITypeLiteral leftType, string @operator, ITypeLiteral rightType, JanCParser.BinaryExprContext context) {
			LeftType = leftType;
			Operator = @operator;
			RightType = rightType;
			Context = context;
		}

		public override string GetDescription() {
			if (Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Context)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(Context)}: error: left-side type of '{Operator}' expression is '{LeftType.Name}' but right-side type is '{RightType.Name}'")
				.Append(GetLineWithPointer(Context))
				.ToString();
		}

		public override string ToString() {
			return $"error: left-side type of '{Operator}' expression is '{LeftType.Name}' but right-side type is '{RightType.Name}'";
		}
	}
}
