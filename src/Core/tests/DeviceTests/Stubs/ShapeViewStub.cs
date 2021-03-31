using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.DeviceTests.Stubs
{
	public partial class ShapeViewStub : StubBase, IShapeView
	{
		public Shape Shape { get; set; }

		public Color Fill { get; set; }

		public Color Stroke { get; set; }

		public double StrokeThickness { get; set; }

		public DoubleCollection StrokeDashArray { get; set; }

		public double StrokeDashOffset { get; set; }

		public PenLineCap StrokeLineCap { get; set; }

		public PenLineJoin StrokeLineJoin { get; set; }

		public double StrokeMiterLimit { get; set; }

		public Stretch Stretch { get; set; }
	}
}