namespace YaJS.Runtime {
	using Runtime.Enumerators;
	using Runtime.Exceptions;

	/// <summary>
	/// Перечислитель. Используется для реализации for-in
	/// </summary>
	public abstract class JSEnumerator : JSValue {
		public static readonly JSEnumerator Empty = new JSEmptyEnumerator();

		protected JSEnumerator() : base(JSValueType.Enumerator) {
		}

		public abstract bool MoveNext();
		public abstract JSValue Current { get; }

		public override JSEnumerator GetEnumerator() {
			throw new TypeErrorException();
		}

		public override string TypeOf() {
			throw new TypeErrorException();
		}

		public override JSEnumerator RequireEnumerator() {
			return (this);
		}

		public override JSValue ToPrimitiveValue() {
			throw new TypeErrorException();
		}
	}
}
