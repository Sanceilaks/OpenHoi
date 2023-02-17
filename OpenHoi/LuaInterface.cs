namespace OpenHoi.LuaInterface;

using NLua;

public static class LuaInterface 
{
	public static Lua CreateState(bool loadClr = true) 
	{
		var state = new Lua();
		if (loadClr)
			state.LoadCLRPackage();
		return state;
	} 
}