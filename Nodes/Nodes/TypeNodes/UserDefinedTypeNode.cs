using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public class UserDefinedTypeNode : TypeNode {
		public UserDefinedTypeNode(JanCParser.TypeLiteralContext context, string name, Types? type=null, List<string> accessNames=null) : base(context) {
			NameRef = new StringRef(name);
			Type = type;
			ModuleNames = accessNames ?? new List<string>();
		}

		public enum Types {
			Struct,
			Enum,
			Module,
			Function
		}

		public ITypeDecl Decl { get; set; } = null;
		// module1.module2.struct will give access names: ["module1", "module2"]
		public List<string> ModuleNames { get; }

		public Types? Type { get; set; } = null;

		public override string Name { get => NameRef.Value; set => NameRef.Value = value; }
		public StringRef NameRef { get; private set; }
		public void LinkNameTo(ITypeDecl decl) {
			NameRef = decl.NameRef;
		}

		public override bool IsNumeric => false;

		public override bool IsInvalid => Decl is null;

		public override bool Equals(object other) {
			if(other is UserDefinedTypeNode node) {
				return 
					node.Type == Type &&
					node.Name == Name &&
					node.IsInvalid == IsInvalid;
			}
			return false;
		}

		public override int GetHashCode() {
			throw new NotImplementedException();
		}
	}
}
