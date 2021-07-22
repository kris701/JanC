using Exceptions.Syntax;
using Nodes;
using System.Collections.Generic;
using Tools;

namespace ContextAnalyzer {
	public interface IContextAnalyzer {
		public IASTNode Source { get; }
		public SemanticErrorListener ErrorListener { get; }
		public List<Phase> Phases { get; }
		public IASTNode DecoratedAST { get; }
		public IContextAnalyzer ReadSource(IASTNode source);
		public IContextAnalyzer SetPhases(List<Phase> phases = null);
		public IContextAnalyzer SetErrorListener(SemanticErrorListener errorListener = null);
		public IContextAnalyzer DecorateAST();
	}
}
