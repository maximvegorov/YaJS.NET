using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using YaJS.Runtime.Constructors;
using YaJS.Runtime.Constructors.Errors;
using YaJS.Runtime.Objects;
using YaJS.Runtime.Objects.Errors;
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

			// Так как существуют циклические зависимости между прототипами встроенных объектов и встроенными методами
			// создание и инициализация разделены между собой

			// Создать прототипы встроенных объектов
			Object = new JSObject(this);
			Boolean = new JSObject(this, Object);
			Number = new JSObject(this, Object);
			String = new JSObject(this, Object);
			Array = new JSObject(this, Object);
			Function = new JSObject(this, Object);

			Error = new JSObject(this, Object);
			InternalError = new JSObject(this, Error);
			ReferenceError = new JSObject(this, Error);
			SyntaxError = new JSObject(this, Error);
			TypeError = new JSObject(this, Error);
			RangeError = new JSObject(this, Error);

			// Инициализировать прототипы встроенных объектов (все конструкторы являются функциями в JS)
			JSObjectConstructor.InitPrototype(Object, Function);
			JSNumberConstructor.InitPrototype(Number, Function);
			JSStringConstructor.InitPrototype(String, Function);
			JSArrayConstructor.InitPrototype(Array, Function);
			JSFunctionConstructor.InitPrototype(Function);

			JSErrorConstructor.InitPrototype(Error, Function);

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

		public ExecutionThread NewThread(CompiledFunction globalFunction) {
			Contract.Requires<ArgumentNullException>(globalFunction != null, "globalFunction");
			return (new ExecutionThread(this, globalFunction));
		}

		public JSValue Execute(CompiledFunction globalFunction) {
			var thread = NewThread(globalFunction);
			while (thread.ExecuteStep()) {
			}
			return (thread.Result);
		}

		public JSObject NewObject() {
			return (new JSObject(this, Object));
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

		public JSFunction NewFunction(LocalScope outerScope, CompiledFunction compiledFunction) {
			Contract.Requires<ArgumentNullException>(outerScope != null, "outerScope");
			Contract.Requires<ArgumentNullException>(compiledFunction != null, "compiledFunction");
			return (new JSManagedFunction(this, outerScope, compiledFunction, Function));
		}

		public JSError NewError(string message) {
			return (new JSError(this, message, Error));
		}

		internal JSError NewInternalError(string message, string stackTrace) {
			return (new JSInternalError(this, message, stackTrace, InternalError));
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

		/// <summary>
		/// Компилятор
		/// </summary>
		public ICompilerServices Compiler { get; private set; }

		/// <summary>
		/// Глобальный объект
		/// </summary>
		public JSObject GlobalObject { get; private set; }

		/// <summary>
		/// Прототипы встроенных объектов
		/// </summary>
		public JSObject Object { get; private set; }

		public JSObject Boolean { get; private set; }
		public JSObject Number { get; private set; }
		public JSObject String { get; private set; }
		public JSObject Array { get; private set; }
		public JSObject Function { get; private set; }
		internal JSObject Error { get; private set; }
		internal JSObject InternalError { get; private set; }
		internal JSObject ReferenceError { get; private set; }
		internal JSObject SyntaxError { get; private set; }
		internal JSObject TypeError { get; private set; }
		internal JSObject RangeError { get; private set; }
	}
}
