namespace Exceptions.Syntax {
	/// <summary>
	/// These are three different interfaces to give semantic errors
	/// </summary>
	public interface ISemanticIssue { }
	public interface ISemanticError : ISemanticIssue { }
	public interface ISemanticWarning : ISemanticIssue { }
}

