using System.Collections.Generic;

namespace YaJS.Runtime {
	/// <summary>
	/// Сервис компилятора. Используется для компиляции JS кода в byte-код на лету
	/// </summary>
	public interface ICompilerServices {
		CompiledFunction Compile(IEnumerable<string> argumentNames, string functionBody);
	}
}
