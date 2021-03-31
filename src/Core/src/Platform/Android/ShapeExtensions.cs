using System.Graphics;
using System.Graphics.Android;
using Android.Graphics;

namespace Microsoft.Maui
{
	public static class ShapeExtensions
	{
		public static Path ToNative(this PathF pathF)
		{
			return pathF.AsAndroidPath();
		}
	}
}