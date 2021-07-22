using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public class ModuleDeclNode : BaseDeclNode, IStaticTypeDecl {
		public ModuleDeclNode(string name, List<IUnit> content, JanCParser.ModuleDeclContext context) : base(context, name) {
			Content = content;
			Type = new UserDefinedTypeNode(null, name, UserDefinedTypeNode.Types.Module) { Decl = this };
			((UserDefinedTypeNode)Type).LinkNameTo(this);
			Context = context;
		}
		public ITypeLiteral Type { get; set; }
		public List<IUnit> Content { get; set; }
		public JanCParser.ModuleDeclContext Context { get; set; }
		public override IToken NameToken => Context?.name;
		public override List<IASTNode> Children => NodesHelper.NewNodes.With(Type).With(Content);
	}
}
