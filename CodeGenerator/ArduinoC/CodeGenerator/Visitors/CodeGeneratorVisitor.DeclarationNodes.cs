using CodeGenerator.ArduinoC.Nodes;
using Nodes;
using Exceptions.Exceptions.Base;
using System.Linq;
using System.Text;
using static CodeGenerator.ArduinoC.CodeGenerator.CodeGeneratorVisitorHelpers;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(VarDeclNode node) {
			var sb = new StringBuilder();
			if (node.IsConst.Value) {
				sb.Append("const ");
			}
			sb.Append(Visit(node.Type))
			.Append(' ')
			.Append(node.Name);
			if (node.Value is not null) {
				sb.Append('=');

				int refDepthDelta = 0;
				if (node.Type is RefTypeNode nodeRef)
					refDepthDelta += nodeRef.RefDepth;
				if (node.Value.Type is RefTypeNode valueRef)
					refDepthDelta -= valueRef.RefDepth;

				if (refDepthDelta > 0) {
					sb.Append(Ref(node.Value, refDepthDelta));
				}
				else {
					sb.Append(Visit(node.Value));
				}
			}
			return sb.ToString();
		}

		public string Visit(FuncDeclNode node) {
			var sb = new StringBuilder();
			sb.Append(Visit(node.ReturnType));
			sb.Append(' ');
			sb.Append(node.Name);
			sb.Append('(');
			foreach (VarDeclNode param in node.Parameters.SkipLast(1)) {
				sb.Append(Visit(param).TrimEnd(';'));
				sb.Append(", ");
			}
			if (node.Parameters.Count > 0) {
				sb.Append(Visit(node.Parameters.Last()));
			}
			sb.Append(") ");
			sb.Append(Visit(node.Body));
			return sb.ToString();
		}

		public string Visit(StructDeclNode node) {
			var sb = new StringBuilder()
			.Append("struct ")
			.Append(node.Name)
			.AppendLine(" {");
			Indent();
			foreach (VarDeclNode member in node.Members) {
				sb.Append(Tab(tabs))
				.Append(Visit(member))
				.AppendLine(";");
			}
			Dedent();
			sb.Append(Tab(tabs))
			.Append("}");
			return sb.ToString();
		}

		public string Visit(FunctionForwardDeclaration node) {
			var sb = new StringBuilder()
				.Append(Visit(node.Function.ReturnType))
				.Append(' ')
				.Append(node.Function.Name)
				.Append('(');
			var firstParam = node.Function.Parameters.FirstOrDefault();
			if (firstParam is not null)
				sb.Append(Visit(firstParam));
			foreach (var param in node.Function.Parameters.Skip(1)) {
				sb.Append(", ");
				sb.Append(Visit(param));
			}
			return sb
				.Append(")")
				.ToString();
		}

		public string Visit(StructForwardDeclaration node) {
			return new StringBuilder()
				.Append("struct ")
				.Append(node.Struct.Name)
				.ToString();
		}

		public string Visit(ModuleDeclNode node) {
			// GlobalCodeListener puts module code directly into global scope.
			throw new UnreachableException();
		}
	}
}
