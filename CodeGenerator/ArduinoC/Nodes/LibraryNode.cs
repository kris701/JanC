using Nodes;
using System.Collections.Generic;

namespace CodeGenerator.ArduinoC.Nodes {
	public class LibraryNode : BaseASTNode, IUnit, ICPreCompilerItem {
		public LibraryNode(string name) : base(null) {
			Name = name;
		}
		public string Name { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes; }
	}
}
