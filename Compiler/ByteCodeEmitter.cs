using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace YaJS.Runtime {
	/// <summary>
	/// Emitter byte-кода
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	internal struct ByteCodeEmitter {
		private Stream _output;

		public ByteCodeEmitter(Stream output) {
			Contract.Requires(output != null);
			_output = output;
		}

		public void Emit(OpCode code) {
			_output.WriteByte((byte)code);
		}

		private void Write(byte[] bytes) {
			Contract.Requires(bytes != null && bytes.Length > 0);
			_output.Write(bytes, 0, bytes.Length);
		}

		public void Emit(OpCode code, bool op) {
			_output.WriteByte((byte)code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, int op) {
			_output.WriteByte((byte)code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, double op) {
			_output.WriteByte((byte)code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, string op) {
			_output.WriteByte((byte)code);
			if (string.IsNullOrEmpty(op)) {
				Write(BitConverter.GetBytes(0));
			}
			else {
				var bytes = Encoding.UTF8.GetBytes(op);
				Write(BitConverter.GetBytes(bytes.Length));
				Write(bytes);
			}
		}
	}
}
