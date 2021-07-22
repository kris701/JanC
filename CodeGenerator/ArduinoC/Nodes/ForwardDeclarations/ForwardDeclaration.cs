using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.ArduinoC.Nodes {
	public class ForwardDeclaration : BaseASTNode, IUnit {
		public ForwardDeclaration() : base(null) { }
		// Forward-declared item is really not a child of this node
		public override List<IASTNode> Children => NodesHelper.NewNodes;
	}
}
