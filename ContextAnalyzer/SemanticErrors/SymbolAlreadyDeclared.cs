using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;
using Nodes;

namespace ContextAnalyzer.SemanticErrors {
	internal class SymbolAlreadyDeclared : SemanticException, ISemanticError {
		public ITypedDecl Current { get; set; }
		public ITypedDecl Prev { get; set; }

		public SymbolAlreadyDeclared(ITypedDecl current, ITypedDecl prev) {
			Current = current;
			Prev = prev;
		}

		public override string GetDescription() {
			if (Current.GenericContext == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Current.GenericContext)} is null");
			if (Prev.GenericContext == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Prev.GenericContext)} is null");
			if (Current.NameToken == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Current.NameToken)} is null");
			if (Prev.NameToken == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Prev.NameToken)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(Current.GenericContext)}: error: redeclaration of '{Current.Name}'")
				.AppendLine(GetLineWithPointer(Current.GenericContext, Current.NameToken))
				.AppendLine($"{Location(Prev.GenericContext)}: note: previous declaration of '{Current.Name}'")
				.Append(GetLineWithPointer(Prev.GenericContext, Prev.NameToken!))
				.ToString();
		}
		public override string ToString() {
			return $"error: redeclaration of '{Current.Name}'";
		}
	}
}
