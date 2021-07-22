using Exceptions.Exceptions;
using Compiler;
using Nodes;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Tools;

namespace JanCCLI.Commands {
	internal static class ASTCommand {
		internal static string PrintAST(string path, IJanCCompiler janCCompiler, bool printToConsole, FileInfo fileToWriteTo, bool full = false) {
			Stopwatch stopWatch = new Stopwatch();

			if (full)
				Console.WriteLine("===== Generating Full AST =======");
			else
				Console.WriteLine("======== Generating AST =========");

			try {
				stopWatch.Start();
				janCCompiler = janCCompiler.ReadSource(new FileInfo(path)).GenerateCST().GenerateAST();
				stopWatch.Stop();

				if (printToConsole) {
					Console.WriteLine("-------- AST Tree ---------");
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(PrintAST(janCCompiler.ASTTree));
					Console.ResetColor();
					Console.WriteLine("---------------------------");
				}

				if (fileToWriteTo != null) {
					using var writer = fileToWriteTo.AppendText();
					writer.Write(PrintAST(janCCompiler.ASTTree));
					Console.WriteLine($"AST Tree written to file: {fileToWriteTo.Name}");
				}
			}
			catch (LanguageException) { stopWatch.Stop(); }

			PrintSummary.Summary(stopWatch, janCCompiler);
			return "=================================";
		}

		public static string PrintAST(IASTNode node, int tabs = 0, bool displayFullTypeName = false, StringBuilder sb = null) {
			if (sb is null) { sb = new StringBuilder(); }
			var indent = new string('\t', tabs);
			if (displayFullTypeName) {
				sb.AppendLine($"{indent}<{node.GetType()}>");
			}
			else {
				sb.AppendLine($"{indent}<{node.GetType().Name}>");
			}
			foreach (IASTNode child in node.Children) {
				PrintAST(child, tabs + 1, displayFullTypeName, sb);
			}
			return sb.ToString();
		}
	}
}
