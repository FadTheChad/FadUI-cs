using Raylib_cs;
using FadUI.Core;

namespace FadUI.Views;

public class Button : View
{
	public string Text { get; set; } = "Button";
	public Color BackgroundColor { get; set; } = Color.Blue;
	public Color BackgroundColorHovered { get; set; } = Color.SkyBlue;
	public Color TextColor { get; set; } = Color.White;

	public Action? OnClick { get; set; }

	private Color _renderColor;
	private bool _isHovered;
	private bool _initialized = false;

	public override bool HandleInput(float dt)
	{
		if (!_initialized)
		{
			_renderColor = BackgroundColor;
			_initialized = true;
		}

		var mousePos = Raylib.GetMousePosition();

		_isHovered = Raylib.CheckCollisionPointRec(mousePos, Bounds);

		AnimationManager.TweenColor(() => _renderColor, c => _renderColor = c, _isHovered ? BackgroundColorHovered : BackgroundColor, 0.2f);

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
		if (!_initialized)
		{
			_renderColor = BackgroundColor;
			_initialized = true;
		}

		// Draw button background
		Raylib.DrawRectangle(
			(int)GlobalPosition.X,
			(int)GlobalPosition.Y,
			(int)Size.X,
			(int)Size.Y,
			_renderColor
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
