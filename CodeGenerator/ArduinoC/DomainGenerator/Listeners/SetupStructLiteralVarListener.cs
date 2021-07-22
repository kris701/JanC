using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using Tables;
using Tools;
using Nodes.ASTHelpers;
using CodeGenerator.ArduinoC.Nodes;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	/// <summary>
	/// Properly initializes the struct as a variable and tells the literal what variable it should refer to
	/// </summary>
	internal class SetupStructLiteralVarListener : ASTListener {
		private readonly SymbolTable _symbolTable;
		private int counter = 1;

		public SetupStructLiteralVarListener(SymbolTable symbolTable) {
			if (symbolTable == null)
				throw new ArgumentNullException();
			_symbolTable = symbolTable;
		}

		public void Enter(BlockNode block) {
			SetupStructLiteralVariables(block.Content);
		}

		public void Enter(GlobalScopeNode globalScope) {
			SetupStructLiteralVariables(globalScope.Content);
		}

		private void SetupStructLiteralVariables(List<IUnit> units) {
			InsertUnits(units, CreateStructLiteralVariables(units));
		}

		private List<Tuple<int, StructLiteralVarNode>> CreateStructLiteralVariables(List<IUnit> units) {
			var variables = new List<Tuple<int, StructLiteralVarNode>>();
			int unitIndex = 0; // Keeps track of where the variables should be placed
			foreach (var unit in units) {
				foreach (var unitVariable in CreateStructLiteralVariablesForUnit(unit).Reverse())
					variables.Add(Tuple.Create(unitIndex, unitVariable));
				unitIndex++;
			}
			return variables;
		}

		private IEnumerable<StructLiteralVarNode> CreateStructLiteralVariablesForUnit(IUnit unit) {
			var structLiterals = GetNodeAndChildrenThatAreNotInsideInnerBlock(unit)
				.OfType<StructLiteralNode>();
			foreach (var structLiteral in structLiterals) {
				var variable = new StructLiteralVarNode(GenerateUnusedName(), structLiteral);
				structLiteral.VariableName = variable.Name;
				yield return variable;
			}
		}

		private static void InsertUnits(List<IUnit> units, List<Tuple<int, StructLiteralVarNode>> NodesToInsert) {
			int offset = 0;
			foreach (var (unitIndex, structVar) in NodesToInsert) {
				units.Insert(unitIndex + offset, structVar);
				offset += 1;
			}
		}

		private string GenerateUnusedName() {
			string name;
			do { name = GenerateName(); } while (_symbolTable.IsAlreadyDeclaredInCurrentScope(name));
			return name;
		}

		private string GenerateName()
			=> "_struct_" + (counter++).ToString();

		private static List<IASTNode> GetNodeAndChildrenThatAreNotInsideInnerBlock(IUnit unit) {
			var queue = new Stack<IASTNode>();
			var children = new List<IASTNode>() { };
			queue.Push(unit);
			while (queue.Count > 0) {
				var node = queue.Pop();
				if (node is BlockNode)
					continue;
				children.Add(node);
				queue.PushRange(node.Children);
			}
			return children;
		}
	}
}
