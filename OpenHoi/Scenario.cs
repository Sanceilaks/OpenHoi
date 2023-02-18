using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenHoi;

public class Scenario 
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Author { get; set; }
	public string? StartsAt { get; set; }

	public Scenario(NLua.LuaTable? table)
	{
		Name = table!["name"].ToString();
		Description = table!["description"].ToString();
		Author = table!["author"].ToString();
		StartsAt = table!["starts_at"].ToString();
	}

	public static Scenario Load(string directory)
	{
		if (!Directory.Exists(directory))
			throw new DirectoryNotFoundException($"Cannot find {directory}");

		if (!File.Exists(Path.Combine(directory, "scenario.lua")))
			throw new FileNotFoundException($"Cannot find scenario.lua in {directory}");

		var state = LuaInterface.LuaInterface.CreateState(false);
		var result = state.DoFile(Path.Combine(directory, "scenario.lua"));

		try {
			var scenario = result[0] as NLua.LuaTable;
			return new Scenario(scenario);
		} catch (Exception e) {
			throw new ArgumentException($"Failed to load scenario from {directory}", e);
		}
	}
}