using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime {
	using Runtime.Constructors;
	using Runtime.Objects;
	using Runtime.Objects.Errors;

	/// <summary>
	/// Виртуальная машина
	/// </summary>
	public sealed class VirtualMachine {
		public VirtualMachine(ICompilerServices compiler) {
			Contract.Requires<ArgumentNullException>(compiler != null, "compiler");

			Compiler = compiler;

			// Создать глобальный объект
			Global = new JSObject();

			// Так как существуют циклические зависимости между прототипами встроенных объектов и встроенными методами
			// создание и инициализация разделены между собой

			// Создать прототипы встроенных объектов
			Object = new JSObject();
			Boolean = new JSObject(Object);
			Number = new JSObject(Object);
			String = new JSObject(Object);
			Array = new JSObject(Object);
			Function = new JSObject(Object);

			Error = new JSObject(Object);
			InternalError = new JSObject(Error);
			ReferenceError = new JSObject(Error);
			SyntaxError = new JSObject(Error);
			TypeError = new JSObject(Error);

			// Инициализировать прототипы встроенных объектов (все конструкторы являются функциями в JS)
			JSObjectConstructor.InitPrototype(Object, Function);
			JSNumberConstructor.InitPrototype(Number, Function);
			JSStringConstructor.InitPrototype(String, Function);
			JSArrayConstructor.InitPrototype(Array, Function);
			JSFunctionConstructor.InitPrototype(Function);

			JSErrorConstructor.InitPrototype(Error, Function);

			// Инициализировать глобальный объект
			Global.OwnMembers.Add("Object", new JSObjectConstructor(Function));
			Global.OwnMembers.Add("Boolean", new JSBooleanConstructor(Function));
			Global.OwnMembers.Add("Number", new JSNumberConstructor(Function));
			Global.OwnMembers.Add("String", new JSStringConstructor(Function));
			Global.OwnMembers.Add("Array", new JSArrayConstructor(Function));
			Global.OwnMembers.Add("Function", new JSFunctionConstructor(Function));

			Global.OwnMembers.Add("Error", new JSErrorConstructor(Function));
		}

		public ExecutionThread NewThread(CompiledFunction mainFunction) {
			Contract.Requires<ArgumentNullException>(mainFunction != null, "mainFunction");
			return (new ExecutionThread(this, mainFunction));
		}

		public JSObject NewObject() {
			return (new JSObject(Object));
		}

		public JSBoolean NewBoolean(bool value) {
			return (new JSBoolean(value, Boolean));
		}

		public JSNumber NewNumber(double value) {
			return (new JSNumber(value, Number));
		}

		public JSString NewString(string value) {
			return (new JSString(value, String));
		}

		public JSArray NewArray(List<JSValue> items) {
			Contract.Requires<ArgumentNullException>(items != null, "function");
			return (new JSArray(items, Array));
		}

		public JSFunction NewFunction(CompiledFunction compiledFunction, LocalScope outerScope) {
			Contract.Requires<ArgumentNullException>(compiledFunction != null, "compiledFunction");
			return (new JSManagedFunction(compiledFunction, outerScope, Function));
		}

		public JSError NewError(string message) {
			return (new JSError(message, Error));
		}

		internal JSError NewInternalError(string message, string stackTrace) {
			return (new JSInternalError(message, stackTrace, InternalError));
		}

		public JSError NewReferenceError(string message) {
			return (new JSReferenceError(message, ReferenceError));
		}

		public JSError NewSyntaxError(string message) {
			return (new JSSyntaxError(message, SyntaxError));
		}

		public JSError NewTypeError(string message) {
			return (new JSTypeError(message, TypeError));
		}

		/// <summary>
		/// Компилятор
		/// </summary>
		public ICompilerServices Compiler { get; private set; }

		/// <summary>
		/// Глобальный объект
		/// </summary>
		public JSObject Global { get; private set; }

		/// <summary>
		/// Прототипы встроенных объектов
		/// </summary>
		public JSObject Object { get; private set; }
		public JSObject Boolean { get; private set; }
		public JSObject Number { get; private set; }
		public JSObject String { get; private set; }
		public JSObject Array { get; private set; }
		public JSObject Function { get; private set; }

		public JSObject Error { get; private set; }
		public JSObject InternalError { get; private set; }
		public JSObject ReferenceError { get; private set; }
		public JSObject SyntaxError { get; private set; }
		public JSObject TypeError { get; private set; }
	}
}
