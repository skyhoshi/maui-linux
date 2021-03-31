using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class ShapeViewHandler
	{
		public static PropertyMapper<IShapeView, ShapeViewHandler> ShapeViewMapper = new PropertyMapper<IShapeView, ShapeViewHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IShapeView.Shape)] = MapShape,
			[nameof(IShapeView.Stretch)] = MapStretch,
			[nameof(IShapeView.Fill)] = MapFill,
			[nameof(IShapeView.Stroke)] = MapStroke,
			[nameof(IShapeView.StrokeThickness)] = MapStrokeThickness,
			[nameof(IShapeView.StrokeDashArray)] = MapStrokeDashArray,
			[nameof(IShapeView.StrokeDashOffset)] = MapStrokeDashOffset,
			[nameof(IShapeView.StrokeLineCap)] = MapStrokeLineCap,
			[nameof(IShapeView.StrokeLineJoin)] = MapStrokeLineJoin,
			[nameof(IShapeView.StrokeMiterLimit)] = MapStrokeMiterLimit,
			[nameof(IShapeView.Height)] = MapHeight,
			[nameof(IShapeView.Width)] = MapWidth
		};

		public ShapeViewHandler() : base(ShapeViewMapper)
		{

		}

		public ShapeViewHandler(PropertyMapper mapper) : base(mapper ?? ShapeViewMapper)
		{

		}
	}
}