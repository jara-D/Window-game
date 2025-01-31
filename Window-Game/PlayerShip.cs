using System;
using System.Numerics; // Vector2
using RayDot;
using Raylib_cs; // RayDot

namespace OwO_UwU
{
	class PlayerShip : MoverNode
	{
		private float rotSpeed;
		private float thrustForce;
		public bool Moving = false;
		public Vector2 Size;
		private float bulletCoolDown;


		public PlayerShip(string name) : base(name)
		{
			Position = new Vector2(Settings.ScreenSize.X / 2, Settings.ScreenSize.Y / 2);
			Scale = new Vector2(0.9f, 0.9f);
			thrustForce = 500;
			rotSpeed = 2;
			Size = new Vector2(50, 50);
			bulletCoolDown = 0.0f;
		}

		public override void Update(float deltaTime) // override implementation of MoverNode.Update()
		{
			// MoverNode (IMovable)
			base.Update(deltaTime);
			BorderBounce();
			if (Moving)
			{
				Thrust();
			}
			else
			{
				BreakThrust(deltaTime);
			}
		}




		public void RotateLeft(float deltaTime)
		{
			Rotation -= rotSpeed * deltaTime;
		}

		public void RotateRight(float deltaTime)
		{
			Rotation += rotSpeed * deltaTime;
		}

		public void Thrust()
		{
			float x = (float)Math.Cos(Rotation);
			float y = (float)Math.Sin(Rotation);
			AddForce(new Vector2(x, y) * thrustForce);
		}

		public void BreakThrust(float deltaTime)
		{
			if (Velocity.Length() > 0.1f)
			{
				Velocity *= 1 - (deltaTime * 2);
			}
			else
			{
				Velocity = Vector2.Zero;
			}
		}

		private void BorderBounce()
		{
			int swidth = (int)Settings.ScreenSize.X;
			int sheight = (int)Settings.ScreenSize.Y;
			int halfwidth = (int)Size.X / 2;
			int halfheight = (int)Size.Y / 2;
			Vector2 pos = Position;
			Vector2 vel = Velocity;
			if (pos.X > swidth - halfwidth) { pos.X = swidth - halfwidth; vel.X *= -1; }
			if (pos.X < 0 + halfwidth) { pos.X = 0 + halfwidth; vel.X *= -1; }
			if (pos.Y > sheight - halfheight) { pos.Y = sheight - halfheight; vel.Y *= -1; }
			if (pos.Y < 0 + halfheight) { pos.Y = 0 + halfheight; vel.Y *= -1; }
			Position = pos;
			Velocity = vel;
		}

		public Bullet Shoot(float deltaTime)
		{
			bulletCoolDown += deltaTime;
			if (bulletCoolDown >= 0.3f)
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
	}

}
//blep ᓚᘏᗢ