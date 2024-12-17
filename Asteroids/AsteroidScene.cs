using System; // Random, Math
using System.Numerics; // Vector2
using System.Collections.Generic; // List
using RayDot; // RayDot
using Raylib_cs; // Raylib

namespace UserLand
{
	class AsteroidScene : SceneNode
	{
		private SpaceShip player;
		private List<Bullet> bullets;
		private List<Asteroid> asteroids;
		private SpriteNode victory;
		private SpriteNode gameover;
		private TextNode fpstext;
		private float timer;
		private int framecounter;
		private SpriteNode background;


		public AsteroidScene() : base()
		{
			Reload();
		}

		public void Reload()
		{
			Children.Clear();
			State = State.Playing;
						background = new SpriteNode("resources/background.png");
			background.Pivot = new Vector2(0, 0);
			AddChild(background);

			victory = new SpriteNode("resources/victory.png");
			victory.Color = Color.GREEN;
			victory.Position = new Vector2(0, -256); // outside screen
			AddChild(victory);

			gameover = new SpriteNode("resources/gameover.png");
			gameover.Color = Color.RED;
			gameover.Position = new Vector2(0, -256); // outside screen
			AddChild(gameover);

			player = new SpaceShip("resources/player.png");
			AddChild(player);

			fpstext = new TextNode("calculating FPS...", 20);
			fpstext.Position = new Vector2(10, 10);
			fpstext.Color = Color.GREEN;
			AddChild(fpstext);
			timer = 0.0f;
			framecounter = 0;

			bullets = new List<Bullet>();
			asteroids = new List<Asteroid>();




			SpawnAsteroids(5, Settings.ScreenSize / 2, Generation.First);
		}

		public override void Update(float deltaTime)
		{
			// Calculate framerate
			framecounter++;
			timer += deltaTime;
			if (timer > 1.0f) {
				fpstext.Text = framecounter.ToString() + " FPS";
				framecounter = 0;
				timer = 0.0f;
			}

			HandleInput(deltaTime);

			HandlePlayer(deltaTime);
			HandleBullets(deltaTime);
			HandleAsteroids(deltaTime);

			if (asteroids.Count == 0)
			{
				State = State.Won;
			}
			if (State == State.Won)
			{
				victory.Position = Settings.ScreenSize / 2;
			}
			if (State == State.Lost)
			{
				gameover.Position = Settings.ScreenSize / 2;
			}
		}

		private void HandlePlayer(float deltaTime)
		{
			if (State == State.Lost)
			{
				return;
			}
			// player <-> asteroids
			foreach (Asteroid asteroid in asteroids)
			{
				// distance to player
				float distance = Vector2.Distance(asteroid.WorldPosition, player.WorldPosition);
				float toCheck = player.TextureSize.X / 2 + (asteroid.TextureSize.X / 2 * asteroid.Scale.X) * 0.8f;

				if (distance < toCheck)
				{
					if (Children.Contains(player)) { Children.Remove(player); }
					State = State.Lost;
				}
			}
		}

