using System.IO;

namespace JanCCLI {
	public interface IJanCCLI {
		public bool PrintToConsole { get; }
		public FileInfo FileToWriteTo { get; }

		public void ParseCommand(string[] args);
	}
}
