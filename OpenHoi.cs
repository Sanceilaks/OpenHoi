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
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
	public ImGuiRenderer GUIRenderer; 
	public List<Scenario> Scenarios = new List<Scenario>();
	private Gui _gui;

    public OpenHoi()
	{
        _graphics = new GraphicsDeviceManager(this);
		_gui = new Gui(this);
        Content.RootDirectory = "Game";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
		Parallel.ForEach(Directory.GetDirectories("Game\\Scenarios"), dir => 
			{
				try {
					var scenario = Scenario.Load(dir);
					Scenarios.Add(scenario);
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

        base.Draw(gameTime);

		GUIRenderer.BeginLayout(gameTime);

		_gui.Draw(gameTime);

		ImGui.Begin("OpenHoi");
		ImGui.Text($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
		ImGui.Text($"Author: {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company}");
		ImGui.Text($"Starts at: {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
		ImGui.End();

		ImGui.Begin("Scenarios");
		if (ImGui.BeginTable("Scenarios", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable))
		{
			ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("Description", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("Author", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("Starts at", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableHeadersRow();

			foreach (var scenario in Scenarios)
			{
				ImGui.TableNextRow();
				ImGui.TableNextColumn();
				ImGui.Text(scenario.Name);
				ImGui.TableNextColumn();
				ImGui.Text(scenario.Description);
				ImGui.TableNextColumn();
				ImGui.Text(scenario.Author);
				ImGui.TableNextColumn();
				ImGui.Text(scenario.StartsAt);
			}

			ImGui.EndTable();
		}
		ImGui.End();

		GUIRenderer.EndLayout();
    }

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}
}
