using Antlr4.Runtime;
using Exceptions.ErrorListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrammarTests
{
	[TestClass]
	public class GrammarTests {

		#region Type Tests

		[TestMethod]
		// Works cases
		[DataRow("int a = 1")]
		[DataRow("int a")]
		[DataRow("int a = b")]
		[DataRow("int func(){}")]
		// Fails cases
		[DataRow("int a = -", true)]
		public void IntTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("str a = \"1\"")]
		[DataRow("str a")]
		[DataRow("str a = b")]
		[DataRow("str func(){}")]
		// Fails cases
		[DataRow("str a = ", true)]
		[DataRow("str a = '1\"", true)]
		public void StrTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("void func(){}")]
		[DataRow("void func(int a,int b) { a = b }")]
		[DataRow("void func(int a) { a = 5 }")]
		// Fails cases
		[DataRow("void func()", true)]
		// [DataRow("void func {}", true)] // Causes stack overflow
		public void VoidTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("void func(ref<int> a){ a = 5 }")]
		[DataRow("ref<int> a")]
		[DataRow("ref<userdefinedstructnamehere> a")]
		[DataRow("void func(int a, ref<int> b) { b = a }")]
		// Fails cases
		[DataRow("ref int a", true)]
		[DataRow("ref<notype a", true)]
		[DataRow("ref<int>", true)]
		public void RefTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		#endregion

		#region Scope Tests

		[TestMethod]
		// Works cases
		[DataRow("every 10 {}")]
		[DataRow("every 10 { int a = 5 }")]
		// Fails cases
		[DataRow("every", true)]
		[DataRow("every -10", true)]
		[DataRow("10 every {}", true)]
		[DataRow("every {}", true)]
		public void EveryTaskTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("on (1) {}")]
		[DataRow("on (istrue(1,2)) { int a = 5 }")]
		// Fails cases
		[DataRow("on", true)]
		[DataRow("on {}", true)]
		[DataRow("(1) on {}", true)]
		[DataRow("on 1 {}", true)]
		public void OnTaskTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("once {}")]
		[DataRow("once { int a = 5 }")]
		// Fails cases
		[DataRow("once", true)]
		[DataRow("{} once", true)]
		[DataRow("once 1 {}", true)]
		public void OnceTaskTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("idle {}")]
		[DataRow("idle { int a = 5 }")]
		[DataRow("idle a = 5")]
		// Fails cases
		[DataRow("idle", true)]
		[DataRow("{} idle", true)]
		[DataRow("idle 1 {}", true)]
		public void IdleTaskTypeTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("void func(){if (1){}}")]
		[DataRow("void func(){if (1){ int a = 5 }}")]
		[DataRow("void func(){if (1 == 5){}}")]
		[DataRow("void func(){if (1 <= 5){}}")]
		[DataRow("void func(){if (1){} else {}}")]
		[DataRow("void func(){if (1){} else { int a = 1 }}")]
		[DataRow("if (1){}")]
		[DataRow("if (1){ int a = 5 }")]
		[DataRow("if (1) a = 5 ")]
		[DataRow("if (1) a = 5 else a = 7")]
		// Fails cases
		[DataRow("void func(){if (){}}", true)]
		[DataRow("void func(){if (1){} else}", true)]
		[DataRow("void func(){if (1){} {}}", true)]
		public void IfTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("void func(){while (1){}}")]
		[DataRow("void func(){while (1){ int a = 5 }}")]
		[DataRow("void func(){while (1 == 5){}}")]
		[DataRow("void func(){while (1 <= 5){}}")]
		[DataRow("while (1){}")]
		[DataRow("while (1){ int a = 5 }")]
		// Fails cases
		[DataRow("while (){}", true)]
		[DataRow("void func(){while (){}}", true)]
		public void WhileTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("void func(){for (int i = 0, i < 10, i += 1){}}")]
		[DataRow("void func(){for (int i = 0, i < 10, i += 1){ int a = i }}")]
		[DataRow("void func(){for (int i = 10, i > 10, i -= 1){}}")]
		[DataRow("void func(){for (int i = 0, i < 10, i += 10){}}")]
		[DataRow("for (int i = 0, i < 10, i += 1){}")]
		[DataRow("for (int i = 0, i < 10, i += 1){ int a = 5 }")]
		[DataRow("for (int i = 0, i < 10, i += 1) a = 5")]
		// Fails cases
		[DataRow("void func(){for (){}}", true)]
		[DataRow("void func(){for (int i = 0){}}", true)]
		[DataRow("void func(){for (int i = 0, i += 1){}}", true)]
		[DataRow("void func(){for (int i = 0, i < 10){}}", true)]
		[DataRow("void func(){for (int i = 0,,){}}", true)]
		[DataRow("void func(){for (,int i = 0,){}}", true)]
		[DataRow("void func(){for (,,int i = 0){}}", true)]
		[DataRow("void func(){for (i += 1){}}", true)]
		[DataRow("void func(){for (i += 1, int i = 0){}}", true)]
		[DataRow("void func(){for (i += 1, int i = 0, i < 10){}}", true)]
		[DataRow("void func(){for (i += 1,,){}}", true)]
		[DataRow("void func(){for (,i += 1,){}}", true)]
		[DataRow("void func(){for (,,i += 1){}}", true)]
		public void ForTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("void func(){ return }")]
		[DataRow("void func(){ return 1 }")]
		[DataRow("void func(){ return 1 + 1 }")]
		// Fails cases
		public void ReturnTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("switch(expr) { 0 {} 1 {} 2 {} }")]
		[DataRow("switch(expr) { 0 {} 1 {} 2 {} } else { }")]
		[DataRow("switch(expr) { 0 {} 1 {} 2 {} } else { test() }")]
		// Fails cases
		[DataRow("switch() { 0 {} }", true)]
		[DataRow("switch(expr) { 0 }", true)]
		[DataRow("switch(expr) { case 0: break }", true)]
		[DataRow("switch(expr) { case 0 break }", true)]
		public void Switches(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		[TestMethod]
		// Works cases
		[DataRow("critical {}")]
		[DataRow("critical sleep(1000)")]
		[DataRow("critical { sleep(1000) }")]
		// Fails cases
		[DataRow("critical() { }", true)]
		[DataRow("critical", true)]
		public void Critical(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		#endregion

		#region Assignment Tests

		[TestMethod]
		// Works cases
		[DataRow("int a = 1")]
		[DataRow("int a = -1")]
		[DataRow("void func(){a -= 1}")]
		[DataRow("void func(){a += 1}")]
		[DataRow("void func(){a *= 1}")]
		[DataRow("void func(){a /= 1}")]
		[DataRow("void func(){a += otherfunc()}")]
		// Fails cases
		[DataRow("int a -= 1", true)]
		[DataRow("int a += 1", true)]
		[DataRow("int a *= 1", true)]
		[DataRow("int a /= 1", true)]
		[DataRow("void func(){a =* 1}", true)]
		[DataRow("void func(){a =/ 1}", true)]
		public void AssignmentTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		#endregion

		#region Binary expr test

		[TestMethod]
		// Works cases
		[DataRow("void func(){ a + b }")]
		[DataRow("void func(){ a - b }")]
		[DataRow("void func(){ a * b }")]
		[DataRow("void func(){ a / b }")]
		[DataRow("void func(){ a == b }")]
		[DataRow("void func(){ a != b }")]
		[DataRow("void func(){ a < b }")]
		[DataRow("void func(){ a <= b }")]
		[DataRow("void func(){ a > b }")]
		[DataRow("void func(){ a >= b }")]
		// Fails cases
		[DataRow("void func(){ a >=> b }", true)]
		[DataRow("void func(){ a ====== b }", true)]
		public void BinaryExprTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		#endregion

		#region Unary expr test

		[TestMethod]
		// Works cases
		[DataRow("void func(){ !a }")]
		[DataRow("void func(){ if (!a) {} }")]
		// Fails cases
		[DataRow("void func(){ a! }", true)]
		public void UnaryExprTest(string inputTest, bool expectError = false) {
			if (expectError)
				InputFails(inputTest);
			else
				InputWorks(inputTest);
		}

		#endregion

		#region Comment test
		[TestMethod]
		[DataRow("//")]
		[DataRow("//\r\n")]
		[DataRow("//...")]
		[DataRow("//...\r\n")]
		[DataRow("// ...")]
		[DataRow("// ...\r\n")]
		[DataRow("/// ...")]
		[DataRow("/// ...\r\n")]
		[DataRow("//// ...")]
		[DataRow("//// ...\r\n")]
		[DataRow("//// in x = 1;")]
		[DataRow("//// in x = 1;\r\n")]
		public void SingleLineComment_Correct(string input) {
			InputWorks(input);
		}

		[TestMethod]
		[DataRow("/")] // Missing slash
		[DataRow("/\r\n")] // Missing slash
		[DataRow("/...")] // Missing slash with stuff after
		[DataRow("/...\r\n")] // Missing slash with stuff after
		[DataRow("/ ...")] // Missing slash with stuff after
		[DataRow("/ / ...")] // Space between slashes
		[DataRow("/ / ...\r\n")] // Space between slashes
		[DataRow("/ // ...")] // While there is a comment, it shouldn't comment out things before it
		[DataRow("// ...\r\n...")] // Should only comment out line
		public void SingleLineComment_Incorrect(string input) {
			InputFails(input);
		}

		[TestMethod]
		[DataRow("/**/")]
		[DataRow("/**/\r\n")]
		[DataRow("/* */")]
		[DataRow("/*...*/")]
		[DataRow("/* ... */")]
		[DataRow("/*/**/")]
		[DataRow("/**//**/")]
		[DataRow("/*\r\n*/")]
		[DataRow("/*\r\n\r\n*/")]
		[DataRow("/*\r\n*/\r\n")]
		public void MultiLineComment_Correct(string input) {
			InputWorks(input);
		}

		[TestMethod]
		[DataRow("/*")]
		[DataRow("*/")]
		[DataRow("/")]
		[DataRow("*")]
		[DataRow("*/")]
		[DataRow("/**")]
		[DataRow("/*/")]
		public void MultiLineComment_Incorrect(string input) {
			InputFails(input);
		}

		[TestMethod]
		[DataRow("/*/**/*/")]
		[DataRow("/*/*...*/*/")]
		[DataRow("/*.../*...*/...*/")]
		[DataRow("/* ... /* ... */ ... */")]
		[DataRow("/* ... \r\n/* ... \r\n*/\r\n ... */")]
		public void NestedMultiLineComment_Correct(string input) {
			InputWorks(input);
		}

		[TestMethod]
		[DataRow("/*/*/*/")]
		[DataRow("/***/*/")]
		[DataRow("/*/**//")]
		[DataRow("/*/**/*")]
		[DataRow("*/**/*/")]
		public void NestedMultiLineComment_Incorrect(string input) {
			InputFails(input);
		}
		#endregion

		#region Uncategorized
		[TestMethod]
		[DataRow("struct MyStruct{}")] // No members
		[DataRow("struct MyStruct{int x}")] // One member
		[DataRow("struct MyStruct{int x\nint y}")] // Two members
		[DataRow("struct MyStruct{int x=1}")] // Default value
		public void StructDeclaration_WorkingExamples(string input) {
			InputWorks(input);
		}

		[TestMethod]
		[DataRow("struct MyStruct{x)")] // Invalid member
		[DataRow("struct {int x}")] // Missing name
		[DataRow("struct MyStruct")] // Missing members
		[DataRow("struct MyStruct{int x int y}")] // Multiple members on one line
		public void StructDeclaration_FailingExamples(string input) {
			InputFails(input);
		}

		[TestMethod]
		[DataRow("MyStruct{}")] // No members
		[DataRow("MyStruct{1}")] // One member
		[DataRow("MyStruct{1, 2}")] // Two members
		[DataRow("MyStruct{1, y=2}")] // Named member
		[DataRow("MyStruct{1,\n2}")] // Lines between comma
		public void StructLiteral_WorkingExamples(string input) {
			InputWorks(input);
		}

		[TestMethod]
		[DataRow("item.member")]
		[DataRow("123.member")]
		[DataRow("call().member")]
		[DataRow("tree.branch.leaf")]
		public void MemberAccess_WorkingExamples(string input) {
			InputWorks(input);
		}

		[TestMethod]
		[DataRow("point.123")] // Member name is number
		[DataRow("point.")] // No member
		[DataRow(".member")] // No item
		public void MemberAccess_FailingExamples(string input) {
			InputFails(input);
		}

		#endregion

		#region Private Test Methods

		private static void InputWorks(string inputTest) {
			LexerErrorListener lexerErrorListener = new LexerErrorListener();
			ParserErrorListener parserErrorListener = new ParserErrorListener();

			var cst = (ParserRuleContext)getParser(inputTest, lexerErrorListener, parserErrorListener).compileUnit();

			Assert.IsTrue(lexerErrorListener.Errors.Count == 0);
			Assert.IsTrue(parserErrorListener.Errors.Count == 0);
			Assert.IsNotNull(cst);
		}

		private static void InputFails(string inputTest) {
			LexerErrorListener lexerErrorListener = new LexerErrorListener();
			ParserErrorListener parserErrorListener = new ParserErrorListener();

			getParser(inputTest, lexerErrorListener, parserErrorListener).compileUnit();

			if (lexerErrorListener.Errors.Count == 0 && parserErrorListener.Errors.Count == 0)
				Assert.Fail($"Input: \"{inputTest}\" parsed, but shouldn't have");
		}

		private static JanCParser getParser(string input, LexerErrorListener lexerErrorListener, ParserErrorListener parserErrorListener) {
			var lexer = new JanCLexer(new AntlrInputStream(input));
			lexer.RemoveErrorListeners();
			lexer.AddErrorListener(lexerErrorListener);

			var parser = new JanCParser(new CommonTokenStream(lexer));
			parser.RemoveErrorListeners();
			parser.BuildParseTree = true;
			parser.AddErrorListener(parserErrorListener);

			return parser;
		}

		#endregion
	}
}
