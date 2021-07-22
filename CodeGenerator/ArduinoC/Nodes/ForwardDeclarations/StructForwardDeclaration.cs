using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.ArduinoC.Nodes {
	public class StructForwardDeclaration : ForwardDeclaration {
		public StructForwardDeclaration(StructDeclNode @struct) : base() {
			Struct = @struct;
		}
		public StructDeclNode Struct { get; set; }
	}
}
