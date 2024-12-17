using RayDot; // RayDot

namespace UserLand
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
