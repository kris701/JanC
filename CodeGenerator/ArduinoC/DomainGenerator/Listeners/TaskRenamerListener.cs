using Nodes;
using Tools;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	internal class TaskRenamerListener : ASTListener {
		private int taskCounter = 0;

		public TaskRenamerListener() { }

		/// <summary>
		/// Method to enter this listener
		/// </summary>
		/// <param name="node"></param>
		public void Enter(BaseTaskNode node) {
			node.Name = $"{node.Name}{taskCounter}";
			taskCounter++;
		}
	}
}
