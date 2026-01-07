using FadUI.Views;

namespace FadUI.Core;

public class UIManager
{
	private Stack<View> _layers = [];

	public void Draw()
	{
		foreach (var layer in _layers.Reverse())
		{
			layer.Draw();
		}
	}

	public void Update(float dt)
	{
		// Handle input from top to bottom layer
		foreach (var layer in _layers)
		{
			// if the input is consumed (returns true), stop checking for layers below.
			// eg for Views like Popups, HandleInput will always return true.
			if (layer.HandleInput(dt))
				break;
		}

		foreach (var layer in _layers)
		{
			layer.Update(dt);
		}

		AnimationManager.Update(dt);
	}

	public void PushLayer(View layer)
	{
		_layers.Push(layer);
	}

	public void PopLayer()
	{
		_layers.Pop();
	}
}
