namespace OpenHoi.LuaInterface;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NLua;

public class LuaModule 
{
	public string? Path { get; set; }
	public Lua? Lua { get; private set; }
	public bool ShouldReload { get; set; } = false;
	public event FileSystemEventHandler? ModuleChanged;
	private List<object>? _moduleResults = new List<object>();
	public List<object> ModuleResult { 
		get 
		{
			if (AutoReload && ShouldReload)
			{
				_moduleResults = Load(true)!.ToList();
				ShouldReload = false;
				Debug.WriteLine($"Reloaded \"{Path}\"", "OpenHoi[LUA]");
			}
			return _moduleResults!;
		} 
	}
	public bool AutoReload {get; set;} = false;
	public bool IgnoreErrors { get; set; } = false;
	public bool PrintErrors { get; set; } = true;
	private FileSystemWatcher? _watcher;

	public LuaModule(Lua lua, string path, bool load = false, bool autoReload = false, 
		bool ignoreLuaError = false, bool printLuaError = false)
	{
		if (!File.Exists(path))
			throw new FileNotFoundException($"Module \"{path}\" not found");
		Path = path;
		Lua = lua;
		AutoReload = autoReload;
		IgnoreErrors = ignoreLuaError;
		PrintErrors = printLuaError;
		CreateWatcher();

		if (load)
			Load(true);
	}

	public LuaModule(string path, bool load = false, bool autoReload = false) 
		: this(LuaInterface.CreateState(), path, load, autoReload)
	{
	}

	private void CreateWatcher()
	{
		_watcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(Path)!, "*.lua");
		_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
		_watcher.Changed += (sender, args) => ShouldReload = true;
		_watcher.Changed += ModuleChanged;

		_watcher.EnableRaisingEvents = true;
	}

	public object[]? Load(bool save = false)
	{
		try 
		{
			var result = Lua!.DoFile(Path);
			if (save)
				_moduleResults = result.ToList();
			return result;
		}
		catch (NLua.Exceptions.LuaScriptException ex)
		{
			if (PrintErrors)
				LuaInterface.ErrorMessage(ex.Message, Path);
			if (!IgnoreErrors)
				throw ex;
			else
				_moduleResults = null;
			return null;
		}
	}

	public object[]? CallModule(params object[] args)
	{
		if (ModuleResult != null && ModuleResult.Count > 0 && ModuleResult[0] is NLua.LuaFunction)
		{
			var function = ModuleResult[0] as NLua.LuaFunction;
			return LuaInterface.CallFunction(Path!, function!, args);
		}
		return null;
	}
}