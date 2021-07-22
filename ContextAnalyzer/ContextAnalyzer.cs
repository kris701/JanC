using Exceptions.Syntax;
using Nodes;
using System;
using System.Collections.Generic;
using Tools;

namespace ContextAnalyzer {
	public class ContextAnalyzer : IContextAnalyzer {
		public IASTNode Source { get; internal set; }
		public SemanticErrorListener ErrorListener { get; internal set; }
		public List<Phase> Phases { get; internal set; }
		public IASTNode DecoratedAST { get; internal set; }

		public ContextAnalyzer() { }

		public IContextAnalyzer ReadSource(IASTNode source) {
			if (source == null)
				throw new ArgumentNullException("Source AST node cannot be null!");

			Source = source;

			return this;
		}

		public IContextAnalyzer SetPhases(List<Phase> phases = null) {
			if (phases is null)
				Phases = DefaultPhases.DefaultPhasesList;
			else
				Phases = phases;
			return this;
		}

		public IContextAnalyzer SetErrorListener(SemanticErrorListener errorListener = null) {
			if (errorListener == null)
				ErrorListener = new SemanticErrorListener();
			else
				ErrorListener = errorListener;

			return this;
		}

		public IContextAnalyzer DecorateAST() {
			if (Phases == null)
				throw new ArgumentNullException("Phases cannot be null!");
			if (Source == null)
				throw new ArgumentNullException("Source AST node cannot be null!");
			if (ErrorListener == null)
				throw new ArgumentNullException("Error listener cannot be null!");

			DecoratedAST = Source;

			foreach (Phase phase in Phases)
				phase.Execute(DecoratedAST, ErrorListener);

			if (ErrorListener.HadErrors || ErrorListener.HadWarnings)
				DecoratedAST = null;

			return this;
		}
	}
}
