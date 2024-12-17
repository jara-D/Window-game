using System.Numerics; // Vector2
using Raylib_cs; // Raylib

namespace RayDot
{
	class Core
	{
		public Core(string title)
		{
			int width = (int)Settings.ScreenSize.X;
			int height = (int)Settings.ScreenSize.Y;
			Raylib.InitWindow(width, height, title);
		}

		public bool Run(SceneNode scene)
		{
			// update the window size
			Raylib.SetWindowSize((int)Settings.ScreenSize.X, (int)Settings.ScreenSize.Y);

			if (Raylib.WindowShouldClose())
			{
				ResourceManager.Instance.CleanUp();
				Raylib.CloseWindow();
				return false;
			}

			// how many seconds since the last frame?
			float deltaTime = Raylib.GetFrameTime();

			// Transform all Nodes in the Scene
			scene.TransformNode(Matrix4x4.Identity);

			// draw the scene
			Raylib.BeginDrawing();
				Raylib.ClearBackground(Color.BLACK);

				// Update (and Draw) all nodes in the Scene
				scene.UpdateNode(deltaTime);

			Raylib.EndDrawing();

			return true;
		}
	}
}
