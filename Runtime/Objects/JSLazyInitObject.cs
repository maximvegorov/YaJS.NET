using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace YaJS.Runtime.Objects {
	/// <summary>
	/// Объект с отложенной инициализацией
	/// </summary>
	public abstract class JSLazyInitObject : JSObject {
		private readonly Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> _lazyMemberFactories;

		protected JSLazyInitObject(
			VirtualMachine vm,
			Dictionary<string, Func<VirtualMachine, JSObject, JSValue>> lazyMemberFactories,
			JSObject inherited)
			: base(vm, inherited) {
			Contract.Requires(lazyMemberFactories != null);
			_lazyMemberFactories = lazyMemberFactories;
		}

		protected override IEnumerable<string> GetEnumerableMembers() {
			return (OwnMembers.Keys.Concat(_lazyMemberFactories.Keys));
		}

		public override bool ContainsMember(JSValue member) {
			var name = member.CastToString();
			return (ContainsMember(name) || _lazyMemberFactories.ContainsKey(name));
		}

		public override JSValue GetMember(JSValue member) {
			var name = member.CastToString();
			var result = base.GetMember(name);
			if (result == null) {
				Func<VirtualMachine, JSObject, JSValue> factory;
				if (_lazyMemberFactories.TryGetValue(name, out factory)) {
					result = factory(VM, this);
					OwnMembers.Add(name, result);
				}
			}
			return (result ?? Undefined);
		}

		public override bool DeleteMember(JSValue member) {
			var name = member.CastToString();
			if (_lazyMemberFactories.ContainsKey(name))
				return (false);
			return (base.DeleteMember(name));
		}
	}
}
