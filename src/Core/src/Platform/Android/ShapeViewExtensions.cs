using Android.Graphics;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public static class ShapeViewExtensions
	{
		public static void UpdateShape(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.UpdateShape(shapeView.Shape);
		}

		public static void UpdateStretch(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.UpdateStretch(shapeView.Stretch);
		}

		public static void UpdateFill(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.UpdateFill(shapeView.Fill);
		}

		public static void UpdateStroke(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.UpdateStroke(shapeView.Stroke);
		}

		public static void UpdateStrokeThickness(this MauiShapeView nativeView, IShapeView shapeView)
		{
			float strokeThickness = (float)shapeView.StrokeThickness;

			nativeView.UpdateStrokeThickness(strokeThickness);
		}

		public static void UpdateStrokeDashArray(this MauiShapeView nativeView, IShapeView shapeView)
		{
			if (shapeView.StrokeDashArray == null)
				return;

			nativeView.UpdateStrokeDashArray(shapeView.StrokeDashArray.ToArray());
		}

		public static void UpdateStrokeDashOffset(this MauiShapeView nativeView, IShapeView shapeView)
		{
			float strokeDashOffset = (float)shapeView.StrokeDashOffset;

			nativeView.UpdateStrokeDashOffset(strokeDashOffset);
		}

		public static void UpdateStrokeLineCap(this MauiShapeView nativeView, IShapeView shapeView)
		{
			PenLineCap lineCap = shapeView.StrokeLineCap;
			Paint.Cap strokeLineCap = Paint.Cap.Butt!;

			switch (lineCap)
			{
				case PenLineCap.Flat:
					strokeLineCap = Paint.Cap.Butt!;
					break;
				case PenLineCap.Square:
					strokeLineCap = Paint.Cap.Square!;
					break;
				case PenLineCap.Round:
					strokeLineCap = Paint.Cap.Round!;
					break;
			}

			nativeView.UpdateStrokeLineCap(strokeLineCap);
		}

		public static void UpdateStrokeLineJoin(this MauiShapeView nativeView, IShapeView shapeView)
		{
			PenLineJoin lineJoin = shapeView.StrokeLineJoin;
			Paint.Join strokeLineJoin = Paint.Join.Miter!;

			switch (lineJoin)
			{
				case PenLineJoin.Miter:
					strokeLineJoin = Paint.Join.Miter!;
					break;
				case PenLineJoin.Bevel:
					strokeLineJoin = Paint.Join.Bevel!;
					break;
				case PenLineJoin.Round:
					strokeLineJoin = Paint.Join.Round!;
					break;
			}

			nativeView.UpdateStrokeLineJoin(strokeLineJoin);
		}

		public static void UpdateStrokeMiterLimit(this MauiShapeView nativeView, IShapeView shapeView)
		{
			float strokeMiterLimit = (float)shapeView.StrokeMiterLimit;

			nativeView.UpdateStrokeMiterLimit(strokeMiterLimit);
		}

		public static void UpdateHeight(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.SetSize(shapeView);
		}

		public static void UpdateWidth(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.SetSize(shapeView);
		}

		internal static void SetSize(this MauiShapeView nativeView, IShapeView shapeView)
		{
			var height = shapeView.Height;
			var width = shapeView.Width;

			nativeView.UpdateSize(width, height);
		}
	}
}