using Exceptions.Exceptions;
using Compiler;
using System;
using System.Diagnostics;
using System.IO;

namespace JanCCLI.Commands {
	internal static class CompileCommand {
		internal static string PrintCompile(string path, IJanCCompiler janCCompiler, bool printToConsole, FileInfo fileToWriteTo) {
			Stopwatch stopWatch = new Stopwatch();

			Console.WriteLine("======== Compile Started ========");

			try {
				stopWatch.Start();
				janCCompiler = janCCompiler.ReadSource(new FileInfo(path)).GenerateCST().GenerateAST().DecorateAST().GenerateCode();
				stopWatch.Stop();

				if (printToConsole) {
					Console.WriteLine("------- Output Code -------");
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(janCCompiler.CompiledCCode);
					Console.ResetColor();
					Console.WriteLine("---------------------------");
				}

				if (fileToWriteTo != null) {
					using var writer = fileToWriteTo.AppendText();
					writer.Write(janCCompiler.CompiledCCode);
					Console.WriteLine($"Output code written to file: {fileToWriteTo.Name}");
				}
			}
			catch (LanguageException) { stopWatch.Stop(); }

			PrintSummary.Summary(stopWatch, janCCompiler);
			return "=================================";
		}
	}
}
