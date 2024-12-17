using System; // Math
using System.Numerics; // Vector2
using RayDot; // RayDot
using Raylib_cs; // Raylib

namespace UserLand
{
	class SpaceShip : MoverNode
	{
		private float rotSpeed;
		private float thrustForce;

		private SpriteNode body;
		private SpriteNode engineLeft;
		private SpriteNode engineRight;

		private float bulletCoolDown;

		public SpaceShip(string name) : base(name)
		{
			Position = new Vector2(200, Settings.ScreenSize.Y / 2);
			Pivot = new Vector2(0.45f, 0.5f);
			Scale = new Vector2(0.9f, 0.9f);

			// Velocity = new Vector2(0.0f, 0.0f);
			// Acceleration = new Vector2(0.0f, 0.0f);
			// Mass = 1.0f;

			rotSpeed = (float)Math.PI; // rad/second
			thrustForce = 500;

			body = new SpriteNode("resources/playership.png");
			body.Scale = new Vector2(1.05f, 1.05f);
			body.Pivot = new Vector2(0.4f, 0.5f);
			AddChild(this.body);

			engineLeft = new SpriteNode("resources/engine.png");
			Color = Color.RED;
			engineLeft.Position = new Vector2(0, -12);
			engineLeft.Scale = new Vector2(0.5f, 0.5f);
			engineLeft.Rotation = (float) Math.PI / 6;
			AddChild(engineLeft);

			engineRight = new SpriteNode("resources/engine.png");
			Color = Color.RED;
			engineRight.Position = new Vector2(0, 12);
			engineRight.Scale = new Vector2(0.5f, 0.5f);
			engineRight.Rotation = (float) - Math.PI / 6;
			AddChild(engineRight);

			bulletCoolDown = 0.0f;

			NoThrust();
		}

		public override void Update(float deltaTime) // override implementation of MoverNode.Update()
		{
			// MoverNode (IMovable)
			base.Update(deltaTime);
			// Or do:
			// Move(deltaTime);

			BorderWrap();
			// BorderBounce();
		}

		public Bullet Shoot(float deltaTime)
		{
			bulletCoolDown += deltaTime;
			if (bulletCoolDown >= 0.1f)
			{
				bulletCoolDown = 0.0f;
				Bullet b = new Bullet(500.0f);
				b.Rotation = this.WorldRotation;
				float vel_x = (float)Math.Cos(b.Rotation);
				float vel_y = (float)Math.Sin(b.Rotation);
				Vector2 direction = new Vector2(vel_x, vel_y);
				b.Position = this.WorldPosition + (direction * this.TextureSize.X / 2);
				b.Velocity = direction * b.Speed;
				return b;
			}
			return null;
		}

		public void RotateRight(float deltaTime)
		{
			Rotation += rotSpeed * deltaTime;
			engineLeft.Color = Color.ORANGE;
		}

		public void RotateLeft(float deltaTime)
		{
			Rotation -= rotSpeed * deltaTime;
			engineRight.Color = Color.ORANGE;
		}

		public void StopRotating()
		{
			engineLeft.Color = Color.YELLOW;
			engineRight.Color = Color.YELLOW;
		}

		public void Thrust()
		{
			Color = Color.ORANGE;
			TextureName = "resources/playerthrust.png";
			float x = (float)Math.Cos(Rotation);
			float y = (float)Math.Sin(Rotation);
			AddForce(new Vector2(x, y) * thrustForce);
		}

		public void NoThrust()
		{
			Color = Color.YELLOW;
			TextureName = "resources/player.png";
		}

		private void BorderWrap()
		{
			int swidth = (int)Settings.ScreenSize.X;
			int sheight = (int)Settings.ScreenSize.Y;

			// access protected fields in Node
			if (position.X > swidth)  { position.X = 0; }
			if (position.X < 0)       { position.X = swidth; }
			if (position.Y > sheight) { position.Y = 0; }
			if (position.Y < 0)       { position.Y = sheight; }
		}

		private void BorderBounce()
		{
			int swidth = (int)Settings.ScreenSize.X;
			int sheight = (int)Settings.ScreenSize.Y;
			int halfwidth = (int)TextureSize.X / 2;
			int halfheight = (int)TextureSize.Y / 2;

			Vector2 pos = Position;
			Vector2 vel = Velocity;
			if (pos.X > swidth - halfwidth)   { pos.X = swidth - halfwidth;   vel.X *= -1; }
			if (pos.X < 0 + halfwidth)        { pos.X = 0 + halfwidth;        vel.X *= -1; }
			if (pos.Y > sheight - halfheight) { pos.Y = sheight - halfheight; vel.Y *= -1; }
			if (pos.Y < 0 + halfheight)       { pos.Y = 0 + halfheight;       vel.Y *= -1; }
			Position = pos;
			Velocity = vel;
		}
	}
}
