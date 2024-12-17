using System.Numerics;

namespace RayDot
{
	enum State
	{
		Quit,
		Playing,
		Lost,
		Won
	}

	abstract class SceneNode : Node
	{
		public Vector2 Camera { get; }
		public State State { get; set; }

		protected SceneNode() : base()
		{
			Camera = new Vector2(Settings.ScreenSize.X/2, Settings.ScreenSize.Y/2);
			State = State.Playing;
		}
	}
}
