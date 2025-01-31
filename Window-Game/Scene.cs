using System;
using System.Numerics;
using System.Collections.Generic;
using RayDot;
using Raylib_cs;
using System.Threading;
using System.ComponentModel;


namespace OwO_UwU
{

    class Scene : SceneNode
    {
        private PlayerShip player;
        private TextNode playerInfo;
        private List<Bullet> bullets;
        private Timer shrinkTimer;
        private List<Enemy> Enemies;
        private Timer EnemInterval;
        private SpriteNode gameover;

        public Scene() : base()
        {
            player = new PlayerShip("resources/Window/gayship.png");
            player.Position = new Vector2(Settings.ScreenSize.X / 2, Settings.ScreenSize.Y / 2);
            player.Scale = new Vector2((float)1.5, (float)1.5);
            AddChild(player);

            gameover = new SpriteNode("resources/Astroid/gameover.png");
			gameover.Color = Color.RED;
			gameover.Position = new Vector2(0, -256); // outside screen
			AddChild(gameover);

            // player info
            playerInfo = new TextNode("Velocity: 0", 20);
            playerInfo.Position = new Vector2(10, 30);
            AddChild(playerInfo);

            shrinkTimer = new Timer(ShrinkScreen, null, 0, 200);
            bullets = new List<Bullet>();
            Enemies = new List<Enemy>();
           
            //spawn enemies every 2000 milliseconds ᓚᘏᗢ
            EnemInterval = new Timer (EnemyTimed, null, 0, 2000);
        }

        public override void Update(float deltaTime)
        {
            handleDisplays(deltaTime);
            HandleBullets(deltaTime);
            HandleInput(deltaTime);
            HandlePlayer(deltaTime);
            HandleEnemies(deltaTime);

            Raylib.DrawFPS(10, 10);

            if (State == State.Lost)
			{
				gameover.Position = Settings.ScreenSize / 2;
			}
        }

        private void EnemyTimed(object state)
        {
            SpawnEnemy(2);
        }


        private void SpawnEnemy(int amount)
        {
            var rand = new Random();
            for (int i = 0; i < amount; i++)
            {
                Enemy a = new();
                Vector2 pos = new Vector2();
                // chose a random spawnpoint outside the screen
                int side = rand.Next(4);

                switch (side)
                {
                    case 0:
                        pos.X = rand.Next((int)Settings.ScreenSize.X);
                        pos.Y = -50;
                        break;
                    case 1:
                        pos.X = rand.Next((int)Settings.ScreenSize.X);
                        pos.Y = Settings.ScreenSize.Y + 50;
                        break;
                    case 2:
                        pos.X = -50;
                        pos.Y = rand.Next((int)Settings.ScreenSize.Y);
                        break;
                    case 3:
                        pos.X = Settings.ScreenSize.X + 50;
                        pos.Y = rand.Next((int)Settings.ScreenSize.Y);
                        break;
                }
                a.Position = pos;

                Vector2 vel = new Vector2();
                vel.X = (float)(rand.NextDouble() * 400) - 200;
                vel.Y = (float)(rand.NextDouble() * 400) - 200;
                a.Velocity = vel;

                float pi = (float)Math.PI;
                a.RotSpeed = (float)(rand.NextDouble() * pi * 4) - pi * 2;

                Enemies.Add(a);
                AddChild(a);
            }
        }
        private void handleDisplays(float deltaTime)
        {
            // Update player info
            playerInfo.Text = "Velocity: " + player.Velocity.Length().ToString();
        }

        private void HandlePlayer(float deltaTime)
        {
            if (State == State.Lost)
            {
                return;
            }

            foreach (Enemy Enemy in Enemies)
            {
                // distance to player
                float distance = Vector2.Distance(Enemy.WorldPosition, player.WorldPosition);
                float toCheck = player.TextureSize.X / 2 + (Enemy.TextureSize.X / 2 * Enemy.Scale.X) * 0.8f;

                if (distance < toCheck)
                {
                    if (Children.Contains(player)) 
                    { 
                        Children.Remove(player);
                    }
                    State = State.Lost;
                }
            }
        }

