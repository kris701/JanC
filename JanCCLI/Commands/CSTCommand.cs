using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Exceptions.Exceptions;
using Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Tools;

namespace JanCCLI.Commands {
	internal static class CSTCommand {
		internal static string PrintCST(string path, IJanCCompiler janCCompiler, bool printToConsole, FileInfo fileToWriteTo) {
			Stopwatch stopWatch = new Stopwatch();

			Console.WriteLine("======== Generating CST =========");

			try {
				stopWatch.Start();
				janCCompiler = janCCompiler.ReadSource(new FileInfo(path)).GenerateCST();
				stopWatch.Stop();

				if (printToConsole) {
					Console.WriteLine("-------- CST Tree ---------");
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(PrintCST(janCCompiler.CSTTree));
					Console.ResetColor();
					Console.WriteLine("---------------------------");
				}

				if (fileToWriteTo != null) {
					using var writer = fileToWriteTo.AppendText();
					writer.Write(PrintCST(janCCompiler.CSTTree));
					Console.WriteLine($"CST Tree written to file: {fileToWriteTo.Name}");
				}
			}
			catch (LanguageException) { stopWatch.Stop(); }

			PrintSummary.Summary(stopWatch, janCCompiler);
			return "=================================";
		}

		private static string PrintCST(IParseTree tree, int tabs = 0) {
			var sb = new StringBuilder();
			var indent = new String('\t', tabs);
			sb.AppendLine($"{indent}<{tree.GetType()}>   {tree.GetText()}");
			foreach (var child in EnumerateTree(tree)) {
				sb.AppendLine(PrintCST(child, tabs + 1));
			}
			return sb.ToString();

			static IEnumerable<IParseTree> EnumerateTree(ITree tree) {
				for (var i = 0; i < tree.ChildCount; i++) {
					yield return (IParseTree)tree.GetChild(i);
				}
			}
		}
	}
}
