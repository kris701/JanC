using Nodes;
using System.Text;

namespace CodeGenerator.ArduinoC.CodeGenerator.Visitors {
	internal partial class CodeGeneratorVisitor {
		public string GenerateCode(IASTNode rootContext) {
			return Visit(rootContext);
		}

		public string Visit(IASTNode node) {
			// Overflow exception here means missing visitor
			return Visit((dynamic)node);
		}

		private void Indent(int increase=1) {tabs += increase; }
		private void Dedent(int decrease=1) { tabs -= decrease; }

		private int tabs = 0;

		private string Ref(IExpr id, int depth) {
			if (depth == 0) return Visit(id);
			return new StringBuilder()
				.Append('(')
				.Append(new string('&', depth))
				.Append(Visit(id))
				.Append(')')
				.ToString();
		}
		private string DeRef(IExpr @ref, int depth) {
			if (depth == 0) return Visit(@ref);
			return new StringBuilder()
				.Append('(')
				.Append(new string('*', depth))
				.Append(Visit(@ref))
				.Append(')')
				.ToString();
		}
		private string Value(IExpr expr) {
			if (expr.Type is RefTypeNode @ref)
				return DeRef(expr, @ref.RefDepth);
			return Visit(expr);
		}


	}
}
