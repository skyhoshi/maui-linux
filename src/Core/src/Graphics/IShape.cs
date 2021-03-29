namespace Microsoft.Maui.Graphics
{
	/// <summary>
	/// Provides a base definition class for shape elements, such as
	/// Ellipse, Polygon, or Rectangle.
	/// </summary>
	public interface IShape
	{
		/// <summary>
		/// Indicates the brush used to paint the shape's interior.
		/// </summary>
		Color Fill { get; }

		/// <summary>
		/// Indicates the color used to paint the shape's outline.
		/// </summary>
		Color Stroke { get; }

		/// <summary>
		/// Represents a collection of double values that indicate the pattern of dashes and gaps
		/// that are used to outline a shape.
		/// </summary>
		double StrokeThickness { get; }

		/// <summary>
		/// Specifies the distance within the dash pattern where a dash begins.
		/// </summary>
		DoubleCollection StrokeDashArray { get; }

		/// <summary>
		/// Specifies the distance within the dash pattern where a dash begins.
		/// </summary>
		double StrokeDashOffset { get; }

		/// <summary>
		/// Describes the shape at the start and end of a line or segment.
		/// </summary>
		PenLineCap StrokeLineCap { get; }

		/// <summary>
		/// Specifies the type of join that is used at the vertices of a shape.
		/// </summary>
		PenLineJoin StrokeLineJoin { get; }

		/// <summary>
		/// Specifies the limit on the ratio of the miter length to half the StrokeThickness
		/// of a shape. 
		/// </summary>
		double StrokeMiterLimit { get; }

		/// <summary>
		/// Describes how the shape fills its allocated space.
		/// </summary>
		Stretch Aspect { get; }
	}
}
