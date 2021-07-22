using Antlr4.Runtime;

namespace Nodes {
	public abstract class BaseConditionalNode : BaseASTNode, IStmt, IImprContainer {
		public BaseConditionalNode(ParserRuleContext context, IExpr condition, IImpr body) : base(context) {
			Condition = condition;
			Body = body;
		}
		public IExpr Condition { get; }
		public IImpr Body { get; set; }
	}
}
