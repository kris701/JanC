using System.Collections.Generic;

namespace Nodes {
	public class GlobalScopeNode : BaseASTNode {
		public GlobalScopeNode(JanCParser.CompileUnitContext context, List<IUnit> content) : base(context) {
			Content = content;
		}
		public JanCParser.CompileUnitContext Context { get; }
		public List<IUnit> Content = new List<IUnit>();
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Content); }
	}
}
