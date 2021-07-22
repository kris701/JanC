using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public abstract class ComplexTypeNode : TypeNode {
		public ComplexTypeNode(JanCParser.TypeLiteralContext context) : base(context) {
		}
	}
}
