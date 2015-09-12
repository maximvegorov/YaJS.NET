using System;
using System.Diagnostics.Contracts;
using System.Text;
using YaJS.Runtime.Exceptions;

namespace YaJS.Runtime {
	/// <summary>
	/// Reader byte-кода
	/// </summary>
	internal sealed class ByteCodeReader {
		private readonly byte[] _compiledCode;

		public ByteCodeReader(byte[] compiledCode) {
			Contract.Requires(compiledCode != null && compiledCode.Length > 0);
			_compiledCode = compiledCode;
			Offset = 0;
		}

		public OpCode ReadOpCode() {
			if (Offset >= _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			return ((OpCode)_compiledCode[Offset++]);
		}

		public bool ReadBoolean() {
			if (Offset >= _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = _compiledCode[Offset] != 0;
			Offset += sizeof (byte);
			return (result);
		}

		public int ReadInteger() {
			if (Offset + sizeof (int) > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = BitConverter.ToInt32(_compiledCode, Offset);
			Offset += sizeof (int);
			return (result);
		}

		public double ReadFloat() {
			if (Offset + sizeof (double) > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = BitConverter.ToDouble(_compiledCode, Offset);
			Offset += sizeof (double);
			return (result);
		}

		public string ReadString() {
			if (Offset + sizeof (int) > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var length = BitConverter.ToInt32(_compiledCode, Offset);
			Offset += sizeof (int);
			if (length < 0)
				throw new NegativeStringConstLengthException();
			if (length == 0)
				return (string.Empty);
			if (Offset + length > _compiledCode.Length)
				throw new UnexpectedEndOfCodeException();
			var result = Encoding.UTF8.GetString(_compiledCode, Offset, length);
			Offset += length;
			return (result);
		}

		public void Seek(int newOffset) {
			// При переходе вперед должен быть доступен хотя бы один байт (код инструкции) 
			if (newOffset < 0 || newOffset >= _compiledCode.Length - 1)
				throw new InvalidGotoOffsetException();
			Offset = newOffset;
		}

		public int Offset { get; private set; }
	}
}
