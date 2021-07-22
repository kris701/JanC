namespace Tables.Pins {
	/// <summary>
	/// Class containing pin info
	/// </summary>
	public class PinInfo {
		public PinState State { get; set; }
		public JanCParser.CallExprContext Context { get; set; }
	}
}
