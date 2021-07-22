namespace JanCCLI.CLIParsing {
	public class Command : BaseCommand {
		public new delegate string CommandDelegate();
		public Command(CommandDelegate @delegate, params string[] tokens) : base(@delegate, tokens) { }
	}
	public class Command<T> : BaseCommand {
		public new delegate string CommandDelegate(T param);
		public Command(CommandDelegate @delegate, params string[] tokens) : base(@delegate, tokens) { }
	}
	public class Command<T, T2> : BaseCommand {
		public new delegate string CommandDelegate(T param, T2 param2);
		public Command(CommandDelegate @delegate, params string[] tokens) : base(@delegate, tokens) { }
	}
}
