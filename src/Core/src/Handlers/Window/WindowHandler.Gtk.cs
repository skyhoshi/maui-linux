using System;

namespace Microsoft.Maui.Handlers
{

	public partial class WindowHandler : ElementHandler<IWindow, Gtk.Window>
	{

		public static void MapTitle(IWindowHandler handler, IWindow window) =>
			handler.PlatformView.UpdateTitle(window);

		public static void MapContent(IWindowHandler handler, IWindow window)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			var nativeContent = window.Content.ToPlatform(handler.MauiContext);

			handler.PlatformView.Child = nativeContent;
		}

		[MissingMapper]
		public static void MapRequestDisplayDensity(IWindowHandler handler, IWindow window, object? args) { }

		public static void MapToolbar(IWindowHandler handler, IWindow view)
		{
			if (view is IToolbarElement tb)
				ViewHandler.MapToolbar(handler, tb);
		}
	}

}