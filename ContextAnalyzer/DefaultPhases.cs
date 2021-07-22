using ContextAnalyzer.Tables;
using ContextAnalyzer.Listener;
using Exceptions.Exceptions;
using Exceptions.Exceptions.Base;
using Exceptions.Syntax;
using Nodes;
using System.Collections.Generic;
using Tables;
using Tools;

namespace ContextAnalyzer {
	/// <summary>
	/// Static class with all the information for the default phases of the compiler
	/// </summary>
	internal static class DefaultPhases {
		internal static List<Phase> DefaultPhasesList {
			get {
				var pinTable = new ContextPinTable();
				var symbolTable = new ContextSymbolTable();
				ReservedFunctionsTable.InsertBuiltInFunctions(symbolTable);
				var phases = new List<Phase>
				{
					// Phase 1: Record globals, so we don't need forward declarations.
					new Phase(
						strategy: DefaultPhasesMethods.VisitGlobals,
						listeners: new List<ASTListener> {
							new GlobalsRecorderListener(symbolTable),
						}
					),
					// Phase 2: Perform semantic analysis.
					new Phase(
						strategy: DefaultPhasesMethods.VisitDepthFirst,
						listeners: new List<ASTListener> {
							new ScopeListener(symbolTable),
							new TypeEvaluatorListener(symbolTable),
							new StructRecursionListener(symbolTable),
							new PinCheckerListener(pinTable, symbolTable),
						}
					)
				};
				return phases;
			}
		}
	}
}
