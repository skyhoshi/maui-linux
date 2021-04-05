using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
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
			{
				var borderCALayer = nativeView.GetLayerOfType<BorderCALayer>();

				if (borderCALayer != null)
					borderCALayer.UpdateBackgroundColor(view.BorderColor);
				else
					nativeView.BackgroundColor = color.ToNative();
			}
		}

		public static void UpdateBorderColor(this UIView nativeView, IView view)
		{
			nativeView.GetOrCreateLayerOfType<BorderCALayer>()?.UpdateBorderColor(view.BorderColor);
		}

		public static void UpdateBorderWidth(this UIView nativeView, IView view)
		{
			nativeView.GetOrCreateLayerOfType<BorderCALayer>()?.UpdateBorderWidth(view.BorderWidth);
		}

		public static void UpdateCornerRadius(this UIView nativeView, IView view)
		{
			nativeView.GetOrCreateLayerOfType<BorderCALayer>()?.UpdateCornerRadius(view.CornerRadius);
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

		internal static T? GetLayerOfType<T>(this UIView view) where T : CALayer
		{
			if (view.Layer is T layer)
				return layer;

			var subLayer = view.Layer?.Sublayers?.FirstOrDefault(x => x is T);

			return subLayer as T;
		}

		internal static T GetOrCreateLayerOfType<T>(this UIView view) where T : CALayer
		{
			if (view.Layer is T layer)
				return layer;

			var subLayer = view.Layer?.Sublayers?.FirstOrDefault(x => x is T);

			if (subLayer == null)
			{
				subLayer = Activator.CreateInstance<T>();
				view.Layer?.InsertSublayer(subLayer, 0);
			}

			return (T)subLayer;
		}
	}
}