namespace Microsoft.Maui.Graphics
{
	/// <summary>
	/// Represents a View that enables you to draw a shape to the screen.
	/// </summary>
	public interface IShapeView : IView
	{
		/// <summary>
		/// Gets the Shape definition to render.
		/// </summary>
		IShape Shape { get; }
	}
}