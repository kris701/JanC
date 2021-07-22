using Antlr4.Runtime;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.ArduinoC.Nodes {
	public class StaticVariableDecl : BaseExprNode, IExpr {
		public StaticVariableDecl(IExpr value) : base(null) {
			Value = value;
			Type = TypeNode.Void;
		}
		public ParserRuleContext Context { get; } = null;
		public IExpr Value { get; set; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes; }
	}
}
