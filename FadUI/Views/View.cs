using Raylib_cs;
using System.Numerics;

namespace FadUI.Views;

public enum Alignment
{
	TopLeft,
	TopCenter,
	TopRight,
	CenterLeft,
	Center,
	CenterRight,
	BottomLeft,
	BottomCenter,
	BottomRight
}

public class View
{
	private List<View> _children = [];
	public IReadOnlyList<View> Children => _children;

	public View? Parent { get; private set; }

	public Vector2 LocalPosition { get; set; }
	public Vector2 Size { get; set; }
	public Alignment Align { get; set; } = Alignment.TopLeft;

	public Vector2 GlobalPosition
	{
		get
		{
			Vector2 basePos = Parent == null ? Vector2.Zero : Parent.GlobalPosition;
			return basePos + GetAlignmentOffset() + LocalPosition;
		}
	}

	private Vector2 GetAlignmentOffset()
	{
		// Get dimensions of the container (Parent or Screen)
		float containerWidth = Parent == null ? Raylib.GetScreenWidth() : Parent.Size.X;
		float containerHeight = Parent == null ? Raylib.GetScreenHeight() : Parent.Size.Y;

		return Align switch
		{
			Alignment.TopLeft => Vector2.Zero,
			Alignment.TopCenter => new Vector2(containerWidth / 2 - Size.X / 2, 0),
			Alignment.TopRight => new Vector2(containerWidth - Size.X, 0),
			Alignment.CenterLeft => new Vector2(0, containerHeight / 2 - Size.Y / 2),
			Alignment.Center => new Vector2(containerWidth / 2 - Size.X / 2, containerHeight / 2 - Size.Y / 2),
			Alignment.CenterRight => new Vector2(containerWidth - Size.X, containerHeight / 2 - Size.Y / 2),
			Alignment.BottomLeft => new Vector2(0, containerHeight - Size.Y),
			Alignment.BottomCenter => new Vector2(containerWidth / 2 - Size.X / 2, containerHeight - Size.Y),
			Alignment.BottomRight => new Vector2(containerWidth - Size.X, containerHeight - Size.Y),
			_ => Vector2.Zero
		};
	}

	public Rectangle Bounds => new(GlobalPosition.X, GlobalPosition.Y, Size.X, Size.Y);

	public bool IsVisible { get; set; } = true;

	public bool IsDraggable { get; set; } = false;
	protected bool _isDragging = false;
	private Vector2 _dragOffset = Vector2.Zero;

	public void AddChild(View child)
	{
		if (child.Parent != null)
			throw new InvalidOperationException("View already has a parent.");

		child.Parent = this;
		_children.Add(child);
	}

	public void RemoveChild(View child)
	{
		if (_children.Remove(child))
			child.Parent = null;
	}

	public virtual bool HandleInput(float dt)
	{
		if (!IsVisible) return false;

		for (int i = _children.Count - 1; i >= 0; i--)
		{
			if (_children[i].HandleInput(dt))
				return true;
		}

		// === DRAGGING ===
		if (IsDraggable)
		{
			Vector2 mousePos = Raylib.GetMousePosition();

			if (Raylib.IsMouseButtonPressed(MouseButton.Left))
			{
				if (Raylib.CheckCollisionPointRec(mousePos, Bounds))
				{
					_isDragging = true;

					Vector2 currentGlobalPos = GlobalPosition; // store pos before we reset alignment

					Align = Alignment.TopLeft;

					LocalPosition = currentGlobalPos; // now set local to global, as it is Top Left / Absolute

					_dragOffset = LocalPosition - mousePos;

				}
			}

			if (_isDragging)
			{
				if (Raylib.IsMouseButtonDown(MouseButton.Left))
				{
					LocalPosition = mousePos + _dragOffset;
				}
				else
				{
					_isDragging = false;
				}
			}
		}


		return false;
	}

	public virtual void Update(float dt)
	{
		foreach (var child in _children) child.Update(dt);
	}

	public virtual void Draw()
	{
		if (!IsVisible) return;
		foreach (var child in _children) child.Draw();
	}
}