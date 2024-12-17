using System.Numerics;

namespace RayDot
{
	interface IMovable
	{
		Vector2 Velocity { get; set; }
		Vector2 Acceleration { get; }
		float Mass { get; set; }

		void Move(float deltaTime);
		void AddForce(Vector2 force);
	}
}
