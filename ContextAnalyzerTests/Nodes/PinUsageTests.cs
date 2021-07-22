using ContextAnalyzer.SemanticErrors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.TypeNode;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.UndecoratedAST;
using Nodes.ASTHelpers;

namespace ContextAnalyzer.Nodes {
	[TestClass]
	public class PinUsageTests : BaseRulesTests {
		private IUnit Read(int pinId, bool analog = false) => analog ? BuiltinAST.AnalogRead(pinId) : BuiltinAST.DigitalRead(pinId);
		private IUnit Write(int pinId, int value, bool analog = false) => analog ? BuiltinAST.AnalogWrite(pinId, value) : BuiltinAST.DigitalWrite(pinId, value);

		[TestMethod]
		[DataRow(1, false)]
		[DataRow(2, true)]
		public void CanUseIntLiteralAsPinId(int pinId, bool analog) {
			global
				.With(Read(pinId, analog));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow(3, false)]
		[DataRow(4, true)]
		public void CanUseConstWithIntLiteralAsPinId(int pinId, bool analog) {
			global
				.With(Int.ConstVarDecl("id", Literal(pinId)))
				.With(Read(pinId, analog));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(PinIdNotConstant))]
		[DataRow(5, false)]
		[DataRow(6, true)]
		public void CannotUseIntVariableAsPinId(int pinId, bool analog) {
			CallExprNode call = analog ? Call("analogRead") : Call("digitalRead");
			global.With(Int.VarDecl("id", Literal(pinId)))
				.With(call
					.With(new ArgNode(null, Identifier("id"))));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(PinIdNotConstant))]
		[DataRow(7, false)]
		[DataRow(8, true)]
		public void CannotUseMathExprAsPinId(int pinId, bool analog) {
			CallExprNode call = analog ? Call("analogRead") : Call("digitalRead");
			call.With(new ArgNode(null, new BinaryExprNode(null,
					Literal(pinId), "+", Literal(pinId))));
			global
				.With(call);

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow(9, false)]
		[DataRow(10, true)]
		public void CanReadTwiceFromSamePin(int pinId, bool analog) {
			global
				.With(Read(pinId, analog))
				.With(Read(pinId, analog));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow(11, 0, false)]
		[DataRow(12, 1, true)]
		public void CanWriteTwiceToSamePin(int pinId, int value, bool analog) {
			global
				.With(Write(pinId, value, analog))
				.With(Write(pinId, value, analog));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[DataRow(13, 14, false)]
		[DataRow(14, 15, true)]
		public void CanReadFromDifferentPins(int pinIdA, int pinIdB, bool analog) {
			global
				.With(Read(pinIdA, analog))
				.With(Read(pinIdB, analog));

			ThrowErrorsIfAny();
		}

		[TestMethod]
		[ExpectedException(typeof(InconsistentPinUsage))]
		[DataRow(15, 0, false)]
		[DataRow(16, 1, true)]
		public void CannotReadAndWriteFromSamePin(int pinId, int value, bool analog) {
			global
				.With(Read(pinId, analog))
				.With(Write(pinId, value, analog));

			ThrowErrorsIfAny();
		}
	}
}
