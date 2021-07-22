using Antlr4.Runtime;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.ArduinoC.Nodes {
	public class LongVariableDecl : BaseExprNode, IExpr {
		public LongVariableDecl(string name, IExpr value) : base(null) {
			Value = value;
			Name = name;
			Type = TypeNode.Void;
		}
		public ParserRuleContext Context { get; } = null;
		public string Name { get; set; }
		public IExpr Value { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes; }
	}
}
