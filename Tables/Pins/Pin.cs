namespace Tables.Pins {
	public enum PinState {
		OUTPUT,
		INPUT
	}

	/// <summary>
	/// Class defining pins
	/// </summary>
	public class Pin {
		public int Id;
		public PinState State;
	}
}
