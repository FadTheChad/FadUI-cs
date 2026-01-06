using FadUI.Core;
using FadUI.Views;
using Raylib_cs;
using System.Numerics;

internal static class Program
{
	private static void Main()
	{
		Raylib.SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.VSyncHint);
		Raylib.InitWindow(800, 480, "FadUI");

		var ui = new UIManager();

		// Create an VBox
		var vbox = new VBox(
			new Button() { Size = new Vector2(100, 25), LocalPosition = Vector2.Zero, Text = "Button 1" },
			new Button() { Size = new Vector2(100, 25), LocalPosition = Vector2.Zero, Text = "Button 2" },
			new Button() { Size = new Vector2(100, 25), LocalPosition = Vector2.Zero, Text = "Button 3" }
		)
		{
			LocalPosition = new Vector2(100, 100),
		};


		// Add the VBox to the UI
		ui.PushLayer(vbox);
		ui.PushLayer(new DialogBox("Test Dialog", "This is a test dialog box.",
		[
			new Button() { Size = new Vector2(80, 30), Text = "OK", OnClick = () => { ui.PopLayer(); } },
			new Button() { Size = new Vector2(80, 30), Text = "Cancel", OnClick = () => { ui.PopLayer(); }},
		])
		{
			IsDraggable = true,
			Align = Alignment.Center
		});

		//ui.PushLayer(new DialogBox("Test Dialog2", "This is a test dialog box2.",
		//[
		//	new Button() { Size = new Vector2(80, 30), Text = "OK" },
		//	new Button() { Size = new Vector2(80, 30), Text = "Cancel" },
		//])
		//{
		//	IsDraggable = true,
		//	Align = Alignment.TopCenter
		//});

		while (!Raylib.WindowShouldClose())
		{
			float dt = Raylib.GetFrameTime();

			ui.Update(dt);

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.RayWhite);

			ui.Draw();

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}