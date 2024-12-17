using System; // Math.PI
using Raylib_cs; // Raylib

namespace RayDot
{
	class TextNode : Node, IDrawable
	{
		private String text;
		private int fontsize;
		private Color color;

		public String Text {
			get { return text; }
			set { text = value; }
		}
		public int FontSize {
			get { return fontsize; }
			set { fontsize = value; }
		}
		public Color Color {
			get { return color; }
			set { color = value; }
		}

		public TextNode(string txt, int size) : base()
		{
			Text = txt;
			FontSize = size;
			Color = Color.WHITE;
		}

		public override void Update(float deltaTime)
		{
			Draw();
		}

		public void Draw()
		{
			Raylib.DrawText(Text, (int)Position.X, (int)Position.Y, FontSize, Color);
		}

	}
}
