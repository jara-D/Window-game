using System.Numerics;
using System.Runtime.InteropServices.Swift;
using RayDot;

namespace OwO_UwU
{

	class Enemy : MoverNode
	{
		public float RotSpeed { get; set; }


		public Enemy() : base("resources/Window/smallorange.png")
		{
			RotSpeed = 0.0f;
			maxSpeed = 100.0f;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			Rotation += RotSpeed * deltaTime / 3;
			BorderWrap();
		}

		public void BorderWrap()
		{
			int swidth = (int)Settings.ScreenSize.X;
			int sheight = (int)Settings.ScreenSize.Y;

			Vector2 pos = new Vector2(Position.X, Position.Y);
			if (pos.X > swidth + 64 * Scale.X) { pos.X = 0 - 64 * Scale.X; }
			if (pos.X < 0 - 64 * Scale.X) { pos.X = swidth + 64 * Scale.X; }
			if (pos.Y > sheight + 64 * Scale.Y) { pos.Y = 0 - 64 * Scale.Y; }
			if (pos.Y < 0 - 64 * Scale.Y) { pos.Y = sheight + 64 * Scale.Y; }
			Position = pos;
		}
	}
}