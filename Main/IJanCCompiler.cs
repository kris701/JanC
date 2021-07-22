using Antlr4.Runtime;
using ASTGenerator;
using CodeGenerator;
using ContextAnalyzer;
using CSTGenerator;
using Exceptions.Exceptions;
using Nodes;
using System.Collections.Generic;
using System.IO;
using Tools;

namespace Compiler {
	public interface IJanCCompiler {
		public States State { get; }
		public FileInfo SourceFile { get; }
		public string SourceString { get; }
		public string CompiledCCode { get; }
		public List<LanguageException> Errors { get; }
		public bool HadErrors { get; }
		public List<LanguageException> Warnings { get; }
		public bool HadWarnings { get; }
		public ParserRuleContext CSTTree { get; }
		public IASTNode ASTTree { get; }

		public IJanCCompiler ReadSource(FileInfo file);
		public IJanCCompiler ReadSource(string source);
		public IJanCCompiler GenerateCST();
		public IJanCCompiler GenerateAST();
		public IJanCCompiler DecorateAST();
		public IJanCCompiler GenerateCode();

		public ICSTBuilder CSTGenerator { get; }
		public IASTBuilder ASTGenerator { get; }
		public IContextAnalyzer ContextAnalyzer { get; }
		public ICodeGenerator CodeGenerator { get; }


		public string GetErrorAndWarningSummary();
		public string GetErrorSummary();
		public string GetWarningsSummary();


		public enum States {
			None,
			ReadingFile,
			Compiling,
			HadErrors,
			FinishedWithWarnings,
			Finished
		}
	}
}
