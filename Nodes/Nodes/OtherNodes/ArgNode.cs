using System.Collections.Generic;

namespace Nodes {
	public class ArgNode : BaseASTNode {
		// Possibly named arguments in the future.
		//string name;
		public ArgNode(JanCParser.ArgumentContext context, IExpr value) : base(context) {
			Context = context;
			Value = value;
		}
		public JanCParser.ArgumentContext Context { get; }
		public virtual ITypeLiteral Type => Value.Type;
		public IExpr Value { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Value); }
	}
}
