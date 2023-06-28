using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.ImGui;
using ImGuiNET;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using System.Linq;
using System.Diagnostics;
using System;
using System.Reflection;

namespace OpenHoi;

public class OpenHoi : Game
{
	private Texture2D _texture;
	private GraphicsDeviceManager? _graphics;
	private SpriteBatch? _spriteBatch;
	public ImGuiRenderer? GUIRenderer;
	public List<Modification> Modifications = new List<Modification>();
	private Gui? _gui;

	public OpenHoi()
	{
		LuaInterface.LuaInterface.Game = this;
		_graphics = new GraphicsDeviceManager(this);
		_gui = new Gui(this);
		Content.RootDirectory = "Game";
		IsMouseVisible = true;

		Window.AllowUserResizing = true;
	}

	protected override void Initialize()
	{
		GUIRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		Parallel.ForEach(Directory.GetDirectories("Game\\Modifications"), dir =>
			{
				try
				{
					var modification = Modification.Load(dir);
					Modifications.Add(modification);
					Debug.WriteLine($"Loaded \"{modification.Name}\"", "OpenHoi");
				}
				catch (Exception e) when (e is FileNotFoundException || e is ArgumentException)
				{
					Debug.WriteLine($"Skipping {dir} due to \"{e.Message}\"", "OpenHoi");
				}
			}
		);

		_spriteBatch = new SpriteBatch(GraphicsDevice);
		_texture = Texture2D.FromFile(GraphicsDevice, "Game/assets/texture.jpg");
	}

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch!.Begin();
		_spriteBatch!.Draw(_texture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
		_spriteBatch!.End();

		base.Draw(gameTime);

		GUIRenderer!.BeginLayout(gameTime);

		_gui!.Draw(gameTime);
		GUIRenderer!.EndLayout();
	}

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}
}
