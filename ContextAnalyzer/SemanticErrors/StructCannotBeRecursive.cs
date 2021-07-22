using Exceptions.Exceptions;
using Nodes;
using System;
using System.Text;

namespace Exceptions.Syntax.SemanticErrors {
	internal class StructCannotBeRecursive : SemanticException, ISemanticError {
		public StructDeclNode Struct { get; set; }
		public VarDeclNode Member { get; set; }

		public StructCannotBeRecursive(StructDeclNode @struct, VarDeclNode member) {
			Struct = @struct;
			Member = member;
		}

		public override string GetDescription() {
			if (Struct.Context == null)
				throw new InvalidOperationException();
			if (Member.Context == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Member.Context)}: error: the struct '{Struct.Name}' contains itself")
				.AppendLine(GetLineWithPointer(Member.Context))
				.AppendLine($"note: a struct can contain a reference to itself: ref<{Struct.Name}> {Member.Name}")
				.ToString();
		}

		public override string ToString() {
			return "Struct cannot be recursive";
		}
	}
}

