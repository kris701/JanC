using Antlr4.Runtime;
using Nodes;
using System.Collections.Generic;

namespace CodeGenerator.ArduinoC.Nodes {
	public class TaskSetupNode : BaseExprNode, IExpr {
		public TaskSetupNode(string name) : base(null) {
			Name = name;
			Type = TypeNode.Void;
		}
		public ParserRuleContext Context { get; } = null;
		public string Name { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Type); }
	}
}
