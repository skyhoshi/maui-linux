using System;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class ShapeViewHandler : AbstractViewHandler<IShapeView, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapShape(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStretch(IViewHandler handler, IShapeView shapeView) { }
		public static void MapFill(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStroke(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStrokeThickness(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStrokeDashArray(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStrokeDashOffset(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStrokeLineCap(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStrokeLineJoin(IViewHandler handler, IShapeView shapeView) { }
		public static void MapStrokeMiterLimit(IViewHandler handler, IShapeView shapeView) { }
		public static void MapHeight(IViewHandler handler, IShapeView shapeView) { }
		public static void MapWidth(IViewHandler handler, IShapeView shapeView) { }
	}
}