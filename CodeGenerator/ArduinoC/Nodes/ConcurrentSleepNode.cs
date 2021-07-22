using Antlr4.Runtime;
using Nodes;
using System.Collections.Generic;

namespace CodeGenerator.ArduinoC.Nodes {
	public class ConcurrentSleepNode : BaseExprNode, IExpr {
		public ConcurrentSleepNode(string delay) : base(null) {
			Delay = delay;
			Type = TypeNode.Void;
		}
		public ParserRuleContext Context { get; } = null;
		public string Delay { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes; }
	}
}
