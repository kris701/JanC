using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public class RefTypeNode : ComplexTypeNode {
		public RefTypeNode(JanCParser.TypeLiteralContext context, ITypeLiteral subType) : base(context) {
			SubType = subType;
			if (subType is RefTypeNode subRef) {
				BottomType = subRef.BottomType;
				RefDepth = subRef.RefDepth + 1;
			}
			else {
				BottomType = subType;
				RefDepth = 1;
			}

			Name = $"ref<{subType.Name}>";
		}

		public ITypeLiteral SubType { get; }
		public ITypeLiteral BottomType { get; }
		public int RefDepth { get; }

		public override string Name { get; set; }

		public override bool IsNumeric => false;

		public override bool IsInvalid => SubType.IsInvalid;

		public override bool Equals(object other) {
			if (other is RefTypeNode @ref) {
				return SubType.Equals(@ref.SubType);
			}
			return false;
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}

		public override List<IASTNode> Children => NodesHelper.NewNodes.With(SubType);
	}
}
