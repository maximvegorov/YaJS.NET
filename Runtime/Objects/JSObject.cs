using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект
	/// </summary>
	public class JSObject : JSValue, IEnumerable<JSValue> {
		private readonly JSObject _inherited;
		private Dictionary<string, JSValue> _ownMembers;

		public JSObject(VirtualMachine vm, JSObject inherited = null)
			: base(JSValueType.Object) {
			Contract.Requires(vm != null);
			VM = vm;
			_inherited = inherited;
		}

		public override string ToString() {
			if (_ownMembers == null)
				return ("{}");
			var result = new StringBuilder();
			result.Append('{');
			foreach (var member in _ownMembers)
				result.Append('"').Append(member.Key.Replace("\"", "\\\"")).Append('"').Append(':').Append(member.Value).Append(',');
			if (result.Length > 1)
				result.Length -= 2;
			result.Append('}');
			return (result.ToString());
		}

		protected virtual IEnumerable<string> GetEnumerableMembers() {
			return (_ownMembers != null ? _ownMembers.Keys : Enumerable.Empty<string>());
		}

		protected bool ContainsMember(string member) {
			Contract.Requires(member != null);
			for (var current = this; current != null; current = current._inherited) {
				if (current._ownMembers != null && current._ownMembers.ContainsKey(member))
					return (true);
			}
			return (false);
		}

		public virtual bool ContainsMember(JSValue member) {
			Contract.Requires(member != null);
			return (ContainsMember(member.CastToString()));
		}

		protected internal JSValue GetMember(string member) {
			Contract.Requires(member != null);
			for (var current = this; current != null; current = current._inherited) {
				JSValue result;
				if (current._ownMembers != null && current._ownMembers.TryGetValue(member, out result))
					return (result);
			}
			return (null);
		}

		public virtual JSValue GetMember(JSValue member) {
			Contract.Requires(member != null);
			return (GetMember(member.CastToString()) ?? Undefined);
		}

		protected internal void SetMember(string member, JSValue value) {
			Contract.Requires(member != null);
			Contract.Requires(value != null);
			OwnMembers[member] = value;
		}

		public virtual void SetMember(JSValue member, JSValue value) {
			Contract.Requires(member != null);
			Contract.Requires(value != null);
			SetMember(member.CastToString(), value);
		}

		protected internal bool DeleteMember(string member) {
			Contract.Requires(member != null);
			if (_ownMembers != null)
				_ownMembers.Remove(member);
			return (true);
		}

		public virtual bool DeleteMember(JSValue member) {
			Contract.Requires(member != null);
			return (DeleteMember(member.CastToString()));
		}

		public override bool StrictEqualsTo(JSValue value) {
			return (ReferenceEquals(this, value));
		}

		public override bool IsInstanceOf(JSFunction constructor) {
			var prototype = constructor.GetPrototype();
			Contract.Assert(prototype != null);
			for (var current = _inherited; current != null; current = current._inherited) {
				if (ReferenceEquals(current, prototype))
					return (true);
			}
			return (false);
		}

		public override string TypeOf() {
			return ("object");
		}

		public override bool CastToBoolean() {
			return (true);
		}

		public override JSObject RequireObject() {
			return (this);
		}

		public override void CastToPrimitiveValue(ExecutionThread thread, Action<JSValue> onCompleteCallback) {
			var valueOf = GetMember("valueOf");
			if (valueOf == null) {
				onCompleteCallback(this);
				return;
			}
			thread.CallFunction(
				valueOf.RequireFunction(),
				this,
				JSFunction.EmptyArgumentList,
				true,
				() => onCompleteCallback(thread.CurrentFrame.Pop()));
		}

		public override JSObject ToObject(VirtualMachine vm) {
			return (this);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return (GetEnumerator());
		}

		public override IEnumerator<JSValue> GetEnumerator() {
			if (_ownMembers == null)
				yield break;
			var processed = new HashSet<string>();
			for (var current = this; current != null; current = current.Inherited) {
				foreach (var member in current.GetEnumerableMembers()) {
					if (processed.Contains(member))
						continue;
					processed.Add(member);
					yield return member;
				}
			}
		}

		/// <summary>
		/// Виртуальная машина к которой относится объект
		/// </summary>
		protected VirtualMachine VM { get; private set; }

		/// <summary>
		/// Прототип объекта
		/// </summary>
		public JSObject Inherited { get { return (_inherited); } }

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
