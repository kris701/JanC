using CodeGenerator.ArduinoC.Nodes;
using Nodes;
using System.Text;
using static CodeGenerator.ArduinoC.CodeGenerator.CodeGeneratorVisitorHelpers;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(BaseTaskNode node) {
			return $"void {node.Name}(void *pvParameters) {Visit(node.Body)}";
		}
		public string Visit(GlobalScopeNode node) {
			var sb = new StringBuilder();
			foreach (IUnit unit in node.Content) {
				sb.Append(Visit((dynamic)unit));

				if (unit is ICPreCompilerItem)
					sb.AppendLine();
				else 
					sb.AppendLine(";");
			}
			return sb.ToString();
		}

		public string Visit(BlockNode node) {
			var sb = new StringBuilder();
			sb.AppendLine("{");
			Indent();
			foreach (IUnit subNode in node.Content) {
				sb.AppendLine($"{Tab(tabs)}{Visit((dynamic)subNode)};");
			}
			Dedent();
			sb.Append($"{Tab(tabs)}}}");
			return sb.ToString();
		}

		public string Visit(CriticalNode node) {
			var sb = new StringBuilder();
			sb.AppendLine($"taskENTER_CRITICAL();");
			sb.AppendLine($"{Tab(tabs)}{Visit((dynamic)node.Body)}");
			sb.Append($"{Tab(tabs)}taskEXIT_CRITICAL()");
			return sb.ToString();
		}
	}
}
