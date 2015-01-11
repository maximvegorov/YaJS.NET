using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace YaJS.Runtime.Objects {
	using Runtime.Exceptions;

	/// <summary>
	/// Объект
	/// </summary>
	public class JSObject : JSValue {
		private Dictionary<string, JSValue> _ownMembers;

		public JSObject(VirtualMachine vm, JSObject inherited = null)
			: base(JSValueType.Object) {
			Contract.Requires(vm != null);
			VM = vm;
			Inherited = inherited;
		}

		public virtual bool ContainsMember(JSValue member) {
			throw new TypeErrorException();
		}

		public virtual JSValue GetMember(JSValue member) {
			throw new TypeErrorException();
		}

		public virtual void SetMember(JSValue member, JSValue value) {
			throw new TypeErrorException();
		}

		public virtual bool DeleteMember(JSValue member) {
			throw new TypeErrorException();
		}

		public override string TypeOf() {
			return ("object");
		}

		/// <summary>
		/// Виртуальная машина к которой относится объект
		/// </summary>
		protected VirtualMachine VM { get; private set; }
		/// <summary>
		/// Прототип объекта
		/// </summary>
		protected JSObject Inherited { get; private set; }
		/// <summary>
		/// Коллекция собственных свойств
		/// </summary>
		public Dictionary<string, JSValue> OwnMembers {
			get {
				if (_ownMembers == null)
					_ownMembers = new Dictionary<string, JSValue>();
				return (_ownMembers);
			}
		}
	}
}
