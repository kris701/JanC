using Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace CodeGenerator.ArduinoC.DomainGenerator.Listeners {
	/// <summary>
	/// Renames declarations inside modules to include module name,
	/// such that they can be (more-or-less safely) put into global scope later.
	/// </summary>
	internal class ModuleDeclarationRenamerListener : ASTListener {
		public void Enter(ModuleDeclNode node) {
			foreach (var decl in node.Content.OfType<IDecl>()) {
				decl.Name = $"{node.Name}_{decl.Name}";
			}
		}
	}
}
