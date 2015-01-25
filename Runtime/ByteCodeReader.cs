using System;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime.Exceptions;

namespace YaJS.Runtime {
	/// <summary>
	/// Reader byte-кода
	/// </summary>
	internal struct ByteCodeReader {
		private readonly byte[] _compiledCode;
		private int _offset;

		public ByteCodeReader(byte[] compiledCode) {
			Contract.Requires(compiledCode != null && compiledCode.Length > 0);
			_compiledCode = compiledCode;
			_offset = 0;
		}

		public OpCode ReadOpCode() {
			if (_offset >= _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			return ((OpCode)_compiledCode[_offset++]);
		}

		public bool ReadBoolean() {
			if (_offset >= _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = BitConverter.ToBoolean(_compiledCode, _offset);
			_offset += sizeof (bool);
			return (result);
		}

		public int ReadInteger() {
			if (_offset + sizeof (int) > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = BitConverter.ToInt32(_compiledCode, _offset);
			_offset += sizeof (int);
			return (result);
		}

		public double ReadFloat() {
			if (_offset + sizeof (double) > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = BitConverter.ToDouble(_compiledCode, _offset);
			_offset += sizeof (double);
			return (result);
		}

		public string ReadString() {
			if (_offset + sizeof (int) > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var length = BitConverter.ToInt32(_compiledCode, _offset);
			if (length < 0)
				throw new NegativeStringConstLengthException();
			if (length == 0)
				return (string.Empty);
			if (_offset + length > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = Encoding.UTF8.GetString(_compiledCode, _offset, length);
			_offset += length;
			return (result);
		}

		public void Seek(int offset) {
			var newOffset = _offset + offset;
			// При переходе вперед должен быть доступен хотя бы один байт (код инструкции) 
			if (newOffset < 0 || newOffset >= _compiledCode.Length - 1)
				throw new InvalidGotoOffsetException();
			_offset = newOffset;
		}

		public int Offset {
			get { return (_offset); }
		}
	}
}
