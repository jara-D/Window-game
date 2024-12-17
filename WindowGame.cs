using RayDot; // RayDot

namespace OwO_UwU //i had nothing to do with this -egg
{
  class WindowGame
  {
    private Core core;
    private Scene scene;

    public WindowGame()
    {
      core = new Core("Window Game");
      scene = new();
    }

    public void Play()
    {
			while (core.Run(scene))
			{
				;
			}
    }
  }
}
