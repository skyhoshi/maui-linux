using Android.Content;
using Android.Views;

namespace Microsoft.Maui.Handlers
{
	public partial class AbstractViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		public Context? Context => MauiContext?.Context;

		public void SetFrame(Rectangle frame)
		{
			var nativeView = View;

			if (nativeView == null)
				return;

			if (frame.Width < 0 || frame.Height < 0)
			{
				// TODO ezhart Determine whether this still happens
				// This is just some initial Forms value nonsense, nothing is actually laying out yet
				return;
			}

			if (Context == null)
				return;

			var left = Context.ToPixels(frame.Left);
			var top = Context.ToPixels(frame.Top);
			var bottom = Context.ToPixels(frame.Bottom);
			var right = Context.ToPixels(frame.Right);

			UpdateLayoutParams(frame.Width, frame.Height);

			System.Diagnostics.Debug.WriteLine($">>>>>> AbstractViewHandler {typeof(TVirtualView)} SetFrame {frame}");

			nativeView.Layout((int)left, (int)top, (int)right, (int)bottom);
		}

		public virtual Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (TypedNativeView == null)
			{
				return Size.Zero;
			}

			if (Context == null)
			{
				return new Size(widthConstraint, heightConstraint);
			}

			var deviceWidthConstraint = Context.ToPixels(widthConstraint);
			var deviceHeightConstraint = Context.ToPixels(heightConstraint);

			var widthSpec = MeasureSpecMode.AtMost.MakeMeasureSpec((int)deviceWidthConstraint);
			var heightSpec = MeasureSpecMode.AtMost.MakeMeasureSpec((int)deviceHeightConstraint);

			System.Diagnostics.Debug.WriteLine($">>>>>> AbstractViewHandler {typeof(TVirtualView)} GetDesiredSize");

			TypedNativeView.Measure(widthSpec, heightSpec);

			System.Diagnostics.Debug.WriteLine($">>>>>> AbstractViewHandler {typeof(TVirtualView)} result {TypedNativeView.MeasuredWidth}, {TypedNativeView.MeasuredHeight} (in pixels)");

			return Context.FromPixels(TypedNativeView.MeasuredWidth, TypedNativeView.MeasuredHeight);
		}

		void SetupContainer()
		{

		}

		void RemoveContainer()
		{

		}

		void UpdateLayoutParams(double width, double height)
		{
			// This will check the cross-platform size/layout info and update the layout parameters accordingly. We want the native
			// size/layout info to map as closely as we can to the xplat size/layout info because the platform will use the
			// native info to make decisions about things like whether to request re-layouts. 

			// For example, if we explicitly set the width of a Label, we generally don't want a text change to trigger a 
			// layout. Explicitly setting the width in the layout parameters lets Android know it should just update the text
			// in the TextView without requesting a layout update.

			if (View == null || VirtualView == null || View.Context == null)
			{
				return;
			}

			var context = View.Context;

			int explicitWidth = -1;
			int explicitHeight = -1;

			if (VirtualView.Width > -1)
			{
				// The virtual view has an explicit Width
				explicitWidth = (int)context.ToPixels(width);
			}

			if (VirtualView.Height > -1)
			{
				// The virtual view has an explicit Height
				explicitHeight = (int)context.ToPixels(height);
			}

			if (explicitHeight == -1 && explicitWidth == -1)
			{
				// Since we don't have any explicit width/height set, just leave the layout parameters alone
				return;
			}

			if (View.LayoutParameters == null)
			{
				// We'll default to WrapContent in both directions
				View.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,
					ViewGroup.LayoutParams.WrapContent);
			}

			if (explicitWidth > -1)
			{
				View.LayoutParameters.Width = explicitWidth;
			}

			if (explicitHeight > -1)
			{
				View.LayoutParameters.Height = explicitHeight;
			}
		}
	}
}