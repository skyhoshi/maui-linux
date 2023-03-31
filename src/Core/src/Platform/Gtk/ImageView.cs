using Gdk;

namespace Microsoft.Maui.Platform
{

	// https://docs.gtk.org/gtk3/class.Imgage.html 

	// GtkImage has nothing like Aspect; maybe an ownerdrawn class is needed 
	// could be: https://docs.gtk.org/gtk3/class.DrawingArea.html
	// or Microsoft.Maui.Graphics.Platform.Gtk.GtkGraphicsView

	public class ImageView : Gtk.Image
	{
		// OriginalSizeBuf is added so when we want to resize the image using Pixbuf we can access the
		// initial Pixbuf and resize that. 
		Pixbuf? _originalSizeBuf;
		// Image might be null because it is possible to have a WidthRequest before Pixbuf has a value.
		// In this case, we'll just save the value, and once Pixbuf is initialized we'll update its size
		// to have the saved values
		int? _lastRequestedWidth;
		int? _lastRequestedHeight;

		public Pixbuf? Image
		{
			get => Pixbuf;
			set
			{
				_originalSizeBuf = value;
				if (_lastRequestedHeight is not null || _lastRequestedWidth is not null)
				{
					Pixbuf = _originalSizeBuf?.ScaleSimple(
						_lastRequestedWidth ?? _originalSizeBuf.Width,
						_lastRequestedHeight ?? _originalSizeBuf.Height,
						InterpType.Bilinear
					);
				}
				else
				{
					Pixbuf = value;
				}
			}
		}

		internal void ChangeHeight(int height)
		{
			if (Image is null)
			{
				_lastRequestedHeight = height;
			}
			else
			{
				var newBuf = _originalSizeBuf?.ScaleSimple(Pixbuf.Width, height, InterpType.Bilinear);
				Pixbuf = newBuf;
			}
		}

		internal void ChangeWidth(int width)
		{
			if (Image is null)
			{
				_lastRequestedWidth = width;
			}
			else
			{
				var newBuf = _originalSizeBuf?.ScaleSimple(width, Pixbuf.Height, InterpType.Bilinear);
				Pixbuf = newBuf;
			}
		}

	}

}