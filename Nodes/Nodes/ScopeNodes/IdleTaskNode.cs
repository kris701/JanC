
namespace Nodes {
	public class IdleTaskNode : BaseTaskNode {
		public IdleTaskNode(JanCParser.IdleTaskNodeContext context, IImpr body) : base(context, "IdleTask", body) {
			Context = context;
			Body = body;
		}
	}
}
