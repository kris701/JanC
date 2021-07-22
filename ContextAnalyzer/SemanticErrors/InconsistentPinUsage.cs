using Exceptions.Exceptions;
using Exceptions.Syntax;
using System;
using System.Text;
using Tables.Pins;

namespace ContextAnalyzer.SemanticErrors {
	internal class InconsistentPinUsage : SemanticException, ISemanticError {
		public int PinId { get; set; }
		public PinState PrevState { get; set; }
		public PinState NewState { get; set; }
		public JanCParser.CallExprContext Context { get; set; }
		public JanCParser.CallExprContext PrevUsageContext { get; set; }

		public InconsistentPinUsage(int pinId, PinState newState, PinState prevState, JanCParser.CallExprContext context, JanCParser.CallExprContext prevUsageContext) {
			PinId = pinId;
			NewState = newState;
			PrevState = prevState;
			Context = context;
			PrevUsageContext = prevUsageContext;
		}

		public override string GetDescription() {
			if (Context == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(Context)} is null");
			if (PrevUsageContext == null)
				throw new InvalidOperationException($"{nameof(GetDescription)} called, but {nameof(PrevUsageContext)} is null");

			return new StringBuilder()
				.AppendLine($"{Location(Context)}: error: pin {PinId} used for {NewState} but it was previously used for {PrevState}")
				.AppendLine(GetLineWithPointer(Context))
				.AppendLine($"{Location(PrevUsageContext)}: note: previous usage of pin {PinId}")
				.Append(GetLineWithPointer(PrevUsageContext))
				.ToString();
		}

		public override string ToString() {
			return $"error: pin {PinId} used for {NewState} but it was previously used for {PrevState}";
		}
	}
}
