using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JanCCLI.CLIParsing {
	internal class NamedCommandCluster {
		public string Name { get; set; }
		public IEnumerable<BaseCommand> Commands { get; set; }

		public NamedCommandCluster(string name, IEnumerable<BaseCommand> commands) {
			Name = name;
			Commands = commands;
		}

		public string GetDescription() {

			var helpString = new StringBuilder();

			helpString.AppendLine(Name);
			foreach (var cmd in Commands) {
				foreach (var token in cmd.Tokens) {
					var parameterString = cmd
						.GetParameterInfo()
						.Select(i => $"<{i.Name}>")
						.Join(' ');

					helpString.AppendLine($"\t{token} {parameterString}");
				}
			}

			return helpString.ToString();
		}

	}

	internal static class StringHelper {
		public static string Join(this IEnumerable<string> strings, char seperator) {
			return string.Join(seperator, strings);
		}
	}

}
