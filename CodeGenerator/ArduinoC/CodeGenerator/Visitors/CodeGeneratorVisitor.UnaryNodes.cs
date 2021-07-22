using CodeGenerator.ArduinoC.Nodes;
using Nodes;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(NegateNode node) {
			return $"-{Visit(node.Value)}";
		}

		public string Visit(NotNode node) {
			return $"!{Visit(node.Value)}";
		}

		public string Visit(StaticVariableDecl node) {
			return $"static {Visit(node.Value)}";
		}

		public string Visit(LongVariableDecl node) {
			return $"unsigned long {node.Name} = {Visit(node.Value)}";
		}
	}
}
