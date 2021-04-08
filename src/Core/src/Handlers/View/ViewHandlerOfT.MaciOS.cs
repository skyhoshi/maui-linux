using UIKit;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		UIView? View => ContainerView ?? (UIView?)base.NativeView;

		UIView? INativeViewHandler.NativeView => View;

		public override void SetFrame(Rectangle rect)
		{
			if (View != null)
				View.Frame = rect.ToCGRect();
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (NativeView == null)
			{
				return new Size(widthConstraint, heightConstraint);
			}

			var sizeThatFits = NativeView.SizeThatFits(new CoreGraphics.CGSize((float)widthConstraint, (float)heightConstraint));

			var size = new Size(
				sizeThatFits.Width == float.PositiveInfinity ? double.PositiveInfinity : sizeThatFits.Width,
				sizeThatFits.Height == float.PositiveInfinity ? double.PositiveInfinity : sizeThatFits.Height);

			if (double.IsInfinity(size.Width) || double.IsInfinity(size.Height))
			{
				NativeView.SizeToFit();

				size = new Size(NativeView.Frame.Width, NativeView.Frame.Height);
			}

			return size;
		}

		protected override void SetupContainer()
		{
			var oldParent = NativeView?.Superview;
			ContainerView ??= new ContainerView();

			if (oldParent == ContainerView)
				return;

			ContainerView.MainView = NativeView;
		}

		protected override void RemoveContainer()
		{

		}
	}
}