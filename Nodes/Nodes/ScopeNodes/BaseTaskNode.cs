using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public abstract class BaseTaskNode : BaseDeclNode, IImprContainer {
		public BaseTaskNode(ParserRuleContext context, string name, IImpr impr) : base(context, name) {
			Context = context;
			Body = impr;
		}
		public ParserRuleContext Context { get; internal set; }
		public IImpr Body { get; set; }
		public override IToken NameToken => null;
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Body); }
	}
}
