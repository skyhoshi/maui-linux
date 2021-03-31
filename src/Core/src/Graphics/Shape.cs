using System.Graphics;

namespace Microsoft.Maui.Graphics
{
	public abstract class Shape : IShape
	{
		public Color Fill { get; set; }

		public Color Stroke { get; set; }

		public double StrokeThickness { get; set; }

		public DoubleCollection? StrokeDashArray { get; set; }

		public double StrokeDashOffset { get; set; }

		public PenLineCap StrokeLineCap { get; set; }

		public PenLineJoin StrokeLineJoin { get; set; }

		public double StrokeMiterLimit { get; set; }

		public Stretch Stretch { get; set; }

		public abstract PathF CreatePath(RectangleF rect, float density = 1.0f);
	}
}