using CodeGenerator.ArduinoC.CodeGenerator.Visitors;
using CodeGenerator.ArduinoC.DomainGenerator;
using Nodes;
using System;
using System.Collections.Generic;
using Tools;

namespace CodeGenerator.ArduinoC {
	public class ArduinoCGenerator : ICodeGenerator {
		public IASTNode Source { get; internal set; }
		public List<Phase> Phases { get; internal set; }
		public IASTNode GeneratedDomain { get; internal set; }
		public string GeneratedCode { get; internal set; }

		public ArduinoCGenerator() { }

		public ICodeGenerator ReadSource(IASTNode source) {
			if (source == null)
				throw new ArgumentNullException();

			Source = source;

			return this;
		}

		public ICodeGenerator SetPhases(List<Phase> phases = null) {
			if (phases is null)
				Phases = DefaultPhases.DefaultPhasesList;
			else
				Phases = phases;
			return this;
		}

		public ICodeGenerator GenerateDomain() {
			if (Phases == null)
				throw new ArgumentNullException();
			if (Source == null)
				throw new ArgumentNullException();

			GeneratedDomain = Source;

			foreach (Phase phase in Phases)
				phase.Execute(GeneratedDomain, null);

			return this;
		}

		public ICodeGenerator GenerateCode() {
			if (GeneratedDomain == null)
				throw new ArgumentNullException();

			CodeGeneratorVisitor codeGeneratorVisitor = new CodeGeneratorVisitor();
			GeneratedCode = codeGeneratorVisitor.GenerateCode(GeneratedDomain);

			return this;
		}
	}
}
