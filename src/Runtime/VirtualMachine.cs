using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Constructors;
using YaJS.Runtime.Constructors.Errors;
using YaJS.Runtime.Objects;
using YaJS.Runtime.Objects.Errors;
using YaJS.Runtime.Objects.Prototypes;
using YaJS.Runtime.Scopes;
using YaJS.Runtime.Values;

namespace YaJS.Runtime {
	/// <summary>
	/// Виртуальная машина
	/// </summary>
	public sealed class VirtualMachine {
		public VirtualMachine(ICompilerServices compiler) {
			Contract.Requires<ArgumentNullException>(compiler != null, "compiler");

			Compiler = compiler;

			// Создать глобальный объект
			GlobalObject = new JSObject(this);

			// Создать прототипы встроенных объектов
			Object = new JSObjectPrototype(this);
			Boolean = new JSBooleanPrototype(this, Object);
			Number = new JSNumberPrototype(this, Object);
			String = new JSStringPrototype(this, Object);
			Array = new JSArrayPrototype(this, Object);
			Function = new JSFunctionPrototype(this, Object);

			Error = new JSUnenumerableObject(this, Object);
			ReferenceError = new JSUnenumerableObject(this, Error);
			SyntaxError = new JSUnenumerableObject(this, Error);
			TypeError = new JSUnenumerableObject(this, Error);
			RangeError = new JSUnenumerableObject(this, Error);
            InternalError = new JSUnenumerableObject(this, Error);

            // Инициализировать глобальный объект
            GlobalObject.OwnMembers.Add("Object", new JSObjectConstructor(this, Function));
			GlobalObject.OwnMembers.Add("Boolean", new JSBooleanConstructor(this, Function));
			GlobalObject.OwnMembers.Add("Number", new JSNumberConstructor(this, Function));
			GlobalObject.OwnMembers.Add("String", new JSStringConstructor(this, Function));
			GlobalObject.OwnMembers.Add("Array", new JSArrayConstructor(this, Function));
			GlobalObject.OwnMembers.Add("Function", new JSFunctionConstructor(this, Function));

			GlobalObject.OwnMembers.Add("Error", new JSErrorConstructor(this, Function));
			GlobalObject.OwnMembers.Add("InternalError", new JSInternalErrorConstructor(this, Function));
			GlobalObject.OwnMembers.Add("ReferenceError", new JSReferenceErrorConstructor(this, Function));
			GlobalObject.OwnMembers.Add("SyntaxError", new JSSyntaxErrorConstructor(this, Function));
			GlobalObject.OwnMembers.Add("TypeError", new JSTypeErrorConstructor(this, Function));
			GlobalObject.OwnMembers.Add("RangeError", new JSRangeErrorConstructor(this, Function));
		}

		public ExecutionThread NewThread(CompiledFunction globalFunction, JSValue[] globalArguments = null) {
			Contract.Requires<ArgumentNullException>(globalFunction != null, "globalFunction");
			return (new ExecutionThread(this, globalFunction, globalArguments));
		}

		public JSValue Execute(CompiledFunction globalFunction, JSValue[] globalArguments = null) {
			var thread = NewThread(globalFunction, globalArguments);
			for (;;) {
				var isTerminated = thread.ExecuteStep();
				if (isTerminated)
					break;
			}
			return (thread.Result);
		}

		public JSObject NewObject() {
			return (new JSObject(this, Object));
		}

		public JSObject NewObject(JSObject inherited) {
			return (new JSObject(this, inherited));
		}

		public JSObject NewBoolean(bool value) {
			return (new JSBoolean(this, value, Boolean));
		}

		public JSObject NewNumber(JSNumberValue value) {
			return (new JSNumber(this, value, Number));
		}

		public JSObject NewString(string value) {
			return (new JSString(this, value, String));
		}

		public JSObject NewArray(List<JSValue> items) {
			Contract.Requires<ArgumentNullException>(items != null, "items");
			return (new JSArray(this, items, Array));
		}

		public JSFunction NewFunction(VariableScope outerScope, CompiledFunction compiledFunction) {
			Contract.Requires<ArgumentNullException>(outerScope != null, "outerScope");
			Contract.Requires<ArgumentNullException>(compiledFunction != null, "compiledFunction");
            if (string.IsNullOrEmpty(compiledFunction.Name))
			    return (new JSManagedFunction(this, outerScope, compiledFunction, Function));
            var functionScope = new NamedFunctionVariableScope(outerScope);
		    var function = new JSManagedFunction(this, functionScope, compiledFunction, Function);
            functionScope.Bind(function);
		    return (function);
		}

		internal JSArguments NewArguments(JSValue[] values) {
			return (new JSArguments(this, values, Object));
		}

		public JSError NewError(string message) {
			return (new JSError(this, message, Error));
		}

		public JSError NewReferenceError(string message) {
			return (new JSReferenceError(this, message, ReferenceError));
		}

		public JSError NewSyntaxError(string message) {
			return (new JSSyntaxError(this, message, SyntaxError));
		}

		public JSError NewTypeError(string message) {
			return (new JSTypeError(this, message, TypeError));
		}

		public JSError NewRangeError(string message) {
			return (new JSRangeError(this, message, RangeError));
		}

        internal JSError NewInternalError(string message, string stackTrace) {
            return (new JSInternalError(this, message, stackTrace, InternalError));
        }

        /// <summary>
        /// Компилятор
        /// </summary>
        public ICompilerServices Compiler { get; private set; }

		/// <summary>
		/// Глобальный объект
		/// </summary>
		public JSObject GlobalObject { get; }

		/// <summary>
		/// Прототипы встроенных объектов
		/// </summary>
		public JSObject Object { get; }

		public JSObject Boolean { get; }
		public JSObject Number { get; }
		public JSObject String { get; }
		public JSObject Array { get; }
		public JSObject Function { get; }

		internal JSObject Error { get; }
		internal JSObject ReferenceError { get; }
		internal JSObject SyntaxError { get; }
		internal JSObject TypeError { get; }
		internal JSObject RangeError { get; }
        internal JSObject InternalError { get; }
    }
}
