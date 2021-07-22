using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes {
	public abstract class BaseExprNode : BaseASTNode, IExpr {
		protected BaseExprNode(ParserRuleContext context) : base(context) { }

		public virtual ITypeLiteral Type { get; set; }
	}
}
