using ContextAnalyzer;
using Exceptions.Exceptions;
using Exceptions.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nodes.ASTHelpers.CommonAST;

namespace ContextAnalyzer.Nodes {
	public abstract class BaseRulesTests {
		internal SemanticErrorListener errorListener;
		internal IContextAnalyzer contextAnalyzer;
		internal GlobalScopeNode global;

		[TestInitialize]
		public virtual void Setup() {
			errorListener = new SemanticErrorListener();
			contextAnalyzer = new ContextAnalyzer().SetPhases().SetErrorListener(errorListener);
			global = Root();
		}

		public void ThrowErrorsIfAny() {
			contextAnalyzer = contextAnalyzer.ReadSource(global).DecorateAST();

			if (errorListener.HadErrors)
				throw errorListener.Errors[0];
		}
	}
}
