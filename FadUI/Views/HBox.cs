using System.Numerics;

namespace FadUI.Views;

public class HBox : View
{
	public float Spacing { get; set; } = 5f;

	public HBox() { }

	// allow user to add views direct in constructor
	public HBox(params View[] views)
	{
		foreach (var view in views)
		{
			AddChild(view);
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float totalWidth = 0f;
		float maxHeight = 0f;

		for (int i = 0; i < Children.Count; i++)
		{
			var child = Children[i];

			child.LocalPosition = new Vector2(totalWidth, child.LocalPosition.Y);

			totalWidth += child.Size.X;
			maxHeight = MathF.Max(maxHeight, child.Size.Y);

			if (i < Children.Count - 1)
			{
				totalWidth += Spacing;
			}
		}

		Size = new Vector2(totalWidth, maxHeight);
	}
}