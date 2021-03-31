using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class ShapeViewHandler : AbstractViewHandler<IShapeView, MauiShapeView>
	{
		protected override MauiShapeView CreateNativeView()
		{
			return new MauiShapeView();
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (TypedNativeView != null)
			{
				return TypedNativeView.ShapeLayer.GetDesiredSize();
			}

			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public static void MapShape(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateShape(shapeView);
		}

		public static void MapStretch(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStretch(shapeView);
		}

		public static void MapFill(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateFill(shapeView);
		}

		public static void MapStroke(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStroke(shapeView);
		}

		public static void MapStrokeThickness(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStrokeThickness(shapeView);
		}

		public static void MapStrokeDashArray(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStrokeDashArray(shapeView);
		}

		public static void MapStrokeDashOffset(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStrokeDashOffset(shapeView);
		}

		public static void MapStrokeLineCap(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStrokeLineCap(shapeView);
		}

		public static void MapStrokeLineJoin(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStrokeLineJoin(shapeView);
		}

		public static void MapStrokeMiterLimit(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateStrokeMiterLimit(shapeView);
		}

		public static void MapHeight(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateHeight(shapeView);
		}

		public static void MapWidth(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.TypedNativeView?.UpdateWidth(shapeView);
		}
	}
}