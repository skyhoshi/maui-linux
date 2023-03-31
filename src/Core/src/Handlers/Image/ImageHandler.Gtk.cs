#nullable enable
using System;
using System.Threading.Tasks;
using Gdk;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{

	public partial class ImageHandler : ViewHandler<IImage, ImageView>
	{

		protected override ImageView CreatePlatformView()
		{
			var img = new ImageView();

			return img;
		}

		[MissingMapper]
		public static void MapAspect(IImageHandler handler, IImage image) { }

		[MissingMapper]
		public static void MapIsAnimationPlaying(IImageHandler handler, IImage image) { }

		public static void MapSource(IImageHandler handler, IImage image) =>
			MapSourceAsync(handler, image).FireAndForget(handler);

		public static async Task MapSourceAsync(IImageHandler handler, IImage image)
		{
			if (handler.PlatformView == null)
				return;

			await handler.SourceLoader.UpdateImageSourceAsync();

		}

		void OnSetImageSource(Pixbuf? obj)
		{
			PlatformView.Image = obj;
		}

		static int? Request(double viewSize) => viewSize >= 0 ? (int)Math.Round(viewSize) : null;
		private void UpdateWidthRequest(IImage image)
		{
			var widthRequest = Request(image.Width * PlatformView.ScaleFactor);

			if (widthRequest is not null && widthRequest != PlatformView.WidthRequest && widthRequest != PlatformView.AllocatedWidth)
			{
				PlatformView.ChangeWidth(widthRequest.Value);
				PlatformView.QueueResize();
			}
		}

		private void UpdateHeightRequest(IImage image)
		{
			var heightRequest = Request(image.Height * PlatformView.ScaleFactor);

			if (heightRequest is not null && heightRequest != PlatformView.WidthRequest && heightRequest != PlatformView.AllocatedWidth)
			{
				PlatformView.ChangeHeight(heightRequest.Value);
				PlatformView.QueueResize();
			}
		}

		public static void MapWidthRequest(IImageHandler handler, IImage view)
		{
			((ImageHandler)handler).UpdateWidthRequest(view);
		}

		public static void MapHeightRequest(IImageHandler handler, IImage view)
		{
			((ImageHandler)handler).UpdateHeightRequest(view);
		}

	}

}