using Nodes;
using System.Collections.Generic;
using Tools;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	internal class ReservedFunctionsConverterListener : ASTListener {
		private static readonly Dictionary<string, string> map = new Dictionary<string, string>() {
			{ "digitalRead", "digitalRead" },
			{ "digitalWrite", "digitalWrite" },
			{ "analogRead", "analogRead" },
			{ "analogWrite", "analogWrite" },
			{ "sleep", "vTaskDelay" },
			{ "ref", "&" }
		};
		public ReservedFunctionsConverterListener() { }

		public void Enter(CallExprNode node) {
			if (node.Item is IdentifierExpr callIdentifier) {
				if (map.ContainsKey(callIdentifier.Name))
					callIdentifier.LocalRename(map[callIdentifier.Name]);
			}
		}
	}
}
