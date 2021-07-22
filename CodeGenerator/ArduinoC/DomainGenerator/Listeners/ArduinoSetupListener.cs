using Nodes;
using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using CodeGenerator.ArduinoC.Nodes;
using System.Collections.Generic;
using Tables;
using Tables.Pins;
using Tools;
using System;
using static Nodes.ASTHelpers.CommonAST;
using Nodes.ASTHelpers;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	/// <summary>
	/// A listener to add the necessary Arduino and FreeRTOS nodes
	/// </summary>
	internal class ArduinoSetupListener : ASTListener {
		private readonly ContextSymbolTable _symbolTable;
		private readonly PinTable _pinTable;

		public ArduinoSetupListener(ContextSymbolTable symbolTable, PinTable pinTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			if (pinTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
			_pinTable = pinTable;
		}

		/// <summary>
		/// Method to enter this listener
		/// </summary>
		/// <param name="node"></param>
		public void Enter(GlobalScopeNode node) {
			node.Content.Insert(0, new LibraryNode(name: "Arduino_FreeRTOS.h"));
			node.Content.Add(BuildSetup(_pinTable.Pins));
			node.Content.Add(BuiltinAST.LoopDecl);
		}

		/// <summary>
		/// Method to add the setup node
		/// </summary>
		/// <param name="pins"></param>
		/// <param name="tasks"></param>
		/// <returns></returns>
		private FuncDeclNode BuildSetup(List<Pin> pins) {
			var setup = BuiltinAST.SetupDecl;
			AddPinsToSetup(setup, pins);
			_symbolTable.Record(setup);
			return setup;
		}

		/// <summary>
		/// Method to add pinMode() nodes to setup, based on a list of pins
		/// </summary>
		/// <param name="setup"></param>
		/// <param name="pins"></param>
		private static void AddPinsToSetup(FuncDeclNode setup, List<Pin> pins) {
			foreach (Pin pin in pins) {
				setup.AppendBody(new PinModeNode(
					pinId: pin.Id,
					state: pin.State
				));
			}
		}
	}
}
