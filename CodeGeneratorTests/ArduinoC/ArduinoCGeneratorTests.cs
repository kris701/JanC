using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeGenerator.ArduinoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodes;
using Tools;
using CodeGenerator.ArduinoC.DomainGenerator;
using Nodes.ASTHelpers;
using static Nodes.ASTHelpers.CommonAST;

namespace CodeGenerator.ArduinoC.Tests {
	[TestClass()]
	public class ArduinoCGeneratorTests {
		#region Test Setup

		private ICodeGenerator codeGenerator;

		[TestInitialize()]
		public void Setup() {
			codeGenerator = new ArduinoCGenerator();
		}

		#endregion

		#region ReadSource Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadSource_ThrowsIfSourceNull() {
			codeGenerator.ReadSource(null);
		}

		[TestMethod()]
		public void ReadSource_SetsSource() {
			IASTNode source = new GlobalScopeNode(null, new List<IUnit>());

			codeGenerator = codeGenerator.ReadSource(source);

			Assert.AreEqual(source, codeGenerator.Source);
		}
		#endregion

		#region SetPhases Tests
		[TestMethod()]
		public void SetPhases_SetsPhasesIfGiven() {
			List<Phase> phases = new List<Phase>();

			codeGenerator = codeGenerator.SetPhases(phases);

			Assert.AreEqual(phases, codeGenerator.Phases);
		}

		[TestMethod()]
		public void SetPhases_SetsDefaultPhasesIfNoneGiven() {
			codeGenerator = codeGenerator.SetPhases(null);

			Assert.IsTrue(codeGenerator.Phases.Count == DefaultPhases.DefaultPhasesList.Count);
		}
		#endregion

		#region GenerateDomain Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateDomain_ThrowsIfPhasesNull() {
			codeGenerator.GenerateDomain();
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateDomain_ThrowsIfSourceNull() {
			codeGenerator = codeGenerator.SetPhases();

			codeGenerator.GenerateDomain();
		}

		[TestMethod()]
		public void GenerateDomain_GeneratesDomain() {
			codeGenerator = codeGenerator.ReadSource(Root()).SetPhases();

			codeGenerator = codeGenerator.GenerateDomain();

			Assert.IsTrue(codeGenerator.GeneratedDomain.Children.Count > 0);
		}
		#endregion

		#region GenerateCode Tests
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GenerateCode_ThrowsIfGeneratedDomainNull() {
			codeGenerator.GenerateCode();
		}

		[TestMethod()]
		[TestCategory("SlowTest")]
		public void GenerateDomain_GeneratesCode() {
			codeGenerator = codeGenerator.ReadSource(Root()).SetPhases().GenerateDomain();

			codeGenerator = codeGenerator.GenerateCode();

			Assert.AreNotEqual("", codeGenerator.GeneratedCode);
		}
		#endregion
	}
}
