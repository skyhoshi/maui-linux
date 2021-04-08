using AppKit;

namespace Microsoft.Maui
{
	public interface INativeViewHandler : IViewHandler
	{
		new NSView? NativeView { get; }
	}
}