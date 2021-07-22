using Antlr4.Runtime;
using Nodes;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;

namespace ContextAnalyzer.SemanticErrors {
	internal class ReturnTypeMismatch : SemanticException, ISemanticError {
		public ReturnStmtNode ReturnStmt { get; set; }
		public FuncDeclNode FuncDecl { get; set; }

		public ReturnTypeMismatch(ReturnStmtNode returnStmt, FuncDeclNode funcDecl) {
			ReturnStmt = returnStmt;
			FuncDecl = funcDecl;
		}
		public override string GetDescription() {
			var returnContext = GetValueContextElseFallbackToReturnContext();
			var typeContext = GetTypeContext(FuncDecl);

			if (returnContext == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(GetValueContextElseFallbackToReturnContext)} returns null");
			if (typeContext == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(GetTypeContext)} returns null");

			return new StringBuilder()
				.AppendLine($"{Location(returnContext)}: error: expected a return type of '{FuncDecl.ReturnType.Name}' but '{ReturnStmt.Type.Name}' was given")
				.AppendLine(GetLineWithPointer(returnContext))
				.AppendLine($"{Location(FuncDecl.Context!)}: note: declaration of '{FuncDecl.Name}()'")
				.Append(GetLineWithPointer(typeContext))
				.ToString();
		}
		public override string ToString() {
			return $"error: expected a return type of '{FuncDecl.ReturnType.Name}' but '{ReturnStmt.Type.Name}' was given";
		}
		private ParserRuleContext GetValueContextElseFallbackToReturnContext() {
			if (ReturnStmt.Value != null)
				return ReturnStmt.Value.GenericContext!;
			else
				return ReturnStmt.Context!;
		}

		private static JanCParser.TypeLiteralContext GetTypeContext(FuncDeclNode funcDecl) {
			if (funcDecl.Context != null)
				return funcDecl.Context.typeLiteral();
			else
				return null;
		}
	}
}
