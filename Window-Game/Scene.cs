using System; // Random, Math
using System.Numerics; // Vector2
using System.Collections.Generic; // List
using RayDot; // RayDot
using Raylib_cs;

namespace OwO_UwU
{
  class Scene : SceneNode
  {
    private PlayerShip player;
    private TextNode playerInfo;
    private List<Bullet> bullets;


    public Scene() : base()
    {
      player = new PlayerShip();
      player.Position = new Vector2(Settings.ScreenSize.X / 2, Settings.ScreenSize.Y / 2);
      player.Scale = new Vector2((float)1.5, (float)1.5);
      AddChild(player);


      // player info
      playerInfo = new TextNode("Velocity: 0", 20);
      playerInfo.Position = new Vector2(10, 30);
      AddChild(playerInfo);




      bullets = new List<Bullet>();

      Reload();
    }

    public void Reload()
    {

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
      if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
      {
        player.Moving = true;
        // player.Thrust();
      }
      else
      {
        player.Moving = false;
      }

      if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
      {
        //sssssssssssssssssssssssssnek
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
        // Shoot();
      }
      if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
      {
        // test
        removeScreenSection();
      }
    }
    private void removeScreenSection()
    {
      Random rand = new();
      int maxRemove = 100;
      int minRemove = 20;

      switch (rand.NextInt64(0, 2))
      {
        case 0:
          Settings.ScreenSize.Y -= rand.NextInt64(minRemove, maxRemove);
          break;
        case 1:
          Settings.ScreenSize.X -= rand.NextInt64(minRemove, maxRemove);
          break;
      }

      Console.WriteLine("New screen size: " + Settings.ScreenSize);
    }
  }
}