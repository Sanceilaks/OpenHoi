using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenHoi;

public class Modification
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Author { get; set; }
	public string? StartsAt { get; set; }

	public Modification(NLua.LuaTable? table)
	{
		Name = table!["name"].ToString();
		Description = table!["description"].ToString();
		Author = table!["author"].ToString();
		StartsAt = table!["starts_at"].ToString();
	}

	public static Modification Load(string directory)
	{
		if (!Directory.Exists(directory))
			throw new DirectoryNotFoundException($"Cannot find {directory}");

		if (!File.Exists(Path.Combine(directory, "mod.lua")))
			throw new FileNotFoundException($"Cannot find mod.lua in {directory}");

		var state = LuaInterface.LuaInterface.CreateState(false);
		var result = state.DoFile(Path.Combine(directory, "mod.lua"));

		try {
			var modification = result[0] as NLua.LuaTable;
			return new Modification(modification);
		} catch (Exception e) {
			throw new ArgumentException($"Failed to load scenario from {directory}", e);
		}
	}
}