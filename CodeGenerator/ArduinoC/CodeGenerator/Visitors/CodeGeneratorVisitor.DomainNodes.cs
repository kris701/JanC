using CodeGenerator.ArduinoC.Nodes;
using Exceptions.Exceptions.Base;
using Tables.Pins;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string Visit(ConcurrentSleepNode node) {
			return $"vTaskDelay({node.Delay}/portTICK_PERIOD_MS)";
		}

		public string Visit(TaskSetupNode node) {
			return $"xTaskCreate({node.Name}, \"Task\", 100, NULL, 0, NULL)";
		}

		public string Visit(PinModeNode node) {
			string state = node.State switch {
				PinState.OUTPUT => "OUTPUT",
				PinState.INPUT => "INPUT",
				_ => throw new UnreachableException(),
			};
			return $"pinMode({node.PinId},{state})";
		}
	}
}
