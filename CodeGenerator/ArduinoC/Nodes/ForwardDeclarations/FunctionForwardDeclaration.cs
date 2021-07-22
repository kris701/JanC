using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.ArduinoC.Nodes {
	public class FunctionForwardDeclaration : ForwardDeclaration {
		public FunctionForwardDeclaration(FuncDeclNode function) : base() {
			Function = function;
		}
		public FuncDeclNode Function { get; set; }
	}
}
