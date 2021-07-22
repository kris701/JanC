using Nodes;
using CodeGenerator.ArduinoC.Nodes;
using Exceptions.Exceptions.Base;
using System.Globalization;
using System.Linq;
using System.Text;
using System;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(PrimitiveTypeNode node) {
			return node.Type switch {
				PrimitiveTypeNode.Types.Bool => "bool",
				PrimitiveTypeNode.Types.Float => "float",
				PrimitiveTypeNode.Types.Integer => "int",
				PrimitiveTypeNode.Types.Invalid => throw new InvalidOperationException(),
				PrimitiveTypeNode.Types.String => "String",
				PrimitiveTypeNode.Types.Void => "void",

				_ => throw new NotImplementedException(),
			};
		}
		public string Visit(RefTypeNode node) {
			return $"{Visit(node.SubType)}*";
		}

		public string Visit(UserDefinedTypeNode node) {
			return node.Type switch {
				UserDefinedTypeNode.Types.Enum => $"enum {node.Name}",
				UserDefinedTypeNode.Types.Struct => $"struct {node.Name}",
				_ => throw new NotImplementedException(),
			};
		}

		public string Visit(IdentifierExpr node) {
			return node.Name;
		}
		public string Visit(GroupingExprNode node) {
			return $"({Visit(node.Value)})";
		}

		public string Visit(BoolLiteralNode node) {
			return node.Value ? "true" : "false";
		}

		public string Visit(IntLiteralNode node) {
			return node.Value.ToString();
		}
		public string Visit(FloatLiteralNode node) {
			return node.Value.ToString(CultureInfo.GetCultureInfo("en-US"));
		}

		public string Visit(StringLiteralNode node) {
			return $"\"{node.Value}\"";
		}

		public string Visit(StructLiteralVarNode node) {
			return new StringBuilder()
				.Append("struct ")
				.Append(node.StructLiteral.Name)
				.Append(" ")
				.Append(node.Name)
				.Append(" = {")
				.Append(string.Join(',', node.StructLiteral.Members.Select(member => Visit(member))))
				.Append("}")
				.ToString();
		}

		public string Visit(StructLiteralNode node) {
			return node.VariableName;
		}

		public string Visit(StructLiteralMemberNode node) {
			if (node.Name is null)
				return Visit(node.Value);
			else
				return $".{node.Name}={Visit(node.Value)}";
		}

		public string Visit(AssignStmtNode node) {
			string @operator = node.Operator switch {
				AssignOperator.Assign => "=",
				AssignOperator.PlusAssign => "+=",
				AssignOperator.MinusAssign => "-=",
				AssignOperator.MultiplyAssign => "*=",
				AssignOperator.DivideAssign => "/="
			};

			if (node.Location.Type is RefTypeNode || node.Value.Type is RefTypeNode) {
				int deltaDepth = 0;
				if (node.Location.Type is RefTypeNode leftRef) deltaDepth += leftRef.RefDepth;
				if (node.Value.Type is RefTypeNode rightRef) deltaDepth -= rightRef.RefDepth;

				return $"{DeRef(node.Location, Math.Max(0, deltaDepth))} {@operator} {DeRef(node.Value, Math.Max(0, -deltaDepth))}";
			}
			else {
				return $"{Visit(node.Location)} {@operator} {Visit(node.Value)}";
			}
		}
		public string Visit(LibraryNode node) {
			return $"#include <{node.Name}>";
		}
		public string Visit(ReturnStmtNode node) {
			var sb = new StringBuilder();
			sb.Append("return");
			if (node.Value is not null)
				sb.Append($" {Visit(node.Value)}");
			return sb.ToString();
		}
		public string Visit(CallExprNode node) {
			var sb = new StringBuilder();
			sb.Append(Visit(node.Item));
			sb.Append('(');
			foreach (dynamic arg in node.Arguments.SkipLast(1)) {
				sb.Append(Visit(arg));
				sb.Append(", ");
			}
			if (node.Arguments.Count > 0) {
				sb.Append(Visit(node.Arguments.Last()));
			}
			sb.Append(')');
			return sb.ToString();
		}
		public string Visit(RefCallExprNode node) {
			return $"(&({Visit(node.Argument)}))";
		}
		public string Visit(ArgNode node) {
			return Visit(node.Value);
		}
	}
}
