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
			nativeView.GetOrCreateBorderDrawable(view)?.UpdateBackgroundColor(view.BackgroundColor);
		}

		public static void UpdateBorderColor(this AView nativeView, IView view)
		{
			nativeView.GetOrCreateBorderDrawable(view)?.UpdateBorderColor(view.BorderColor);
		}

		public static void UpdateBorderWidth(this AView nativeView, IView view)
		{
			nativeView.GetOrCreateBorderDrawable(view)?.UpdateBorderWidth(view.BorderWidth);
		}

		public static void UpdateCornerRadius(this AView nativeView, IView view)
		{
			nativeView.GetOrCreateBorderDrawable(view)?.UpdateCornerRadius(view.CornerRadius);
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

		internal static BorderShapeDrawable? GetBorderDrawable(this AView nativeView)
		{
			return nativeView.Background as BorderShapeDrawable;
		}

		internal static BorderShapeDrawable? GetOrCreateBorderDrawable(this AView nativeView, IView view)
		{
			var borderShapeDrawable = nativeView.GetBorderDrawable();

			if(borderShapeDrawable == null)
			{
				nativeView.SetBorderDrawable(view);
				borderShapeDrawable = nativeView.GetBorderDrawable();
			}

			return borderShapeDrawable;
		}

		internal static void SetBorderDrawable(this AView nativeView, IView view)
		{
			if (nativeView != null && !(nativeView.Background is BorderShapeDrawable))
			{
				nativeView.Background?.Dispose();
				nativeView.Background = new BorderShapeDrawable(nativeView.Context, view);
			}
		}
	}
}