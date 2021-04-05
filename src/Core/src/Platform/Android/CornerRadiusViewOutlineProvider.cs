using Android.Graphics;
using Android.Views;

namespace Microsoft.Maui
{
	public class CornerRadiusViewOutlineProvider : ViewOutlineProvider
    {
        private readonly float[] _cornerRadii;

        public CornerRadiusViewOutlineProvider(float[] cornerRadii)
        {
            _cornerRadii = cornerRadii;
        }

        public override void GetOutline(View? view, Outline? outline)
        {
            if (view == null)
                return;

            var roundPath = new Path();

            roundPath.AddRoundRect(-1, 0, view.Width + 2, view.Height, _cornerRadii, Path.Direction.Cw!);

            outline?.SetConvexPath(roundPath);
        }
    }
}