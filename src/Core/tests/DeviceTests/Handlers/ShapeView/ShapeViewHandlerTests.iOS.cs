using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ShapeViewHandlerTests
	{
		MauiShapeView GetNativeShapeView(ShapeViewHandler shapeViewHandler) =>
			(MauiShapeView)shapeViewHandler.View;

		Task ValidateNativeFill(IShapeView shapeView, Color color)
		{
			return InvokeOnMainThreadAsync(() =>
			{
				return GetNativeShapeView(CreateHandler(shapeView)).AssertContainsColor(color);
			});
		}
	}
}