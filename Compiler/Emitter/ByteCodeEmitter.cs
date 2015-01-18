using System;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime;

namespace YaJS.Compiler.Emitter {
	/// <summary>
	/// Emitter byte-кода
	/// </summary>
	internal sealed class ByteCodeEmitter {
		private const int DefaultCapacity = 16;
		private static readonly byte[] InvalidOffset = BitConverter.GetBytes(-1);
		private byte[] _buffer = new byte[DefaultCapacity];
		private int _byteLength;

		private void Grow(int minDelta) {
			Contract.Requires(minDelta > 0);
			int delta;
			if (_buffer.Length > 1024)
				delta = _buffer.Length / 4;
			else
				delta = _buffer.Length;
			var buffer = new byte[_buffer.Length + Math.Max(minDelta, delta)];
			Buffer.BlockCopy(_buffer, 0, buffer, 0, _buffer.Length);
			_buffer = buffer;
		}

		private void Write(OpCode code) {
			if (_buffer.Length == _byteLength)
				Grow(sizeof (byte));
			_buffer[_byteLength] = (byte)code;
			_byteLength++;
		}

		public void Emit(OpCode code) {
			Write(code);
		}

		private void Write(byte[] bytes) {
			Contract.Requires(bytes != null && bytes.Length > 0);
			if (_buffer.Length < _byteLength + bytes.Length)
				Grow(bytes.Length);
			Buffer.BlockCopy(bytes, 0, _buffer, _byteLength, bytes.Length);
			_byteLength += bytes.Length;
		}

		public void Emit(OpCode code, bool op) {
			Write(code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, int op) {
			Write(code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, double op) {
			Write(code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, string op) {
			Write(code);
			if (string.IsNullOrEmpty(op))
				Write(BitConverter.GetBytes(0));
			else {
				var bytes = Encoding.UTF8.GetBytes(op);
				Write(BitConverter.GetBytes(bytes.Length));
				Write(bytes);
			}
		}

		public Label DefineLabel() {
			return (new Label(this));
		}

		public void Emit(OpCode op, Label label) {
			Contract.Requires(label != null);
			if (label.Offset.HasValue)
				Emit(op, label.Offset.Value);
			else {
				Emit(op);
				label.AppendUnresolved(_byteLength);
				Write(InvalidOffset);
			}
		}

		public void MarkLabel(Label label) {
			Contract.Requires(label != null);
			label.Resolve(_byteLength);
		}

		public byte[] ToCompiledCode() {
			if (_buffer.Length == _byteLength)
				return (_buffer);
			var result = new byte[_byteLength];
			Buffer.BlockCopy(_buffer, 0, result, 0, _byteLength);
			return (result);
		}

		public byte[] RawBuffer { get { return (_buffer); } }
	}
}
