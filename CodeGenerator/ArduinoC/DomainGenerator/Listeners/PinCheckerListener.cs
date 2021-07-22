using CodeGenerator.ArduinoC.DomainGenerator.ContextTables;
using Nodes;
using System;
using System.Linq;
using Tables;
using Tables.Pins;
using Tools;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	internal class PinCheckerListener : ASTListener {
		private readonly PinTable _pinTable;
		private readonly ContextSymbolTable _symbolTable;
		public PinCheckerListener(PinTable pinTable, ContextSymbolTable symbolTable) {
			if (pinTable == null)
				throw new ArgumentNullException();
			if (symbolTable == null)
				throw new ArgumentNullException();
			_pinTable = pinTable;
			_symbolTable = symbolTable;
		}

		public void Leave(VarDeclNode node) {
			_symbolTable.Record(node);
		}

		/// <summary>
		/// Method to leave this listener
		/// </summary>
		/// <param name="node"></param>
		public void Leave(CallExprNode node) {
			PinState? state = InferPinState(node);
			if (state is not null) {
				int? pinId = ReadPinId(node.Arguments.First().Value);
				if (pinId.HasValue)
					_pinTable.RecordPinUsage(pinId.Value, state.Value, node.Context!);
			}
		}

		private int? ReadPinId(IExpr expr) {
			int? pinId = null;
			if (expr is IntLiteralNode literalA)
				pinId = literalA.Value;
			else if (expr is IdentifierExpr identifier && identifier.Type.Equals(TypeNode.Int)) {
				pinId = ReadPinIdFromConstVariableWithIntLiteralValue(identifier);
			}
			return pinId;
		}

		private int? ReadPinIdFromConstVariableWithIntLiteralValue(IdentifierExpr identifier) {
			int? pinId = null;
			var varDecl = _symbolTable.GetDeclaration(identifier.Name) as VarDeclNode;
			if (varDecl is not null && (varDecl.IsConst ?? false) && varDecl.Value is not null && varDecl.Value is IntLiteralNode literalB)
				pinId = literalB.Value;
			return pinId;
		}

		/// <summary>
		/// Method to return the correct <seealso cref="PinModeNode.PinState"/> from a function name
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		private static PinState? InferPinState(CallExprNode call) {
			return call.Item switch {
				IdentifierExpr identifier => identifier.Name switch {
					"digitalWrite" => PinState.OUTPUT,
					"analogWrite" => PinState.OUTPUT,
					"digitalRead" => PinState.INPUT,
					"analogRead" => PinState.INPUT,
					_ => null,
				},
				_ => null,
			};
		}
	}
}
