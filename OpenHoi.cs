using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.ImGui;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using System.Linq;
using System.Diagnostics;
using System;

namespace OpenHoi;

public class OpenHoi : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
	public ImGuiRenderer GUIRenderer;


    public OpenHoi()
	{
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Game";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
		var scenaries = new List<Scenario>();

		Parallel.ForEach(Directory.GetDirectories("Game\\Scenarios"), dir => 
			{
				try {
					var scenario = Scenario.Load(dir);
					scenaries.Add(scenario);
					Debug.WriteLine($"Loaded \"{scenario.Name}\"", "OpenHoi");
				} catch (Exception e) when (e is FileNotFoundException || e is ArgumentException) {
					Debug.WriteLine($"Skipping {dir} due to \"{e.Message}\"", "OpenHoi");
				}
			}
		);

		GUIRenderer = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
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

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}
}
