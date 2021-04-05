using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Microsoft.Maui
{
	public static class ViewExtensions
	{
		public static UIColor? GetBackgroundColor(this UIView view)
			=> view?.BackgroundColor;

		public static void UpdateIsEnabled(this UIView nativeView, IView view)
		{
			if (!(nativeView is UIControl uiControl))
				return;

			uiControl.Enabled = view.IsEnabled;
		}

		public static void UpdateBackgroundColor(this UIView nativeView, IView view)
		{
			if (nativeView == null)
				return;

			var color = view.BackgroundColor;

			if (!color.IsDefault)
				nativeView.BackgroundColor = color.ToNative();
		}

		public static void UpdateBorderColor(this UIView nativeView, IView view)
		{
			nativeView.FindLayerOfType<BorderCALayer>()?.UpdateBorderColor(view.BorderColor);
		}

		public static void UpdateBorderWidth(this UIView nativeView, IView view)
		{
			nativeView.FindLayerOfType<BorderCALayer>()?.UpdateBorderWidth(view.BorderWidth);
		}

		public static void UpdateCornerRadius(this UIView nativeView, IView view)
		{
			nativeView.FindLayerOfType<BorderCALayer>()?.UpdateCornerRadius(view.CornerRadius);
			//UIBezierPath roundedRectPath = CreateRoundedRectPath(nativeView.Bounds, view.CornerRadius);

			//var maskLayer = new CAShapeLayer
			//{
			//	Frame = nativeView.Bounds,
			//	Path = roundedRectPath.CGPath
			//};

			//nativeView.Layer.Mask = maskLayer;
			//nativeView.Layer.MasksToBounds = true;
		}

		public static void UpdateAutomationId(this UIView nativeView, IView view) =>
			nativeView.AccessibilityIdentifier = view.AutomationId;

		public static T? FindDescendantView<T>(this UIView view) where T : UIView
		{
			var queue = new Queue<UIView>();
			queue.Enqueue(view);

			while (queue.Count > 0)
			{
				var descendantView = queue.Dequeue();

				if (descendantView is T result)
					return result;

				for (var i = 0; i < descendantView.Subviews?.Length; i++)
					queue.Enqueue(descendantView.Subviews[i]);
			}

			return null;
		}

		internal static UIBezierPath CreateRoundedRectPath(CGRect rect, CornerRadius cornerRadius)
		{
			var path = new UIBezierPath();

			path.MoveTo(new CGPoint(rect.Width - cornerRadius.TopRight, rect.Y));
			path.AddArc(new CGPoint((float)rect.X + rect.Width - cornerRadius.TopRight, (float)rect.Y + cornerRadius.TopRight), (nfloat)cornerRadius.TopRight, (float)(Math.PI * 1.5), (float)Math.PI * 2, true);
			path.AddLineTo(new CGPoint(rect.Width, rect.Height - cornerRadius.BottomRight));
			path.AddArc(new CGPoint((float)rect.X + rect.Width - cornerRadius.BottomRight, (float)rect.Y + rect.Height - cornerRadius.BottomRight), (nfloat)cornerRadius.BottomRight, 0, (float)(Math.PI * .5), true);
			path.AddLineTo(new CGPoint(cornerRadius.BottomLeft, rect.Height));
			path.AddArc(new CGPoint((float)rect.X + cornerRadius.BottomLeft, (float)rect.Y + rect.Height - cornerRadius.BottomLeft), (nfloat)cornerRadius.BottomLeft, (float)(Math.PI * .5), (float)Math.PI, true);
			path.AddLineTo(new CGPoint(rect.X, cornerRadius.TopLeft));
			path.AddArc(new CGPoint((float)rect.X + cornerRadius.TopLeft, (float)rect.Y + cornerRadius.TopLeft), (nfloat)cornerRadius.TopLeft, (float)Math.PI, (float)(Math.PI * 1.5), true);

			path.ClosePath();

			return path;
		}

		internal static T FindLayerOfType<T>(this UIView view) where T : CALayer
		{
			if (view.Layer is T layer)
				return layer;

			var subLayer = view.Layer?.Sublayers?.FirstOrDefault(x => x is T);
			if (subLayer == null)
			{
				subLayer = new BorderCALayer();
				view.Layer?.InsertSublayer(subLayer, 0);
			}

			return (T)subLayer;
		}
	}
}