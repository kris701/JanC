using System.Collections.Generic;

namespace Nodes {
	public class BlockNode : BaseASTNode, IStmt {
		public BlockNode(JanCParser.BlockStmtContext context, List<IUnit> content) : base(context) {
			Context = context;
			Content = content;
		}
		public JanCParser.BlockStmtContext Context { get; }
		public List<IUnit> Content { get; }
		public override List<IASTNode> Children { get => NodesHelper.NewNodes.With(Content); }
		public static BlockNode EmptyBlock => new BlockNode(context: null, content: new List<IUnit>());
	}
}
