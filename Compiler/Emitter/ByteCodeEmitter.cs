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
		private int _offset;

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
			if (_buffer.Length == _offset)
				Grow(sizeof (byte));
			_buffer[_offset] = (byte)code;
			_offset++;
		}

		private void Write(bool op) {
			if (_buffer.Length == _offset)
				Grow(sizeof (byte));
			_buffer[_offset] = (byte)(op ? 1 : 0);
			_offset++;
		}

		public void Emit(OpCode code) {
			Write(code);
		}

		private void Write(byte[] bytes) {
			Contract.Requires(bytes != null && bytes.Length > 0);
			if (_buffer.Length < _offset + bytes.Length)
				Grow(bytes.Length);
			Buffer.BlockCopy(bytes, 0, _buffer, _offset, bytes.Length);
			_offset += bytes.Length;
		}

		public void Emit(OpCode code, bool op) {
			Write(code);
			Write(op);
		}

		public void Emit(OpCode code, int op) {
			Write(code);
			Write(BitConverter.GetBytes(op));
		}

		public void Emit(OpCode code, double op) {
			Write(code);
			Write(BitConverter.GetBytes(op));
		}

		private void Write(string op) {
			if (string.IsNullOrEmpty(op))
				Write(BitConverter.GetBytes(0));
			else {
				var bytes = Encoding.UTF8.GetBytes(op);
				Write(BitConverter.GetBytes(bytes.Length));
				Write(bytes);
			}
		}

		public void Emit(OpCode code, string op) {
			Write(code);
			Write(op);
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
				label.AppendUnresolved(_offset);
				Write(InvalidOffset);
			}
		}

		public void MarkLabel(Label label) {
			Contract.Requires(label != null);
			label.Resolve(_offset);
		}

		public byte[] ToCompiledCode() {
			if (_buffer.Length == _offset)
				return (_buffer);
			var result = new byte[_offset];
			Buffer.BlockCopy(_buffer, 0, result, 0, _offset);
			return (result);
		}

		public byte[] RawBuffer { get { return (_buffer); } }
		public int Offset { get { return (_offset); } }
	}
}
