using System;
using CoreGraphics;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public static class ShapeViewExtensions
	{
		public static void UpdateShape(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.ShapeLayer.UpdateShape(shapeView.Shape);
		}

		public static void UpdateStretch(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.ShapeLayer.UpdateStretch(shapeView.Stretch);
		}

		public static void UpdateFill(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.ShapeLayer.UpdateFill(shapeView.Fill);
		}

		public static void UpdateStroke(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.ShapeLayer.UpdateStroke(shapeView.Stroke);
		}

		public static void UpdateStrokeThickness(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nativeView.ShapeLayer.UpdateStrokeThickness(shapeView.StrokeThickness);
		}

		public static void UpdateStrokeDashArray(this MauiShapeView nativeView, IShapeView shapeView)
		{
			if (shapeView.StrokeDashArray == null || shapeView.StrokeDashArray.Count == 0)
				nativeView.ShapeLayer.UpdateStrokeDashArray(new nfloat[0]);
			else
			{
				nfloat[] dashArray;
				double[] array;

				if (shapeView.StrokeDashArray.Count % 2 == 0)
				{
					array = new double[shapeView.StrokeDashArray.Count];
					dashArray = new nfloat[shapeView.StrokeDashArray.Count];
					shapeView.StrokeDashArray.CopyTo(array, 0);
				}
				else
				{
					array = new double[2 * shapeView.StrokeDashArray.Count];
					dashArray = new nfloat[2 * shapeView.StrokeDashArray.Count];
					shapeView.StrokeDashArray.CopyTo(array, 0);
					shapeView.StrokeDashArray.CopyTo(array, shapeView.StrokeDashArray.Count);
				}

				double thickness = shapeView.StrokeThickness;

				for (int i = 0; i < array.Length; i++)
					dashArray[i] = new nfloat(thickness * array[i]);

				nativeView.ShapeLayer.UpdateStrokeDashArray(dashArray);
			}
		}

		public static void UpdateStrokeDashOffset(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nfloat strokeDashOffset = (nfloat)shapeView.StrokeDashOffset;

			nativeView.ShapeLayer.UpdateStrokeDashOffset(strokeDashOffset);
		}

		public static void UpdateStrokeLineCap(this MauiShapeView nativeView, IShapeView shapeView)
		{
			PenLineCap lineCap = shapeView.StrokeLineCap;
			CGLineCap strokeLineCap = CGLineCap.Butt;

			switch (lineCap)
			{
				case PenLineCap.Flat:
					strokeLineCap = CGLineCap.Butt;
					break;
				case PenLineCap.Square:
					strokeLineCap = CGLineCap.Square;
					break;
				case PenLineCap.Round:
					strokeLineCap = CGLineCap.Round;
					break;
			}

			nativeView.ShapeLayer.UpdateStrokeLineCap(strokeLineCap);
		}

		public static void UpdateStrokeLineJoin(this MauiShapeView nativeView, IShapeView shapeView)
		{
			PenLineJoin lineJoin = shapeView.StrokeLineJoin;
			CGLineJoin strokeLineJoin = CGLineJoin.Miter;

			switch (lineJoin)
			{
				case PenLineJoin.Miter:
					strokeLineJoin = CGLineJoin.Miter;
					break;
				case PenLineJoin.Bevel:
					strokeLineJoin = CGLineJoin.Bevel;
					break;
				case PenLineJoin.Round:
					strokeLineJoin = CGLineJoin.Round;
					break;
			}

			nativeView.ShapeLayer.UpdateStrokeLineJoin(strokeLineJoin);
		}

		public static void UpdateStrokeMiterLimit(this MauiShapeView nativeView, IShapeView shapeView)
		{
			nfloat strokeMiterLimit = new nfloat(shapeView.StrokeMiterLimit);

			nativeView.ShapeLayer.UpdateStrokeMiterLimit(strokeMiterLimit);
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
			var height = (float)shapeView.Height;
			var width = (float)shapeView.Width;

			nativeView.ShapeLayer.UpdateSize(new CGSize(width, height));
		}
	}
}