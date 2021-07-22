using System;
using System.Reflection;

namespace JanCCLI.CLIParsing {
	public abstract class BaseCommand {
		public delegate string CommandDelegate(params object[] arguements);

		public Delegate @Delegate { get; set; }
		public string[] Tokens { get; set; }

		public BaseCommand(Delegate @delegate, string[] tokens) {
			@Delegate = @delegate;
			Tokens = tokens;
		}

		public ParameterInfo[] GetParameterInfo() => @Delegate.GetMethodInfo().GetParameters();
	}
}
