
namespace Nodes {
	public class OnceTaskNode : BaseTaskNode {
		public OnceTaskNode(JanCParser.OnceTaskNodeContext context, IImpr body) : base(context, "OnceTask", body) {
			Context = context;
			Body = body;
		}
	}
}
