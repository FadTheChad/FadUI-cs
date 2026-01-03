using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace FadUI.Core;
public class App
{
	private IWindow _window;

	public void Run()
	{
		WindowOptions options = WindowOptions.Default with
		{
			Size = new Vector2D<int>(800, 600),
			Title = "My first Silk.NET application!"
		};

		_window = Window.Create(options);

		_window.Load += OnLoad;
		_window.Update += OnUpdate;
		_window.Render += OnRender;

		_window.Run();
	}

	private void OnLoad()
	{
		Console.WriteLine("Window loaded.");

		IInputContext input = _window.CreateInput();

		for (int i = 0; i < input.Keyboards.Count; i++)
			input.Keyboards[i].KeyDown += KeyDown;
	}

	private void OnUpdate(double deltaTime) { }

	private void OnRender(double deltaTime) { }

	private void KeyDown(IKeyboard keyboard, Key key, int keyCode)
	{
		if (key == Key.Escape)
			_window.Close();
	}
}
