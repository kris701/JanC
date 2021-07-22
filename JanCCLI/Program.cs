using System;
using System.IO;

namespace JanCCLI {
	internal class Program {
		internal static void Main(string[] args) {
			JanCCLI janCCLI = new JanCCLI();
			janCCLI.ParseCommand(args);
		}
	}
}
