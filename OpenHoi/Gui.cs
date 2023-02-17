using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace OpenHoi;

public class Gui
{
	private OpenHoi _game;
	private NLua.Lua _lua;

	public Gui(OpenHoi game)
	{
		_game = game;
		_lua = LuaInterface.LuaInterface.CreateState();
	}

	public void Draw(GameTime time)
	{
		var function = _lua.DoFile("Game/scripts/gui/main_menu.lua")[0] as NLua.LuaFunction;

		try {
			function.Call(time.TotalGameTime.TotalSeconds);
		} catch (NLua.Exceptions.LuaScriptException e) {
			Debug.WriteLine($"Failed to call main_menu.lua. {e.Message}", "OpenHoi");
		}
		
	}
}