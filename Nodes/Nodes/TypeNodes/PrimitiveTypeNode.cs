using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public class PrimitiveTypeNode : TypeNode {
		public enum Types {
			Invalid,
			Void,
			Integer,
			String,
			Bool,
			Float
		}

		public Types Type { get; }

		public PrimitiveTypeNode(JanCParser.TypeLiteralContext context, Types type) : base(context) {
			Type = type;
		}

		public override string Name { get => Enum.GetName(typeof(Types), Type); set => throw new InvalidOperationException(); }

		public override bool IsNumeric {
			get {
				switch (Type) {
					case Types.Invalid:
						throw new InvalidOperationException();

					case Types.Void:
					case Types.String:
					case Types.Bool:
						return false;

					case Types.Integer:
					case Types.Float:
						return true;

					default:
						throw new NotImplementedException();
				}
			}
		}

		public override bool IsInvalid => Type == Types.Invalid;

		public override bool Equals(object other) {
			if(other is PrimitiveTypeNode primitive) {
				return primitive.Type == this.Type;
			}
			return false;
		}

		public override int GetHashCode() {
			return Type.GetHashCode();
		}
	}
}
