using System; // Console
using RayDot; // RayDot

namespace UserLand
{
	class AsteroidGame
	{
		private Core core;
		private AsteroidScene currentScene;

		public AsteroidGame()
		{
			core = new Core("RayDot Asteroids");

			currentScene = new AsteroidScene();
		}

		public void Play()
		{
			while (core.Run(currentScene))
			{
				;
			}
			Console.WriteLine("Thank you for playing!");
		}
	}
}
