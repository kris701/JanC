using CodeGenerator.ArduinoC.DomainGenerator.Listeners;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using Exceptions.Exceptions;
using Exceptions.Exceptions.Base;
using Exceptions.Syntax;
using Nodes;
using System.Collections.Generic;
using Tables;
using Tools;

namespace CodeGenerator.ArduinoC.DomainGenerator {
	/// <summary>
	/// Static class with all the information for the default phases of the compiler
	/// </summary>
	internal static class DefaultPhases {
		internal static List<Phase> DefaultPhasesList {
			get {
				var symbolTable = new ContextSymbolTable();
				var pinTable = new PinTable();
				var phases = new List<Phase>
				{
					// Phase 1: Record globals, so we don't need forward declarations.
					new Phase(
						strategy: DefaultPhasesMethods.VisitDepthFirstDeclarations,
						listeners: new List<ASTListener> {
							new ModuleDeclarationRenamerListener(),
						}
					),
					// Phase 2: Renames tasks, so they dont conflict with eachother
					new Phase(
						strategy: DefaultPhasesMethods.VisitDepthFirstDeclarations,
						listeners: new List<ASTListener> {
							new TaskRenamerListener(),
						}
					),
					// Phase 3: Record function calls
					new Phase(
						strategy: DefaultPhasesMethods.VisitDepthFirst,
						listeners: new List<ASTListener> {
							new PinCheckerListener(pinTable, symbolTable),
						}
					),
					// Phase 4: Add prefix to all userdefined names
					new Phase(
						strategy: DefaultPhasesMethods.VisitDepthFirst,
						listeners: new List<ASTListener> {
							new PrefixNamesListener(),
						}
					),
					// Phase 5: Insert Arduino and FreeRTOS nodes.
					new Phase(
						strategy: DefaultPhasesMethods.VisitGlobals,
						listeners: new List<ASTListener> {
							new ArduinoSetupListener(symbolTable, pinTable),
							new TaskSetupListener(symbolTable),
							new GlobalCodeListener(symbolTable)
						}
					),
					// Phase 6: Cleanup types and function names
					new Phase(
						strategy: DefaultPhasesMethods.VisitDepthFirst,
						listeners: new List<ASTListener> {
							new ReservedFunctionsConverterListener(),
							new SetupStructLiteralVarListener(symbolTable)
						}
					),
				};
				return phases;
			}
		}
	}
}
