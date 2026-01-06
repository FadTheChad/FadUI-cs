using Raylib_cs;

namespace FadUI.Views;
public class Button : View
{
	public string Text { get; set; } = "Button";
	public Color BackgroundColor { get; set; } = Color.Blue;
	public Color BackgroundColorHovered { get; set; } = Color.SkyBlue;
	public Color TextColor { get; set; } = Color.White;

	public Action? OnClick { get; set; }

	public override bool HandleInput(float dt)
	{
		var mousePos = Raylib.GetMousePosition();

		if (Raylib.CheckCollisionPointRec(mousePos, Bounds))
		{
			BackgroundColor = BackgroundColorHovered;
		}
		else
		{
			BackgroundColor = Color.Blue;
		}

		if (IsVisible && Raylib.IsMouseButtonPressed(MouseButton.Left))
		{
			if (Raylib.CheckCollisionPointRec(mousePos, Bounds))
			{
				// Button was clicked
				Console.WriteLine($"Button '{Text}' clicked!");
				OnClick?.Invoke();
				return true; // Consume input
			}
		}

		return base.HandleInput(dt);
	}

	public override void Draw()
	{
		// Draw button background
		Raylib.DrawRectangle(
			(int)GlobalPosition.X,
			(int)GlobalPosition.Y,
			(int)Size.X,
			(int)Size.Y,
			BackgroundColor
		);

		// Draw button text (centered)
		int textWidth = Raylib.MeasureText(Text, 20);
		Raylib.DrawText(
			Text,
			(int)(GlobalPosition.X + (Size.X - textWidth) / 2),
			(int)(GlobalPosition.Y + (Size.Y - 20) / 2),
			20,
			TextColor
		);
		// Draw children
		base.Draw();
	}
}
