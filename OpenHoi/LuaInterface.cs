namespace OpenHoi.LuaInterface;

using System.Diagnostics;
using System.Reflection;
using NLua;

public static class LuaInterface 
{
	private static void Print(string message)
	{
		Debug.WriteLine(message);
	}

	public static Lua CreateState(bool loadClr = true) 
	{
		var state = new Lua();
		if (loadClr)
		{
			state.LoadCLRPackage();
		}

		state.DoString($@"
			package.path = package.path .. [[;{System.IO.Path.GetFullPath("Game/scripts/modules")}\?.lua]]
		");

		state.DoString(@"
			print = function(data)
				__print_impl(tostring(data))
			end
		");
		state.RegisterFunction("__print_impl", typeof(LuaInterface).GetMethod("Print", BindingFlags.Static | BindingFlags.NonPublic));
		
		return state;
	}

	public static void ErrorMessage(string message, string? file = null)
	{
		if (file != null)
			message += $" in \"{file}\"";
		Debug.WriteLine(message, "OpenHoi[LUA]");
	}
}