		private void HandleAsteroids(float deltaTime)
		{
			if (State == State.Lost)
			{
				return;
			}

			// asteroids <-> bullets
			List<Bullet> bulletsToDelete = new List<Bullet>();
			List<Asteroid> asteroidsToDelete = new List<Asteroid>();
			List<Vector2> positionsToSpawnSecondGen = new List<Vector2>();
			List<Vector2> positionsToSpawnThirdGen = new List<Vector2>();
			foreach (Asteroid asteroid in asteroids)
			{
				foreach (Bullet bullet in bullets)
				{
					// distance to bullet
					float distance = Vector2.Distance(asteroid.WorldPosition, bullet.WorldPosition);
					float toCheck = asteroid.TextureSize.X / 2 * asteroid.Scale.X;
					if (distance < toCheck)
					{
						if (asteroid.Generation == Generation.First)
						{
							asteroidsToDelete.Add(asteroid);
							bulletsToDelete.Add(bullet);
							positionsToSpawnSecondGen.Add(asteroid.WorldPosition);
						}
						if (asteroid.Generation == Generation.Second)
						{
							asteroidsToDelete.Add(asteroid);
							bulletsToDelete.Add(bullet);
							positionsToSpawnThirdGen.Add(asteroid.WorldPosition);
						}
						if (asteroid.Generation == Generation.Third)
						{
							asteroidsToDelete.Add(asteroid);
							bulletsToDelete.Add(bullet);
						}
					}
				}
			}

			// Spawn new Asteroids
			foreach (var pos in positionsToSpawnSecondGen)
			{
				SpawnAsteroids(2, pos, Generation.Second);
			}
			foreach (var pos in positionsToSpawnThirdGen)
			{
				SpawnAsteroids(3, pos, Generation.Third);
			}

			// delete Asteroids and Bullets
			foreach (Asteroid asteroid in asteroidsToDelete)
			{
				asteroids.Remove(asteroid);
				RemoveChild(asteroid);
			}
			foreach (Bullet bullet in bulletsToDelete)
			{
				bullets.Remove(bullet);
				RemoveChild(bullet);
			}

		}

		private void SpawnAsteroids(int amount, Vector2 pos, Generation gen)
		{
			var rand = new Random();
			for (int i = 0; i < amount; i++)
			{
				Asteroid a = new Asteroid(gen);
				a.Position = pos;
				if (gen == Generation.Second)
				{
					a.Scale = new Vector2(0.7f, 0.7f);
				}
				if (gen == Generation.Third)
				{
					a.Scale = new Vector2(0.3f, 0.3f);
				}

				Vector2 vel = new Vector2();
				vel.X = (float) (rand.NextDouble() * 400) - 200;
				vel.Y = (float) (rand.NextDouble() * 400) - 200;
				a.Velocity = vel;

				float pi = (float) Math.PI;
				a.RotSpeed = (float) (rand.NextDouble() * pi * 4) - pi * 2;

				asteroids.Add(a);
				AddChild(a);
			}
		}

		private void HandleBullets(float deltaTime)
		{
			// update all bullets
			for (int i = bullets.Count-1; i >= 0; i--)
			{
				Bullet bullet = bullets[i];
				// check if bullet is outside screen area
				if (bullet.Position.Y < 0 ||
					bullet.Position.Y > Settings.ScreenSize.Y ||
					bullet.Position.X < 0 ||
					bullet.Position.X > Settings.ScreenSize.X
					)
				{
					bullets.Remove(bullet);
					RemoveChild(bullet);
				}
			}
		}

		private void HandleInput(float deltaTime)
		{
			// Reload Game
			if (Raylib.IsKeyReleased(KeyboardKey.KEY_R))
			{
				Reload();
			}
			// Player Rotate
			if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
			{
				player.RotateRight(deltaTime);
			}
			if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
			{
				player.RotateLeft(deltaTime);
			}
			if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
			{
				player.StopRotating();
			}
			if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
			{
				player.StopRotating();
			}
			// Player Thrust
			if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
			{
				player.Thrust();
			}
			if (Raylib.IsKeyReleased(KeyboardKey.KEY_LEFT_SHIFT))
			{
				player.NoThrust();
			}
			// Player Shoot
			if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
			{
				if (!(State == State.Lost))
				{
					Bullet bullet = player.Shoot(deltaTime);
					if (bullet != null)
					{
						AddChild(bullet);
						bullets.Add(bullet);
					}
				}
			}

			// Camera
			/*
			float camspeed = 200.0f;
			if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
			{
				Camera.X += deltaTime * camspeed;
			}
			if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
			{
				Camera.X -= deltaTime * camspeed;
			}
			if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
			{
				Camera.Y += deltaTime * camspeed;
			}
			if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
			{
				Camera.Y -= deltaTime * camspeed;
			}
			*/
		}
	}
}
