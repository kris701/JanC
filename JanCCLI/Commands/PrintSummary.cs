using Compiler;
using System;
using System.Diagnostics;

namespace JanCCLI.Commands {
	internal static class PrintSummary {
		internal static void Summary(Stopwatch stopWatch, IJanCCompiler janCCompiler) {
			Console.WriteLine("Summary:");
			Console.WriteLine($"Took: {stopWatch.ElapsedMilliseconds}ms to process");

			if (janCCompiler.HadErrors)
				Console.ForegroundColor = ConsoleColor.Red;
			else
				Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(janCCompiler.GetErrorSummary());

			if (janCCompiler.HadWarnings)
				Console.ForegroundColor = ConsoleColor.Yellow;
			else
				Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(janCCompiler.GetWarningsSummary());
			Console.ResetColor();
		}
	}
}
