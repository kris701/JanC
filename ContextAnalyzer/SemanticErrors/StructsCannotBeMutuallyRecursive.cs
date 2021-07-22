using Exceptions.Exceptions;
using Nodes;
using System;
using System.Text;

namespace Exceptions.Syntax.SemanticErrors {
	internal class StructsCannotBeMutuallyRecursive : SemanticException, ISemanticError {
		public StructDeclNode Struct { get; set; }
		public VarDeclNode Member { get; set; }

		public StructsCannotBeMutuallyRecursive(StructDeclNode @struct, VarDeclNode member) {
			Struct = @struct;
			Member = member;
		}

		public override string GetDescription() {
			if (Struct.Context == null)
				throw new InvalidOperationException();
			if (Member.Context == null)
				throw new InvalidOperationException();

			return new StringBuilder()
				.AppendLine($"{Location(Member.Context)}: error: the struct '{Struct.Name}' is mutually recursive. It contains another struct which in turn contains the original struct which in turn contains the other struct again, repeating forever")
				.AppendLine(GetLineWithPointer(Member.Context))
				.AppendLine($"note: structs cannot mutually contain each other, but they can reference each other: ref<{Struct.Name}> {Member.Name}")
				.ToString();
		}

		public override string ToString() {
			return "Struct cannot be recursive";
		}
	}
}

