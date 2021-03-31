using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using AColor = Android.Graphics.Color;
using AMatrix = Android.Graphics.Matrix;
using APath = Android.Graphics.Path;
using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public class MauiShapeView : AView
	{
		readonly ShapeDrawable _drawable;
		protected float _density;

		Graphics.Shape? _shape;

		APath? _path;
		readonly RectF _pathFillBounds;
		readonly RectF _pathStrokeBounds;

		Color _stroke;
		Color _fill;

		float _strokeWidth;
		float[]? _strokeDash;
		float _strokeDashOffset;

		Stretch _stretch;

		AMatrix? _transform;

		public MauiShapeView(Context? context) : base(context)
		{
			_drawable = new ShapeDrawable(null);

			if (_drawable.Paint != null)
				_drawable.Paint.AntiAlias = true;

			_density = Resources?.DisplayMetrics?.Density ?? 1.0f;

			_pathFillBounds = new RectF();
			_pathStrokeBounds = new RectF();

			_stretch = Stretch.None;
		}

		protected override void OnDraw(Canvas? canvas)
		{
			base.OnDraw(canvas);

			if (_path == null)
				return;

			AMatrix transformMatrix = CreateMatrix();

			_path.Transform(transformMatrix);
			transformMatrix.MapRect(_pathFillBounds);
			transformMatrix.MapRect(_pathStrokeBounds);

			if (_fill != null && _drawable.Paint != null)
			{
				_drawable.Paint.SetStyle(Paint.Style.Fill);

				AColor fillColor = Color.Default.ToNative();

				if (_fill != Color.Default)
					fillColor = _fill.ToNative();

				_drawable.Paint.Color = fillColor;

				_drawable.Draw(canvas);
				_drawable.Paint.SetShader(null);
			}

			if (_stroke != null && _drawable.Paint != null)
			{
				_drawable.Paint.SetStyle(Paint.Style.Stroke);

				AColor strokeColor = Color.Default.ToNative();

				if (_stroke != Color.Default)
					strokeColor = _stroke.ToNative();

				_drawable.Paint.Color = strokeColor;

				_drawable.Draw(canvas);
				_drawable.Paint.SetShader(null);
			}

			AMatrix inverseTransformMatrix = new AMatrix();
			transformMatrix.Invert(inverseTransformMatrix);
			_path.Transform(inverseTransformMatrix);
			inverseTransformMatrix.MapRect(_pathFillBounds);
			inverseTransformMatrix.MapRect(_pathStrokeBounds);
		}

		public void UpdateShape(Graphics.Shape shape)
		{
			_shape = shape;
		}

		public void UpdateShapeTransform(AMatrix matrix)
		{
			_transform = matrix;
			_path?.Transform(_transform);
			Invalidate();
		}

		public void UpdateStretch(Stretch stretch)
		{
			_stretch = stretch;
			Invalidate();
		}

		public void UpdateFill(Color fill)
		{
			_fill = fill;
			Invalidate();
		}

		public void UpdateStroke(Color stroke)
		{
			_stroke = stroke;
			Invalidate();
		}

		public void UpdateStrokeThickness(float strokeWidth)
		{
			_strokeWidth = _density * strokeWidth;

			if (_drawable.Paint != null)
				_drawable.Paint.StrokeWidth = _strokeWidth;

			UpdateStrokeDash();
		}

		public void UpdateStrokeDashArray(float[] dash)
		{
			_strokeDash = dash;
			UpdateStrokeDash();
		}

		public void UpdateStrokeDashOffset(float strokeDashOffset)
		{
			_strokeDashOffset = strokeDashOffset;
			UpdateStrokeDash();
		}

		public void UpdateStrokeDash()
		{
			if (_strokeDash != null && _strokeDash.Length > 1)
			{
				float[] strokeDash = new float[_strokeDash.Length];

				for (int i = 0; i < _strokeDash.Length; i++)
					strokeDash[i] = _strokeDash[i] * _strokeWidth;

				if (_drawable.Paint != null)
					_drawable.Paint.SetPathEffect(new DashPathEffect(strokeDash, _strokeDashOffset * _strokeWidth));
			}
			else
			{
				if (_drawable.Paint != null)
					_drawable.Paint.SetPathEffect(null);
			}

			UpdatePathStrokeBounds();
		}

		public void UpdateStrokeLineCap(Paint.Cap strokeCap)
		{
			if (_drawable.Paint != null)
				_drawable.Paint.StrokeCap = strokeCap;

			UpdatePathStrokeBounds();
		}

		public void UpdateStrokeLineJoin(Paint.Join strokeJoin)
		{
			if (_drawable.Paint != null)
				_drawable.Paint.StrokeJoin = strokeJoin;

			Invalidate();
		}

		public void UpdateStrokeMiterLimit(float strokeMiterLimit)
		{
			if (_drawable.Paint != null)
				_drawable.Paint.StrokeMiter = strokeMiterLimit * 2;

			UpdatePathStrokeBounds();
		}

		public void UpdateSize(double width, double height)
		{
			_drawable.SetBounds(0, 0, (int)(width * _density), (int)(height * _density));

			if (_shape != null)
			{
				var rect = new System.Graphics.RectangleF(0, 0, _drawable.Bounds.Width(), _drawable.Bounds.Height());
				var path = _shape.CreatePath(rect, _density);

				_path = path.ToNative();

				UpdatePathShape();
			}
		}

		protected void UpdatePathShape()
		{
			if (_path != null && !_drawable.Bounds.IsEmpty)
				_drawable.Shape = new PathShape(_path, _drawable.Bounds.Width(), _drawable.Bounds.Height());
			else
				_drawable.Shape = null;

			if (_path != null && _drawable.Paint != null)
			{
				using (APath fillPath = new APath())
				{
					_drawable.Paint.StrokeWidth = 0.01f;
					_drawable.Paint.SetStyle(Paint.Style.Stroke);
					_drawable.Paint.GetFillPath(_path, fillPath);
					fillPath.ComputeBounds(_pathFillBounds, false);
					_drawable.Paint.StrokeWidth = _strokeWidth;
				}
			}
			else
			{
				_pathFillBounds.SetEmpty();
			}

			UpdatePathStrokeBounds();
		}

		AMatrix CreateMatrix()
		{
			AMatrix matrix = new AMatrix();

			RectF drawableBounds = new RectF(_drawable.Bounds);
			float halfStrokeWidth = _drawable.Paint?.StrokeWidth / 2 ?? 0;

			drawableBounds.Left += halfStrokeWidth;
			drawableBounds.Top += halfStrokeWidth;
			drawableBounds.Right -= halfStrokeWidth;
			drawableBounds.Bottom -= halfStrokeWidth;

			switch (_stretch)
			{
				case Stretch.None:
					break;
				case Stretch.Fill:
					matrix.SetRectToRect(_pathFillBounds, drawableBounds, AMatrix.ScaleToFit.Fill);
					break;
				case Stretch.Uniform:
					matrix.SetRectToRect(_pathFillBounds, drawableBounds, AMatrix.ScaleToFit.Center);
					break;
				case Stretch.UniformToFill:
					float widthScale = drawableBounds.Width() / _pathFillBounds.Width();
					float heightScale = drawableBounds.Height() / _pathFillBounds.Height();
					float maxScale = Math.Max(widthScale, heightScale);
					matrix.SetScale(maxScale, maxScale);
					matrix.PostTranslate(
						drawableBounds.Left - maxScale * _pathFillBounds.Left,
						drawableBounds.Top - maxScale * _pathFillBounds.Top);
					break;
			}

			return matrix;
		}

		void UpdatePathStrokeBounds()
		{
			if (_path != null && _drawable.Paint != null)
			{
				using (APath strokePath = new APath())
				{
					_drawable.Paint.SetStyle(Paint.Style.Stroke);
					_drawable.Paint.GetFillPath(_path, strokePath);
					strokePath.ComputeBounds(_pathStrokeBounds, false);
				}
			}
			else
			{
				_pathStrokeBounds.SetEmpty();
			}

			Invalidate();
		}
	}
}