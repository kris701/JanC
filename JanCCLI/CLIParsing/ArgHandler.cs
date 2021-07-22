using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JanCCLI.CLIParsing {
	public class ArgHandler {
		internal static string printHelpCommandName = "Print Help";

		public ArgHandler() {
			RegisterCommand(
				printHelpCommandName,
				new Command(PrintHelp, "-h", "-help", "-?")
			);
		}

		// For lookup by token
		internal Dictionary<string, BaseCommand> CommandDictionayByToken { get; set; } = new Dictionary<string, BaseCommand>();

		// For lookup by Help-command
		internal List<NamedCommandCluster> CommandList { get; set; } = new List<NamedCommandCluster>();

		public string PrintHelp() {
			var helpString = new StringBuilder();

			foreach (NamedCommandCluster cluster in CommandList)
				helpString.AppendLine(cluster.GetDescription());

			return helpString.ToString();
		}

		public void RegisterCommand(string argName, IEnumerable<BaseCommand> commands) {
			CommandList.Add(new NamedCommandCluster(argName, commands));

			// For each command, add every token to the dictionary
			foreach (BaseCommand cmd in commands)
				foreach (string token in cmd.Tokens)
					CommandDictionayByToken.Add(token, cmd);
		}

		public void RegisterCommand(string argName, BaseCommand command) {
			RegisterCommand(argName, new BaseCommand[] { command });
		}

		public IEnumerable<string> ParseArgs(IEnumerable<string> args) {
			IEnumerator<string> argsEnumerator = args.GetEnumerator();

			while (argsEnumerator.MoveNext()) {
				string commandToken = argsEnumerator.Current;
				var command = CommandDictionayByToken[commandToken];

				object commandResult = InvokeCommandAndConsumeArgs(command, argsEnumerator);

				if (commandResult is not null)
					yield return (string)commandResult;
			}
		}

		/// <summary>
		/// Consumes as many args as there are parameters of command, then invokes command with the values.
		/// </summary>
		/// <param name="command">The BaseCommand to invoke</param>
		/// <param name="args">An enumerator for args, the enumerator will be enumerated over the parameters for command</param>
		/// <returns>The string returned by the command, or null if nothing was returned</returns>
		private static object InvokeCommandAndConsumeArgs(BaseCommand command, IEnumerator<string> args) {
			var parameters = GetParameters(command, args);

			try {
				object methodOutput = command.@Delegate.DynamicInvoke(parameters);
				if (command.Delegate.GetMethodInfo().ReturnType != typeof(void))
					return (string)methodOutput;
				else
					return null;
			}
			catch (TargetInvocationException e) {
				if (e.InnerException != null) throw e.InnerException;
				else throw;
			}

		}

		/// <summary>
		/// Consumes as many args as there are parameters of command, then returns an array of corresponding values
		/// </summary>
		/// <param name="command">The BaseCommand to get parameters for</param>
		/// <param name="args">An enumerator for args, the enumerator will be enumerated over the parameters for command</param>
		/// <returns>An array of objects, which has been converted to the datatypes corresponding to the parameters of command. They just need a cast</returns>
		private static object[] GetParameters(BaseCommand command, IEnumerator<string> args) {
			var parameterInfo = command.GetParameterInfo();

			var output = new object[parameterInfo.Length];
			for (var i = 0; i < parameterInfo.Length; i++) {
				args.MoveNext();
				output[i] = Convert.ChangeType(args.Current, parameterInfo[i].ParameterType);
			}
			return output;
		}

	}
}
