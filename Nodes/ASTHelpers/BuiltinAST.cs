using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.ASTHelpers.CommonAST;
using static Nodes.ASTHelpers.DecoratedAST;
using static Nodes.TypeNode;

namespace Nodes.ASTHelpers {
	public static class BuiltinAST {
		public static FuncDeclNode DigitalReadDecl => Int.Function("digitalRead")
	.With(Int.VarDecl("pin"));
		public static FuncDeclNode DigitalWriteDecl => TypeNode.Void.Function("digitalWrite")
				.With(Int.VarDecl("pin"))
				.With(Int.VarDecl("value"));
		public static FuncDeclNode AnalogReadDecl => Int.Function("analogRead")
				.With(Int.VarDecl("pin"));
		public static FuncDeclNode AnalogWriteDecl => TypeNode.Void.Function("analogWrite")
				.With(Int.VarDecl("pin"))
				.With(Int.VarDecl("value"));
		public static FuncDeclNode SleepDecl => TypeNode.Void.Function("sleep")
				.With(Int.VarDecl("delay"));
		public static FuncDeclNode SetupDecl => TypeNode.Void.Function("setup");
		public static FuncDeclNode LoopDecl => TypeNode.Void.Function("loop");
		public static CallExprNode DigitalRead(int pinId) {
			return Call(DigitalReadDecl).With(Argument(pinId));
		}
		public static CallExprNode DigitalWrite(int pinId, int value) {
			return Call(DigitalWriteDecl).With(Argument(pinId)).With(Argument(value));
		}
		public static CallExprNode AnalogRead(int pinId) {
			return Call(AnalogReadDecl).With(Argument(pinId));
		}
		public static CallExprNode AnalogWrite(int pinId, int value) {
			return Call(AnalogWriteDecl).With(Argument(pinId)).With(Argument(value));
		}
		public static CallExprNode Sleep(int delay) {
			return Call(SleepDecl).With(Argument(delay));
		}
	}
}
