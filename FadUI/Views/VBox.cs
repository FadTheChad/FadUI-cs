using System.Numerics;

namespace FadUI.Views;
public class VBox : View
{
	public float Spacing { get; set; } = 5f;

	public VBox() { }

	// allow user to add views direct in constructor
	public VBox(params View[] views)
	{
		foreach (var view in views)
		{
			AddChild(view);
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float totalHeight = 0f;
		float maxWidth = 0f;

		for (int i = 0; i < Children.Count; i++)
		{
			var child = Children[i];

			child.LocalPosition = new Vector2(child.LocalPosition.X, totalHeight);

			totalHeight += child.Size.Y;
			maxWidth = MathF.Max(maxWidth, child.Size.X);

			if (i < Children.Count - 1)
			{
				totalHeight += Spacing;
			}
		}

		Size = new Vector2(maxWidth, totalHeight);
	}
}
