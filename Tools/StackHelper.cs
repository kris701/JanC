using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools {
	public static class StackHelper {
		public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items) {
			foreach (var item in items)
				stack.Push(item);
		}
	}
}