        private void HandleEnemies(float deltaTime)
        {
            if (State == State.Lost)
            {
                return;
            }
            List<Bullet> bulletsToDelete = new List<Bullet>();
            List<Enemy> EnemiesToDelete = new List<Enemy>();
            foreach (Enemy Enemy in Enemies)
            {
                var dir = Vector2.Subtract(player.WorldPosition, Enemy.WorldPosition);
                Vector2 force = Vector2.Multiply(Vector2.Normalize(dir), 200);
                Enemy.Acceleration = force;
                
                foreach (Bullet bullet in bullets)
                {
                    float distance = Vector2.Distance(Enemy.WorldPosition, bullet.WorldPosition);
                    float toCheck = Enemy.TextureSize.X / 2 * Enemy.Scale.X;
                    if (distance < toCheck)
                    {
                        EnemiesToDelete.Add(Enemy);
                        bulletsToDelete.Add(bullet);
                    }
                }
            }

            foreach (Enemy Enemy in EnemiesToDelete)
            {
                Enemies.Remove(Enemy);
                RemoveChild(Enemy);
            }
            foreach (Bullet bullet in bulletsToDelete)
            {
                bullets.Remove(bullet);
                RemoveChild(bullet);
            }

        }

        private void HandleBullets(float deltaTime)
        {
            // update all bullets
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = bullets[i];
                // check if bullet is outside screen area
                if (bullet.Position.Y < 0 ||
                    bullet.Position.Y > Settings.ScreenSize.Y ||
                    bullet.Position.X < 0 ||
                    bullet.Position.X > Settings.ScreenSize.X)
                {
                    bullets.Remove(bullet);
                    RemoveChild(bullet);
                    int window = Raylib.GetCurrentMonitor();
                    int windowWidth = Raylib.GetMonitorWidth(window);
                    int windowHeight = Raylib.GetMonitorHeight(window);

                    // Top
                    if (bullet.Position.Y < 0)
                    {
                        if (Raylib.GetWindowPosition().Y - 17 > 0)
                        {
                            Settings.ScreenSize.Y += 15;
                            Raylib.SetWindowPosition((int)Raylib.GetWindowPosition().X, (int)Raylib.GetWindowPosition().Y - 17);
                        }
                    }
                    // Bottom
                    if (bullet.Position.Y > Settings.ScreenSize.Y)
                    {
                        if (Raylib.GetWindowPosition().Y + Settings.ScreenSize.Y + 17 < windowHeight)
                        {
                            Settings.ScreenSize.Y += 15;
                        }
                    }
                    // Left
                    if (bullet.Position.X < 0)
                    {
                        if (Raylib.GetWindowPosition().X - 17 > 0)
                        {
                            Settings.ScreenSize.X += 15;
                            Raylib.SetWindowPosition((int)Raylib.GetWindowPosition().X - 17, (int)Raylib.GetWindowPosition().Y);
                        }
                    }
                    // Right
                    if (bullet.Position.X > Settings.ScreenSize.X)
                    {
                        if (Raylib.GetWindowPosition().X + Settings.ScreenSize.X + 17 < windowWidth)
                        {
                            Settings.ScreenSize.X += 15;
                        }
                    }
                }
            }
        }

        private void HandleInput(float deltaTime)
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
            player.Moving = true;
            }
            else
            {
            player.Moving = false;
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
            player.BreakThrust(deltaTime);
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
            player.RotateLeft(deltaTime);
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
            player.RotateRight(deltaTime);
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
            {
            if (!(State == State.Lost))
            {
                Bullet bullet = player.Shoot(deltaTime);
                if (bullet != null)
                {
                AddChild(bullet);
                bullets.Add(bullet);
                // Apply recoil force to the player
                Vector2 recoilForce = Vector2.Multiply(Vector2.Normalize(-player.Direction), 100000);
                player.AddForce(recoilForce);
                }
            }
            }
        }

        private void ShrinkScreen(object state)
        {
            if (State == State.Lost)
            {
                return;
            }
            Settings.ScreenSize.Y -= 2;
            Settings.ScreenSize.X -= 2;
            int newPosX = (int)Raylib.GetWindowPosition().X + 1;
            int newPosY = (int)Raylib.GetWindowPosition().Y + 1;

            Raylib.SetWindowPosition(newPosX, newPosY);
            
            int newWidth = (int)Settings.ScreenSize.X;
            int newHeight = (int)Settings.ScreenSize.Y;

            Raylib.SetWindowSize(newWidth, newHeight);
        }
    }
}