using RayDot; // RayDot

namespace OwO_UwU
{
	class Bullet : MoverNode
	{
		public float Speed { get; set; }

		public Bullet(float speed) : base()
		{
			Speed = speed;
		}
	}
}
