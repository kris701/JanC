namespace JanCCLI.CLIParsing {
	public class Flag : BaseCommand {
		public new delegate void CommandDelegate();
		public Flag(CommandDelegate @delegate, params string[] tokens) : base(@delegate, tokens) { }
	}
	public class Flag<T> : BaseCommand {
		public new delegate void CommandDelegate(T param1);
		public Flag(CommandDelegate @delegate, params string[] tokens) : base(@delegate, tokens) { }
	}
	public class Flag<T, T2> : BaseCommand {
		public new delegate void CommandDelegate(T param1, T2 param2);
		public Flag(CommandDelegate @delegate, params string[] tokens) : base(@delegate, tokens) { }
	}
}
