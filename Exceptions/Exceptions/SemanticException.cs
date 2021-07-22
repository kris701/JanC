using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Exceptions.Syntax;
using System.IO;
using System.Linq;
using System.Text;

namespace Exceptions.Exceptions {
	/// <summary>
	/// An abstract class to represent semantic errors and point to the correct position of the error
	/// </summary>
	public class SemanticException : LanguageException, ISemanticIssue {
		public SemanticException() : base() { }
		public SemanticException(string message) : base(message) { }

		/// <summary>
		/// Returns a string like 8:1, for an error starting on line 8.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected static string Location(ParserRuleContext context) {
			return Location(context.Start);
		}

		/// <summary>
		/// Used to point out errors. Produces a string like the following:
		///	 int a = "string"
		///			 ^
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		protected static string Location(IToken token) {
			// Add 1 to column, since it otherwise starts at 0.
			return $"{token.Line}:{token.Column + 1}";
		}

		/// <summary>
		/// A function to return a correct string, pointing to the character where the semantic error is
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected string GetLineWithPointer(ParserRuleContext context) {
			return GetLineWithPointer(context, context.Start);
		}

		/// <summary>
		/// A function to return a correct string, pointing to the character where the semantic error is
		/// </summary>
		/// <param name="context"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		protected string GetLineWithPointer(ParserRuleContext context, IToken token) {
			string line = GetContextLine(context);
			int indentation = CountIndentation(line);
			string correctedLine = new string(line.Skip(indentation).ToArray());
			int correctedColumn;
			if (token != null)
				correctedColumn = token.Column - indentation;
			else
				correctedColumn = 0;
			return GetLineWithPointer(correctedLine, correctedColumn);
		}

		/// <summary>
		/// A function to return a correct string, pointing to the character where the semantic error is
		/// </summary>
		/// <param name="line"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		protected static string GetLineWithPointer(string line, int column) {
			var sb = new StringBuilder();
			sb.AppendLine(line);
			sb.Append(Space(column));
			sb.Append('^');
			return sb.ToString();
		}

		/// <summary>
		/// Get the full line of a rule context
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private string GetContextLine(ParserRuleContext context) {
			int line = context.Start.Line;
			var unitContext = GetEntireUnitContext(context);
			return GetContextLine(unitContext, line);
		}

		/// <summary>
		/// Get the full line of a context rule, with a line offset
		/// </summary>
		/// <param name="context"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		private string GetContextLine(ParserRuleContext context, int line) {
			var text = GetFullText(context);
			int lineOffset = line - context.Start.Line;
			return GetLine(text, lineOffset);
		}

		/// <summary>
		/// Read nth line of a string
		/// </summary>
		/// <param name="text"></param>
		/// <param name="lineOffset">Line to read, 0-indexed</param>
		/// <returns></returns>
		private string GetLine(string text, int lineOffset) {
			using StringReader reader = new StringReader(text);
			for (int i = 0; i < lineOffset; i++)
				reader.ReadLine();
			return reader.ReadLine()
				?? throw new EndOfStreamException($"{nameof(lineOffset)}({lineOffset}) out of bounds in {nameof(GetLine)}");
		}

		/// <summary>
		/// Get the "base" of a rule context
		/// </summary>
		/// <param name="current"></param>
		/// <returns></returns>
		private static ParserRuleContext GetEntireUnitContext(ParserRuleContext current) {
			while (!IsUnitOrAbove(current))
				current = (ParserRuleContext)current.Parent;
			return current;
		}

		/// <summary>
		/// Check if a rule context is a "Unit Context" meaning its the "base" of a line
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static bool IsUnitOrAbove(ParserRuleContext context) {
			return context is JanCParser.UnitContext || context is JanCParser.CompileUnitContext;
		}

		/// <summary>
		/// Get the indentation of a line
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		private static int CountIndentation(string line) {
			int count = 0;
			foreach (char c in line) {
				if (char.IsWhiteSpace(c))
					count += 1;
				else
					break;
			}
			return count;
		}

		/// <summary>
		/// Get a string of spaces, based on a number
		/// </summary>
		/// <param name="spaces"></param>
		/// <returns></returns>
		private static string Space(int spaces) {
			return new string(' ', spaces);
		}

		private static string GetFullText(ParserRuleContext context) {
			if (context.Start == null || context.Stop == null || context.Start.StartIndex < 0 || context.Stop.StopIndex < 0)
				return context.GetText(); // Fallback

			return context.Start.InputStream.GetText(Interval.Of(context.Start.StartIndex, context.Stop.StopIndex));
		}
	}
}
