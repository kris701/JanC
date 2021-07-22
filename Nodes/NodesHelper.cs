using System.Collections.Generic;

namespace Nodes {
	// Helps build the Children method for IASTNode.
	public static class NodesHelper {
		public static List<IASTNode> With<T>(this List<IASTNode> list, List<T> other) where T : class {
			foreach (IASTNode item in other) {
				list.Add(item);
			}
			return list;
		}

		public static List<IASTNode> With(this List<IASTNode> list, IASTNode item) {
			list.Add(item);
			return list;
		}

		public static List<IASTNode> With<T>(this List<IASTNode> list, T item) where T : class {
			if (item is not null) {
				list.Add((IASTNode)item);
			}
			return list;
		}

		public static List<IASTNode> NewNodes {
			get {
				var list = new List<IASTNode>();
				return list;
			}
		}
	}
}
