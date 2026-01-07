using Raylib_cs;
using System.Numerics;

namespace FadUI.Views;

public class DialogBox : View
{
	public string Title { get; set; } = "Dialog";
	public string Description { get; set; } = "This is a dialog box.";
	public bool IsPopup { get; set; } = true;

	private const float Padding = 15f;
	private const int TitleSize = 20;
	private const int DescSize = 16;

	private HBox _buttonContainer;

	public DialogBox(string title, string description, List<Button> buttons)
	{
		Title = title;
		Description = description;

		_buttonContainer = new HBox()
		{
			Align = Alignment.BottomCenter,
			LocalPosition = new Vector2(0, -Padding)
		};

		foreach (var button in buttons)
		{
			_buttonContainer.AddChild(button);
		}

		AddChild(_buttonContainer);
		CalculateSize();
	}

	private void CalculateSize()
	{
		float titleWidth = Raylib.MeasureText(Title, TitleSize);
		float descWidth = Raylib.MeasureText(Description, DescSize);

		float contentWidth = MathF.Max(titleWidth, MathF.Max(descWidth, _buttonContainer.Size.X));

		Size = new Vector2(
			contentWidth + (Padding * 2),
			Padding + TitleSize + 10 + DescSize + 40 + _buttonContainer.Size.Y + Padding
		);
	}

	public override bool HandleInput(float dt)
	{
		base.HandleInput(dt);

		//Vector2 mousePos = Raylib.GetMousePosition();

		//Vector2 dragOffset = Vector2.Zero;
		//if (Raylib.IsMouseButtonPressed(MouseButton.Left))
		//{
		//	if (Raylib.CheckCollisionPointRec(mousePos, Bounds))
		//	{
		//		_isDragging = true;
		//		dragOffset = GlobalPosition - mousePos;
		//	}
		//}

		//if (_isDragging)
		//{
		//	if (Raylib.IsMouseButtonDown(MouseButton.Left))
		//	{
		//		LocalPosition = mousePos + dragOffset;
		//	}
		//	else
		//	{
		//		_isDragging = false;
		//	}
		//}

		return IsPopup;
	}

	public override void Draw()
	{
		if (!IsVisible) return;

		Raylib.DrawRectangleRec(Bounds, Color.RayWhite);
		Raylib.DrawRectangleLinesEx(Bounds, 2, _isDragging ? Color.Blue : Color.Gray);

		Raylib.DrawText(Title, (int)(GlobalPosition.X + Padding), (int)(GlobalPosition.Y + Padding), TitleSize, Color.Black);
		Raylib.DrawText(Description, (int)(GlobalPosition.X + Padding), (int)(GlobalPosition.Y + Padding + TitleSize + 10), DescSize, Color.DarkGray);

		base.Draw();
	}
}