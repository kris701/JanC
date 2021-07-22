
namespace Nodes {
	public class EveryTaskNode : BaseTaskNode {
		public EveryTaskNode(JanCParser.EveryTaskNodeContext context, string delay, IImpr body) : base(context, "EveryTask", body) {
			Context = context;
			Delay = delay;
			Body = body;
		}
		public string Delay { get; set; }
	}
}
