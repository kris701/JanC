using Nodes;
using ContextAnalyzer.SemanticErrors;
using ContextAnalyzer.Tables;
using System.Linq;
using Tables.Pins;
using Tools;
using System;


// Note: Compare and contrast to CodeGenerator.PinModeListener
namespace ContextAnalyzer.Listener {
	/// <summary>
	/// Listener to check that pins don't both become output and input at the same time
	/// </summary>
	internal class PinCheckerListener : ASTListener {
		private ContextPinTable _pinTable;
		private ContextSymbolTable _symbolTable;
		public PinCheckerListener(ContextPinTable pinTable, ContextSymbolTable symbolTable) {
			if (pinTable == null)
				throw new ArgumentNullException();
			if (symbolTable == null)
				throw new ArgumentNullException();
			_pinTable = pinTable;
			_symbolTable = symbolTable;
		}

		/// <summary>
		/// Method to leave this listener
		/// </summary>
		/// <param name="node"></param>
		public void Leave(CallExprNode node) {
			PinState? state = InferPinState(node.Item);
			if (state is not null) {
				if (node.Arguments.Count == 0)
					throw new ArgumentCountMismatchBuildInFunction(node.Item.Type.Name, 0, (int)state.Value + 1, node.Context);
				int? pinId = ReadPinId(node.Arguments.First().Value);
				if (pinId.HasValue)
					_pinTable.RecordPinUsage(pinId.Value, state.Value, node.Context!);
				else
					throw new PinIdNotConstant(node.Arguments.First().Context);
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
		private static PinState? InferPinState(IExpr item) {
			if (item.Type is UserDefinedTypeNode type && type.Type.Equals(UserDefinedTypeNode.Types.Function)) {
				var functionName = item.Type.Name;
				return functionName switch {
					"digitalWrite" => PinState.OUTPUT,
					"analogWrite" => PinState.OUTPUT,
					"digitalRead" => PinState.INPUT,
					"analogRead" => PinState.INPUT,
					_ => null,
				};
			} else
				return null;
		}
	}
}
