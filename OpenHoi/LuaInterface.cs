namespace OpenHoi.LuaInterface;

using System.Diagnostics;
using NLua;

public static class LuaInterface 
{
	public static Lua CreateState(bool loadClr = true) 
	{
		var state = new Lua();
		if (loadClr)
			state.LoadCLRPackage();
		state.DoString($@"
			package.path = package.path .. [[;{System.IO.Path.GetFullPath("Game/scripts/modules")}\?.lua]]
		");
		return state;
	} 
}