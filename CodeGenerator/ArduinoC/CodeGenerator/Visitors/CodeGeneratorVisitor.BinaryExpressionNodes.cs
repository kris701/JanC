using Nodes;
using System;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(BinaryExprNode node) {
			return $"{Value(node.Left)} {node.Operator} {Value(node.Right)}";
		}
		public string Visit(MemberAccessNode node) {
			var type = (UserDefinedTypeNode)node.Item.Type;
			if (type.Type.Equals(UserDefinedTypeNode.Types.Module))
				return node.MemberName;
			else
				return $"{Visit(node.Item)}.{node.MemberName}";
		}
	}
}
