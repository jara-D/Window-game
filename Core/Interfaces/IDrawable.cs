using Raylib_cs; // Color

namespace RayDot
{
	interface IDrawable
	{
		Color Color { get; set; }

		void Draw();
	}
}
