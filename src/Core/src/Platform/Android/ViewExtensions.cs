using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public static class ViewExtensions
	{
		const int DefaultAutomationTagId = -1;

		public static int AutomationTagId { get; set; } = DefaultAutomationTagId;

		public static void UpdateIsEnabled(this AView nativeView, IView view)
		{
			if (nativeView != null)
				nativeView.Enabled = view.IsEnabled;
		}

		public static void UpdateBackgroundColor(this AView nativeView, IView view)
		{
			var backgroundColor = view.BackgroundColor;
			if (!backgroundColor.IsDefault)
				nativeView?.SetBackgroundColor(backgroundColor.ToNative());
		}

		public static void UpdateBorderColor(this AView nativeView, IView view)
		{

		}

		public static void UpdateBorderWidth(this AView nativeView, IView view)
		{

		}

		public static void UpdateCornerRadius(this AView nativeView, IView view)
		{
			if (nativeView.Context == null)
				return;

			var cornerRadii = new[]
			{
				nativeView.Context.ToPixels(view.CornerRadius.TopLeft),
				nativeView.Context.ToPixels(view.CornerRadius.TopLeft),

				nativeView.Context.ToPixels(view.CornerRadius.TopRight),
				nativeView.Context.ToPixels(view.CornerRadius.TopRight),

				nativeView.Context.ToPixels(view.CornerRadius.BottomRight),
				nativeView.Context.ToPixels(view.CornerRadius.BottomRight),

				nativeView.Context.ToPixels(view.CornerRadius.BottomLeft),
				nativeView.Context.ToPixels(view.CornerRadius.BottomLeft)
			};

			nativeView.OutlineProvider?.Dispose();
			nativeView.OutlineProvider = new CornerRadiusViewOutlineProvider(cornerRadii);
			nativeView.ClipToOutline = true;
		}

		public static bool GetClipToOutline(this AView view)
		{
			return view.ClipToOutline;
		}

		public static void SetClipToOutline(this AView view, bool value)
		{
			view.ClipToOutline = value;
		}

		public static void UpdateAutomationId(this AView nativeView, IView view)
		{
			if (AutomationTagId == DefaultAutomationTagId)
			{
				AutomationTagId = Resource.Id.automation_tag_id;
			}

			nativeView.SetTag(AutomationTagId, view.AutomationId);
		}
	}
}