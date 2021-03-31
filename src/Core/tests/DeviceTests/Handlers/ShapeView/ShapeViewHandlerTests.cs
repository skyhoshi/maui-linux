using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.ShapeView)]
	public partial class ShapeViewHandlerTests : HandlerTestBase<ShapeViewHandler, ShapeViewStub>
	{
		[Fact(DisplayName = "Rectangle Initializes Correctly")]
		public async Task RectangleInitializesCorrectly()
		{
			var rectangle = new ShapeViewStub()
			{
				Shape = new Graphics.Rectangle(),
				Fill = Color.Red,
				Height = 50,
				Width = 100
			};

			await ValidateNativeFill(rectangle, Color.Red);
		}

		[Fact(DisplayName = "Ellipse Initializes Correctly")]
		public async Task EllipseInitializesCorrectly()
		{
			var ellipse = new ShapeViewStub()
			{
				Shape = new Graphics.Ellipse(),
				Fill = Color.Blue,
				Height = 50,
				Width = 100
			};

			await ValidateNativeFill(ellipse, Color.Blue);
		}

		[Fact(DisplayName = "Line Initializes Correctly")]
		public async Task LineInitializesCorrectly()
		{
			var line = new ShapeViewStub()
			{
				Shape = new Graphics.Line { X1 = 0, Y1 = 0, X2 = 90, Y2 = 45 },
				Stroke = Color.Purple,
				StrokeThickness = 4,
				Height = 50,
				Width = 100
			};

			await ValidateNativeFill(line, Color.Purple);
		}

		[Fact(DisplayName = "Polyline Initializes Correctly")]
		public async Task PolylineInitializesCorrectly()
		{
			var polyline = new ShapeViewStub()
			{
				Shape = new Graphics.Polyline { Points = new PointCollection() { new Point(10, 10), new Point(100, 50), new Point(50, 90) } },
				Stroke = Color.Green,
				StrokeThickness = 4,
				Height = 50,
				Width = 100
			};

			await ValidateNativeFill(polyline, Color.Green);
		}

		[Fact(DisplayName = "Polygon Initializes Correctly")]
		public async Task PolygonInitializesCorrectly()
		{
			var polygon = new ShapeViewStub()
			{
				Shape = new Graphics.Polygon { Points = new PointCollection() { new Point(10, 10), new Point(100, 50), new Point(50, 90) } },
				Fill = Color.Lime,
				Stroke = Color.Black,
				StrokeThickness = 4,
				Height = 50,
				Width = 100
			};

			await ValidateNativeFill(polygon, Color.Lime);
		}

		[Fact(DisplayName = "Path Initializes Correctly")]
		public async Task PathInitializesCorrectly()
		{
			var path = new ShapeViewStub()
			{
				Shape = new Graphics.Path { Data = "M15.999996,0L31.999999,13.000001 15.999996,26.199999 0,13.000001z" },
				Fill = Color.Coral,
				Stroke = Color.Black,
				StrokeThickness = 1,
				Height = 50,
				Width = 50
			};

			await ValidateNativeFill(path, Color.Coral);
		}
	}
}
