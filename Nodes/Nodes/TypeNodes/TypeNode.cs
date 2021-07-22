using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public abstract class TypeNode : BaseASTNode, ITypeLiteral {

		public TypeNode(JanCParser.TypeLiteralContext context) : base(context) {
			Context = context;
		}
		public JanCParser.TypeLiteralContext Context { get; }
		public abstract string Name { get; set; }

		public override List<IASTNode> Children { get => NodesHelper.NewNodes; }

		public abstract override bool Equals(object other);

		public abstract override int GetHashCode();

		public abstract bool IsNumeric { get; }

		public abstract bool IsInvalid { get; }
		public override string ToString() { return Name; }

		#region Type constructors
		public static PrimitiveTypeNode Invalid {
			get => new PrimitiveTypeNode(
				context: null,
				type: PrimitiveTypeNode.Types.Invalid
			);
		}
		public static PrimitiveTypeNode Void {
			get => new PrimitiveTypeNode(
				context: null,
				type: PrimitiveTypeNode.Types.Void
			);
		}
		public static PrimitiveTypeNode Int {
			get => new PrimitiveTypeNode(
				context: null,
				type: PrimitiveTypeNode.Types.Integer
			);
		}
		public static PrimitiveTypeNode String {
			get => new PrimitiveTypeNode(
				context: null,
				type: PrimitiveTypeNode.Types.String
			);
		}
		public static PrimitiveTypeNode Float {
			get => new PrimitiveTypeNode(
				context: null,
				type: PrimitiveTypeNode.Types.Float
			);
		}
		public static PrimitiveTypeNode Bool {
			get => new PrimitiveTypeNode(
				context: null,
				type: PrimitiveTypeNode.Types.Bool
			);
		}

		public static RefTypeNode Ref(ITypeLiteral subType) {
			return new RefTypeNode(
				context: null,
				subType: subType
			);
		}

		public static UserDefinedTypeNode Struct(string name) {
			return new UserDefinedTypeNode(
				context: null,
				name: name
			);
		}
		#endregion
	}
}
