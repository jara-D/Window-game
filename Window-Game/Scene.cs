using System;
using System.Numerics;
using System.Collections.Generic;
using RayDot;
using Raylib_cs;
using System.Threading;

namespace OwO_UwU
{
    class Scene : SceneNode
    {
        private PlayerShip player;
        private TextNode playerInfo;
        private List<Bullet> bullets;
        private Thread shrinkThread;

        public Scene() : base()
        {
            player = new PlayerShip("resources/Window/gayship.png");
            player.Position = new Vector2(Settings.ScreenSize.X / 2, Settings.ScreenSize.Y / 2);
            player.Scale = new Vector2((float)1.5, (float)1.5);
            AddChild(player);

            // player info
            playerInfo = new TextNode("Velocity: 0", 20);
            playerInfo.Position = new Vector2(10, 30);
            AddChild(playerInfo);

            bullets = new List<Bullet>();

        }


        public override void Update(float deltaTime)
        {
            handleDisplays(deltaTime);
            HandleBullets(deltaTime);
            HandleInput(deltaTime);
            HandlePlayer(deltaTime);

            Raylib.DrawFPS(10, 10);
        }

        private void handleDisplays(float deltaTime)
        {
            // Update player info
            playerInfo.Text = "Velocity: " + player.Velocity.Length().ToString();
        }

        private void HandlePlayer(float deltaTime)
        {

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
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                player.Shoot();
            }
        }
    }
}