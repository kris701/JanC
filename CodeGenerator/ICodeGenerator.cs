using Nodes;
using System.Collections.Generic;
using Tools;

namespace CodeGenerator {
	public interface ICodeGenerator {
		public IASTNode Source { get; }
		public List<Phase> Phases { get; }
		public IASTNode GeneratedDomain { get; }
		public string GeneratedCode { get; }

		public ICodeGenerator ReadSource(IASTNode source);
		public ICodeGenerator SetPhases(List<Phase> phases = null);
		public ICodeGenerator GenerateDomain();
		public ICodeGenerator GenerateCode();
	}
}
