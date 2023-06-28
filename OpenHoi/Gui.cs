using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace OpenHoi;

public class Gui
{
	private OpenHoi _game;
	private NLua.Lua _lua;
	private LuaInterface.LuaModule _mainMenuModule;

	public Gui(OpenHoi game)
	{
		_game = game;
		_lua = LuaInterface.LuaInterface.CreateState();

		_mainMenuModule = new LuaInterface.LuaModule(_lua, "Game/scripts/gui/main_menu.lua", true, true, true, true);
	}

	public void Draw(GameTime time)
	{
		_mainMenuModule.CallModule(time.ElapsedGameTime.Milliseconds);
	}
}