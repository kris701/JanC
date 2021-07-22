using Antlr4.Runtime;
using System.Collections.Generic;

namespace Nodes {
	public interface IUnit : IASTNode {

	}

	public interface IImpr : IUnit {

	}

	public interface IExpr : IImpr {
		public ITypeLiteral Type { get; set; }
	}

	public interface IUnary : IExpr {
		public IExpr Value { get; }
	}

	public interface ILiteral : IExpr {

	}
	public interface IConstable {
		bool? IsConst { get; set; }
	}

	public interface ITypeLiteral : IASTNode {
		public JanCParser.TypeLiteralContext Context { get; }
		public abstract string Name { get; set; }
		public bool Equals(object other);
		public int GetHashCode();
		public bool IsNumeric { get; }
		public bool IsInvalid { get; }
		public string ToString();
	}

	public interface IStmt : IImpr {
	}

	public interface IAssign : IStmt {
	}

	public interface IDecl : IUnit {
		public StringRef NameRef { get; }
		public string Name { get; set; }
		public IToken NameToken { get; }
	}

	// Declarations that have a type (variables, functions)
	public interface ITypedDecl : IDecl {
		public ITypeLiteral Type { get; }
	}

	// For user-defined type declarations (struct, enum, class)
	public interface ITypeDecl : ITypedDecl {
	}

	// For compile-time types that cannot be instantiated at run-time (function, module)
	public interface IStaticTypeDecl : ITypeDecl {

	}

	public interface IImprContainer : IASTNode {
		public IImpr Body { get; set; }
	}
}
