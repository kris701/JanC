grammar JanC;

// Global Scope
	compileUnit: EOL* (unit EOL+)* (unit EOL*)? EOF;

// Unit and Impr
	unit:	decl | impr;
	impr:	stmt | expr;

// Decl
	decl:	'every' delay=NUM EOL* impr																							# EveryTaskNode
		|	'on' '(' condition=expr ')' EOL* impr																				# OnTaskNode
		|	'once' EOL* impr																										# OnceTaskNode
		|	'idle' EOL* impr																										# IdleTaskNode
		|	type=typeLiteral name=ID '(' parameters? ')' impr																# FuncDecl
		|	varDecl																											# VarDeclNodeB
		|	'struct' name=ID EOL* '{' EOL* (varDecl (EOL+ varDecl)*)? EOL* '}'													# StructDecl
		|	'module' name=ID EOL* '{' EOL* (unit (EOL+ unit)*)? EOL* '}'															# ModuleDecl
		;

	parameters: varDecl (EOL* ',' varDecl)*;

// Stmt
	stmt:	varDecl																											# VarDeclNodeA
		|	'{' EOL* (impr (EOL+ impr)*)? EOL* '}'																			# BlockStmt
		|	'if' '(' condition=expr ')' EOL* thenBody=impr EOL* ('else' EOL* elseBody=impr EOL*)?							# IfStmt
		|	'while' '(' condition=expr ')' EOL* body=impr																	# WhileStmt
		|	'for' EOL* '(' init=impr ',' condition=expr ',' iteration=impr ')' EOL* body=impr								# ForStmt
		|	location=expr op=(ASSIGN|SUB_ASSIGN|ADD_ASSIGN|MUL_ASSIGN|DIV_ASSIGN) value=expr								# AssignStmt
		|	'return' (value=expr)?																							# ReturnStmt
		|	'switch' '(' value=expr ')' EOL* '{' switchCaseStmt+ '}' (EOL* 'else' EOL* defaultCase=impr)?					# SwitchStmt
		|	'critical' EOL* body=impr																						# CriticalStmt
		;

	switchCaseStmt:		EOL* value=expr impr EOL*;

// Expr
	expr:	'ref' '(' EOL* argument EOL* ')'																				# RefCallExpr
		|	item=expr '(' EOL* arguments? EOL* ')'																			# CallExpr
		|	'(' EOL* expr EOL* ')'																							# GroupingExpr
		|	op=MINUS expr																									# UnaryExpr
		|	structExpr=expr DOT memberName=identifier																		# MemberAccess
		|	left=expr op=(TIMES|DIV) right=expr																				# BinaryExpr
		|	left=expr op=(PLUS|MINUS) right=expr																			# BinaryExpr
		|	left=expr op=(EQUALS|NOT_EQUALS|LESS_THAN|LESS_THAN_OR_EQUALS|GREATER_THAN|GREATER_THAN_OR_EQUALS) right=expr	# BinaryExpr
		|	value=NUM																										# NumberLiteral
		|	value=STR																										# StringLiteral
		|	identifier																										# IdentifierExpr
		|	value=('true'|'false')																							# BoolLiteral
		|	'!' expresion=expr																								# NotNode
		|	typeLiteral '{' EOL* (structLiteralMember (EOL* ',' EOL* structLiteralMember)*)? EOL* '}'						# StructLiteral
		;

	structLiteralMember: (name=ID '=' )? value=expr;

	arguments: argument (EOL* ',' argument)*;
	argument: expr; // Named args may be possible in the future.

	identifier: id=ID;

// Types
	typeLiteral: baseType=ID																								# LiteralType
		|	'ref<' typeLiteral '>'																							# LiteralTypeRef
		|	typeLiteral '.' memberName=ID																					# LiteralTypeAccess
		;

// Other
	varDecl: CONST? type=typeLiteral name=ID ('=' value=expr)?;

// Lexer Elements
	// Lexer Tokens
		PLUS:					'+';	MINUS:			'-';	TIMES:						'*';
		DIV:					'/';	ASSIGN:			'=';	SUB_ASSIGN:					'-=';
		ADD_ASSIGN:				'+=';	MUL_ASSIGN:		'*=';	DIV_ASSIGN:					'/=';
		EQUALS:					'==';	NOT_EQUALS:		'!=';	LESS_THAN:					'<';
		LESS_THAN_OR_EQUALS:	'<=';	GREATER_THAN:	'>';	GREATER_THAN_OR_EQUALS:		'>=';
		DOT:					'.';	CONST:			'const';

	// Lexer Literals
		STR:	'"' ~('\n'|'"')* '"';
		NUM:	[0-9]+ ('.' [0-9]+)? ([eE] [+-]? [0-9]+)?;
		ID:		[a-zA-Z_][a-zA-Z_0-9]*;

	// Lexer Comment
		COMMENT: '/*' (COMMENT|.)*? '*/' -> skip;
		LINE_COMMENT: '//' ~[\r\n]* -> skip;

	// Lexer Other
		EOL:	[\r\n];
		WS:		[ \t] -> channel(HIDDEN);
