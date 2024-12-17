using System; // Math.PI
using System.Numerics; // Vector2
using Raylib_cs; // Raylib

namespace RayDot
{
	class SpriteNode : Node, IDrawable
	{
		// Sprite
		private string textureName;
		private Vector2 textureSize;
		private Vector2 pivot;
		private Color color;

		public string TextureName {
			get { return textureName; }
			set { textureName = value; }
		}
		public Vector2 TextureSize {
			get { return textureSize; }
		}
		public Vector2 Pivot {
			get { return pivot; }
			set { pivot = value; }
		}
		public Color Color {
			get { return color; }
			set { color = value; }
		}

		public SpriteNode(string name) : base()
		{
			TextureName = name;
			textureSize = new Vector2(0, 0); // Draw() updates this if necessary
			Pivot = new Vector2(0.5f, 0.5f);
			Color = Color.WHITE;
		}

		public override void Update(float deltaTime)
		{
			Draw();
		}

		public void Draw()
		{
			Texture2D texture = ResourceManager.Instance.GetTexture(TextureName);
			float width = texture.width;
			float height = texture.height;
			// this Entity might not know its Size yet...
			if (TextureSize.X == 0 || TextureSize.Y == 0)
			{
				textureSize = new Vector2(width, height);
			}
			// draw the Texture
			Rectangle sourceRec = new Rectangle(0.0f, 0.0f, width, height);
			Rectangle destRec = new Rectangle(WorldPosition.X, WorldPosition.Y, width * WorldScale.X, height * WorldScale.Y);
			Vector2 pivot = new Vector2(width * Pivot.X * WorldScale.X, height * Pivot.Y * WorldScale.Y);
			float rot = WorldRotation * 180 / (float) Math.PI;
			Raylib.DrawTexturePro(texture, sourceRec, destRec, pivot, rot, Color);
		}

	}
}
