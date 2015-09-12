using System;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Базовый класс для всех функций
	/// </summary>
	public abstract class JSFunction : JSObject {
		private const string PrototypeMemberName = "prototype";
		public static readonly JSValue[] EmptyArgumentList = new JSValue[0];

		protected JSFunction(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(inherited == vm.Function);
		}

		public virtual JSObject GetPrototype() {
			JSValue result;
			if (!OwnMembers.TryGetValue(PrototypeMemberName, out result)) {
				result = VM.NewObject();
				OwnMembers.Add(PrototypeMemberName, result);
			}
			return (result.RequireObject());
		}

		/// <summary>
		/// Вызвать Native-функцию
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="outerScope">Внешняя область локальных переменных</param>
		/// <param name="args">Список параметров</param>
		/// <returns></returns>
		public virtual JSValue Construct(ExecutionThread thread, VariableScope outerScope, JSValue[] args) {
			Contract.Requires(thread != null);
			Contract.Requires(outerScope != null);
			Contract.Requires(args != null);
			throw new NotSupportedException();
		}

		/// <summary>
		/// Вызвать Native-функцию
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="context">Контекст</param>
		/// <param name="outerScope">Внешняя область локальных переменных</param>
		/// <param name="args">Список параметров</param>
		/// <returns></returns>
		public virtual JSValue Invoke(ExecutionThread thread, JSObject context, VariableScope outerScope, JSValue[] args) {
			Contract.Requires(thread != null);
			Contract.Requires(context != null);
			Contract.Requires(outerScope != null);
			Contract.Requires(args != null);
			throw new NotSupportedException();
		}

		public override bool ContainsMember(JSValue member) {
			var name = member.CastToString();
			switch (name) {
				case "prototype":
				case "length":
					return (true);
				default:
					return (base.ContainsMember(name));
			}
		}

		public override JSValue GetMember(JSValue member) {
			var name = member.CastToString();
			switch (name) {
				case "prototype":
					return (GetPrototype());
				case "length":
					return ((JSNumberValue)ParameterCount);
				default:
					return (base.GetMember(name));
			}
		}

		public override void SetMember(JSValue member, JSValue value) {
			var name = member.CastToString();
			if (name == "length")
				throw new TypeErrorException();
			base.SetMember(name, value);
		}

		public override bool DeleteMember(JSValue member) {
			var name = member.CastToString();
			switch (name) {
				case "prototype":
				case "length":
					throw new TypeErrorException();
				default:
					return (base.DeleteMember(name));
			}
		}

		public override string TypeOf() {
			return ("function");
		}

		public override JSFunction RequireFunction() {
			return (this);
		}

		/// <summary>
		/// Native-функция?
		/// </summary>
		public abstract bool IsNative { get; }

		/// <summary>
		/// Кол-во параметров ожидаемых функцией по умолчанию
		/// </summary>
		public abstract int ParameterCount { get; }
	}
}
