using Antlr4.Runtime;
using ASTGenerator;
using CodeGenerator;
using CodeGenerator.ArduinoC;
using ContextAnalyzer;
using CSTGenerator;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using Compiler.Exceptions;
using Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tools;

namespace Compiler {
	public class JanCCompiler : IJanCCompiler {
		public IJanCCompiler.States State { get; internal set; } = IJanCCompiler.States.None;
		public FileInfo SourceFile { get; internal set; }
		public string SourceString { get; internal set; }
		public string CompiledCCode { get; internal set; }
		public List<LanguageException> Errors { get; } = new List<LanguageException>();
		public bool HadErrors => Errors.Count > 0;
		public List<LanguageException> Warnings { get; } = new List<LanguageException>();
		public bool HadWarnings => Warnings.Count > 0;
		public bool AllowWarnings { get; set; } = true;
		private List<string> _stackTrace = new List<string>();

		public ICSTBuilder CSTGenerator { get; internal set; }
		public IASTBuilder ASTGenerator { get; internal set; }
		public IContextAnalyzer ContextAnalyzer { get; internal set; }
		public ICodeGenerator CodeGenerator { get; internal set; }

		public ParserRuleContext CSTTree { get; internal set; }
		public IASTNode ASTTree { get; internal set; }

		public JanCCompiler(
			ICSTBuilder CSTGenerator = null, 
			IASTBuilder ASTGenerator = null, 
			ICodeGenerator codeGenerator = null, 
			IContextAnalyzer contextAnalyzer = null
			) {
			this.CSTGenerator = CSTGenerator ?? new CSTBuilder();
			this.ASTGenerator = ASTGenerator ?? new ASTBuilder();
			CodeGenerator = codeGenerator ?? new ArduinoCGenerator();
			ContextAnalyzer = contextAnalyzer ?? new ContextAnalyzer.ContextAnalyzer();
		}

		public IJanCCompiler ReadSource(FileInfo file) {
			_stackTrace.Add("ReadSource( Fileinfo )");

			State = IJanCCompiler.States.ReadingFile;

			SourceFile = file;
			SourceString = File.ReadAllText(file.FullName);

			return this;
		}
		public IJanCCompiler ReadSource(string source) {
			_stackTrace.Add("ReadSource( String )");

			SourceString = source;

			return this;
		}

		public IJanCCompiler GenerateCST() {
			_stackTrace.Add("GenerateCST()");

			State = IJanCCompiler.States.Compiling;
			if (SourceString == null)
				throw new ArgumentNullException("SourceString was null!");
			if (CSTGenerator == null)
				throw new ArgumentNullException("´CSTGenerator´ was null!");

			if (SourceFile != null)
				CSTGenerator = CSTGenerator.ReadSource(SourceFile);
			else
				CSTGenerator = CSTGenerator.ReadSource(SourceString);

			CSTTree = CSTGenerator.BuildCST().CSTTree;
			Errors.AddRange(CSTGenerator.LexerErrorListener.Errors);
			Errors.AddRange(CSTGenerator.ParserErrorListener.Errors);

			ThrowIfState();

			return this;
		}

		public IJanCCompiler GenerateAST() {
			_stackTrace.Add("GenerateAST()");

			State = IJanCCompiler.States.Compiling;

			if (CSTTree == null)
				throw new ArgumentNullException("CSTTree was null!");
			if (ASTGenerator == null)
				throw new ArgumentNullException("´ASTGenerator´ was null!");

			ASTTree = ASTGenerator.ReadSource(CSTTree).BuildAST().ASTTree;

			ThrowIfState();

			return this;
		}

		public IJanCCompiler DecorateAST() {
			_stackTrace.Add("DecorateAST()");

			State = IJanCCompiler.States.Compiling;

			if (ASTTree == null)
				throw new ArgumentNullException("ASTTree was null!");
			if (ContextAnalyzer == null)
				throw new ArgumentNullException("´ContextAnalyzer´ was null!");

			var semanticErrorListener = new SemanticErrorListener();
			ASTTree = ContextAnalyzer.ReadSource(ASTTree).SetErrorListener(semanticErrorListener).SetPhases().DecorateAST().DecoratedAST;

			Errors.AddRange(semanticErrorListener.Errors);
			Warnings.AddRange(semanticErrorListener.Warnings);

			ThrowIfState();

			return this;
		}

		public IJanCCompiler GenerateCode() {
			_stackTrace.Add("GenerateCode()");

			if (ASTTree == null)
				throw new ArgumentNullException("ASTTree was null!");
			if (CodeGenerator == null)
				throw new ArgumentNullException("´CodeGenerator´ was null!");

			CompiledCCode = CodeGenerator.ReadSource(ASTTree).SetPhases().GenerateDomain().GenerateCode().GeneratedCode;

			ThrowIfState();

			return this;
		}

		internal void ThrowIfState() {
			if (HadErrors)
				State = IJanCCompiler.States.HadErrors;
			else if (HadWarnings)
				State = IJanCCompiler.States.FinishedWithWarnings;
			else
				State = IJanCCompiler.States.Finished;

			switch (State) {
				case IJanCCompiler.States.HadErrors:
					Errors.Add(new StackTrace(_stackTrace));
					throw new LanguageException(GetErrorAndWarningSummary());
				case IJanCCompiler.States.FinishedWithWarnings:
					if (!AllowWarnings) {
						Errors.Add(new StackTrace(_stackTrace));
						throw new LanguageException(GetErrorAndWarningSummary());
					}
					break;
			}
		}

		public string GetErrorAndWarningSummary() {
			return $"{GetErrorSummary()}{Environment.NewLine}{GetWarningsSummary()}";
		}

		public string GetErrorSummary() {
			if (!HadErrors) {
				return "No errors";
			}
			else {
				var errorMessage = new StringBuilder($"--- Errors ---{Environment.NewLine}");

				foreach (var error in Errors) {
					errorMessage.AppendLine(error.GetDescription());
				}

				return errorMessage.ToString();
			}
		}

		public string GetWarningsSummary() {
			if (!HadWarnings) {
				return "No warnings";
			}
			else {
				var warningMessage = new StringBuilder($"--- Warnings ---{Environment.NewLine}");

				foreach (var warning in Warnings) {
					warningMessage.AppendLine(warning.GetDescription());
				}

				return warningMessage.ToString();
			}
		}
	}
}
