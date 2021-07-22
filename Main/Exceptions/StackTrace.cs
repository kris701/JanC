using Exceptions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions {
	public class StackTrace : LanguageException {
		public List<string> Stack { get; }
		public StackTrace(List<string> stack) : base("") {
			Stack = stack;
		}

		public override string GetDescription() {
			string outStr = $"Stack Trace:{Environment.NewLine}";
			int count = 0;
			foreach (string s in Stack)
				outStr += $"{count++}: {s}{Environment.NewLine}";
			return outStr;
		}
	}
}
