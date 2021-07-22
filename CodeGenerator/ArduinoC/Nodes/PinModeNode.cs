using Nodes;
using System.Collections.Generic;
using Tables.Pins;

namespace CodeGenerator.ArduinoC.Nodes {
	public class PinModeNode : BaseExprNode, IExpr {
		public PinModeNode(int pinId, PinState state) : base(null) {
			PinId = pinId;
			State = state;
			Type = TypeNode.Void;
		}
		public int PinId { get; set; }
		public PinState State { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes; }
	}
}
