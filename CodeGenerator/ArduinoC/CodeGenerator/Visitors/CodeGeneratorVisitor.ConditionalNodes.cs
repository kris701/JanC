using Nodes;
using System.Text;
using static CodeGenerator.ArduinoC.CodeGenerator.CodeGeneratorVisitorHelpers;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(IfStmtNode node) {
			var sb = new StringBuilder();
			sb.Append($"if ({Visit(node.Condition)}) {Visit(node.Body)}");
			if (node.ElseBody != null) {
				if (node.Body is not BlockNode)
					sb.Append(';');
				sb.Append($" else {Visit(node.ElseBody)}");
			}
			return sb.ToString();
		}
		public string Visit(WhileStmtNode node) {
			var sb = new StringBuilder();
			sb.Append($"while ({Visit(node.Condition)}) {Visit(node.Body)}");
			return sb.ToString();
		}

		public string Visit(ForStmtNode node) {
			var sb = new StringBuilder();
			sb.Append($"for ({Visit(node.Init)}; {Visit(node.Condition)}; {Visit(node.Update)}) {Visit(node.Body)}");
			return sb.ToString();
		}

		public string Visit(SwitchStmtNode node) {
			var sb = new StringBuilder();
			Indent();
			sb.AppendLine($"switch({Visit(node.Value)}) {{");
			foreach (var @case in node.Cases)
				sb.AppendLine($"{Visit(@case)}; break;");

			if (node.DefaultCase != null)
				sb.AppendLine($"{Tab(tabs)}default: {Visit(node.DefaultCase)}");

			Dedent();
			sb.Append($"{Tab(tabs)}}}");

			return sb.ToString();
		}
		public string Visit(SwitchCaseStmtNode node) {
			string casePart = $"{Tab(tabs)}case ";
			Indent();
			string valuePart = $"{Visit(node.Value)}: ";
			Dedent();
			string blockPart = $"{Visit(node.Body)}";

			return casePart + valuePart + blockPart;
		}
	}
}
