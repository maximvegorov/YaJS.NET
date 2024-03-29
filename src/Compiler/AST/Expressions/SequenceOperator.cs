﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace YaJS.Compiler.AST.Expressions {
	public sealed class SequenceOperator : Expression {
		internal SequenceOperator(List<Expression> operands)
			: base(ExpressionType.Sequence) {
			Contract.Requires(operands != null && operands.Count > 0);
			Operands = operands;
		}

		public override string ToString() {
			var result = new StringBuilder();
			foreach (var operand in Operands)
				result.Append(operand).Append(',');
			result.Length -= 1;
			return (result.ToString());
		}

		internal override void CompileBy(FunctionCompiler compiler, bool isLastOperator) {
			for (var i = 0; i < Operands.Count - 1; i++)
				Operands[i].CompileBy(compiler, true);
			Operands[Operands.Count - 1].CompileBy(compiler, isLastOperator);
		}

		public override bool CanHaveMembers { get { return (Operands[Operands.Count - 1].CanHaveMembers); } }
		public override bool CanHaveMutableMembers { get { return (Operands[Operands.Count - 1].CanHaveMutableMembers); } }
		public override bool CanBeConstructor { get { return (Operands[Operands.Count - 1].CanBeConstructor); } }
		public override bool CanBeFunction { get { return (Operands[Operands.Count - 1].CanBeFunction); } }
		public override bool CanBeDeleted { get { return (Operands[Operands.Count - 1].CanBeDeleted); } }
		public override bool CanBeObject { get { return (Operands[Operands.Count - 1].CanBeObject); } }
		public override bool IsConstant { get { return (Operands.All(o => o.IsConstant)); } }
		public List<Expression> Operands { get; private set; }
	}
}
