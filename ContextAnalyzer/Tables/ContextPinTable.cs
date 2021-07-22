using ContextAnalyzer.SemanticErrors;
using System.Collections.Generic;
using Tables;
using Tables.Pins;

namespace ContextAnalyzer.Tables {
	/// <summary>
	/// Lookup tabel for pins
	/// </summary>
	public class ContextPinTable : PinTable {
		public ContextPinTable() {
			_pins = new Dictionary<int, PinInfo>();
		}

		/// <summary>
		/// Method to record that a pin is being used
		/// </summary>
		/// <param name="pinId"></param>
		/// <param name="usage"></param>
		/// <param name="context"></param>
		public new void RecordPinUsage(int pinId, PinState usage, JanCParser.CallExprContext context) {
			if (_pins.ContainsKey(pinId)) {
				PinState currentUsage = _pins[pinId].State;
				if (!currentUsage.Equals(usage)) {
					throw new InconsistentPinUsage(
						pinId: pinId,
						prevUsageContext: _pins[pinId].Context,
						context: context,
						prevState: _pins[pinId].State,
						newState: usage
					);
				}
			}
			else {
				_pins.Add(pinId, new PinInfo { State = usage, Context = context });
			}
		}
	}
}
