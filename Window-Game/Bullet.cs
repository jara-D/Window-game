using RayDot; // RayDot

namespace OwO_UwU
{
	class Bullet : MoverNode
	{
		public float Speed { get; set; }

		public Bullet(float speed) : base("resources/bullet.png")
		{
			Speed = speed;
		}
	}
}
