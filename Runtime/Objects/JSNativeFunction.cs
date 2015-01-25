﻿namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Является базовым классом для всех native-функций
	/// </summary>
	public abstract class JSNativeFunction : JSFunction {
		protected JSNativeFunction(VirtualMachine vm, JSObject inherited)
			: base(vm, inherited) {
		}

		/// <summary>
		/// Native-функция?
		/// </summary>
		public override bool IsNative {
			get { return (true); }
		}
	}
}
