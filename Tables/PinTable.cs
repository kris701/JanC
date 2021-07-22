using System.Collections.Generic;
using System.Linq;
using Tables.Pins;

namespace Tables {
	/// <summary>
	/// Lookup tabel for pins
	/// </summary>
	public class PinTable {
		protected Dictionary<int, PinInfo> _pins;
		public List<Pin> Pins {
			get => _pins.Keys.Select(pinId => new Pin { Id = pinId, State = _pins[pinId].State }).ToList();
		}

		public PinTable() {
			_pins = new Dictionary<int, PinInfo>();
		}

		/// <summary>
		/// Method to record that a pin is being used
		/// </summary>
		/// <param name="pinId"></param>
		/// <param name="usage"></param>
		/// <param name="context"></param>
		public void RecordPinUsage(int pinId, PinState usage, JanCParser.CallExprContext context) {
			if (!_pins.ContainsKey(pinId)) {
				_pins.Add(pinId, new PinInfo { State = usage, Context = context });
			}
		}
	}
}
