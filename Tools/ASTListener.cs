using Nodes;

namespace Tools {
	public class ASTListener {
		// Called when entering node
		public void Enter(IASTNode node) { }
		// Called when leaving node (after all its children have been visited)
		public void Leave(IASTNode node) { }
	}
}