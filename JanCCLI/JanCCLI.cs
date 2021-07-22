using Exceptions.Exceptions;
using JanCCLI.CLIParsing;
using JanCCLI.Commands;
using Compiler;
using System;
using System.IO;

namespace JanCCLI {
	internal class JanCCLI : IJanCCLI {
		public bool PrintToConsole { get; internal set; } = false;
		public FileInfo FileToWriteTo { get; internal set; } = null;
		internal ArgHandler _argHandler;
		internal IJanCCompiler _janCCompiler;

		public JanCCLI() {
			_janCCompiler = new JanCCompiler();
			_argHandler = GetRegisteredCommands();
		}

		public void ParseCommand(string[] args) {
			if (args.Length > 0)
				foreach (string output in _argHandler.ParseArgs(args)) {
					Console.WriteLine(output);
					_janCCompiler = new JanCCompiler();
				}
			else {
				Console.WriteLine("No arguments given. Available arguments:");
				Console.WriteLine(_argHandler.PrintHelp());
			}
		}

		private ArgHandler GetRegisteredCommands() {
			var argHandler = new ArgHandler();

			argHandler.RegisterCommand(
				"Compile",
				new Command<string>(
					(string path) => CompileCommand.PrintCompile(path, _janCCompiler, PrintToConsole, FileToWriteTo),
					"-c", "--compile"
				)
			);

			argHandler.RegisterCommand(
				"Concrete Syntax Tree",
				new Command<string>(
					(string path) => CSTCommand.PrintCST(path, _janCCompiler, PrintToConsole, FileToWriteTo),
					"-cst"
				)
			);

			argHandler.RegisterCommand(
				"Abstract Syntax Tree",
				new BaseCommand[] {
					new Command<string>(
						(string path) => ASTCommand.PrintAST(path, _janCCompiler, PrintToConsole, FileToWriteTo),
						"-ast"
					),
					new Command<string>(
						(string path) => ASTCommand.PrintAST(path, _janCCompiler, PrintToConsole, FileToWriteTo, true),
						"-ast-full"
					)
				}
			);

			argHandler.RegisterCommand(
				"Print output to console",
				new Flag(
					() => { PrintToConsole = true; },
				"-p", "-print")
			);

			argHandler.RegisterCommand(
				"Write output to file",
				new Flag<string>(
					(string filePath) => {
						FileToWriteTo = new FileInfo(filePath);
					},
				"-o", "-outfile")
			);

			return argHandler;
		}
	}
}
