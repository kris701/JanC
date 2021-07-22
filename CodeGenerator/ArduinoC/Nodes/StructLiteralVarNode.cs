using Nodes;
using System.Collections.Generic;

namespace CodeGenerator.ArduinoC.Nodes {
	public class StructLiteralVarNode : BaseASTNode, IStmt {
		public StructLiteralVarNode(string name, StructLiteralNode structLiteral) : base(null) {
			Name = name;
			StructLiteral = structLiteral;
		}
		public string Name { get; set; }
		public StructLiteralNode StructLiteral { get; set; }
		public ITypeLiteral Type { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes; } // Intentionally omitted.
	}
}
