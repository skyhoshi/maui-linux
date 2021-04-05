namespace Microsoft.Maui
{
	/// <summary>
	/// Provides functionality to set the view borders.
	/// </summary>
	public interface IBorder
	{
		/// <summary>
		/// Gets the the color of a view's border.
		/// </summary>
		Color BorderColor { get; }

		/// <summary>
		/// Gets the the width of a view's border.
		/// </summary>
		double BorderWidth { get; }
	}
}