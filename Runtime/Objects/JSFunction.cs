using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Базовый класс для всех функций
	/// </summary>
	public abstract class JSFunction : JSObject {
		public const string PrototypeMemberName = "prototype";

		protected JSFunction(JSObject inherited)
			: base(inherited) {
			Contract.Requires(inherited != null);
		}

		public virtual JSObject GetPrototype(VirtualMachine vm) {
			Contract.Requires(vm != null);
			JSValue result;
			if (!OwnMembers.TryGetValue(PrototypeMemberName, out result)) {
				result = new JSObject(vm.Object);
				OwnMembers.Add(PrototypeMemberName, result);
			}
			return (result.GetAsObject());
		}

		/// <summary>
		/// Вызвать Native-функцию
		/// </summary>
		/// <param name="vm">Виртуальная машина</param>
		/// <param name="context">Контекст. В случае если функция вызывается как конструктор, то null</param>
		/// <param name="outerScope">Внешнняя область локальных переменных</param>
		/// <param name="args">Список параметров</param>
		/// <returns></returns>
		public virtual JSValue Invoke(
			VirtualMachine vm, JSObject context, LocalScope outerScope, List<JSValue> args
		) {
			Contract.Requires(vm != null);
			Contract.Requires(context != null);
			Contract.Requires(outerScope != null);
			Contract.Requires(args != null);
			throw new NotSupportedException();
		}

		/// <summary>
		/// Native-функция?
		/// </summary>
		public abstract bool IsNative { get; }
	}
}
