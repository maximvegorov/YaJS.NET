using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime.Exceptions;
using YaJS.Runtime.Values;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Массив
	/// </summary>
	internal sealed class JSArray : JSObject {
		public JSArray(VirtualMachine vm, List<JSValue> items, JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(items != null);
			Contract.Requires(inherited == vm.Array);
			Items = items;
		}

		public override string ToString() {
			var result = new StringBuilder();
			result.Append('[');
			for (var i = 0; i < Items.Count; i++)
				result.Append(Items[i] ?? Undefined).Append(", ");
			if (result.Length > 1)
				result.Length -= 2;
			result.Append(']');
			return (result.ToString());
		}

		public override IEnumerator<JSValue> GetEnumerator() {
			for (var i = 0; i < Items.Count; i++) {
				var item = Items[i];
				if (item != null)
					yield return (JSNumberValue)i;
			}
			var remainMembers = base.GetEnumerator();
			while (remainMembers.MoveNext())
				yield return (remainMembers.Current);
		}

		public override bool ContainsMember(JSValue member) {
			if (member.Type == JSValueType.Integer) {
				var index = member.CastToInteger();
				if (index >= 0) {
					if (index >= Items.Count)
						return (false);
					return (Items[index] != null);
				}
			}
			var name = member.CastToString();
			return (name == "length" || base.ContainsMember(name));
		}

		public override JSValue GetMember(JSValue member) {
			if (member.Type == JSValueType.Integer) {
				var index = member.CastToInteger();
				if (index >= 0) {
					if (index >= Items.Count)
						return (JSValue.Undefined);
					return (Items[index] ?? Undefined);
				}
			}
			var name = member.CastToString();
			if (name == "length")
				return ((JSNumberValue)Items.Count);
			return (base.GetMember(name) ?? Undefined);
		}

		public override void SetMember(JSValue member, JSValue value) {
			if (member.Type == JSValueType.Integer) {
				var index = member.CastToInteger();
				if (0 <= index) {
					if (index < Items.Count)
						Items[index] = value;
					else {
						Items.Capacity = index + 1;
						for (var i = Items.Count; i < index; i++)
							Items.Add(null);
						Items.Add(value);
					}
					return;
				}
			}
			var name = member.CastToString();
			if (name == "length") {
				var newLength = value.CastToInteger();
				if (newLength < 0)
					throw new RangeErrorException("Invalid array length");
				Items.Capacity = newLength;
				for (var i = Items.Count; i < Items.Capacity; i++)
					Items.Add(null);
				return;
			}
			base.SetMember(name, value);
		}

		public override bool DeleteMember(JSValue member) {
			if (member.Type == JSValueType.Integer) {
				var index = member.CastToInteger();
				if (index >= 0) {
					if (index < Items.Count)
						Items[index] = null;
					return (true);
				}
			}
			var name = member.CastToString();
			return (name != "length" && base.DeleteMember(name));
		}

		/// <summary>
		/// Элементы массива
		/// </summary>
		public List<JSValue> Items { get; private set; }
	}
}
