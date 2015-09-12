using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Objects {
	internal sealed class JSArguments : JSObject {
		private readonly JSValue[] _values;

		public JSArguments(VirtualMachine vm, JSValue[] values, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(values != null);
			Contract.Requires(inherited == vm.Object);
			_values = values;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			for (var i = 0; i < _values.Length; i++)
				result.Append(_values[i] ?? Undefined).Append(", ");
			if (result.Length > 1)
				result.Length -= 2;
			result.Append(']');
			return (result.ToString());
		}

		public override IEnumerator<JSValue> GetEnumerator() {
			for (var i = 0; i < _values.Length; i++)
				yield return (JSNumberValue)i;
		}

		public override bool ContainsMember(JSValue member) {
			if (member.Type == JSValueType.Integer) {
				var index = member.CastToInteger();
				if (index >= 0)
					return (index < _values.Length);
			}
			var name = member.CastToString();
			return (name == "length" || base.ContainsMember(name));
		}

		public override JSValue GetMember(JSValue member) {
			if (member.Type == JSValueType.Integer) {
				var index = member.CastToInteger();
				if (index >= 0)
					return (index < _values.Length ? _values[index] : Undefined);
			}
			var name = member.CastToString();
			if (name == "length")
				return ((JSNumberValue)_values.Length);
			return (base.GetMember(name) ?? Undefined);
		}

		public override void SetMember(JSValue member, JSValue value) {
			throw new TypeErrorException();
		}

		public override bool DeleteMember(JSValue member) {
			throw new TypeErrorException();
		}
	}
}
