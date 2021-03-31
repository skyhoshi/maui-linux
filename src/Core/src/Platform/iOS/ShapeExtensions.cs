using System.Graphics;
using System.Graphics.CoreGraphics;
using CoreGraphics;

namespace Microsoft.Maui
{
	public static class ShapeExtensions
	{
		public static CGPath ToNative(this PathF pathF)
		{
			return pathF.AsCGPath();
		}
	}
}