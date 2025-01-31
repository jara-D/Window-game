using RayDot; // RayDot

namespace OwO_UwU
{
	class Bullet : MoverNode
	{
		public float Speed { get; set; }

		public Bullet(float speed) : base("resources/Window/bullet.png")
		{
			Speed = speed;
			maxSpeed = 1000.0f;
		}
	}
}
