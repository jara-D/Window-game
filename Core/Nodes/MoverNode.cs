using System.Numerics;

namespace RayDot
{
	abstract class MoverNode : SpriteNode, IMovable
	{
		// Implementation of IMovable: Velocity, Acceleration, Mass
		protected Vector2 velocity;
		protected Vector2 acceleration;
		protected float mass;

		public Vector2 Velocity {
			get { return velocity; }
			set { velocity = value; }
		}
		public Vector2 Acceleration {
			get { return acceleration; }
		}
		public float Mass {
			get { return mass; }
			set { mass = value; }
		}

		protected MoverNode(string name) : base(name)
		{
			Velocity = new Vector2(0.0f, 0.0f);
			acceleration = new Vector2(0.0f, 0.0f);
			Mass = 1.0f;
		}

		public override void Update (float deltaTime)
		{
			base.Update(deltaTime); // SpriteNode.Draw()
			Move(deltaTime); // IMovable
		}

		// Implementation of IMovable: Move(float deltaTime), AddForce(Vector2 force)
		public void Move(float deltaTime)
		{
			// apply motion 101
			Velocity += Acceleration * deltaTime;
			Position += Velocity * deltaTime;
			// reset acceleration
			acceleration *= 0.0f;
		}

		public void AddForce(Vector2 force)
		{
			acceleration += force / Mass;
		}
	}
}
