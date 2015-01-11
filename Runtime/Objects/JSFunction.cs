using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Базовый класс для всех функций
	/// </summary>
	public abstract class JSFunction : JSObject {
		private const string PrototypeMemberName = "prototype";

		protected JSFunction(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(inherited != null);
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
		/// <param name="outerScope">Внешняя область локальных переменных</param>
		/// <param name="args">Список параметров</param>
		/// <returns></returns>
		public virtual JSValue Construct(LocalScope outerScope, List<JSValue> args) {
			Contract.Requires(outerScope != null);
			Contract.Requires(args != null);
			throw new NotSupportedException();
		}

		/// <summary>
		/// Вызвать Native-функцию
		/// </summary>
		/// <param name="context">Контекст</param>
		/// <param name="outerScope">Внешняя область локальных переменных</param>
		/// <param name="args">Список параметров</param>
		/// <returns></returns>
		public virtual JSValue Invoke(JSObject context, LocalScope outerScope, List<JSValue> args) {
			Contract.Requires(context != null);
			Contract.Requires(outerScope != null);
			Contract.Requires(args != null);
			throw new NotSupportedException();
		}

		public override string TypeOf() {
			return ("function");
		}

		/// <summary>
		/// Native-функция?
		/// </summary>
		public abstract bool IsNative { get; }
	}
}
